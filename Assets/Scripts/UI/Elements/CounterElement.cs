using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SplitSoul.UI.Elements
{
	public class CoutnerElement : MonoBehaviour
	{
		private Text text;
		public int value = 0;

		private void Start()
		{
			text = transform.Find("Text").GetComponent<Text>();
		}

		public void setValue(int value)
		{
			this.value = value;
			text.text = value.ToString();
		}
	}
}
