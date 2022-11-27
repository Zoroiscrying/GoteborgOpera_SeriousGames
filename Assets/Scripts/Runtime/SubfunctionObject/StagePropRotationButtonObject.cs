using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.SubfunctionObject
{
    public class StagePropRotationButtonObject : BaseStagePropSubfunctionButtonObject
    {
        private bool _isRotating = false;

        protected override void DetermineInteractable()
        {
            canBeActivated = !TargetPropObject.IsLocked;
            base.DetermineInteractable();
        }
        protected override void Update()
        {
            base.Update();

            if (!TargetPropObject.IsLocked && _isRotating)
            {
                Vector2 curMouseDirection = SceneCameraReference.GetMouseWorldPosition() -
                                            new Vector2(TargetTransform.position.x, TargetTransform.position.y);
                curMouseDirection.Normalize();
                if (!Mathf.Approximately(curMouseDirection.sqrMagnitude, 0.001f))
                {
                    // degree of the current mouse direction.
                    // float newMouseDegree = (Mathf.Atan(curMouseDirection.y/curMouseDirection.x)) * Mathf.Rad2Deg;
                    var targetQuaternion = Quaternion.LookRotation(Vector3.forward, curMouseDirection);
                    // Debug.Log(newMouseDegree);
                    TargetTransform.rotation = Quaternion.Slerp(TargetTransform.rotation,
                        targetQuaternion, 6.0f * Time.deltaTime);
                    // Quaternion.AngleAxis(newMouseDegree, Vector3.forward)
                }   
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isRotating = false;
            }
        }

        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();
            _isRotating = true;
        }
        
        protected override bool IfButtonCanAppear()
        {
            // if it's not scenery, the button can appear and inwarding or outwarding the object
            return (TargetPropObject.StageObjectData is PropStageObjectData or EffectStageObjectData);
        }
    }
}