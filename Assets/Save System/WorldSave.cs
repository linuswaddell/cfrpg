﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldSave
{
    public string worldName;
	public string saveFileId;
	public bool newlyCreated;
	public SerializableWorldMap worldMap;
	public List<SavedEntity> entities;
	public List<SavedNpc> npcs;
	public List<SerializableScenePortal> scenePortals;

    public WorldSave(string worldName, SerializableWorldMap worldMap, List<SavedEntity> entities, List<SavedNpc> npcs, List<SerializableScenePortal> scenePortals, bool newlyCreated)
	{
        this.worldName = worldName;
		this.worldMap = worldMap;
		this.entities = entities;
		this.npcs = npcs;
		this.scenePortals = scenePortals;
		this.newlyCreated = newlyCreated;
	}
}
