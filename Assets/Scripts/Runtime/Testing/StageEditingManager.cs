using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    
    /// <summary>
    /// Stage Editing Functionalities:
    /// - Instantiate stage prop objects from the Storage (and delete the Storage's Item)
    /// - Delete Stage prop objects from the stage (and add the object to the Storage, the decorators should be kept)
    /// - Manage the sub-buttons of stage prop objects
    /// </summary>
    public class StageEditingManager : MonoBehaviour
    {
        #region Singleton

        public static StageEditingManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<StageEditingManager>();
                }

                if (!_instance)
                {
                    Debug.LogError("No Stage Editing Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }
        private static StageEditingManager _instance;

        #endregion

        // Template Prefabs
        [SerializeField] private GameObject defaultStageObjectPrefab;
        [SerializeField] private List<GameObject> functionButtonPrefabs;

        [SerializeField] private BaseStageObjectData testStageObjectData;

        private List<StagePropObject> _stagePropObjectsInstantiated = new List<StagePropObject>();

        // TODO:: Change this to Touchable Function Button
        private List<BaseTouchable2DObject> _functionButtons = new List<BaseTouchable2DObject>();

        private void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            // test scripts
            if (Input.GetKeyDown(KeyCode.P))
            {
                InstantiateNewPropToStage(Vector2.zero, testStageObjectData);
            }
        }

        private void OnEnable()
        {
            if (_functionButtons.Count == 0)
            {
                for (int i = 0; i < functionButtonPrefabs.Count; i++)
                {
                    _functionButtons.Add( 
                        Instantiate(functionButtonPrefabs[i], Vector3.zero, functionButtonPrefabs[i].transform.rotation).
                            GetComponent<BaseTouchable2DObject>());
                    // disable this for now
                    _functionButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void DeactivateEditingButtons()
        {
            foreach (var button in _functionButtons)
            {
                // deactivate current button game objects
                button.gameObject.SetActive(false);
            }
        }

        
        public void InstantiateNewPropToStage(Vector2 positionXY, BaseStageObjectData objData)
        {
            var stageObj = Instantiate(defaultStageObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStageObjectPrefab.transform.rotation).GetComponent<StagePropObject>();
            stageObj.InitializeFromStageObjectData(objData);
        }
        
        public void EditingStageObject(Vector3 buttonCenter, float buttonSeparateRadius, bool isLocking, LayerZ curLayer,
            Action onStartTranslate, Action onStartRotate, Action onStartLocking, Action onStopLocking,
            Action onLayerInward, Action onLayerOutward)
        {
            int num = 0;
            
            foreach (var button in _functionButtons)
            {
                // first clear the events
                button.ClearButtonDownEvent();
                
                // deactivate current button game objects
                // button.gameObject.SetActive(false);
                
                button.SubscribeOnObjectButtonDown(DeactivateEditingButtons);
                // Inject new events
                if (num == 0) // Translate
                {
                    button.SubscribeOnObjectButtonDown(onStartTranslate);
                }
                else if (num == 1) // Rotate
                {
                    button.SubscribeOnObjectButtonDown(onStartRotate);
                }
                else if (num == 2) // Lock
                {
                    if (isLocking)
                    {
                        button.SubscribeOnObjectButtonDown(onStopLocking);
                    }
                    else
                    {
                        button.SubscribeOnObjectButtonDown(onStartLocking);   
                    }
                }
                else if (num == 3) // Inward
                {
                    button.SubscribeOnObjectButtonDown(onLayerInward);
                }
                else if (num == 4) // Outward
                {
                    button.SubscribeOnObjectButtonDown(onLayerOutward);
                }
                num++;
            }

            // re-animate the buttons
            for (int i = 0; i < _functionButtons.Count; i++)
            {
                const float degreeOffset = 18f;
                float degree = i * 72 + degreeOffset;
                float y = Mathf.Sin(degree * Mathf.Deg2Rad) * buttonSeparateRadius;
                float x = Mathf.Cos(degree * Mathf.Deg2Rad) * buttonSeparateRadius;
                Vector3 position = new Vector3(x + buttonCenter.x, y + buttonCenter.y,
                    (int)LayerZ.Foreground + 0.5f);
                _functionButtons[i].gameObject.transform.position = position;
                _functionButtons[i].gameObject.SetActive(true);
            }

        }
    }
}