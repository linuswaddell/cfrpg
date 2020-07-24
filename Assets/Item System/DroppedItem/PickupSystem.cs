﻿using System.Runtime.InteropServices;
using UnityEngine;

public class PickupSystem
{
	public static bool AttemptPickup (Actor actor, IPickuppable itemObject) 
	{
		Item item = itemObject.ItemPickup;

		if (item == null) {
			return false;
		}
		
		if (actor.GetData().Inventory.AttemptAddItemToInv(item)) {
			GameObject.Destroy (((MonoBehaviour)itemObject).gameObject);
			return true;
		}

		return false;
	}
}