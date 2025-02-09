using System;
using System.Collections.Generic;
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
                    tilemap.DeleteCells(position,1,1,1);
                    GameManager.Instance.UpdatedTilemap();
                    break;
                }
            }
        }

        public int GetCellValue(Vector2Int position)
        {
            foreach (Tilemap tilemap in obstacles)
            {
                WeightedTile tile = tilemap.GetTile<WeightedTile>((Vector3Int)position);
                if (tile != null)
                {
                    int weight = tile.GetWeight();
                    if (weight > 50)
                    {
                        Debug.Log("Building");
                    }
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
