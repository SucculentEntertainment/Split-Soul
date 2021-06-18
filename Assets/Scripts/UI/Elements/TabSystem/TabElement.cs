using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SplitSoul.UI.Elements.TabSystem
{
	[RequireComponent(typeof(Button))]
	public class TabElement : MonoBehaviour
	{
		public TabController tabController;
		public string id;
		private Button btn;

		private void Start()
		{
			tabController.register(this);

			btn = gameObject.GetComponent<Button>();
			btn.onClick.AddListener(() => OnBtnClick());
		}

		private void OnBtnClick() { tabController.OnElementClick(this); }
	}
}
