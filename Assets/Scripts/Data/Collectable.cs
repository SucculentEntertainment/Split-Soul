using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SplitSoul.Data.Scriptable.Inventory;

namespace SplitSoul.Data
{
	public class Collectable
	{
		private string _id;
		private int _amount;
		private Item _item;

		public string id
		{
			get { return _id; }
			set { _id = value; }
		}

		public int amount
		{
			get { return _amount; }
			set { _amount = value; }
		}

		public Item item
		{
			get { return _item; }
			set { _item = value; }
		}

		public Collectable(string id, int amount = 1, Item item = null)
		{
			this.id = id;
			this.amount = amount;
			this.item = item;
		}
	}
}
