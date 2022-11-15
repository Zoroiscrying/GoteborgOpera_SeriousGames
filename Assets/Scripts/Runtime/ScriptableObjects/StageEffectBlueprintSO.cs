using Runtime.StageDataObjects;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StageEffectBlueprintSO", menuName = "GoteborgProject/Blueprint/StageEffectBlueprint", order = 2)]
    public class StageEffectBlueprintSO : BaseStageObjectBlueprintSO
    {
        [SerializeField] private Vector2 effectScale = Vector2.one;
        public Vector2 EffectScale => effectScale;
        
        [SerializeField]    
        private Sprite effectSprite;
        public Sprite EffectSprite => effectSprite;

        public override Sprite PreviewSprite => effectSprite;

        [SerializeField]
        private GameObject effectObjectPrefab;
        public GameObject EffectObjectPrefab => effectObjectPrefab;
        
        [SerializeField]
        private CustomEffectLayer effectLayer;
        public CustomEffectLayer EffectLayer => effectLayer;

        [SerializeField] private AudioClip effectAmbientSound;

        public AudioClip EffectAmbientSound => effectAmbientSound;

        public override StageObjectType ObjectDataType => StageObjectType.Effect;
    }
}