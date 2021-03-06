﻿using System.Collections.Generic;
using UnityEngine;

public class TileMarkerController : MonoBehaviour
{
	[SerializeField] private TileMarker tileMarkerPrefab;

	private static List<GameObject> currentMarkers;
	private static TileMarkerController instance;

    // Start is called before the first frame update
    private void Start()
    {
		instance = this;
		currentMarkers = new List<GameObject> ();
		SceneChangeActivator.OnSceneExit += ClearMarkerList;
    }
	public static void SetTileMarker (Vector2Int worldPosition) {
		HideTileMarkers ();
		if (currentMarkers.Count == 0) {
			currentMarkers.Add (CreateTileMarker ());
		}
		currentMarkers [0].transform.position = new Vector3 (worldPosition.x, worldPosition.y);
		currentMarkers [0].SetActive (true);
	}
	public static void SetTileMarkers (List<Vector2Int> worldPositions) {
		for (int i = 0; i < worldPositions.Count; i++) {
			if (currentMarkers.Count < i + 1) {
				currentMarkers.Add (CreateTileMarker ());
			}
			currentMarkers [i].transform.position = new Vector3 (worldPositions [i].x, worldPositions [i].y);
			currentMarkers [i].SetActive (true);
		}
	}
	public static void HideTileMarkers () {
		if (currentMarkers == null)
			currentMarkers = new List<GameObject> ();
		foreach (GameObject marker in currentMarkers) {
			marker.SetActive (false);
		}
	}

	private static GameObject CreateTileMarker () {
		GameObject marker = GameObject.Instantiate (instance.tileMarkerPrefab.gameObject);
		return marker;
	}

	private static void ClearMarkerList ()
	{
		currentMarkers = null;
	}
}
