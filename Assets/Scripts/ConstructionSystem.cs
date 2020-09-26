﻿using UnityEngine;

public static class ConstructionSystem
{
	private static void InitializeConstruction (GameObject entity)
	{
		string entityId = WorldMapManager.GetEntityIdForObject(entity, SceneObjectManager.GetSceneIdForObject(entity));
		EntityData entityData = ContentLibrary.Instance.Entities.Get(entityId);

		ConstructionProgressTracker tracker = entity.GetComponent<ConstructionProgressTracker>();
		if (tracker == null)
		{
			tracker = entity.AddComponent<ConstructionProgressTracker>();
		}
		tracker.totalIngredientList = entityData.constructionIngredients;
		tracker.remainingIngredientList = tracker.totalIngredientList;
	}

	public static void AddIngredientToConstructableEntity (GameObject entity, ItemData Ingredient)
	{
		string entityId = WorldMapManager.GetEntityIdForObject(entity, SceneObjectManager.GetSceneIdForObject(entity));
		ConstructionProgressTracker constructionProgress = entity.GetComponent<ConstructionProgressTracker>();
	}

	private static void SetEntityToConstructionStage (GameObject entity, string entityId, float constructionCompleteness)
	{
		SpriteRenderer spriteRenderer = entity.GetComponentInChildren<SpriteRenderer>();
		if (spriteRenderer == null)
			return;
		Mathf.Clamp(constructionCompleteness, 0, 1);

		EntityData entityData = ContentLibrary.Instance.Entities.Get(entityId);
		int currentStage;
		currentStage = (Mathf.FloorToInt(constructionCompleteness * entityData.constructionStagesPrefabs.Count));

		// TODO replace gameobject with correct stage prefab
	}
}