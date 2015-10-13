using System.Collections.Generic;

namespace VSFiltersGenerator.src
{
	internal class ItemGroup
	{
		public ItemGroup()
		{
			GroupItems = new List<BaseItem>();
		}

		public List<BaseItem> GroupItems { get; set; }
	}
}