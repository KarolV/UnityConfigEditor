using SourceApp.Interface;

namespace SourceApp.Implementation
{
	public sealed class Manager : IManager
	{
		private IPresentation Presentation { get; set; }

		public Manager(IPresentation presentation)
		{
			this.Presentation = presentation;
		}

		#region Implementation of IManager

		public IPresentation GetPresentation()
		{
			return this.Presentation;
		}

		#endregion
	}
}