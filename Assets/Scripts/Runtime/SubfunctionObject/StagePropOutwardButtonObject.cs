using Runtime.Managers;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.SubfunctionObject
{
    public class StagePropOutwardButtonObject : BaseStagePropSubfunctionButtonObject
    {
        protected override void DetermineInteractable()
        {
            canBeActivated = !TargetPropObject.IsLocked && TargetPropObject.CurLayerZCoord != LayerZ.StageFront;
            base.DetermineInteractable();
        }
        
        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();
            
            Assert.IsFalse(TargetPropObject.CurLayerZCoord == LayerZ.StageFront);

            int newOrderInLayer = (int)++(TargetPropObject.CurLayerZCoord);
            TargetPropObject.UpdateRendererOrderInLayer(newOrderInLayer);
            Debug.Log($"Put object {TargetPropObject.name} to order {newOrderInLayer}");
        }
        
        protected override bool IfButtonCanAppear()
        {
            // if it's not scenery, the button can appear and inwarding or outwarding the object
            return (TargetPropObject.StageObjectData is PropStageObjectData or EffectStageObjectData);
        }
    }
}