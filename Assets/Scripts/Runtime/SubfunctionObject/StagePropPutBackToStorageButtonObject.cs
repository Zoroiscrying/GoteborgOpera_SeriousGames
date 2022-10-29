using Runtime.Testing;
using UnityEngine;

namespace Runtime.SubfunctionObject
{
    public class StagePropPutBackToStorageButtonObject : BaseStagePropSubfunctionButtonObject
    {
        protected override void DetermineInteractable()
        {
            canBeActivated = !TargetPropObject.IsLocked;
            base.DetermineInteractable();
        }

        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();
            
            StageEditingManager.Instance.PutPropFromStageToStorage(TargetPropObject);
        }
    }
}