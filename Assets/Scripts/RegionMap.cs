﻿using System.Collections.Generic;
using UnityEngine;

public class RegionMap
{
	// Maps scenes to dictionaries
	// Dictionaries map locations to map units
	public Dictionary<string, Dictionary<Vector2Int, MapUnit>> mapDict;

	public RegionMap()
	{
		mapDict = new Dictionary<string, Dictionary<Vector2Int, MapUnit>>();
	}
	public RegionMap(Dictionary<string, Dictionary<Vector2Int, MapUnit>> mapDict)
	{
		this.mapDict = mapDict;
	}
}