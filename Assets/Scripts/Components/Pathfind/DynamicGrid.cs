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
                if (tilemap.GetTile((Vector3Int) position) != null)
                {
                    return Int32.MaxValue;
                }
            }
            return 0;
        }
    }
}