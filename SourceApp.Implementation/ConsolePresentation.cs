using System;

using SourceApp.Interface;

namespace SourceApp.Implementation
{
	public sealed class ConsolePresentation : IPresentation
	{
		public ConsolePresentation(ITestObject content)
		{
			Content = content;
		}

		public ITestObject Content { get; private set; }

		#region Implementation of IPresentation

		public bool Load()
		{
			this.Content.SetText(GetInputResult(Message));

			return this.Content != null && string.IsNullOrEmpty(this.Content.Text);
		}

		public bool Load(string arg)
		{
			this.Content.SetText(string.IsNullOrEmpty(arg)
				? GetInputResult(Message)
				: GetInputResult(arg));

			return this.Content == null || !string.IsNullOrEmpty(this.Content.ToString());
		}

		public void GetContent()
		{
			Console.WriteLine("Content[{1}]: {0}", this.Content, this.Content.GetType());
		}

		public void GetContent(string format, params object[] args)
		{
			try
			{
				Console.WriteLine(format, args);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		#endregion

		#region Private Constants, Fields & Methods

		private const string Message = "Insert text:";

		private static string GetInputResult(string message)
		{
			Console.Write(message);
			return Console.ReadLine();
		}

		#endregion
	}
}