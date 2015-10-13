using System;
using System.Linq;
using VSFiltersGenerator.src;

namespace VSFiltersGenerator
{
	internal class Program
	{
		private static string argsError = @"Error! Please use args: 'ProjectName FlterFolder ClassFolder'";

		private static void Main(string[] args)
		{
			if (args.Count() != 3)
			{
				Console.WriteLine(argsError);
				return;
			}
			Console.WriteLine("Start filter generation...");
			ExternalData.ProjectName = args[0];
			ExternalData.FlterFolder = args[1];
			ExternalData.ClassFolder = args[2];

			Console.WriteLine("Generation items...");
			ItemGroup filterItemGroup = new ItemGroup();
			var itemGroupBuilder = new ItemGroupBuilder();
			itemGroupBuilder.GenerateFilters(ExternalData.ClassFolder, null, ref filterItemGroup);
			Console.WriteLine("Generation items complete!");
			Console.WriteLine("Generation build filter XML string...");
			var filterXmlBuilder = new FilterXmlBuilder();
			var xmlFileString = filterXmlBuilder.GenerateFilterXmlFromItemGroup(filterItemGroup);
			Console.WriteLine("Generation build filter XML string complete!");
			Console.WriteLine("Saving in file...");
			XmlSaver.SaveXmlStringToVsFilterFile(xmlFileString);
			Console.WriteLine("Filter generation finished.");
		}
	}
}