using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.StageEditingObjects
{
    [RequireComponent(typeof(StageTouchablePropObject))]
    [ExecuteInEditMode]
    public class StageLightObject : BaseStageObject
    {
        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public override void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            base.InitializeFromStageObjectData(stageObjectData);
            
            if (stageObjectData is not LightStageObjectData lightStageObjectData) return;
            // _spriteRenderer.sprite = sceneryStageObjectData.sceneryObjectBlueprint.EffectSprite;
        }
    }
}