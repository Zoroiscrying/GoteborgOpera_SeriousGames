using System;
using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using UnityEngine;

namespace Runtime.Testing
{
    [RequireComponent(
        typeof(SpriteRenderer), 
        typeof(StageTouchablePropObject))]
    [ExecuteInEditMode]
    public class StagePropObject : BaseStageObject
    {
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private BasePropDecorator basePropDecorator;

        private AudioSource _audioSource;

        public override void UpdateRendererOrderInLayer(int newOrder)
        {
            this._spriteRenderer.sortingOrder = newOrder;
        }
        
        /// <summary>
        /// This function is to be called when a prop from a prop list is instantiated.
        /// </summary>
        /// <param name="propDecorator"></param>
        public void InjectPropDecorator(BasePropDecorator propDecorator)
        {
            this.basePropDecorator = propDecorator;
        }

        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public override void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            base.InitializeFromStageObjectData(stageObjectData);
            
            if (stageObjectData is not PropStageObjectData propStageObjectData) return;
            
            ApplyPropDecorators();
            var stagePropBlueprintSo = (propStageObjectData.baseStageObjectBlueprintSO) as StagePropBlueprintSO;
            if (stagePropBlueprintSo != null)
            {
                GetComponent<SpriteMask>().sprite = stagePropBlueprintSo.PropSprite;
                _audioSource.clip = stagePropBlueprintSo.PropSoundEffect;
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
            if (_spriteRenderer && _stageTouchablePropObject && basePropDecorator.Initialized)
            {
                ApplyPropDecorators();   
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

            // TODO:: This should be changed to a more flexible version, where properties can be changed in the editor
            // TODO:: Might just add a list of 'customize material variable' to the base prop decorator...
            basePropDecorator.SetupRenderer(_spriteRenderer);
        }

        private void ApplyPropDecorators()
        {
            // pass in list of decorators
            basePropDecorator.ApplyDecoration();
            
            if (StageObjectData is not PropStageObjectData propStageObjectData) return;
            
            var stagePropBlueprintSo = (propStageObjectData.baseStageObjectBlueprintSO) as StagePropBlueprintSO;
            if (stagePropBlueprintSo != null)
            {
                GetComponent<SpriteMask>().sprite = stagePropBlueprintSo.PropSprite;
                _spriteRenderer.sprite = stagePropBlueprintSo.PropSprite;
                _spriteRenderer.color = stagePropBlueprintSo.SpriteTint;
                this.transform.localScale = new Vector3(stagePropBlueprintSo.PropScale.x,
                    stagePropBlueprintSo.PropScale.y, 1.0f);
            }
        }
    }
}