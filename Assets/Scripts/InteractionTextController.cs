﻿using UnityEngine;
using TMPro;

public class InteractionTextController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text = null;
	private PickupDetector detector = null;

    // Update is called once per frame
    private void Update()
    {
		if (detector == null) {
			if (ActorRegistry.Get(PlayerController.PlayerActorId) == null)
				return;
			detector = ActorRegistry.Get(PlayerController.PlayerActorId).actorObject.GetComponent<PickupDetector> ();
		}

		IPickuppable currentDetectedObject = detector.GetCurrentDetectedItem ();

		if (currentDetectedObject != null)
		{
			text.text = "Pick up " + currentDetectedObject.ItemPickup.GetData().ItemName;
			if (currentDetectedObject.ItemPickup.quantity > 1)
			{
				text.text += " (" + currentDetectedObject.ItemPickup.quantity + ")";
			}
		}
		else {
			text.text = null;
		}
    }
		
}