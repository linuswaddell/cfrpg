using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{
	[SerializeField] private Image notificationPanel = null;
	[SerializeField] private TextMeshProUGUI notificationText = null;

	private const float NotificationLifetime = 2.25f; // (not including fade time)
	private const float FadeTime = 0.8f;
	private const float FadeInTime = 0.1f; // The time to reappear

	private static NotificationManager instance;

	// Start is called before the first frame update
	private void Start()
	{
		instance = this;
		SetVisible(false);
	}

	public static void Notify(string text)
	{
		if (instance == null)
		{
			Debug.LogWarning("Can't activate notification; no instance of NotificationManager found.");
			return;
		}
		instance.StopAllCoroutines();
		SetText(text);
		SetVisible(true);
		instance.StartCoroutine(WaitAndFade());
	}

	private static void SetVisible(bool visible)
	{
		if (instance == null)
		{
			Debug.LogWarning("No instance of NotificationManager found.");
			return;
		}
		if (visible)
		{
			instance.notificationPanel.CrossFadeAlpha(1, FadeInTime, true);
			instance.notificationText.CrossFadeAlpha(1, FadeInTime, true);
		}
		else
		{
			instance.notificationPanel.CrossFadeAlpha(0, 0, true);
			instance.notificationText.CrossFadeAlpha(0, 0, true);
		}
	}

	private static void SetText(string text)
	{
		if (instance == null)
		{
			Debug.LogWarning("No instance of NotificationManager found.");
			return;
		}
		instance.notificationText.text = text;
	}

	private static IEnumerator WaitAndFade()
	{
		float start = Time.time;
		while (Time.time - start < NotificationLifetime)
			yield return null;
		instance.notificationPanel.CrossFadeAlpha(0f, FadeTime, true);
		instance.notificationText.CrossFadeAlpha(0f, FadeTime, true);
	}
}
