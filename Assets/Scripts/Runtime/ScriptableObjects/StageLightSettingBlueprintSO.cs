using Runtime.Managers;
using Runtime.UserInterface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StageLightSettingBlueprintSO", menuName = "GoteborgProject/Blueprint/StageLightSettingBlueprint", order = 4)]
    public class StageLightSettingBlueprintSO : BaseStageObjectBlueprintSO
    {
        [FormerlySerializedAs("sceneryObjectPrefab")] [SerializeField]
        private GameObject lightObjectPrefab;

        public GameObject LightObjectPrefab => lightObjectPrefab;

        public override Sprite PreviewSprite => SharedAssetsManager.Instance.CustomLightObjectSprite;

        public override StageObjectType ObjectDataType => StageObjectType.Scenery;
    }
}