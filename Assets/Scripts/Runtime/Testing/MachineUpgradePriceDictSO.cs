using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "MachineUpgradePriceDictionary", menuName = "GoteborgProject/MachineUpgradePriceDictSO", order = 2)]
    public class MachineUpgradePriceDictSO : ScriptableObject
    {
        public IDictionary<GResourceType, List<int>> MachineUpgradeCost => machineUpgradeCost;

        /// <summary>
        /// 0 - 0->1
        /// 1 - 1->2
        /// 2 - 2->3 (Count == 3 == Max Machine Level)
        /// </summary>
        [SerializeField]
        private GenericDictionary<GResourceType, List<int>> machineUpgradeCost =
            new()
            {
                { GResourceType.Money, new List<int>()},
                { GResourceType.Wood, new List<int>()},
                { GResourceType.Metal, new List<int>()},
                { GResourceType.Cloth, new List<int>()},
                { GResourceType.Paint, new List<int>()},
            };

        public int CalculateUpgradeCost(GResourceType type, int currentMachineLevel)
        {
            if (currentMachineLevel >= machineUpgradeCost[type].Count)
            {
                return Int32.MaxValue;
            }
            return machineUpgradeCost[type][currentMachineLevel];
        }

        public bool IsMaximumMachineLevel(GResourceType type, int currentMachineLevel)
        {
            return currentMachineLevel >= machineUpgradeCost[type].Count;
        }
    }
}