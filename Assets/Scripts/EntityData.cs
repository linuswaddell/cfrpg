﻿using System.Collections.Generic;
using UnityEngine;

// Describes any object that can be placed on a tile (buildings, plants, boxes, etc.)
[System.Serializable]
public class EntityData
{
	public string entityId; 
	public string entityName;
    public EntityCategory category;

	// Defines any object that takes up space on the tile map
	public GameObject entityPrefab;

	// A prefab for each stage of construction this entity goes through
	public List<GameObject> constructionStagesPrefabs;

	// This should be true of non-blocky objects like plants, fences, etc. (necessary for proper sprite sorting).
	// Take note that this assumes the pivot will always be in the origin tile of multi-tile objects.
	public bool pivotAtCenterOfTile = false;

	// Whether colonists can build this entity
	public bool isConstructable = false;

	// How long it takes to go from the first construction stage (if one exists) to the final entity
	public float constructionTimeMinutes = 0f;

	// Resources required to initially place this entity
	public List<CraftingIngredient> initialCraftingIngredients = new List<CraftingIngredient>();

	// Resources needed to complete construction of this entity after it is placed
	public List<CraftingIngredient> constructionIngredients = new List<CraftingIngredient>();

	// Whether you can just build something over this (should be true of weeds, etc.)
	public bool canBeBuiltOver = false;

	// Determines whether Actors will view the occupied tile as navigable
	public bool canBeWalkedThrough = false;

	// How undesirable this tile is to walk through (if it's a bush for example)
	public float extraTraversalCost = 0f;

	// Defines what tiles the entity covers
	public List<Vector2Int> baseShape = new List<Vector2Int>{new Vector2Int(0,0)};


	[System.Serializable]
	public struct CraftingIngredient {
		public string itemId;
		public int quantity;
	}
}