using System;
using System.Collections.Generic;
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

		private static readonly string PressIfNoArgs =
			string.Concat("There are no args defined. Available parameters are:",
			              "\n- (h) this help",
			              "\n- (x/xml) register by unity config",
			              "\n- (c/conv) register by convention",
			              "\n- (m/mixed) register by combination of convention and unity config",
						  "\n- (a) to run with all options",
						  "\n- any other key to Quit");

		private static void Main(params string[] args)
		{
			if (args == null || args.Length == 0 || !args.Any(a => GetAllAllowedArgs().Contains(a)))
			{
				bool repeat;

				do
				{
					ConsoleKeyInfo keyInfo;
					repeat = PressToContinue(out keyInfo);
					switch (keyInfo.Key)
					{
						case ConsoleKey.A:
							args = GetDefaultArgs();
							break;
						case ConsoleKey.C:
							args = new[] {"c"};
							break;
						case ConsoleKey.M:
							args = new[] {"m"};
							break;
						case ConsoleKey.X:
							args = new[] {"x"};
							break;
						case ConsoleKey.H:
							repeat = true;
							break;
						default:
							return;
					}
				} while ((repeat));
			}

			if (args.Contains("a"))
				args = GetDefaultArgs();

			foreach (var arg in args)
			{
				IUnityContainer container = new UnityContainer();
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
					foreach (var reg in container.Registrations)
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

				Console.WriteLine("Registrations count: {0}", container.Registrations.Count());

				PressToContinue();
			}
		}

		private static string[] GetDefaultArgs()
		{
			return new[] { "c", "m", "x" };
		}

		private static string[] GetAllAllowedArgs()
		{
			return GetDefaultArgs().Union(new[] {"a"}).ToArray();
		}

		private static void SetContainer_ByConvention(IUnityContainer container)
		{
			container.RegisterTypes(AllClasses.FromAssembliesInBasePath(),
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

		private static bool PressToContinue(out ConsoleKeyInfo keyPressed)
		{
				Console.WriteLine(PressIfNoArgs);
				keyPressed = Console.ReadKey(true);
				return ValidInputForPressToContinue(keyPressed);
		}

		private static void PressToContinue()
		{
			Console.WriteLine(PressAnyKey);
			Console.ReadKey(true);
		}

		private static bool ValidInputForPressToContinue(ConsoleKeyInfo readKey)
		{
			return readKey.Equals(new ConsoleKeyInfo('h', ConsoleKey.H, false, false, false)) ||
			       readKey.Equals(new ConsoleKeyInfo('a', ConsoleKey.H, false, false, false)) ||
			       readKey.Equals(new ConsoleKeyInfo('x', ConsoleKey.H, false, false, false)) ||
			       readKey.Equals(new ConsoleKeyInfo('c', ConsoleKey.H, false, false, false)) ||
			       readKey.Equals(new ConsoleKeyInfo('m', ConsoleKey.H, false, false, false));
		}
	}
}