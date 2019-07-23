﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorSpawnpointFinder
{
	const int maxSearchRadius = 50;

	public static Vector2 FindSpawnPoint (string scene)
	{
		return FindSpawnPointNearCoords(scene, new Vector2(0, 0));
	}

	public static Vector2 FindSpawnPointNearCoords (string scene, Vector2 coords)
	{
		for (int i = 0; i < maxSearchRadius; i++)
		{
			List<Vector2> vectors = GenerateSquareRing(i);
			vectors.Shuffle();
			foreach (Vector2 vector2 in vectors)
			{
				Vector2 currentVector2 = vector2 + coords;
				MapUnit unit = WorldMapManager.GetMapObjectAtPoint(currentVector2.ToVector2Int(), scene);
				if (unit != null && !unit.groundMaterial.isWater)
				{
					if (unit.entityId == null || EntityLibrary.GetEntityFromID(unit.entityId).canBeWalkedThrough)
					{
						return currentVector2;
					}
				}
			}
		}
		Debug.LogError("No suitable spawn point found.");
		return Vector2.zero;
	}

	static List<Vector2> GenerateSquareRing (int radius)
	{
		List<Vector2> points = new List<Vector2>();

		if (radius <= 0)
		{
			points.Add(Vector2.zero);
			return points;
		}

		for (int i = -radius; i <= radius; i++)
		{
			// Horizontal lines
			points.Add(new Vector2(i, radius));
			points.Add(new Vector2(i, -radius));
		}
		for (int i = -radius + 1; i <= radius - 1; i++)
		{
			// Vertical lines
			points.Add(new Vector2(radius, i));
			points.Add(new Vector2(-radius, i));
		}

		return points;
	}
}