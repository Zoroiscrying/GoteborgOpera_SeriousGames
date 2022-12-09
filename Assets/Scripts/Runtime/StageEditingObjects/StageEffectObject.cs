using System;
using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.StageEditingObjects
{
    [RequireComponent(typeof(StageTouchablePropObject))]
    [ExecuteInEditMode]
    public class StageEffectObject : BaseStageObject
    {
        private AudioSource _audioSource;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            if (gameObject.scene.isLoaded)
            {
                StageEditingManager.Instance.RemoveEditingStageStateChangedListener(OnEditingStageStateChangedEvent);   
            }
        }

        public override void UpdateRendererOrderInLayer(int newOrder)
        {
            this._spriteRenderer.sortingOrder = newOrder;
        }
        
        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public override void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            base.InitializeFromStageObjectData(stageObjectData);
            
            if (stageObjectData is not EffectStageObjectData effectStageObjectData) return;
            
            var stageEffectBlueprintSo = (effectStageObjectData.baseStageObjectBlueprintSO) as StageEffectBlueprintSO;

            if (effectStageObjectData.baseStageObjectBlueprintSO.PreviewSprite)
            {
                _spriteRenderer.sprite = effectStageObjectData.baseStageObjectBlueprintSO.PreviewSprite;
            }

            if (stageEffectBlueprintSo != null)
            {
                if (stageEffectBlueprintSo.EffectObjectPrefab)
                {
                    var effectObj = Instantiate(stageEffectBlueprintSo.EffectObjectPrefab, this.transform);  
                    effectObj.transform.localScale = new Vector3(stageEffectBlueprintSo.EffectScale.x / this.transform.localScale.x,
                        stageEffectBlueprintSo.EffectScale.y / this.transform.localScale.y, 1.0f);
                }

                if (stageEffectBlueprintSo.EffectLayer == CustomEffectLayer.BackgroundDecal || 
                    stageEffectBlueprintSo.EffectLayer == CustomEffectLayer.ObjectDecal)
                {
                    _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                }
                
                // subscribe event for stage editing / viewing
                StageEditingManager.Instance.AddEditingStageStateChangedListener(OnEditingStageStateChangedEvent);
                
                if (stageEffectBlueprintSo.EffectAmbientSound)
                {
                    _audioSource.clip = stageEffectBlueprintSo.EffectAmbientSound;
                    _audioSource.Play();
                }
            }
        }

        private void OnEditingStageStateChangedEvent(bool isCurrentlyEditing)
        {
            //Debug.Log("Editing State Changed 0");
            if (StageObjectData is EffectStageObjectData effectStageObjectData)
            {
                //Debug.Log("Editing State Changed 1");
                if (effectStageObjectData.baseStageObjectBlueprintSO is StageEffectBlueprintSO effectStageObjectSo)
                {
                    //Debug.Log("Editing State Changed 2" + effectStageObjectSo.EffectLayer.ToString());
                    if (effectStageObjectSo.EffectLayer == CustomEffectLayer.WholeSceneParticleSystem
                        || effectStageObjectSo.EffectLayer == CustomEffectLayer.ObjectParticleSystem)
                    {
                        //Debug.Log("Editing State Changed 3");
                        _spriteRenderer.enabled = isCurrentlyEditing;
                    }
                }
            }
        }
    }
}