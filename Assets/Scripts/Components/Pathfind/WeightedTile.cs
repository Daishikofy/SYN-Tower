using TOWER.Components;
using UnityEngine;
using UnityEngine.Events;

namespace TOWER
{
    [CreateAssetMenu]
    public class WeightedTile : RuleTile
    { 
        [SerializeField] private int weight;
        public int GetWeight() => weight;
    }
}
