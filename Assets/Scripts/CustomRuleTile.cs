﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomRuleTile : TileBase
{
	[SerializeField] private bool useColliders = false;
	[SerializeField] private bool mergeWithEmpty = true;
	[SerializeField] private List<Sprite> sprites = null;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		if (useColliders)
		{
			tileData.colliderType = Tile.ColliderType.Grid;
		}
		else
		{
			tileData.colliderType = Tile.ColliderType.None;
		}

		TileBase[] surrounding = null;
		GetNeighboringTiles(tilemap, position, ref surrounding);
		var iden = Matrix4x4.identity;

		tileData.sprite = sprites[0];
		tileData.flags = TileFlags.LockTransform;
		tileData.transform = iden;

		Matrix4x4 transform = iden;

		// Connect with tiles of the same TileBase, or empty tiles (for the map edge)
		bool top = (this == surrounding[1] || (mergeWithEmpty && surrounding[1] == null));
		bool left = (this == (surrounding[3]) || (mergeWithEmpty && surrounding[3] == null));
		bool right = (this == (surrounding[4]) || (mergeWithEmpty && surrounding[4] == null));
		bool bottom = (this == (surrounding[6]) || (mergeWithEmpty && surrounding[6] == null));
		bool topLeft = (this == (surrounding[0]) || (mergeWithEmpty && surrounding[0] == null));
		bool topRight = (this == (surrounding[2]) || (mergeWithEmpty && surrounding[2] == null));
		bool bottomLeft = (this == (surrounding[5]) || (mergeWithEmpty && surrounding[5] == null));
		bool bottomRight = (this == (surrounding[7]) || (mergeWithEmpty && surrounding[7] == null));

		List<bool> scenarios = new List<bool>
		{
			top && left && right && bottom && topLeft && topRight && bottomLeft && bottomRight,
			top && left && right && bottom && !topLeft && topRight && bottomLeft && bottomRight,
			top && left && right && bottom && topLeft && !topRight && bottomLeft && bottomRight,
			top && left && right && bottom && topLeft && topRight && bottomLeft && !bottomRight,
			top && left && right && bottom && topLeft && topRight && !bottomLeft && bottomRight,
			top && left && right && bottom && !topLeft && topRight && !bottomLeft && bottomRight,
			top && left && right && bottom && !topLeft && !topRight && bottomLeft && bottomRight,
			top && left && right && bottom && topLeft && !topRight && bottomLeft && !bottomRight,
			top && left && right && bottom && topLeft && topRight && !bottomLeft && !bottomRight,
			top && left && right && bottom && !topLeft && topRight && bottomLeft && !bottomRight,
			top && left && right && bottom && topLeft && !topRight && !bottomLeft && bottomRight,
			top && left && right && bottom && topLeft && !topRight && !bottomLeft && !bottomRight,
			top && left && right && bottom && !topLeft && topRight && !bottomLeft && !bottomRight,
			top && left && right && bottom && !topLeft && !topRight && !bottomLeft && bottomRight,
			top && left && right && bottom && !topLeft && !topRight && bottomLeft && !bottomRight,
			top && left && right && bottom && !topLeft && !topRight && !bottomLeft && !bottomRight,
			top && !left && right && bottom && topRight && bottomRight,
			top && !left && right && bottom && !topRight && bottomRight,
			top && !left && right && bottom && topRight && !bottomRight,
			top && !left && right && bottom && !topRight && !bottomRight,
			!top && left && right && bottom && bottomLeft && bottomRight,
			!top && left && right && bottom && bottomLeft && !bottomRight,
			!top && left && right && bottom && !bottomLeft && bottomRight,
			!top && left && right && bottom && !bottomLeft && !bottomRight,
			top && left && !right && bottom && topLeft && bottomLeft,
			top && left && !right && bottom && !topLeft && bottomLeft,
			top && left && !right && bottom && topLeft && !bottomLeft,
			top && left && !right && bottom && !topLeft && !bottomLeft,
			top && left && right && !bottom && topLeft && topRight,
			top && left && right && !bottom && !topLeft && topRight,
			top && left && right && !bottom && topLeft && !topRight,
			top && left && right && !bottom && !topLeft && !topRight,
			top && !left && !right && bottom,
			!top && left && right && !bottom,
			!top && !left && right && bottom && bottomRight,
			!top && !left && right && bottom && !bottomRight,
			!top && left && !right && bottom && bottomLeft,
			!top && left && !right && bottom && !bottomLeft,
			top && left && !right && !bottom && topLeft,
			top && left && !right && !bottom && !topLeft,
			top && !left && right && !bottom && topRight,
			top && !left && right && !bottom && !topRight,
			!top && left && !right && !bottom,
			top && !left && !right && !bottom,
			!top && !left && right && !bottom,
			!top && !left && !right && bottom,
			!top && !left && !right && !bottom
		};

		for (int i = 0; i < scenarios.Count; i++)
		{
			if (scenarios[i])
			{
				tileData.sprite = sprites[i];
			}
		}
	}

	public override void RefreshTile(Vector3Int location, ITilemap tileMap)
	{
		for (int y = -1; y <= 1; y++)
		{
			for (int x = -1; x <= 1; x++)
			{
				base.RefreshTile(location + new Vector3Int(x, y, 0), tileMap);
			}
		}
	}

	private void GetNeighboringTiles(ITilemap tilemap, Vector3Int position, ref TileBase[] neighboringTiles)
	{
		if (neighboringTiles != null)
			return;

		TileBase[] cachedTiles = new TileBase[8];

		int index = 0;
		for (int y = 1; y >= -1; y--)
		{
			for (int x = -1; x <= 1; x++)
			{
				if (x != 0 || y != 0)
				{
					Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, position.z);
					cachedTiles[index++] = tilemap.GetTile(tilePosition);
				}
			}
		}
		neighboringTiles = cachedTiles;
	}
}
