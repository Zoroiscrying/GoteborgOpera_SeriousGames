using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Testing
{
    public class StageTouchableObject : BaseTouchable2DObject
    {
        #region Properties and Variables
        
        private bool _isTranslating = false;
        private bool _isRotating = false;
        private bool _isLocking = false;    
        
        #endregion

        #region Unity Events

        protected override void Update()
        {
            base.Update();

            if (_isTranslating)
            {
                TranslatingObject();
            }

            if (_isRotating)
            {
                RotatingObject();
            }
            
            // hold, released, then press again to confirm
            if (Input.GetMouseButtonUp(0))
            {
                _isTranslating = false;
                _isRotating = false;
            }
        }

        #endregion

        protected override void ActivateObject()
        {
            base.ActivateObject();
            
            StageEditingManager.Instance.EditingStageObject(this.transform.position, 1.0f, _isLocking, _curLayerZCoord,
                StartTranslating, StartRotating, StartLocking, StopLocking, LayerInward, LayerOutward);
        }
        
        // TODO:: The stage editing part's logic should be in 'StagePropObject.cs'
        // This Touchable Object class is just an interaction interface.
        private LayerZ _curLayerZCoord = LayerZ.StageCenter;

        private void TranslatingObject()
        {
            if (SceneCameraReference.Instance.SceneMainCamera != null)
            {
                var desiredPosition = SceneCameraReference.Instance.SceneMainCamera.ScreenToWorldPoint(Input.mousePosition);
                desiredPosition.z = transform.position.z;
                // Debug.Log(Input.mousePosition);
                this.transform.position = desiredPosition;
            }
        }

        private void StartTranslating()
        {
            if (!_isRotating && !_isLocking)
            {
                _isTranslating = true;   
            }
        }
        
        private void StopTranslating()
        {
            Assert.IsTrue(_isTranslating);
            _isTranslating = false;
        }

        private void RotatingObject()
        {
            Vector2 curMouseDirection = SceneCameraReference.GetMouseWorldPosition() -
                                        new Vector2(this.transform.position.x, this.transform.position.y);
            curMouseDirection.Normalize();
            if (!Mathf.Approximately(curMouseDirection.sqrMagnitude, 0.001f))
            {
                // degree of the current mouse direction.
                float newMouseDegree = (Mathf.Atan(curMouseDirection.y/curMouseDirection.x)) * Mathf.Rad2Deg;
                var targetQuaternion = Quaternion.LookRotation(Vector3.forward, curMouseDirection);
                // Debug.Log(newMouseDegree);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                    targetQuaternion, 0.5f);
                // Quaternion.AngleAxis(newMouseDegree, Vector3.forward)
            }
        }
        
        private void StartRotating()
        {
            if (!_isTranslating && !_isLocking)
            {
                _isRotating = true;   
            }
        }

        private void StopRotating()
        {
            Assert.IsTrue(_isRotating);
            _isRotating = false;
        }

        private void StartLocking()
        {
            if (!_isTranslating && !_isRotating)
            {
                _isLocking = true; 
            }
        }

        private void StopLocking()
        {
            Assert.IsTrue(_isLocking);
            _isLocking = false;
        }

        private void LayerInward()
        {
            // not at the back layer
            Assert.IsFalse(_curLayerZCoord == LayerZ.StageBack);
            
            var position = transform.position;
            position = new Vector3(position.x, position.y, (int)--_curLayerZCoord);
            transform.position = position;
        }

        private void LayerOutward()
        {
            // not at the front layer
            Assert.IsFalse(_curLayerZCoord == LayerZ.StageFront);
            
            var position = transform.position;
            position = new Vector3(position.x, position.y, (int)++_curLayerZCoord);
            transform.position = position;
        }
    }
}