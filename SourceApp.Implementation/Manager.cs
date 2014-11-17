using SourceApp.Interface;

namespace SourceApp.Implementation
{
	public sealed class Manager : IManager
	{
		private ITestObject TestObject { get; set; }

		public Manager(ITestObject testObject)
		{
			this.SetTestObject(testObject);
		}

		#region Implementation of IManager

		public void SetTestObject(ITestObject testObject)
		{
			this.TestObject = testObject;
		}

		public ITestObject GetTestObject()
		{
			return this.TestObject;
		}

		#endregion
	}
}