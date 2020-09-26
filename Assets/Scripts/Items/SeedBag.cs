﻿using UnityEngine;

namespace Items
{
	[CreateAssetMenu(fileName = "NewItem", menuName = "Items/SeedBag", order = 1)]
    public class SeedBag : ItemData, IPloppable
    {
        [SerializeField] private string plantEntityId = null;

        void IPloppable.Use(TileLocation target)
        {
            EntityData entity = ContentLibrary.Instance.Entities.Get(plantEntityId);
            if (entity == null)
            {
                Debug.LogError("Entity " + plantEntityId + " not found!");
                return;
            }
            GroundMaterial ground = WorldMapManager.GetGroundMaterialtAtPoint(target.Position.ToVector2Int(), target.Scene);
            GroundMaterial groundCover = WorldMapManager.GetGroundCoverAtPoint(target.Position.ToVector2Int(), target.Scene);

            if (ground == null)
            {
                return;
            }
            if (groundCover == null && ground.isFarmland || groundCover != null && groundCover.isFarmland)
            {
                string currentEntity = WorldMapManager.GetEntityIdAtPoint(target.Position.ToVector2Int(), target.Scene);
                if (currentEntity != null) return;

                WorldMapManager.AttemptPlaceEntityAtPoint(entity, target.Position.ToVector2Int(), target.Scene);
            }
        }
    }
}