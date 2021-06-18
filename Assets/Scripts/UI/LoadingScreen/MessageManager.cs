using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SplitSoul.UI.LoadingScreen
{
	public class Messages
	{
		public List<string> items = new List<string>();
	}

	public class MessageManager : MonoBehaviour
	{
		public TextAsset messageFile;
		public Text label;

		private Messages messages;

		private void Start() { messages = JsonUtility.FromJson<Messages>(messageFile.text); }
		public void OnEnable()
		{
			int index = 0;
			index = Random.Range(0, messages.items.Count - 1);

			label.text = messages.items[index];
		}
	}
}
