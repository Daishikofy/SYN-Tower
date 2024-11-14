using System.Collections;
using System.Collections.Generic;
using TOWER;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    public class BuildingStructuresComponent : MonoBehaviour
    {
        [SerializeField] private WeightedTile structure;
        [SerializeField] private Tilemap tilemap;
        
        public void Build(Vector3 position, Vector2 direction)
        {
            Vector3 tilePosition = position + (Vector3)(direction * tilemap.layoutGrid.cellSize.x);
            Vector3Int tilemapPosition = tilemap.WorldToCell(tilePosition);
            
            if (!tilemap.HasTile(tilemapPosition))
            {
                tilemap.SetTile(tilemapPosition, structure);
            }
        }
    }
}
