namespace SourceApp.Interface
{
	public interface IManager
	{
		void SetTestObject(ITestObject testObject);

		ITestObject GetTestObject();
	}
}