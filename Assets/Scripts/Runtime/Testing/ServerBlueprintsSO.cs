using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "Server Blueprints DB", menuName = "GoteborgProject/ServerBlueprintsList", order = 3)]
    public class ServerBlueprintsSO : ScriptableObject
    {
        public List<BlueprintPackSO> BlueprintPacks => blueprintPacks;
        [SerializeField] private List<BlueprintPackSO> blueprintPacks;

        public List<StagePropBlueprintScriptableObject> StagePropBlueprints => stagePropBlueprints;
        [SerializeField] private List<StagePropBlueprintScriptableObject> stagePropBlueprints;
    }
}