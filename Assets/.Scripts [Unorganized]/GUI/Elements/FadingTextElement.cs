using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingTextElement : MonoBehaviour
{
	[Header("Text parameters")]
	public string text;
	public Gradient gradient;
	public Font font;

	[Header("Animation parameters")]
	public float lifetime;
	public float fadeDuration;

	[Header("References")]
	public Text textObject;

    private void Start()
    {
		textObject.text = text;
		textObject.font = font;
		textObject.color = gradient.Evaluate(0);

		StartCoroutine(waitLifetime(lifetime, fadeDuration));
    }

	IEnumerator waitLifetime(float lifetime, float fadeDuration)
	{
		yield return new WaitForSeconds(lifetime);
		StartCoroutine(fadeOut(fadeDuration));
	}

	IEnumerator fadeOut(float duration)
	{
		float timePassed = 0;

		while(true)
		{
			timePassed += Time.deltaTime;
			if(timePassed >= duration) break;
			float val = timePassed / duration;

			textObject.color = gradient.Evaluate(val);

			yield return null;
		}

		Destroy(this.gameObject);
		yield return null;
	}
}
