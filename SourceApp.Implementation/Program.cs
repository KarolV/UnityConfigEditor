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

			foreach(var arg in args)
			{
				switch (arg)
				{
					case "m":
					case "mixed":
						SetContainer_Mixed(container);
						break;
					case "c":
					case "conv":
						SetContainer_ByConvention(container);
						break;
					case "x":
					case "xml":
						SetContainer_ByConfig(container);
						container.RegisterType<ITestObject, TestObject>(new InjectionConstructor(guid));
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
		}

		private static void SetContainer_ByConvention(IUnityContainer container)
		{
			container.RegisterTypes(AllClasses.FromAssembliesInBasePath(),
			                        WithMappings.FromMatchingInterface,
			                        WithName.Default);
			container.RegisterType<ITestObject, TestObject>("GUID param", new InjectionConstructor(Guid.NewGuid()));
		}

		private static void SetContainer_Mixed(IUnityContainer container)
		{
			container.LoadConfiguration("testContainer");

			container.RegisterType<ITestObject, TestObject>()
					 .RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()));
		}

		private static void SetContainer_ByConfig(IUnityContainer container)
		{
			container.LoadConfiguration("testContainer");
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