using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{

	[SerializeField] private Image notificationPanel;
	[SerializeField] private TextMeshProUGUI notificationText;

	private const float NotificationLifetime = 1.5f;
	private const float FadeTime = 0.8f;

	private static NotificationManager instance;
	private static Color visiblePanelColor;
	private static Color visibleTextColor;

    // Start is called before the first frame update
    private void Start()
    {
		instance = this;
		visiblePanelColor = notificationPanel.color;
		visibleTextColor = notificationText.color;
		SetVisible (false);
    }

	public static void Notify (string text) {
		if (instance == null)
		{
			Debug.LogWarning("Can't activate notification; no instance of NotificationManager found.");
			return;
		}
		instance.StopAllCoroutines ();
		SetText (text);
		SetVisible (true);
		instance.StartCoroutine (WaitAndFade());
	}

	private static void SetVisible (bool visible) {
		if (instance == null)
		{
			Debug.LogWarning("No instance of NotificationManager found.");
			return;
		}
		if (visible) {
			instance.notificationPanel.color = visiblePanelColor;
			instance.notificationText.color = visibleTextColor;
		} else {
			instance.notificationPanel.color = Color.clear;
			instance.notificationText.color = Color.clear;
		}
	}

	private static void SetText (string text) {
		if (instance == null)
		{
			Debug.LogWarning("No instance of NotificationManager found.");
			return;
		}
		instance.notificationText.text = text;
	}

	private static IEnumerator WaitAndFade () {
		float start = Time.time;
		while (Time.time - start < NotificationLifetime)
			yield return null;
		instance.notificationPanel.CrossFadeAlpha (0f, FadeTime, true);
		instance.notificationText.CrossFadeAlpha (0f, FadeTime, true);
	}
}
