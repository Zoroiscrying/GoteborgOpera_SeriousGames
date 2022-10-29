﻿using Runtime.Testing;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.SubfunctionObject
{
    public class StagePropOutwardButtonObject : BaseStagePropSubfunctionButtonObject
    {
        protected override void DetermineInteractable()
        {
            canBeActivated = !TargetPropObject.IsLocked;
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
    }
}