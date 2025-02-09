using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    public interface IDynamicGrid
    {
        public int GetCellValue(Vector2Int position);
    }
}