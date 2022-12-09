using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.StageEditingObjects
{
    [RequireComponent(
        typeof(SpriteRenderer), 
        typeof(StageTouchablePropObject))]
    public class StageActorObject : BaseStageObject
    {
        private AudioSource _audioSource;

        public override void UpdateRendererOrderInLayer(int newOrder)
        {
            this._spriteRenderer.sortingOrder = newOrder;
        }

        public override void FlipRenderer()
        {
            this._spriteRenderer.flipX = !this._spriteRenderer.flipX;
        }
        

        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public override void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            base.InitializeFromStageObjectData(stageObjectData);
            
            if (stageObjectData is not ActorStageObjectData propStageObjectData) return;
            
            ApplyActorDecorators();
            
            var stagePropBlueprintSo = (propStageObjectData.baseStageObjectBlueprintSO) as StageActorBlueprintSO;
            if (stagePropBlueprintSo != null)
            {
                GetComponent<SpriteMask>().sprite = stagePropBlueprintSo.ActorSprite;
                _audioSource.clip = stagePropBlueprintSo.ActorSoundEffect;
                _stageTouchablePropObject.SubscribeOnObjectButtonDown(() =>
                {
                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.Play();
                    }
                });
            }
            GetComponent<PolygonCollider2D>().TryUpdateShapeToAttachedSprite();
        }

        private void OnValidate()
        {
            if (_spriteRenderer && _stageTouchablePropObject)
            {
                ApplyActorDecorators();   
            }
            else
            {
                OnEnable();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void ApplyActorDecorators()
        {
            if (StageObjectData is not ActorStageObjectData actorStageObjectData) return;
            
            var stagePropBlueprintSo = (actorStageObjectData.baseStageObjectBlueprintSO) as StageActorBlueprintSO;
            if (stagePropBlueprintSo != null)
            {
                GetComponent<SpriteMask>().sprite = stagePropBlueprintSo.ActorSprite;
                _spriteRenderer.sprite = stagePropBlueprintSo.ActorSprite;
                this.transform.localScale = new Vector3(stagePropBlueprintSo.ActorSize.x,
                    stagePropBlueprintSo.ActorSize.y, 1.0f);
            }
        }
    }
}