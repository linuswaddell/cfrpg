﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public delegate void PlayerControllerEvent();
	public static event PlayerControllerEvent OnPlayerIdSet;

	[SerializeField] private GameObject cameraRigPrefab;
	public static string PlayerActorId { get; private set; }
	private static Actor actor;
	private static ActorMovementController movement;
	private static bool hasSetUpCamera = false;
	private static GameObject cameraRig;

	[UsedImplicitly]
	private void Update()
	{
		
		if (PlayerActorId == null)
		{
			return;
		}
		if (!hasSetUpCamera)
		{
			Debug.Log("Setting up camera");
			if (!cameraRig)
			{
				cameraRig = Instantiate(cameraRigPrefab, actor.transform, false);
			}
			else
			{
				cameraRig.transform.SetParent(actor.transform, false);
			}
			hasSetUpCamera = true;
		}

		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector2 movementVector = new Vector2(horizontal, vertical);
		if (Math.Abs(horizontal) > 0.01 || Math.Abs(vertical) > 0.01)
		{
			movement.SetWalking(movementVector);
		}
		else
		{
			movement.SetWalking(Vector2.zero);
		}
	}
	[UsedImplicitly]
	private void OnDestroy()
	{
		OnPlayerIdSet = null;
	}

	public static void SetPlayerActor(string actorId)
	{
		Debug.Log("Setting player actor to " + actorId);
		if (!ActorRegistry.IdIsRegistered(actorId))
		{
			Debug.LogError("Tried to set player-controlled actor with non-registered ID \"" + actorId + "\"!");
			return;
		}
		actor = ActorRegistry.Get(actorId).gameObject;
		if (actor == null)
		{
			Debug.LogError("Player actor doesn't have a gameobject!?");
		}
		movement = actor.GetComponent<ActorMovementController>();
		if (movement == null)
		{
			Debug.LogError("Player actor missing movement controller.");
		}

		PlayerActorId = actorId;
		hasSetUpCamera = false;
		OnPlayerIdSet?.Invoke();
	}
}