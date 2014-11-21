namespace SourceApp.Interface
{
	public interface IPresentation
	{
		ITestObject Content { get; }

		bool Load();

		bool Load(string arg);

		void GetContent();

		void GetContent(string format, params object[] args);
	}
}