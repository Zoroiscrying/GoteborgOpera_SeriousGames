using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Testing
{
    public class TestTouchable2DObject : BaseTouchable2DObject
    {
        [SerializeField] private ParticleSystem popParticleSystem;
        
        private bool _followMouseCursor;

        private void Start()
        {
            Assert.IsNotNull(popParticleSystem);
        }

        protected override void Update()
        {
            base.Update();

            if (_followMouseCursor)
            {
                if (SceneCameraReference.Instance.SceneMainCamera != null)
                {
                    var desiredPosition = SceneCameraReference.Instance.SceneMainCamera.ScreenToWorldPoint(Input.mousePosition);
                    desiredPosition.z = transform.position.z;
                    // Debug.Log(Input.mousePosition);
                    this.transform.position = desiredPosition;
                }
            }
        }

        protected override void StartActivatingObject()
        {
            base.StartActivatingObject();
            _followMouseCursor = true;
            this.transform.localScale = Vector3.one * 1.2f;
        }

        protected override void EndActivatingObject()
        {
            base.EndActivatingObject();
            _followMouseCursor = false;
            this.transform.localScale = Vector3.one * 1.0f;
        }

        protected override void ActivateObject()
        {
            base.ActivateObject();
            var ps = Instantiate(popParticleSystem, this.transform.position, popParticleSystem.transform.rotation);
            
            // destroy particle system after 0.75 seconds.
            GameObject o;
            (o = ps.gameObject).SetActive(true);
            Destroy(o, 0.75f);
        }
    }
}