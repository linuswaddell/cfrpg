﻿using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DaylightController : MonoBehaviour
{
	[SerializeField] private GameObject sunPrefab = null;
	[SerializeField] private AnimationCurve brightnessCurve = null;
	[SerializeField] private AnimationCurve redCurve = null;
	[SerializeField] private AnimationCurve greenCurve = null;
	[SerializeField] private AnimationCurve blueCurve = null;
	private GameObject lightObject;
	private Light2D sunLight;
	private const float PEAK_INTENSITY = 1f;
	private const float MIN_INTENSITY = 0.5f;
	private static DaylightController instance;

	// Start is called before the first frame update
	private void Start()
    {
        if (sunLight == null)
		{
			CreateSunLightObject();
		}
		instance = this;
    }

    public static float IntensityAsFraction
    {
        get
        {
			float intensityRange = PEAK_INTENSITY - MIN_INTENSITY;
			float differenceFromMin = instance.sunLight.intensity - MIN_INTENSITY;
			return differenceFromMin / intensityRange;
        }
    }

	// Update is called once per frame
	private void Update()
	{
		float time = TimeKeeper.TimeAsFraction;
		sunLight.intensity = brightnessCurve.Evaluate(time) * (PEAK_INTENSITY - MIN_INTENSITY) + MIN_INTENSITY;

		float r = redCurve.Evaluate(time);
		float g = greenCurve.Evaluate(time);
		float b = blueCurve.Evaluate(time);
		sunLight.color = new Color(r, g, b);
    }

	private void CreateSunLightObject ()
	{
		lightObject = GameObject.Instantiate(sunPrefab);
		lightObject.name = "Sun";
		sunLight = lightObject.GetComponent<Light2D>();
		sunLight.lightType = Light2D.LightType.Global;
		sunLight.intensity = PEAK_INTENSITY;
		sunLight.shadowIntensity = 0;
	}
}
