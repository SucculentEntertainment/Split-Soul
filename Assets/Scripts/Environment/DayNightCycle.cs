using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using SplitSoul.Core;

namespace SplitSoul.Environment
{
	public class DayNightCycle : MonoBehaviour
	{
		// ================================
		//  Parameters
		// ================================

		// --------------------------------
		//  Parameters -> Public
		// --------------------------------

		[Header("Cycle Parameter")]
		[Tooltip("Specifies the amount of seconds per full cycle (Sunrise to Sunrise)")]
		public float cycleDuration = 60;

		[Tooltip("Specifies the color over time per full cycle (Sunrise to Sunrise)")]
		public Gradient colorOverTime;

		[Header("Light Parameters")]
		public float maxSunIntensity = 2f;
		public float maxMoonIntensity = 1f;

		public AnimationCurve sunIntensity;
		public AnimationCurve moonIntensity;

		[Header("Lights")]
		[Tooltip("Maximum time until every light is on / off")]
		public float maxDelay = 1f;

		[Tooltip("Time at which lamps turn on")]
		[Range(0f, 1f)]
		public float onTime = 0.5f;
		[Tooltip("Time at which lamps turn off")]
		[Range(0f, 1f)]
		public float offTime = 1f;

		[Tooltip("List of lights to be turned on/off")]
		[SerializeField] public List<Light2D> lights;

		[Header("Events")]
		[Range(0f, 1f)]
		public float sunriseTime = 0f;
		[Range(0f, 1f)]
		public float dayTime = 0.1f;
		[Range(0f, 1f)]
		public float sunsetTime = 0.4f;
		[Range(0f, 1f)]
		public float nightTime = 0.6f;

		[Header("References")]
		public Light2D sun;
		public Light2D moon;
		public Light2D globalLight;

		public Transform rig;

		// --------------------------------
		//  Parameters -> Internal
		// --------------------------------

		private float cycleTimer = 0f;
		private bool lightState = false;
		private bool changedLights = false;

		// ================================
		//  Functions
		// ================================

		private void Start()
		{
			rig.rotation = Quaternion.Euler(0, 0, 0);
		}

		private void Update()
		{
			//Advance Timer
			cycleTimer += Time.deltaTime;
			if (cycleTimer >= cycleDuration) cycleTimer -= cycleDuration;

			//Normalize time
			float t = cycleTimer / cycleDuration;

			//Set colors for current timestep
			globalLight.color = colorOverTime.Evaluate(t);

			//Set intensity for current timestep
			sun.intensity = sunIntensity.Evaluate(t) * maxSunIntensity;
			moon.intensity = moonIntensity.Evaluate(t) * maxMoonIntensity;

			//Rotate Sun and Moon rig according to current timestep
			rig.rotation = Quaternion.Euler(0, 0, t * -360);

			//Toggle lights
			float roundedT = (float)Math.Round(t, 2, MidpointRounding.AwayFromZero);
			if (roundedT == onTime || roundedT == offTime)
			{
				if (!changedLights)
				{
					changedLights = true;
					StartCoroutine(toggleLights());
				}
			}
			else changedLights = false;

			//Throw events
			if (roundedT == sunriseTime) GameEventSystem.current.Sunrise();
			if (roundedT == dayTime) GameEventSystem.current.DayTime();
			if (roundedT == sunsetTime) GameEventSystem.current.Sunset();
			if (roundedT == nightTime) GameEventSystem.current.NightTime();
		}

		// ================================
		//  Coroutines
		// ================================

		private IEnumerator toggleLights()
		{
			foreach (Light2D l in lights)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, maxDelay / lights.Count));
				if (l != null) l.gameObject.SetActive(!lightState);
			}

			lightState = !lightState;
		}

		// ================================
		//  Events
		// ================================

		private void OnLightRegister(Light2D light)
		{
			if (!lights.Contains(light)) lights.Add(light);
		}

		private void OnLightUnregister(Light2D light)
		{
			if (lights.Contains(light)) lights.Remove(light);
		}
	}
}
