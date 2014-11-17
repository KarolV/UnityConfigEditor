using System;

namespace SourceApp.Interface
{
	public interface ITestObject
	{
		/// <summary>
		/// Unique identifier
		/// </summary>
		Guid ID { get; }

		/// <summary>
		/// Any meanigless text
		/// </summary>
		string Text { get; }

		/// <summary>
		/// Set the text of the object
		/// </summary>
		/// <param name="text"></param>
		void SetText(string text);
	}
}