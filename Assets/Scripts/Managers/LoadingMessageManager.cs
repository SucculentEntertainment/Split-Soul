using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMessages
{
	public List<string> items = new List<string>();
}

public class LoadingMessageManager : MonoBehaviour
{
    public TextAsset messageFile;
	public Text label;

	private LoadingMessages messages;

	private void Start() { messages = JsonUtility.FromJson<LoadingMessages>(messageFile.text); }
	public void OnEnable()
	{
		int index = Random.Range(0, messages.items.Count - 1);
		if(index == null) index = 0;

		label.text = messages.items[index];
	}
}
