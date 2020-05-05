﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName="NewItem", menuName = "Items/Base Item", order = 1)]
public class Item : ScriptableObject {
	

	[SerializeField] private string itemName = null;
	[SerializeField] private string itemId = null;
	[SerializeField] private Sprite itemIcon = null;
	[SerializeField] private bool isEdible = false;
	[SerializeField] private float nutritionalValue = 0.25f;

	public string ItemName {get {return itemName;}}
	public string ItemId {get {return itemId;}}
	public Sprite ItemIcon {get {return itemIcon;}}
	public bool IsEdible {get {return isEdible;}}
	public float NutritionalValue {get {return nutritionalValue;}}

	public string GetItemName() {
		return itemName;
	}

	public string GetItemId() {
		return itemId;
	}

	public Sprite getIconSprite() {
		return itemIcon;
	}
}
