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
            }
        }
    }
}