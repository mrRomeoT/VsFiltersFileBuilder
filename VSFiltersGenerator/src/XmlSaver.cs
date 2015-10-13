using System;
using System.IO;
using System.Text;

namespace VSFiltersGenerator.src
{
	internal class XmlSaver
	{
		public static void SaveXmlStringToVsFilterFile(string xmlString)
		{
			try
			{
				var filePath = ExternalData.FlterFolder + Path.DirectorySeparatorChar + ExternalData.ProjectName +
							   ".vcxproj.filters";

				var writer = new StreamWriter(filePath, false, Encoding.UTF8);

				using (writer)
				{
					writer.Write(xmlString);
					writer.Flush();
				}
				Console.WriteLine("Saving in file complete!");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error - " + ex.Message);
			}
		}
	}
}