using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SourceApp.Implementation;
using SourceApp.Interface;

namespace UnityConfigEditor.Test
{
	[TestClass]
	public class AssemblyModelTest
	{
		private IUnityContainer _container;
		private IEnumerable<string> _excludedContainerNames;

		[TestInitialize]
		public void TestInitialize()
		{
			_excludedContainerNames = new List<string> {"IUnityContainer"};

		}

		[TestMethod]
		public void RegisterClass_InCode_Test()
		{
			_container = new UnityContainer();
			_container.RegisterType<ITestObject, TestObject>()
			          .RegisterType<IManager, Manager>()
			          .RegisterType<ITestObject, TestObject>(new InjectionConstructor(Guid.NewGuid()));

			const int expectedCount = 2;

			CommonEvaluate(expectedCount);
		}

		[TestMethod]
		public void RegisterClass_ByConvention_Test()
		{
			_container = new UnityContainer();
			_container.RegisterTypes(AllClasses.FromLoadedAssemblies(),
			                         WithMappings.FromMatchingInterface,
			                         WithName.Default);

			const int expectedCount = 0;
			CommonEvaluate(expectedCount);
		}

		private void CommonEvaluate(int expectedCount)
		{
			Assert.AreEqual(expectedCount,
			                _container.Registrations
			                          .Count(r => _excludedContainerNames
				                                 .All(x => x != r.MappedToType.Name)));

			foreach (var registration in 
				_container.Registrations
				          .Where(r => _excludedContainerNames
					                 .All(x => x != r.MappedToType.Name)))
			{
				Assert.IsNotNull(registration, string.Format("Registration is null"));
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
			}
		}
	}
}