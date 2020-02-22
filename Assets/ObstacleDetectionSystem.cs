﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObstacleDetectionSystem
{
	private const float CHECK_DIST = 1f;
	private const float COLLIDER_SIZE = 0.8f;
	private const int COLLISION_CHECKER_LAYER = 12;

	private class RegisteredActor
	{
		public RegisteredActor (Actor actor)
		{
			this.actor = actor;
			CreateCollider(this);
		}
		public Actor actor;
		public BoxCollider2D collider;
		public CollisionChecker checker;
	}

	private static Dictionary<string, RegisteredActor> actors;

	public static bool CheckForObstacles(Actor actor, Direction direction)
	{
		RegisterIfUnregistered(actor);

		Vector2 worldPos = actor.gameObject.transform.position;
		Vector2 colliderPos = worldPos + direction.ToVector2() * CHECK_DIST;
		actors[actor.ActorId].collider.transform.position = colliderPos;
		return (actors[actor.ActorId].checker.Colliding);
	}
	public static bool CheckForObstacles(Actor actor, Vector2 worldPos)
	{
		RegisterIfUnregistered(actor);

		actors[actor.ActorId].collider.transform.position = worldPos;
		return (actors[actor.ActorId].checker.Colliding);
	}

	private static void RegisterIfUnregistered(Actor actor)
	{
		if (actors == null)
		{
			actors = new Dictionary<string, RegisteredActor>();
		}
		RegisteredActor registered = null;
		if (actors.ContainsKey(actor.ActorId))
		{
			registered = actors[actor.ActorId];
		}
		else
		{
			actors.Add(actor.ActorId, new RegisteredActor(actor));
		}
	}

	private static void CreateCollider(RegisteredActor actor)
	{
		GameObject colliderObject = new GameObject("Obstacle Check Collider");
		colliderObject.transform.SetParent(actor.actor.gameObject.transform);
		colliderObject.layer = COLLISION_CHECKER_LAYER;
		BoxCollider2D collider = colliderObject.AddComponent<BoxCollider2D>();
		collider.size = Vector2.one * COLLIDER_SIZE;
		collider.offset = Vector2.one * 0.5f; // Offset the collider since it will be positioned on tile coordinates
		collider.isTrigger = true;
		actor.collider = collider;
		actor.checker = collider.gameObject.AddComponent<CollisionChecker>();
	}
}