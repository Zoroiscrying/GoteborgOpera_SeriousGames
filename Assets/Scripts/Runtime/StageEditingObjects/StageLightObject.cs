using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.StageEditingObjects
{
    [RequireComponent(typeof(StageTouchablePropObject))]
    [ExecuteInEditMode]
    public class StageLightObject : BaseStageObject
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

        private void OnEditingStageStateChangedEvent(bool isCurrentlyEditing)
        {
            _spriteRenderer.enabled = isCurrentlyEditing;
        }

        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public override void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            base.InitializeFromStageObjectData(stageObjectData);
            
            if (stageObjectData is not LightStageObjectData lightStageObjectData) return;
            // _spriteRenderer.sprite = sceneryStageObjectData.sceneryObjectBlueprint.EffectSprite;
            
            var stageLightSettingBlueprintSo = (lightStageObjectData.baseStageObjectBlueprintSO) as StageLightSettingBlueprintSO;

            if (stageLightSettingBlueprintSo != null)
            {
                if (stageLightSettingBlueprintSo.PreviewSprite)
                {
                    _spriteRenderer.sprite = stageLightSettingBlueprintSo.PreviewSprite;   
                }
                // subscribe event for stage editing / viewing
                StageEditingManager.Instance.AddEditingStageStateChangedListener(OnEditingStageStateChangedEvent);
            }
        }
    }
}