using System.Collections.Generic;
using Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Testing
{
    public class StageTouchablePropObject : BaseTouchable2DObject
    {
        #region Properties and Variables

        // [SerializeField] private StagePropObject stagePropObject;

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
            // StageEditingManager.Instance.EditingStageObject(stagePropObject, 1.0f);
        }
    }
}