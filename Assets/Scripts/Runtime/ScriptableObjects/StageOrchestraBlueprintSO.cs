using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StageOrchestraBlueprintSO", menuName = "GoteborgProject/Blueprint/StageOrchestraBlueprint", order = 3)]
    public class StageOrchestraBlueprintSO : BaseStageObjectBlueprintSO
    {
        [SerializeField] private AudioClip mainStageBgm;

        public AudioClip MainStageBgm => mainStageBgm;
        
        public override StageObjectType ObjectDataType => StageObjectType.Orchestra;
    }
}