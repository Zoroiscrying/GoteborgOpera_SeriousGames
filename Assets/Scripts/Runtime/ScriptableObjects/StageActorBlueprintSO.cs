using Runtime.UserInterface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StageActorBlueprint_0", menuName = "GoteborgProject/Blueprint/StageActorBlueprint", order = 5)]
    public class StageActorBlueprintSO : BaseStageObjectBlueprintSO
    {
        [SerializeField] private Vector2 actorSize = Vector2.one;
        public Vector2 ActorSize => actorSize;

        [SerializeField] Sprite actorSprite;

        public Sprite ActorSprite => actorSprite;
        
        public override Sprite PreviewSprite => actorSprite;
        
        //[SerializeField] private Color spriteTint = Color.white;

        //public Color SpriteTint => spriteTint;

        [SerializeField] private AudioClip actorSoundEffect;

        public AudioClip ActorSoundEffect => actorSoundEffect;

        public override StageObjectType ObjectDataType => StageObjectType.Actor;
    }
}