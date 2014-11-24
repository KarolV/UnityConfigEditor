using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SourceApp.Implementation;
using SourceApp.Interface;

namespace UnityConfigEditor.Test
{
	[TestClass]
	public class AssemblyModelTest
	{
		private IEnumerable<string> _excludedContainerNames;

		[TestInitialize]
		public void TestInitialize()
		{
			_excludedContainerNames = new List<string> {"IUnityContainer"};

		}

		[TestMethod]
		public void RegisterClass_InCode_Test()
		{
			var container = new UnityContainer();

			container.RegisterType<ITestObject, TestObject>("noparam")
			         .RegisterType<IPresentation, ConsolePresentation>("console")
					 .RegisterType<IManager, Manager>()
					 .RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()));

			const int expectedCount = 4;

			CommonEvaluate(container, expectedCount);
		}

		[TestMethod]
		public void RegisterClass_WithConfig_Test()
		{
			var container = new UnityContainer();

			container.LoadConfiguration("fullContainer");

			const int expectedCount = 4;

			CommonEvaluate(container, expectedCount);
		}

		[TestMethod]
		public void RegisterClass_ByConvention_Test()
		{
			var container = new UnityContainer();
			container.RegisterTypes(AllClasses.FromAssembliesInBasePath(),
			                        WithMappings.FromAllInterfaces,
			                        WithName.Default);

			container.RegisterType<ITestObject, TestObject>("GUID param", new InjectionConstructor(Guid.NewGuid()));

			const int expectedCount = 4;

			CommonEvaluate(container, expectedCount);
		}

		private void CommonEvaluate(IUnityContainer container, int expectedCount)
		{
			Assert.AreEqual(expectedCount,
			                container.Registrations
			                          .Count(r => _excludedContainerNames
				                                 .All(x => x != r.MappedToType.Name)));

			foreach (var registration in 
				container.Registrations
				          .Where(r => _excludedContainerNames
					                 .All(x => x != r.MappedToType.Name)))
			{
				Assert.IsNotNull(registration, string.Format("Registration is null"));
				Console.WriteLine("Registration name: {0}", (registration.Name ?? "[default]").ToUpperInvariant());

				var registeredType = registration.RegisteredType;
				Assert.IsNotNull(registeredType, string.Format("Registered type is null"));
				Console.WriteLine("Registered type");
				Console.WriteLine("\t- {0}", registeredType.Namespace);
				Console.WriteLine("\t- {0}", registeredType.Name);
				Console.WriteLine();

				var mappedToType = registration.MappedToType;
				Assert.IsNotNull(mappedToType, string.Format("Mapped to type is null"));
				Console.WriteLine("Mapped to type");
				Console.WriteLine("\t- {0}", mappedToType.Namespace);
				Console.WriteLine("\t- {0}", mappedToType.Name);
				Console.WriteLine();

				var lifetimeManagerType = registration.LifetimeManagerType;
				Assert.IsNotNull(lifetimeManagerType, string.Format("Lifetime manager type is null"));
				Console.WriteLine("Lifetime manager type");
				Console.WriteLine("\t- {0}", lifetimeManagerType.Namespace);
				Console.WriteLine("\t- {0}", lifetimeManagerType.Name);
				Console.WriteLine("* * * * * * * * * * *");
				Console.WriteLine(registration.GetMappingAsString());
				Console.WriteLine("* * * * * * * * * * *");
				Console.WriteLine("* * * * * * * * * * *");
			}
		}
	}
}