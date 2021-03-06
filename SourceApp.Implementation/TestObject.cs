﻿using System;

using Microsoft.Practices.Unity;

using SourceApp.Interface;

namespace SourceApp.Implementation
{
	public sealed class TestObject : ITestObject
	{
		private string _text;

		[InjectionConstructor]
		public TestObject(Guid id)
		{
			ID = id;
		}

		public TestObject()
		{
			this.ID = Guid.NewGuid();
		}

		#region Implementation of ITestObject

		/// <summary>
		/// Unique identifier
		/// </summary>
		public Guid ID { get; private set; }

		/// <summary>
		/// Any meanigless text
		/// </summary>
		public string Text
		{
			get { return _text ?? (_text = this.ID.ToString()); }
			private set { _text = value; }
		}

		/// <summary>
		/// Set the text of the object
		/// </summary>
		/// <param name="text"></param>
		public void SetText(string text)
		{
			this.Text = text;
		}

		#endregion

		#region Overrides of Object

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return string.Format("Instance[{0}]: \"{1}\"", this.ID, this.Text);
		}

		#endregion
	}
}