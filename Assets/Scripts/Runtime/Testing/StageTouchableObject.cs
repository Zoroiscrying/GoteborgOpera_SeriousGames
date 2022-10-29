using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Testing
{
    public class StageTouchableObject : BaseTouchable2DObject
    {
        #region Properties and Variables

        [SerializeField] private StagePropObject stagePropObject;

        #endregion

        #region Unity Events

        protected override void Update()
        {
            base.Update();
        }

        #endregion

        protected override void ActivateObject()
        {
            base.ActivateObject();
            
            StageEditingManager.Instance.EditingStageObject(stagePropObject, 1.0f);
        }
    }
}