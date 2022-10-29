using System;
using System.Collections.Generic;
using Runtime.SubfunctionObject;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Testing
{
    public enum LayerZ
    {
        Background = -10,

        StageBack = -1,
        StageCenter = 0,
        StageFront = 1,
        
        Foreground = 9,
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
        
        private List<StagePropObject> _stagePropObjectsInstantiated = new List<StagePropObject>();

        // TODO:: Change this to Touchable Function Button
        private List<BaseStagePropSubfunctionButtonObject> _stagePropfunctionButtons = new List<BaseStagePropSubfunctionButtonObject>();

        [SerializeField] private StageEditingUI stageEditingUI;
        
        private bool _shouldOpenStageEditingUI = true;
        private bool _shouldOpenPropProducingUI = true;

        #region Unity Events

        private void Awake()
        {
            _instance = this;
        }
        
        
        private void Update()
        {
            // todo:: move this to proper places (should use UI button to navigate through scenes)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_shouldOpenStageEditingUI)
                {
                    stageEditingUI.StartStageEditingUI();
                    stageEditingUI.InitializeStageEditingUI(StorageManager.Instance.StageObjectDataList);   
                }
                else
                {
                    stageEditingUI.EndStageEditingUI();
                }

                _shouldOpenStageEditingUI = !_shouldOpenStageEditingUI;
            }

            // todo:: move this to proper places (should use UI button to navigate through scenes)
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                PurchaseManager.Instance.TogglePurchaseUIPanel(_shouldOpenPropProducingUI);
                _shouldOpenPropProducingUI = !_shouldOpenPropProducingUI;
            }
        }

        private void OnEnable()
        {
            if (_stagePropfunctionButtons.Count == 0)
            {
                for (int i = 0; i < functionButtonPrefabs.Count; i++)
                {
                    _stagePropfunctionButtons.Add( 
                        Instantiate(functionButtonPrefabs[i], Vector3.zero, functionButtonPrefabs[i].transform.rotation).
                            GetComponent<BaseStagePropSubfunctionButtonObject>());
                    // disable this for now
                    _stagePropfunctionButtons[i].gameObject.SetActive(false);
                }
            }
        }

        #endregion

        // todo:: instantiate props, actors and orchestras should be different.
        public void InstantiateNewPropToStage(Vector2 positionXY, BaseStageObjectData objData)
        {
            var stageObj = Instantiate(defaultStageObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStageObjectPrefab.transform.rotation).GetComponent<StagePropObject>();
            stageObj.InitializeFromStageObjectData(objData);
            _stagePropObjectsInstantiated.Add(stageObj);
        }

        public void PutPropFromStageToStorage(StagePropObject propObject)
        {
            _stagePropObjectsInstantiated.Remove(propObject);
            StorageManager.Instance.AddStageObjectData(propObject.StageObjectData);
            Destroy(propObject.gameObject);
        }

        public void EditingStageObject(StagePropObject propObject, float buttonSeparateRadius)
        {
            foreach (var button in _stagePropfunctionButtons)
            {
                button.gameObject.SetActive(true);
                // first clear the events
                button.ClearButtonDownEvent();
                // deactivate buttons
                button.DeactivateButton();
                // make sure other buttons will be toggled off
                button.SubscribeOnObjectButtonDown(DeactivateEditingButtons);
                // inject individual events to the buttons.
                button.InitializeButtonObject(propObject.transform, propObject);
            }

            // get the current mouse position
            var desiredPosition = propObject.transform.position;
            if (SceneCameraReference.Instance.SceneMainCamera != null)
            {
                desiredPosition = SceneCameraReference.Instance.SceneMainCamera.ScreenToWorldPoint(Input.mousePosition);
            }   
            
            // re-animate the buttons
            float degreeInterval = 360.0f / (float)_stagePropfunctionButtons.Count;
            for (int i = 0; i < _stagePropfunctionButtons.Count; i++)
            {
                float degreeOffset = 90.0f - degreeInterval;
                float degree = i * degreeInterval + degreeOffset;
                float y = Mathf.Sin(degree * Mathf.Deg2Rad) * buttonSeparateRadius;
                float x = Mathf.Cos(degree * Mathf.Deg2Rad) * buttonSeparateRadius;
                Vector3 position = new Vector3(x + desiredPosition.x, y + desiredPosition.y,
                    (int)LayerZ.Foreground + 0.5f);
                
                _stagePropfunctionButtons[i].gameObject.transform.position = position;
                _stagePropfunctionButtons[i].ReactivateButton();
            }
        }
        
        private void DeactivateEditingButtons()
        {
            foreach (var button in _stagePropfunctionButtons)
            {
                // deactivate current button game objects
                button.DeactivateButton();
            }
        }
    }
    
}