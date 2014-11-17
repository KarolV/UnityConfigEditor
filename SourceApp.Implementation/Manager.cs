using SourceApp.Interface;

namespace SourceApp.Implementation
{
	public sealed class Manager
	{
		public ITestObject TestObject { get; private set; }

		public Manager(ITestObject testObject)
		{
			this.TestObject = testObject;
		}
	}
}