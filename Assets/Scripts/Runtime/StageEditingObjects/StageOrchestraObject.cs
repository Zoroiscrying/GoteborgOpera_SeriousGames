using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.StageEditingObjects
{
    [RequireComponent(
        typeof(SpriteRenderer), 
        typeof(StageTouchablePropObject))]
    public class StageOrchestraObject : BaseStageObject
    {
        private SpriteRenderer _spriteRenderer;
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
            
            if (stageObjectData is not OrchestraStageObjectData orchestraStageObjectData) return;
            
            var stageEffectBlueprintSo = (orchestraStageObjectData.baseStageObjectBlueprintSO) as StageOrchestraBlueprintSO;

            if (orchestraStageObjectData.baseStageObjectBlueprintSO.PreviewSprite)
            {
                _spriteRenderer.sprite = orchestraStageObjectData.baseStageObjectBlueprintSO.PreviewSprite;    
            }
            
            if (stageEffectBlueprintSo != null)
            {
                // subscribe event for stage editing / viewing
                StageEditingManager.Instance.AddEditingStageStateChangedListener(OnEditingStageStateChangedEvent);
                OnEditingStageStateChangedEvent(true);
                _audioSource.clip = stageEffectBlueprintSo.MainStageBgm;
                _audioSource.Play();
            }
        }

        private void OnEditingStageStateChangedEvent(bool isCurrentlyEditing)
        {
            if (StageObjectData is OrchestraStageObjectData)
            {
                _spriteRenderer.enabled = isCurrentlyEditing;
            }
            // determine if the orchestra should be played.
            _audioSource.Stop();
            _audioSource.Play();
        }
    }
}