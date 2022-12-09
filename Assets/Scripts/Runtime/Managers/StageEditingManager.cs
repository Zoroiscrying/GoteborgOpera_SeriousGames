using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.StageEditingObjects;
using Runtime.SubfunctionObject;
using Runtime.Testing;
using Runtime.UserInterface;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Managers
{
    public enum LayerZ
    {
        Background = -10,
        Scenery = -9,

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
        [Header("Template Prefabs")]
        [FormerlySerializedAs("defaultStageObjectPrefab")] 
        [SerializeField] private GameObject defaultStagePropObjectPrefab;
        [SerializeField] private GameObject defaultStageActorObjectPrefab;
        [SerializeField] private GameObject defaultStageEffectObjectPrefab;
        [SerializeField] private GameObject defaultStageSceneryObjectPrefab;
        [SerializeField] private GameObject defaultLightSettingObjectPrefab;
        [SerializeField] private GameObject defaultStageOrchestraObjectPrefab;
        
        [SerializeField] private List<GameObject> functionButtonPrefabs;
        
        // Stage Object Data
        public List<BaseStageObject> StageObjectsInstantiated => _stageObjectsInstantiated;
        private List<BaseStageObject> _stageObjectsInstantiated = new List<BaseStageObject>();

        // list of sub-function buttons retrieved from injected function button prefabs
        private List<BaseStagePropSubfunctionButtonObject> _stagePropfunctionButtons = new List<BaseStagePropSubfunctionButtonObject>();
        
        // Stage Editing UI control
        [Header("Stage Edit UI Control")]
        [SerializeField] private StageEditingUI stageEditingUI;

        // Whether Or Not is Editing Prop
        private bool _canEditStageProp = true; 
        private event Action<bool> EditingStagePropStateChanged;
        private event Action SwitchToBackstage;

        [Header("Instantiation Parents")]
        // Stage Editing Singleton controls - for conditions where certain objects shouldn't exist at the same time.
        // e.g. two scenery objects shouldn't appear at the same time.
        [SerializeField] private Transform stageSceneryObjectParent;
        [SerializeField] private Transform stageLightSettingObjectParent;
        [SerializeField] private Transform stageBgmParent;

        [Header("Animation Control")] 
        [SerializeField] private List<SpriteRenderer> frontCurtainList;
        [SerializeField] private List<SpriteRenderer> frontCurtainFoldedList;
        [SerializeField] private List<SpriteRenderer> backCurtainList;

        #region Unity Events

        private void Awake()
        {
            _instance = this;
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

        public void AddEditingStageStateChangedListener(Action<bool> callback)
        {
            EditingStagePropStateChanged += callback;
        }

        public void AddSwitchToBackStageListener(Action callback)
        {
            SwitchToBackstage += callback;
        }
        
        public void RemoveEditingStageStateChangedListener(Action<bool> callback)
        {
            EditingStagePropStateChanged -= callback;
        }
        
        public void RemoveSwitchToBackStageListener(Action callback)
        {
            SwitchToBackstage -= callback;
        }

        /// <summary>
        /// Switch from front stage to back stage, should close the curtains
        /// </summary>
        public void NotifySwitchToBackStage()
        {
            SwitchToBackstage?.Invoke();

            DeactivateEditingButtons();
            
            foreach (var spriteRenderer in frontCurtainList)
            {
                spriteRenderer.DOFade(1f, 1.0f);
            }
            
            foreach (var spriteRenderer in frontCurtainFoldedList)
            {
                spriteRenderer.DOFade(0f, 1.0f);
            }
            
            foreach (var spriteRenderer in backCurtainList)
            {
                spriteRenderer.DOFade(1f, 1.0f);
            }
        }
        
        /// <summary>
        /// Switch from back stage to front stage view, with canEdit true or false
        /// </summary>
        /// <param name="canEdit"></param>
        public void ChangeStageEditPermission(bool canEdit)
        {
            this._canEditStageProp = canEdit;
            EditingStagePropStateChanged?.Invoke(_canEditStageProp);
            
            foreach (var spriteRenderer in frontCurtainList)
            {
                spriteRenderer.DOFade(0f, 1.0f);
            }
            
            foreach (var spriteRenderer in frontCurtainFoldedList)
            {
                spriteRenderer.DOFade(1f, 1.0f);
            }
            
            foreach (var spriteRenderer in backCurtainList)
            {
                spriteRenderer.DOFade(0f, 1.0f);
            }
        }

        #region Stage Object Instantiation
        // todo:: instantiate props, actors and orchestras should be different.
        public void InstantiateNewPropToStage(BaseStageObjectData objectData, Vector2 positionXY)
        {
            var stageObj = Instantiate(defaultStagePropObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStagePropObjectPrefab.transform.rotation).GetComponent<StagePropObject>();
            stageObj.InitializeFromStageObjectData(objectData);
            _stageObjectsInstantiated.Add(stageObj);
        }

        public void InstantiateNewActorToStage(BaseStageObjectData objectData, Vector2 positionXY)
        {
            var stageObj = Instantiate(defaultStageActorObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStageActorObjectPrefab.transform.rotation).GetComponent<StageActorObject>();
            stageObj.InitializeFromStageObjectData(objectData);
            _stageObjectsInstantiated.Add(stageObj);
        }
        
        public void InstantiateNewOrchestraToStage(BaseStageObjectData objectData, Vector2 positionXY)
        {
            if (stageBgmParent.childCount > 0)
            {
                PutObjectFromStageToStorage(stageBgmParent.GetChild(0).GetComponent<BaseStageObject>());
            }
            
            var stageObj = Instantiate(defaultStageOrchestraObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStageOrchestraObjectPrefab.transform.rotation, stageBgmParent).GetComponent<StageOrchestraObject>();
            stageObj.InitializeFromStageObjectData(objectData);
            _stageObjectsInstantiated.Add(stageObj);
        }

        public void InstantiateNewEffectToStage(BaseStageObjectData objectData, Vector2 positionXY)
        {
            var stageObj = Instantiate(defaultStageEffectObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStageEffectObjectPrefab.transform.rotation).GetComponent<StageEffectObject>();
            stageObj.InitializeFromStageObjectData(objectData);
            _stageObjectsInstantiated.Add(stageObj);
        }

        public void InstantiateNewSceneryToStage(BaseStageObjectData objectData, Vector2 positionXY)
        {
            if (stageSceneryObjectParent.childCount > 0)
            {
                PutObjectFromStageToStorage(stageSceneryObjectParent.GetChild(0).GetComponent<BaseStageObject>());
            }
            
            var stageObj = Instantiate(defaultStageSceneryObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultStageSceneryObjectPrefab.transform.rotation, stageSceneryObjectParent).GetComponent<StageSceneryObject>();
            stageObj.InitializeFromStageObjectData(objectData);
            _stageObjectsInstantiated.Add(stageObj);
        }
        
        public void InstantiateNewLightSettingToStage(BaseStageObjectData objectData, Vector2 positionXY)
        {
            if (stageLightSettingObjectParent.childCount > 0)
            {
                PutObjectFromStageToStorage(stageLightSettingObjectParent.GetChild(0).GetComponent<BaseStageObject>());
            }
            
            var stageObj = Instantiate(defaultLightSettingObjectPrefab,
                new Vector3(positionXY.x, positionXY.y, (int)LayerZ.StageCenter),
                defaultLightSettingObjectPrefab.transform.rotation, stageLightSettingObjectParent).GetComponent<StageLightObject>();
            stageObj.InitializeFromStageObjectData(objectData);
            _stageObjectsInstantiated.Add(stageObj);
        }

        #endregion

        public void PutObjectFromStageToStorage(BaseStageObject stageObject)
        {
            _stageObjectsInstantiated.Remove(stageObject);
            StorageManager.Instance.AddStageObjectData(stageObject.StageObjectData);
            Destroy(stageObject.gameObject);
        }

        public void EditingStageObject(BaseStageObject propObject, float buttonSeparateRadius)
        {
            if (_canEditStageProp)
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