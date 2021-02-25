using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public float speed;

    public Slider slider;
    public Slider effectSlider;

    private float prevValue = 0f;
    private float maxValue = 0f;
    private float value = 0f;

    private float lerpTimer;

	public void setMaxValue(float maxValue)
    {
        if (slider == null || effectSlider == null) return;

        slider.maxValue = maxValue;
        slider.value = maxValue;

        effectSlider.maxValue = maxValue;
        effectSlider.value = maxValue;

        prevValue = maxValue;
        this.maxValue = maxValue;
        this.value = maxValue;

        lerpTimer = 0f;
    }

    public void setValue(float value)
	{
        if (slider == null || effectSlider == null) return;
        value = Mathf.Clamp(value, 0, maxValue);

        prevValue = this.value;
        this.value = value;

        lerpTimer = 0f;
	}

	private void Update()
	{
        if (slider == null || effectSlider == null) return;
        if (value == prevValue) return;

        if(value < prevValue)
		{
            slider.value = value;
            lerpTimer += Time.time;
            float progress = lerpTimer / speed;
            effectSlider.value = Mathf.Lerp(prevValue, value, progress);
            if (effectSlider.value == value) prevValue = value;
		}
        else
		{
            effectSlider.value = value;
            lerpTimer += Time.time;
            float progress = lerpTimer / speed;
            slider.value = Mathf.Lerp(prevValue, value, progress);
            if (slider.value == value) prevValue = value;
        }
	}
}
