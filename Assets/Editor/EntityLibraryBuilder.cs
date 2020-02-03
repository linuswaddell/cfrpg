﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

// Builds a ScriptableObject in the editor to store entity data
public static class EntityLibraryBuilder
{
	private const string ENTITIES_FOLDER_PATH = "Content/Entities";
	private const string ENTITY_LIBRARY_PATH = "Resources/EntityLibrary.asset";
	private const string DATA_OBJECT_NAME = "data.asset";

	[MenuItem("Assets/Build Entity Library")]
	public static void BuildEntityLibrary () {
		List<EntityData> entities = ReadEntities ();
		List<string> libraryIds = new List<string> ();
		List<EntityData> libraryEntities = new List<EntityData> ();

		foreach (EntityData entity in entities) {
			libraryIds.Add (entity.entityId);
			libraryEntities.Add (entity);
		}
		// Create a new library prefab
		EntityLibraryObject libraryObject = ScriptableObject.CreateInstance<EntityLibraryObject> (); 
		AssetDatabase.CreateAsset(libraryObject, "Assets/" + ENTITY_LIBRARY_PATH);

		// Relocate the created prefab in the assets folder
		EntityLibraryObject loadedLibraryAsset = (EntityLibraryObject)(AssetDatabase.LoadAssetAtPath ("Assets/" + ENTITY_LIBRARY_PATH, typeof(ScriptableObject)));
		// Make some persistent changes
		Undo.RecordObject (loadedLibraryAsset, "Build entity library prefab");
		loadedLibraryAsset.libraryIds = libraryIds;
		loadedLibraryAsset.libraryEntities = libraryEntities;
		PrefabUtility.RecordPrefabInstancePropertyModifications (loadedLibraryAsset);
		EditorUtility.SetDirty (loadedLibraryAsset);


		// Double check that that worked
		if (loadedLibraryAsset == null || loadedLibraryAsset.libraryIds == null) {
			Debug.LogWarning ("Entity library build failed!");
		} else {
			Debug.Log ("Entity library built.");
		}
	}

	static List<EntityData> ReadEntities () {
		List<EntityData> entities = new List<EntityData> ();

		// 1. go through each folder in entities
		// 2. parse the data file for entity properties and make it into an entitydata
		// 3. add a reference to the actual prefab for the entity to the entitydata
		// 4. do this for all of the entities and make a list

		var entitiesFolder = new DirectoryInfo(Path.Combine(Application.dataPath, ENTITIES_FOLDER_PATH));

		DirectoryInfo[] entityDirs = entitiesFolder.GetDirectories ();
		foreach (DirectoryInfo dir in entityDirs) {
			string entName = dir.Name;
			string dataObjectPath = "Assets/" + ENTITIES_FOLDER_PATH + "/" + dir.Name + "/" + DATA_OBJECT_NAME;
			EntityDataAsset dataObject = (EntityDataAsset)AssetDatabase.LoadMainAssetAtPath(dataObjectPath);
			if (dataObject != null) {
				entities.Add (dataObject.data);
			} else {
				Debug.LogWarning ("Data object not found for entity \"" + entName + "\"!");
			}
		}
		return entities;
	}
}
