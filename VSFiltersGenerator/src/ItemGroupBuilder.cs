using System;
using System.Collections.Generic;
using System.IO;
using VSFiltersGenerator.src;

namespace VSFiltersGenerator
{
	internal class ItemGroupBuilder
	{
		public ItemGroupBuilder()
		{
		}

		public void GenerateFilters(string directoryPath, FilterItem filterItem, ref ItemGroup itemGroup)
		{
			var paths = Directory.GetDirectories(directoryPath);
			var item = new FilterItem((filterItem == null ? "" : (filterItem.Include + "\\"))
										+ Path.GetFileName(directoryPath), Guid.NewGuid());

			item.ClCompiles = GetClItems(directoryPath, "*.cpp");
			item.ClIncludes = GetClItems(directoryPath, "*.h");

			itemGroup.GroupItems.Add(item);

			foreach (var path in paths)
				GenerateFilters(path, item, ref itemGroup);
		}

		private List<BaseItem> GetClItems(string directoryPath, string filter)
		{
			var cppFiles = Directory.GetFiles(directoryPath, filter, SearchOption.TopDirectoryOnly);
			var itemList = new List<BaseItem>();

			foreach (var cppFile in cppFiles)
			{
				var relativePath =
					new Uri(ExternalData.FlterFolder).MakeRelativeUri(new Uri(cppFile)).ToString().Replace('/', Path.DirectorySeparatorChar);
				BaseItem clCompileItem = new BaseItem(relativePath);
				itemList.Add(clCompileItem);
			}
			return itemList;
		}
	}
}