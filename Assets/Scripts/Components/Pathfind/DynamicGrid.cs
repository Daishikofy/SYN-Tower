using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    public class DynamicGrid
    {
        private readonly List<Tilemap> _obstacles;

        public DynamicGrid(List<Tilemap> obstacles)
        {
            _obstacles = obstacles;
        }

        public int GetCellValue(Vector2Int position)
        {
            foreach (Tilemap tilemap in _obstacles)
            {
                WeightedTile tile = tilemap.GetTile<WeightedTile>((Vector3Int)position);
                if (tile != null)
                {
                    int weight = tile.GetWeight();
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