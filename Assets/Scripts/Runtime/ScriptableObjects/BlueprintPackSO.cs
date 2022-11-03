using System.Collections.Generic;
using Runtime.ScriptableObjects;
using UnityEngine;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "BlueprintPack_0", menuName = "GoteborgProject/Blueprint/BlueprintPack", order = 0)]
    public class BlueprintPackSO : ScriptableObject
    {
        [SerializeField] private List<StagePropBlueprintSO> stagePropBlueprints;
        public List<StagePropBlueprintSO> StagePropBlueprints => stagePropBlueprints;
    }
}