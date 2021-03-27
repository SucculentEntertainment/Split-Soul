using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    // ================================
    //  Parameters
    // ================================

    // --------------------------------
    //  Parameters -> Public
    // --------------------------------

    [Header("Cycle Parameter")]
    [Tooltip("Specifies the amount of seconds per full cycle (Midnight to Midnight)")]
    public int cycleDuration = 60;

    public Gradient colorOverTime;

    [Header("Light Parameters")]
    public float maxSunIntensity = 0.4f;
    public float maxMoonIntensity = 0.125f;

    public AnimationCurve sunIntensity;
    public AnimationCurve moonIntensity;

    [Header("References")]
    public Light2D sun;
    public Light2D moon;
    public Light2D globalLight;

    public Transform sunRig;
    public Transform moonRig;

    // --------------------------------
    //  Parameters -> Internal
    // --------------------------------

    private float cycleTimer = 0f;

    // ================================
    //  Functions
    // ================================

    void Update()
    {
        cycleTimer += Time.deltaTime;
        if(cycleTimer >= cycleDuration) cycleTimer -= cycleDuration;

        float t = cycleTimer / cycleDuration;

        globalLight.color = colorOverTime.Evaluate(t);

        sun.intensity = sunIntensity.Evaluate(t) * maxSunIntensity;
        moon.intensity = moonIntensity.Evaluate(t) * maxMoonIntensity;

        sunRig.rotation = Quaternion.Euler(0, 0, t * -360);
        moonRig.rotation = Quaternion.Euler(0, 0, t * -360 - 180);
    }
}
