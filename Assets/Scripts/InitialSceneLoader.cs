﻿using System.Collections;
using UnityEngine;

// Responsible for loading in the main scene and any (TODO) existing interior scenes
// at the start of the game.
public class InitialSceneLoader : MonoBehaviour
{
	public delegate void SceneLoadedEvent();
	public static event SceneLoadedEvent OnInitialScenesLoaded;
	private static string ManagerSceneName = "Main";

	private void OnDestroy ()
	{
		OnInitialScenesLoaded = null;
	}

	public static void LoadScenes () {
		InitialSceneLoader instance = GameObject.FindObjectOfType<InitialSceneLoader> ();
		IEnumerator coroutine = instance.LoadScenesCoroutine (null);
		instance.StartCoroutine (coroutine);
	}
	public static void LoadScenes(SceneLoadedEvent callback) {
		InitialSceneLoader instance = GameObject.FindObjectOfType<InitialSceneLoader> ();
		IEnumerator coroutine = instance.LoadScenesCoroutine (callback);
		instance.StartCoroutine (coroutine);
	}

	private IEnumerator LoadScenesCoroutine (SceneLoadedEvent callback) {
		SceneObjectManager.CreateNewSceneFromPrefab (SceneObjectManager.WorldSceneId);
		if (OnInitialScenesLoaded != null) {
			OnInitialScenesLoaded ();
			Debug.Log ("scenes loaded");
		}
		callback?.Invoke();
		yield return null;
	}
}
