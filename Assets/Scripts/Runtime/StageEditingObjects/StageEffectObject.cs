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
        private SpriteRenderer _spriteRenderer;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
            
            _spriteRenderer.sprite = effectStageObjectData.baseStageObjectBlueprintSO.PreviewSprite;
            
            if (stageEffectBlueprintSo != null)
            {
                Instantiate(stageEffectBlueprintSo.EffectObjectPrefab, this.transform);    
                // subscribe event for stage editing / viewing
                StageEditingManager.Instance.AddEditingStageStateChangedListener(OnEditingStageStateChangedEvent);
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