using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    private Slider slider;
    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void setMaxValue(float maxValue)
    {
        if (slider == null) return;
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void setValue(float value)
	{
        if (slider == null) return;
        slider.value = value;
	}
}
