using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;

using UnityConfigEditor.Interface;

namespace UnityConfigEditor.DataModel.Base
{
	/// <summary>
	/// Base class for model object instances
	/// </summary>
	/// <typeparam name="TItem">The Item member</typeparam>
	/// <typeparam name="TCollMember">The collection of sub-item members</typeparam>
	public abstract class ModelBase<TItem, TCollMember> : IModel<TItem, TCollMember>
		where TCollMember : IModel<TItem, TCollMember>
	{
		#region Implementation of IModel

		/// <summary>
		/// Load all sub-items
		/// </summary>
		public void LoadItems()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Clear all sub-items
		/// </summary>
		public void Clear()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get the string representation of the <see cref="IModel{TItem,TCollMember}"/> instance
		/// </summary>
		/// <returns>The string representation of the <see cref="IModel{TItem,TCollMember}"/></returns>
		public abstract Func<string> CreateString();

		/// <summary>
		/// Get the <see cref="XmlNode"/> representation of the <see cref="IModel{TItem,TCollMember}"/> instance
		/// </summary>
		/// <returns></returns>
		public abstract Func<XmlNode> CreateXmlNode();

		public event PropertyChangedEventHandler ItemChanged;

		#endregion

		#region Public properties

		/// <summary>
		/// Get or set the instance of <see cref="TItem"/> object
		/// </summary>
		public TItem Item
		{
			get { return _item; }
			set
			{
				if (_item.Equals(value)) return;
				_item = value;
				this.Clear();
				this.OnItemChanged();
			}
		}

		/// <summary>
		/// Get or set the string name of the <see cref="TItem"/> object
		/// </summary>
		public string ItemName
		{
			get
			{
				return string.IsNullOrEmpty(_itemName)
					? _item.GetType().Name
					: _itemName;
			}
			set { _itemName = value; }
		}

		/// <summary>
		/// Get the enumerable collection of <see cref="TCollMember"/> objects
		/// </summary>
		public IEnumerable<TCollMember> SubItemCollection
		{
			get
			{
				if (_collection != null) return _collection;
				_collection = new HashSet<TCollMember>();
				this.LoadItems();
				return _collection;
			}
			protected set { _collection = value; }
		}

		public IEnumerable<string> SubItemNameCollection
		{
			get { return SubItemCollection.Select(c => c.ItemName); }
		}

		/// <summary>
		/// Get or set value whether include the <see cref="TItem"/> object to perform action on
		/// </summary>
		public bool IsSelected { get; set; }

		#endregion

		#region Private constants, fields

		private const string ItemObjectName = "Item";

		private TItem _item;
		private IEnumerable<TCollMember> _collection;
		private string _itemName;

		#endregion

		#region Private methods

		private void OnItemChanged()
		{
			if (this.ItemChanged == null) return;
			this.ItemChanged(this, new PropertyChangedEventArgs(ItemObjectName));
		}

		#endregion
	}
}