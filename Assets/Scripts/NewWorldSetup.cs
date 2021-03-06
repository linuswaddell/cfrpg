﻿using UnityEngine;

// Contains methods for setting up a new world after it is loaded for the first time
public static class NewWorldSetup
{
	public static void PerformSetup()
	{
		// Spawn the newly created player
		ActorData playerData = SaveInfo.NewlyCreatedPlayer;
		if (playerData == null)
		{
			Debug.LogError("No player data found for new world!");
		}
		else
		{
			Vector2 spawnPoint = ActorSpawnpointFinder.FindSpawnPointNearCoords(
				RegionMapManager.GetRegionMap(),
				SceneObjectManager.WorldSceneId,
				(SaveInfo.RegionSize / 2) + Vector2.down * 5);
			
			ActorRegistry.Register(playerData);
			Actor player = ActorSpawner.Spawn(playerData.actorId, spawnPoint, SceneObjectManager.WorldSceneId);
			PlayerController.SetPlayerActor(playerData.actorId);
		}
	}
}
