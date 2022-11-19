using Runtime.Managers;
using Runtime.StageDataObjects;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StageEffectBlueprintSO", menuName = "GoteborgProject/Blueprint/StageEffectBlueprint", order = 2)]
    public class StageEffectBlueprintSO : BaseStageObjectBlueprintSO
    {
        [SerializeField] private Vector2 effectScale = Vector2.one * 0.2f;
        public Vector2 EffectScale => effectScale;
        
        [SerializeField]    
        private Sprite effectSprite;
        public Sprite EffectSprite => effectSprite ? effectSprite : SharedAssetsManager.Instance.CustomParticleSystemEffectSprite;

        public override Sprite PreviewSprite => EffectSprite;

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