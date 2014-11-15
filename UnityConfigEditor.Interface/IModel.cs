using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace UnityConfigEditor.Interface
{
	public interface IModel<TItem, TCollMember>
	{
		#region Properties

		/// <summary>
		/// Get or set the instance of <see cref="TItem"/> object
		/// </summary>
		TItem Item { get; set; }

		/// <summary>
		/// Get or set the string name of the <see cref="TItem"/> object
		/// </summary>
		string ItemName { get; set; }

		/// <summary>
		/// Get the enumerable collection of <see cref="TCollMember"/> objects
		/// </summary>
		IEnumerable<TCollMember> SubItemCollection { get; }

		/// <summary>
		/// Get the enumerable collecton of the sub-item names
		/// </summary>
		IEnumerable<string> SubItemNameCollection { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Load all sub-items
		/// </summary>
		void LoadItems();

		/// <summary>
		/// Clear all sub-items
		/// </summary>
		void Clear();

		/// <summary>
		/// Get the string representation of the <see cref="IModel{TItem,TCollMember}"/> instance
		/// </summary>
		/// <returns>The string representation of the <see cref="IModel{TItem,TCollMember}"/></returns>
		Func<string> CreateString();

		/// <summary>
		/// Get the <see cref="XmlNode"/> representation of the <see cref="IModel{TItem,TCollMember}"/> instance
		/// </summary>
		/// <returns></returns>
		Func<XmlNode> CreateXmlNode();

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handler getting triggered when the Model Item has been changed
		/// </summary>
		event PropertyChangedEventHandler ItemChanged;

		#endregion
	}
}