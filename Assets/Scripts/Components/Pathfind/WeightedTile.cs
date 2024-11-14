using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    [CreateAssetMenu]
    public class WeightedTile : RuleTile
    {
        [SerializeField] private int weight;
        public int GetWeight() => weight;
    }
}
