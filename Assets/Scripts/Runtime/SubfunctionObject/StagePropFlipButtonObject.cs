using Runtime.Managers;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.SubfunctionObject
{
    public class StagePropFlipButtonObject : BaseStagePropSubfunctionButtonObject
    {
        protected override void DetermineInteractable()
        {
            canBeActivated = !TargetPropObject.IsLocked;
            base.DetermineInteractable();
        }
        
        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();
            TargetPropObject.FlipRenderer();
        }

        protected override bool IfButtonCanAppear()
        {
            // if it's not scenery, the button can appear and inwarding or outwarding the object
            return (TargetPropObject.StageObjectData is PropStageObjectData);
        }
    }
}