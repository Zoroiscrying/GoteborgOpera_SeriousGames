using UnityEngine;

namespace Runtime.Testing
{
    public class TestTouchableRotate2DObject : BaseTouchable2DObject
    {
        private Quaternion _quaternionBeforeRotation = Quaternion.identity;
        private Vector2 _rotateAnchorDirection = Vector2.right;
        private float _rotateAnchorDegree = 0.0f;
        private bool _listeningMouseMovement;

        protected override void Update()
        {
            base.Update();

            if (_listeningMouseMovement)
            {
                Vector2 curMouseDirection = SceneCameraReference.GetMouseWorldPosition() -
                                           new Vector2(this.transform.position.x, this.transform.position.y);
                curMouseDirection.Normalize();
                if (!Mathf.Approximately(curMouseDirection.sqrMagnitude, 0.001f))
                {
                    // degree of the current mouse direction.
                    float newMouseDegree = (Mathf.Atan(curMouseDirection.y/curMouseDirection.x)) * Mathf.Rad2Deg;
                    var targetQuaternion = Quaternion.LookRotation(Vector3.forward, curMouseDirection);
                    Debug.Log(newMouseDegree);
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                        targetQuaternion, 0.5f);
                    // Quaternion.AngleAxis(newMouseDegree, Vector3.forward)
                }
            }

            // hold, released, then press again to confirm
            if (Input.GetMouseButtonUp(0))
            {
                _listeningMouseMovement = false;
                this.transform.localScale = Vector3.one * 1.0f;
            }
        }

        protected override void StartActivatingObject()
        {
            base.StartActivatingObject();
            //this.transform.localScale = Vector3.one * 1.2f;
        }

        protected override void EndActivatingObject()
        {
            base.EndActivatingObject();
            //this.transform.localScale = Vector3.one * 1.0f;
        }
        
        protected override void ActivateObject()
        {
            base.ActivateObject();
            this.transform.localScale = Vector3.one * 1.2f;
            _quaternionBeforeRotation = this.transform.rotation;
            
            Vector2 curMousePosition = SceneCameraReference.GetMouseWorldPosition();
            _rotateAnchorDirection = curMousePosition - new Vector2(this.transform.position.x, this.transform.position.y);
            if (!Mathf.Approximately(_rotateAnchorDirection.sqrMagnitude, 0.001f))
            {
                _rotateAnchorDirection.Normalize();
                _rotateAnchorDegree = Mathf.Atan(_rotateAnchorDirection.y/_rotateAnchorDirection.x);   
            }
            else
            {
                _rotateAnchorDirection = Vector2.right;
                _rotateAnchorDegree = 0.0f;
            }
            _listeningMouseMovement = true;
        }
    }
}