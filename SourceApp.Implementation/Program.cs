using System;

namespace SourceApp.Implementation
{
	static class Program
	{
		private const string PressAnyKey = "\nPress any key to continue...";

		static void Main(string[] args)
		{


			PressToContinue();
		}

		private static void PressToContinue()
		{
			Console.WriteLine(PressAnyKey);
			Console.ReadKey(true);
		}
	}
}
