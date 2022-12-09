using System.Collections.Generic;
using Runtime.ScriptableObjects;
using UnityEngine;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "BlueprintPack_0", menuName = "GoteborgProject/Blueprint/BlueprintPack", order = 0)]
    public class BlueprintPackSO : ScriptableObject
    {
        [SerializeField] private string packName = "DefaultPackName";
        public string PackName => packName;

        [SerializeField] private List<BaseStageObjectBlueprintSO> stageObjectBlueprints;
        public List<BaseStageObjectBlueprintSO> StageObjectBlueprints => stageObjectBlueprints;

        [Multiline] [SerializeField] private string packDescription;

        public string PackDescription => packDescription;
    }
}