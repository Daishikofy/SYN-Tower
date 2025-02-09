using System;
using System.Collections;
using System.Collections.Generic;
using TOWER;
using TOWER.Components;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    /// <summary>
    /// Component that allows to build structures on a given tilemap
    /// </summary>
    public class BuildingStructuresComponent : MonoBehaviour
    {
        [SerializeField] private WeightedTile structure;
        [SerializeField] private Tilemap tilemap;
        
        private MapController _mapManager;

        private void Awake()
        {
            _mapManager = GameManager.Instance.MapManager;
        }

        public void Build(Vector3 position, Vector2 direction)
        {
            Vector3 tilePosition = position + (Vector3)(direction * tilemap.layoutGrid.cellSize.x);
            Vector3Int tilemapPosition = tilemap.WorldToCell(tilePosition);
            if (tilemap.HasTile(tilemapPosition))
            {
                var hits = Physics2D.RaycastAll((Vector2)position, direction, 200f);
                foreach (var hit in hits)
                {
                    if (hit.collider?.gameObject.name != "Player")
                    {
                        var hc = hit.collider?.gameObject.GetComponent<HealthComponent>();
                        if (hc)
                        {
                            hc.Damage(1);
                            break;
                        }
                    }
                }
            }
            else
            {
                _mapManager.AddTile(tilemap.name, tilemapPosition, structure);
            }
        }
    }
}
