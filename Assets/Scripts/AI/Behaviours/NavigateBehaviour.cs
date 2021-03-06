﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behaviours
{
	public class NavigateBehaviour : IAiBehaviour
	{
		// The max number of times we will recalculate a route if navigation fails
		private const int MAX_NAV_RETRIES = 3;

		private Actor actor;
		private ActorNavigator nav;
		private Location destination;
		private ISet<TileLocation> blockedTiles;
		private ActorBehaviourExecutor.ExecutionCallbackFailable callback;
		private Coroutine coroutineObject;
		private Actor ignoreActor; // Ignore this actor when collision checking, if not null

		private bool isWaitingForNavigationToFinish = false;
		private bool navDidFail = false;
		private int failedAttempts = 0;

		public bool IsRunning { get; private set; } = false;

		public NavigateBehaviour(Actor Actor, Location destination, ActorBehaviourExecutor.ExecutionCallbackFailable callback, Actor ignoreActor = null)
		{
			this.actor = Actor;
			nav = Actor.GetComponent<ActorNavigator>();
			this.destination = destination;
			this.callback = callback;
			blockedTiles = new HashSet<TileLocation>();
			this.ignoreActor = ignoreActor;
		}

		public void Execute()
		{
			if (coroutineObject != null)
			{
				actor.StopCoroutine(coroutineObject);
		
			}
			if (actor.Location == destination)
			{
				// We're already there! instant success.
				callback?.Invoke(true);
				return;
			}

			IsRunning = true;
			coroutineObject = actor.StartCoroutine(TravelCoroutine(destination, callback, ignoreActor));
		}

		public void Cancel()
		{
			if (!IsRunning) return;

			if (coroutineObject != null)
			{
				actor.StopCoroutine(coroutineObject);
			}
			nav.CancelNavigation();
			isWaitingForNavigationToFinish = false;
			IsRunning = false;
			callback?.Invoke(false);
		}
	
		private void RetryNavigation ()
		{
			if (coroutineObject != null)
			{
				actor.StopCoroutine(coroutineObject);
			}
			nav.CancelNavigation();
			isWaitingForNavigationToFinish = false;
			Execute();
		}



		private void OnNavigationFinished (bool didSucceed, Vector2Int discoveredObstacleWorldPos)
		{
			isWaitingForNavigationToFinish = false;
			if (!didSucceed)
			{
				// We're assuming that the scene the blocked tile is on is the same one this Actor is on
				Vector2Int scenePos = TilemapInterface.WorldPosToScenePos(discoveredObstacleWorldPos, actor.CurrentScene).ToVector2Int();
				blockedTiles.Add(new TileLocation(scenePos.x, scenePos.y, actor.CurrentScene));
			}
			navDidFail = !didSucceed;
		}



		private IEnumerator TravelCoroutine(Location destination, ActorBehaviourExecutor.ExecutionCallbackFailable callback, Actor ignoreActor = null)
		{
			nav.CancelNavigation();

			ISet<Vector2> blockedTilesInScene = new HashSet<Vector2>();
			foreach (Location tile in blockedTiles)
			{
				if (tile.Equals(destination))
				{
					// Destination is blocked; instant failure.
					Cancel();
					yield break;
				}

				if (tile.scene == actor.CurrentScene)
				{
					blockedTilesInScene.Add(tile.Vector2);
				}
			}

			if (destination.scene != actor.CurrentScene)
			{
				// Find a portal to traverse scenes

				if (!TryFindSceneEntryLocation(actor.CurrentScene, destination.scene, blockedTilesInScene, out ScenePortal targetPortal, out Vector2 targetLocation))
				{
					// Couldn't find scene entry.
					Cancel();
					yield break;
				}

				IList<Vector2> path = Pathfinder.FindPath(
					actor.transform.localPosition,
					targetLocation,
					actor.CurrentScene,
					blockedTilesInScene);

				if (path == null)
				{
					// No path to scene entry.
					Cancel();
					yield break;
				}

				isWaitingForNavigationToFinish = true;
				nav.FollowPath(
					path,
					actor.CurrentScene,
					OnNavigationFinished,
					ignoreActor);

				while (isWaitingForNavigationToFinish)
				{
					yield return null;
				}

				if (navDidFail)
				{
					failedAttempts++;
					if (failedAttempts < MAX_NAV_RETRIES)
					{
						RetryNavigation();
					}
					else
					{
						Cancel();
					}
					yield break;
				}

				// Turn towards scene portal
				nav.ForceDirection(Pathfinder.GetDirectionToLocation(actor.transform.position, targetPortal.transform.position));
				// Pause for a sec
				yield return new WaitForSeconds(0.3f);
				// Activate portal
				ScenePortalActivator.Activate(actor, targetPortal);

				// Since we just entered a new scene, we can assume that 
				// there are no known blocked tiles in this scene to avoid.
				blockedTilesInScene = new HashSet<Vector2>();

				// ...We did, right?
				if (actor.CurrentScene != destination.scene)
				{
					Debug.LogError("Uhh... That scene portal didn't work?");
					Cancel();
					yield break;
				}
			}

			if (actor.Location == destination)
			{
				// We're already there! instant success.
				callback(true);
				yield break;
			}

			// Destination is on same scene now, for sure.
			IList<Vector2> navPath = Pathfinder.FindPath(
				TilemapInterface.WorldPosToScenePos(actor.transform.position, actor.CurrentScene),
				destination.Vector2,
				actor.CurrentScene,
				blockedTilesInScene);

			if (navPath == null)
			{
				Cancel();
				yield break;
			}

			isWaitingForNavigationToFinish = true;
			nav.FollowPath(navPath, actor.CurrentScene, OnNavigationFinished, ignoreActor);


			while (isWaitingForNavigationToFinish)
			{
				yield return null;
			}
			if (navDidFail)
			{
				failedAttempts++;
				if (failedAttempts < MAX_NAV_RETRIES)
				{
					RetryNavigation();
				}
				else
				{
					Cancel();
				}
				yield break;
			}
			else
			{
				callback?.Invoke(true);
			}
			IsRunning = false;

			yield break;
		}



		// Locates a portal between the given scenes and a position from which that portal can be accessed.
		// Won't navigate through any tiles in the given blacklist in the current scene.
		// Returns false if no portal exists or the portal is blocked.
		private bool TryFindSceneEntryLocation (string currentScene, string targetScene, ISet<Vector2> tileBlacklist, out ScenePortal portal, out Vector2 accessPoint)
		{
			portal = null;
			accessPoint = Vector2.zero;

			List<ScenePortal> availablePortals = ScenePortalLibrary.GetPortalsBetweenScenes(actor.GetComponent<Actor>().CurrentScene, destination.scene);

			if (availablePortals.Count == 0)
			{
				Debug.LogWarning("No scene portal found between scenes \"" + currentScene + "\" and \"" + targetScene + "\".");
				return false;
			}

			ScenePortal targetPortal = availablePortals[0];

			HashSet<Vector2Int> possibleLocations = Pathfinder.GetValidAdjacentTiles(
				actor.CurrentScene,
				TilemapInterface.WorldPosToScenePos(targetPortal.transform.position,
					targetPortal.PortalScene),
				tileBlacklist);

			if (possibleLocations.Count == 0)
			{
				// Scene portal is blocked.
				Debug.LogWarning("Scene portal is blocked.", targetPortal);
				return false;
			}

			portal = targetPortal;
			accessPoint = possibleLocations.PickRandom(); // TODO pick closest access point instead of any access point.
			return true;
		}
	}
}
