﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTileMarker : MonoBehaviour {

	static SelectedTileMarker instance;
	SpriteRenderer renderer;
	bool isFollowingMouse;

	public static bool IsFollowingMouse{get{return instance.isFollowingMouse;}}

	// Use this for initialization
	void Awake () {
		instance = this;
		renderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		if (isFollowingMouse) {
			Vector3 inputPos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f));
			int gridX = Mathf.FloorToInt (inputPos.x);
			int gridY = Mathf.FloorToInt (inputPos.y);
			MoveTo (gridX, gridY);
			if (Input.GetMouseButtonDown (0))
				Debug.Log (TilemapInterface.GetTileAtWorldPosition (gridX, gridY));
		}
	}

	public static void MoveTo (int x, int y) {
		instance.transform.position = new Vector3Int (x, y, 0);
	}

	public static void SetVisible (bool visible) {
		instance.renderer.enabled = visible;
	}

	public static void SetFollowMouse (bool followMouse) {
		instance.isFollowingMouse = followMouse;
	}

	public static Vector3Int CurrentPosition {
		get {
			return new Vector3Int ((int)instance.transform.position.x, (int)instance.transform.position.y, 0);
		}
	}
}
