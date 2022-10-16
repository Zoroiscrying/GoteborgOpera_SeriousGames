using System;
using UnityEngine;

namespace Runtime.Testing
{
    public class SceneCameraReference : MonoBehaviour
    {
        [SerializeField] private Camera sceneMainCamera;
        public Camera SceneMainCamera => sceneMainCamera;

        public static SceneCameraReference Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<SceneCameraReference>();
                }
                return _instance;
            }
        }

        private static SceneCameraReference _instance;

        public static Vector2 GetMouseWorldPosition()
        {
            return Instance.SceneMainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        private void Start()
        {
            _instance = this;
        }
    }
}