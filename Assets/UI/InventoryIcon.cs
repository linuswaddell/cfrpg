using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// For displaying items that can't actually be dragged, as in the hotbar
public class InventoryIcon : MonoBehaviour {

	protected Image renderer;

	private void Start () {
		renderer = GetComponent<Image> ();
	}

	public void SetVisible (bool enabled) {
		if (renderer == null)
			renderer = GetComponent<Image> ();
		if (enabled) {
			this.gameObject.SetActive (true);
			renderer.color = new Color(255, 255, 255, 1);
		} else {
			if (renderer == null)
				return;
			renderer.color = new Color(255, 255, 255, 0);
		}
	}
}
