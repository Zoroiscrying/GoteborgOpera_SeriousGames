using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StageSceneryBlueprintSO", menuName = "GoteborgProject/Blueprint/StageSceneryBlueprint", order = 3)]
    public class StageSceneryBlueprintSO : BaseStageObjectBlueprintSO
    {
        [SerializeField]
        private GameObject sceneryObjectPrefab;

        public GameObject SceneryObjectPrefab => sceneryObjectPrefab;
        
        [SerializeField]
        private Texture2D sceneryPreviewTexture;

        public Texture2D SceneryPreviewTexture => sceneryPreviewTexture;
        
        public override StageObjectType ObjectDataType => StageObjectType.Scenery;
    }
}