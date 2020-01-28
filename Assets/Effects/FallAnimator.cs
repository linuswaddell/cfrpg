﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAnimator : MonoBehaviour
{
    const float gravConstant = 9.8f;

    static FallAnimator instance;
    public class FallingObject 
    { 
        public GameObject gameObject; 
        public float distance;
        public float gravMultiplier;
        public float startY;
		public float startTime;
    }
    static List<FallingObject> objectsToAnimate = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
		
        for (int i = objectsToAnimate.Count - 1; i >= 0; i--)
        {
			float elapsedTime = Time.time - objectsToAnimate[i].startTime;
			float distToFall = gravConstant * objectsToAnimate[i].gravMultiplier * elapsedTime * Time.deltaTime;
			float targetHeight = objectsToAnimate[i].startY - objectsToAnimate[i].distance;
			float newYHeight = objectsToAnimate[i].gameObject.transform.localPosition.y - distToFall;

			// Make sure we never pass the target y-position
			if (objectsToAnimate[i].distance >= 0)
				newYHeight = Mathf.Max(newYHeight, targetHeight);
			else
				newYHeight = Mathf.Min(newYHeight, targetHeight);

            objectsToAnimate[i].gameObject.transform.localPosition = new Vector3
            (
                objectsToAnimate[i].gameObject.transform.localPosition.x,
                newYHeight,
                objectsToAnimate[i].gameObject.transform.localPosition.z
            );

			// Check if the fall is completed, whether we're falling up or down
            if ((objectsToAnimate[i].distance >= 0 && objectsToAnimate[i].gameObject.transform.localPosition.y <= targetHeight) || 
				(objectsToAnimate[i].distance < 0 && objectsToAnimate[i].gameObject.transform.localPosition.y >= targetHeight))
			{
				objectsToAnimate[i] = null;
                objectsToAnimate.RemoveAt(i);
            }
        }
    }
    public static FallingObject AnimateFall(GameObject gameObject, float distance, float gravMultiplier)
    {
        if (Equals(gravMultiplier, 0f))
            return null;

        if (instance == null)
        {
            GameObject instanceObject = new GameObject("FallAnimator");
            instance = instanceObject.AddComponent<FallAnimator>();
            objectsToAnimate = new List<FallingObject>();
			return AnimateFall(gameObject, distance, gravMultiplier);
        }
		// Remove any existing FallingObject objects for this gameobject
		for (int i = objectsToAnimate.Count - 1; i >= 0; i--)
		{
			if (objectsToAnimate[i].gameObject.GetInstanceID() == gameObject.GetInstanceID())
			{
				objectsToAnimate.RemoveAt(i);
			}
		}

		FallingObject objectData = new FallingObject();
        objectData.gameObject = gameObject;
        objectData.distance = distance;
        objectData.gravMultiplier = gravMultiplier;
        objectData.startY = gameObject.transform.localPosition.y;
		objectData.startTime = Time.time;
        objectsToAnimate.Add(objectData);
        return objectData;
    }
	public static void CancelFall (FallingObject fallingObject)
	{
		if (fallingObject == null)
		{
			return;
		}
		if (objectsToAnimate.Contains(fallingObject))
		{
			objectsToAnimate.Remove(fallingObject);
		}
		else
		{
			Debug.LogWarning("Tried to cancel a falling object that isn't registered with FallAnimator.");
		}
	}
}