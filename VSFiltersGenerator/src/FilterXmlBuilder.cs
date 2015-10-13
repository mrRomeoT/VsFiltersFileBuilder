using System.IO;
using System.Text;
using System.Xml;
using VSFiltersGenerator.src;

namespace VSFiltersGenerator
{
	internal class FilterXmlBuilder
	{
		private string _xmlNameSpace;
		private XmlDocument _xmlDocument;
		private string _clInclude = "ClInclude";
		private string _clCompile;

		public FilterXmlBuilder()
		{
			_xmlDocument = new XmlDocument();
			var xmlDeclaration = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
			XmlElement root = _xmlDocument.DocumentElement;
			_xmlDocument.InsertBefore(xmlDeclaration, root);
			_xmlNameSpace = "http://schemas.microsoft.com/developer/msbuild/2003";
		}

		public string GenerateFilterXmlFromItemGroup(ItemGroup filterItemGroup)
		{
			var projectElement = _xmlDocument.CreateElement("Project", _xmlNameSpace);
			projectElement.SetAttribute("ToolsVersion", "", "4.0");
			_xmlDocument.AppendChild(projectElement);
			var filterItemsGroup = _xmlDocument.CreateElement("ItemGroup", _xmlNameSpace);

			foreach (var baseItem in filterItemGroup.GroupItems)
			{
				var filterItem = (FilterItem)baseItem;
				var filterXml = _xmlDocument.CreateElement("Filter", _xmlNameSpace);
				filterXml.SetAttribute("Include", filterItem.Include);
				var uniqueIdentifier = _xmlDocument.CreateElement("UniqueIdentifier", _xmlNameSpace);
				uniqueIdentifier.InnerText = "{" + filterItem.UniqueIdentifier.ToString() + "}";
				filterItemsGroup.AppendChild(filterXml);
				filterXml.AppendChild(uniqueIdentifier);
			}

			_clCompile = "ClCompile";
			var compileItemGroups = MakeXmlElementForItemGroup(filterItemGroup, _clCompile);
			var includeItemGroups = MakeXmlElementForItemGroup(filterItemGroup, _clInclude);

			projectElement.AppendChild(filterItemsGroup);
			projectElement.AppendChild(compileItemGroups);
			projectElement.AppendChild(includeItemGroups);
			return FormatXml(_xmlDocument);
		}

		private XmlElement MakeXmlElementForClElement(BaseItem clCompile, FilterItem filterItem, string clElementName)
		{
			var compileXml = _xmlDocument.CreateElement(clElementName, _xmlNameSpace);
			compileXml.SetAttribute("Include", clCompile.Include);
			var filterXml = _xmlDocument.CreateElement("Filter", _xmlNameSpace);
			filterXml.InnerText = filterItem.Include;
			compileXml.AppendChild(filterXml);
			return compileXml;
		}

		private XmlElement MakeXmlElementForItemGroup(ItemGroup filterItemGroup, string clElementName)
		{
			var includeItemGroups = _xmlDocument.CreateElement("", "ItemGroup", _xmlNameSpace);

			foreach (var baseItem in filterItemGroup.GroupItems)
			{
				var filterItem = (FilterItem)baseItem;
				var clItems = clElementName == _clInclude ? filterItem.ClIncludes : filterItem.ClCompiles;
				foreach (var clCompile in clItems)
					includeItemGroups.AppendChild(MakeXmlElementForClElement(clCompile, filterItem, clElementName));
			}

			return includeItemGroups;
		}

		private string BeautifyXml(XmlDocument doc)
		{
			var sb = new StringBuilder();
			var settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "  ",
				NewLineChars = "\r\n",
				NewLineHandling = NewLineHandling.Replace
			};
			using (var writer = XmlWriter.Create(sb, settings))
			{
				doc.Save(writer);
			}
			return sb.ToString();
		}

		private string FormatXml(XmlDocument xd)
		{
			var sb = new StringBuilder();
			using (XmlTextWriter xtw = new XmlTextWriter(new StringWriter(sb)))
			{
				try
				{
					xtw.Formatting = Formatting.Indented;
					xd.WriteTo(xtw);
				}
				finally
				{
					xtw.Close();
				}
			}

			return sb.ToString();
		}
	}
}