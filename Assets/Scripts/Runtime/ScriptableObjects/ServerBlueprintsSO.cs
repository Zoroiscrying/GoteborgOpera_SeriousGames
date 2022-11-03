using System.Collections.Generic;
using Runtime.ScriptableObjects;
using UnityEngine;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "Server Blueprints DB", menuName = "GoteborgProject/ServerBlueprintsList", order = 3)]
    public class ServerBlueprintsSO : ScriptableObject
    {
        public List<BlueprintPackSO> BlueprintPacks => blueprintPacks;
        [SerializeField] private List<BlueprintPackSO> blueprintPacks;

        public List<StagePropBlueprintSO> StagePropBlueprints => stagePropBlueprints;
        [SerializeField] private List<StagePropBlueprintSO> stagePropBlueprints;
    }
}