using System;
using System.Globalization;
using System.Linq;

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

			IUnityContainer container = new UnityContainer();

			foreach (var arg in args)
			{
				container = new UnityContainer();
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
						break;
				}

				IManager manager;
				try
				{
					manager = container.Resolve<IManager>();
				}
				catch (Exception e)
				{
					Console.WriteLine("Arg: {0}; registrations: {1}\n{2}", arg.ToUpperInvariant(), container.Registrations.Count(), e);
					foreach(var reg in container.Registrations)
						Console.WriteLine("\t- {0}", reg.RegisteredType);

					PressToContinue();
					continue;
				}

				if (!manager.GetPresentation().Load(string.Format("{0} => insert name: ", arg.ToUpperInvariant()))) continue;

				manager.GetPresentation().GetContent();
				var content = manager.GetPresentation().Content;
				manager.GetPresentation()
				       .GetContent("{0}[{1}]; Word count = {2}",
				                   content.Text,
				                   content.ID.ToString(),
				                   content.Text.Split(' ').Count().ToString(CultureInfo.InvariantCulture));

				Console.WriteLine("Registrations count: {0}",container.Registrations.Count());

				PressToContinue();
			}
		}

		private static void SetContainer_ByConvention(IUnityContainer container)
		{
			container.RegisterTypes(AllClasses.FromLoadedAssemblies(),
			                        WithMappings.FromAllInterfaces,
			                        WithName.Default);
			container.RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()));			
		}

		private static void SetContainer_Mixed(IUnityContainer container)
		{
			container.LoadConfiguration("testContainer");

			container.RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()));
		}

		private static void SetContainer_ByConfig(IUnityContainer container)
		{
			container.RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()));
			container.LoadConfiguration("fullContainer");
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