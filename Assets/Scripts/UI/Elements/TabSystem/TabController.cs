using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitSoul.UI.Elements.TabSystem
{
	[SerializableAttribute]
	public class TabGroup
	{
		public string id;
		public List<GameObject> objects;
	}

	public class TabController : MonoBehaviour
	{
		public string defaultTab;
		public List<TabGroup> tabGroups;

		private List<TabElement> tabElements;
		private string activeTab;

		private void Start()
		{
			activeTab = defaultTab;
			activateTab(activeTab);
		}

		public void register(TabElement element)
		{
			if (tabElements == null) tabElements = new List<TabElement>();
			tabElements.Add(element);
		}

		public void resetTabs()
		{
			for (int i = 0; i < tabGroups.Count; i++)
			{
				for (int j = 0; j < tabGroups[i].objects.Count; j++)
				{
					tabGroups[i].objects[j].SetActive(false);
				}
			}
		}

		public void activateTab(string id)
		{
			resetTabs();
			if (id == "") return;

			TabGroup activeGroup = tabGroups.Find(x => x.id == id);
			for (int i = 0; i < activeGroup.objects.Count; i++) { activeGroup.objects[i].SetActive(true); }
		}

		public void OnElementClick(TabElement element)
		{
			if (activeTab == element.id) return;

			activeTab = element.id;
			activateTab(activeTab);
		}
	}
}
