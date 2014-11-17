using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using SourceApp.Interface;

namespace SourceApp.Implementation
{
	internal static class Program
	{
		private const string PressAnyKey = "\nPress any key to continue...";
		private const string PressIfNoArgs = "\nThere are no args - set them up in project properties...";

		private static void Main(params string[] args)
		{
			if (args == null || args.Length == 0)
			{
				PressToContinue(true);
				return;
			}

			var container = new UnityContainer();

			var guid = Guid.NewGuid();

			switch (args[0])
			{
				case "m":
				case "mixed":
					SetContainer_Mixed(container);
					break;
				case "c":
				case "conv":
					SetContainer_ByConvention(container);
					break;
			}

			var manager = container.Resolve<IManager>();
			//manager.GetTestObject().SetText(guid.ToString());
			Console.WriteLine(manager.GetTestObject().ToString());

			manager = container.Resolve<IManager>();
			manager.GetTestObject().SetText("shit");
			Console.WriteLine(manager.GetTestObject().ToString());

			PressToContinue();
		}

		private static void SetContainer_ByConvention(IUnityContainer container)
		{
			container.RegisterTypes(AllClasses.FromLoadedAssemblies(),
			                        WithMappings.FromAllInterfaces,
			                        WithName.Default);
		}

		private static void SetContainer_Mixed(IUnityContainer container)
		{
			container.LoadConfiguration("testContainer");

			container.RegisterType<ITestObject, TestObject>()
					 //.RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()))
					 ;
		}

		private static void PressToContinue(bool isEmpty = false)
		{
			if (isEmpty)
				Console.WriteLine(PressIfNoArgs);

			Console.WriteLine(PressAnyKey);
			Console.ReadKey(true);
		}
	}
}