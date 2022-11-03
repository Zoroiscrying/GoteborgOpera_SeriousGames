using Runtime.Managers;
using Runtime.StageDataObjects;
using UnityEngine;

namespace Runtime.Testing
{
    [RequireComponent(typeof(StageTouchablePropObject))]
    [ExecuteInEditMode]
    public class StageSceneryObject : BaseStageObject
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            //_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public override void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            base.InitializeFromStageObjectData(stageObjectData);
            
            if (stageObjectData is not SceneryStageObjectData sceneryStageObjectData) return;
            // _spriteRenderer.sprite = sceneryStageObjectData.sceneryObjectBlueprint.EffectSprite;
        }
    }
}