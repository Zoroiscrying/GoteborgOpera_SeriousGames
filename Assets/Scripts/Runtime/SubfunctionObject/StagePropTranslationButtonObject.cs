using Runtime.Testing;
using UnityEngine;

namespace Runtime.SubfunctionObject
{
    public class StagePropTranslationButtonObject : BaseStagePropSubfunctionButtonObject
    {
        private bool _isTranslating = false;

        protected override void DetermineInteractable()
        {
            canBeActivated = !TargetPropObject.IsLocked;
            base.DetermineInteractable();
        }

        protected override void Update()
        {
            base.Update();

            if (!TargetPropObject.IsLocked && _isTranslating)
            {
                if (SceneCameraReference.Instance.SceneMainCamera != null)
                {
                    var desiredPosition = SceneCameraReference.Instance.SceneMainCamera.ScreenToWorldPoint(Input.mousePosition);
                    desiredPosition.z = TargetTransform.position.z;
                    TargetTransform.position = desiredPosition;
                }   
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isTranslating = false;
            }
        }

        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();

            _isTranslating = true;
        }
    }
}