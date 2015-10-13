using System;
using System.Collections.Generic;

namespace VSFiltersGenerator.src
{
	internal class FilterItem : BaseItem
	{
		public Guid UniqueIdentifier { get; set; }

		public List<BaseItem> ClIncludes { get; set; }
		public List<BaseItem> ClCompiles { get; set; }

		public FilterItem(string include, Guid uniqueIdentifier)
			: base(include)
		{
			UniqueIdentifier = uniqueIdentifier;
			ClIncludes = new List<BaseItem>();
			ClCompiles = new List<BaseItem>();
		}
	}
}