using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Testing
{
    public enum LayerZ
    {
        Background = 10,

        StageBack = 1,
        StageCenter = 0,
        StageFront = -1,
        
        Foreground = -9,
    }
    
    public class StageTouchableObject : BaseTouchable2DObject
    {
        #region Properties and Variables

        [SerializeField] private GameObject FunctionButtonTemplate;

        private List<BaseTouchable2DObject> _functionButtons = new List<BaseTouchable2DObject>();
        
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
            
            // instantiate the touchable button objects
            // register events to the button objects
            for (int i = 0; i < 5; i++)
            {
                float degree = i * 72;
                const float r = 1.0f;
                float y = Mathf.Sin(degree * Mathf.Deg2Rad) * r;
                float x = Mathf.Cos(degree * Mathf.Deg2Rad) * r;
                Vector3 position = new Vector3(x + this.transform.position.x, y + this.transform.position.y,
                    (int)LayerZ.Foreground + 0.5f);
                _functionButtons.Add( 
                    Instantiate(FunctionButtonTemplate, position, FunctionButtonTemplate.transform.rotation).
                        GetComponent<BaseTouchable2DObject>());
                _functionButtons[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < _functionButtons.Count; i++)
            {
                var button = _functionButtons[i];
                var text = button.GetComponentInChildren<TextMeshPro>();
                if (i == 0)
                {
                    text.text = "T";
                    button.SubscribeOnObjectButtonDown(StartTranslating);
                }
                
                if (i == 1)
                {
                    text.text = "R";
                    button.SubscribeOnObjectButtonDown(StartRotating);
                }
                
                if (i == 2)
                {
                    text.text = "L";
                    if (_isLocking)
                    {
                        button.SubscribeOnObjectButtonDown(StopLocking);
                    }
                    else
                    {
                        button.SubscribeOnObjectButtonDown(StartLocking);   
                    }
                }
                
                if (i == 3)
                {
                    text.text = "I";
                    button.SubscribeOnObjectButtonDown(LayerInward);
                }
                
                if (i == 4)
                {
                    text.text = "O";
                    button.SubscribeOnObjectButtonDown(LayerOutward);
                }
            }
        }

        /// <summary>
        /// TODO:: Move this part to a 'StageLayerControl.cs' class
        /// Background Layer (z = 10).
        /// 
        /// Stage Layer Back (z = 1).
        /// Stage Center Layer (z = 0).
        /// Stage Layer Front (z = -1).
        /// 
        /// Foreground Layer (z = -2).
        /// 
        /// </summary>
        private LayerZ _curLayerZCoord = LayerZ.StageCenter;

        // TODO:: Deactivate buttons and clear registered events
        private void DestroyAllButtons()
        {
            foreach (var button in _functionButtons)
            {
                Destroy(button.gameObject);
            }
            _functionButtons.Clear();
        }

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
            DestroyAllButtons();
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
            DestroyAllButtons();
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
            DestroyAllButtons();
        }

        private void StopLocking()
        {
            Assert.IsTrue(_isLocking);
            _isLocking = false;
            DestroyAllButtons();
        }

        private void LayerInward()
        {
            // not at the back layer
            Assert.IsFalse(_curLayerZCoord == LayerZ.StageBack);
            
            var position = transform.position;
            position = new Vector3(position.x, position.y, (int)--_curLayerZCoord);
            transform.position = position;
            DestroyAllButtons();
        }

        private void LayerOutward()
        {
            // not at the front layer
            Assert.IsFalse(_curLayerZCoord == LayerZ.StageFront);
            
            var position = transform.position;
            position = new Vector3(position.x, position.y, (int)++_curLayerZCoord);
            transform.position = position;
            DestroyAllButtons();
        }
    }
}