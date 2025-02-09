using System;
using System.Collections.Generic;
using TOWER.Components;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    public class MapController : MonoBehaviour, IDynamicGrid
    {
        [SerializeField] private List<Tilemap> obstacles;
        public List<Tilemap> Obstacles { get => obstacles; }
        
        public void AddTile(string tilemapName, Vector3Int position, TileBase tile)
        {
            foreach (Tilemap tilemap in obstacles)
            {
                if (tilemapName == tilemap.name)
                {
                    tilemap.SetTile(position, tile);
                    GameManager.Instance.UpdatedTilemap();
                    break;
                }
            }
        }

        public void RemoveTile(string tilemapName, Vector3Int position)
        {
            foreach (Tilemap tilemap in obstacles)
            {
                if (tilemapName == tilemap.name)
                {
                    tilemap.SetTile(position, null);
                    GameManager.Instance.UpdatedTilemap();
                    break;
                }
            }
        }

        public int GetCellValue(Vector2Int position)
        {
            foreach (Tilemap tilemap in obstacles)
            {
                GameObject tile = tilemap.GetInstantiatedObject((Vector3Int)position);
                if (tile != null)
                {
                    //PERF: Frequently called during pathfinding, might be an issue in the futur
                    return tile.GetComponent<HealthComponent>().CurrentHealth;
                }
                WeightedTile wTile = tilemap.GetTile<WeightedTile>((Vector3Int)position);
                if (wTile != null)
                {
                    int weight = wTile.GetWeight();
                    if (weight < 0)
                    {
                         return Int32.MaxValue;
                    }
                    return weight;
                }
            }
            return 0;
        }
    }
}
