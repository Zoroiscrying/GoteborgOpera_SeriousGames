using System;
using System.Collections.Generic;
using Runtime.Managers;
using Runtime.StageDataObjects;
using Runtime.Testing;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    public class StageEditingUI : MonoBehaviour
    {
        [SerializeField] private BackStageViewUI backStageViewUI;
        [SerializeField] private Button endEditingButton;
        [SerializeField] private Transform stageEditingPanel;
        [SerializeField] private RectTransform propListUIParent;
        [SerializeField] private GameObject stageObjectButtonUIObject;

        [SerializeField] private Button togglePropDisplayButton;
        [SerializeField] private Button toggleActorDisplayButton;
        [SerializeField] private Button toggleOrchestraDisplayButton;
        [SerializeField] private Button toggleEffectDisplayButton;
        [SerializeField] private Button toggleSceneryDisplayButton;
        [SerializeField] private Button toggleLightDisplayButton;
        
        [SerializeField] private Button shareButton;

        [SerializeField] private StageObjectType stageObjectTypesToDisplay = (StageObjectType)~0;
        
        private void OnEnable()
        {
            endEditingButton.onClick.AddListener(EndStageEditingUI);
            togglePropDisplayButton.onClick.AddListener(TogglePropFlag);
            toggleActorDisplayButton.onClick.AddListener(ToggleActorFlag);
            toggleOrchestraDisplayButton.onClick.AddListener(ToggleOrchestraFlag);
            toggleEffectDisplayButton.onClick.AddListener(ToggleEffectFlag);
            toggleSceneryDisplayButton.onClick.AddListener(ToggleSceneryFlag);
            toggleLightDisplayButton.onClick.AddListener(ToggleLightFlag);
            shareButton.onClick.AddListener(() =>
            {
                TempUIHintManager.Instance.HintText("This will help share your screen/video clip to other people.");
            });
        }

        private void OnDisable()
        {
            endEditingButton.onClick.RemoveListener(EndStageEditingUI);
            togglePropDisplayButton.onClick.RemoveListener(TogglePropFlag);
            toggleActorDisplayButton.onClick.RemoveListener(ToggleActorFlag);
            toggleOrchestraDisplayButton.onClick.RemoveListener(ToggleOrchestraFlag);
            toggleEffectDisplayButton.onClick.RemoveListener(ToggleEffectFlag);
            toggleSceneryDisplayButton.onClick.RemoveListener(ToggleSceneryFlag);
            toggleLightDisplayButton.onClick.RemoveListener(ToggleLightFlag);
            shareButton.onClick.RemoveAllListeners();
        }

        public void StartStageEditingUI()
        {
            this.gameObject.SetActive(true);
            stageEditingPanel.gameObject.SetActive(true);
            propListUIParent.gameObject.SetActive(true);
        }

        public void InitializeStageEditingUIList(List<BaseStageObjectData> objDataList)
        {
            DestroyAllUIButtons();

            // create new object buttons
            foreach (var stageObject in objDataList)
            {
                if (stageObjectTypesToDisplay.HasFlag(StageObjectType.Prop) && stageObject is PropStageObjectData)
                {
                    AddNewObjectButton(stageObject, StageObjectType.Prop);
                }
                else if (stageObjectTypesToDisplay.HasFlag(StageObjectType.Actor) && stageObject is ActorStageObjectData)
                {
                    AddNewObjectButton(stageObject, StageObjectType.Actor);   
                }
                else if (stageObjectTypesToDisplay.HasFlag(StageObjectType.Orchestra) && stageObject is OrchestraStageObjectData)
                {
                    AddNewObjectButton(stageObject, StageObjectType.Orchestra);   
                }
                else if (stageObjectTypesToDisplay.HasFlag(StageObjectType.Effect) && stageObject is EffectStageObjectData)
                {
                    AddNewObjectButton(stageObject, StageObjectType.Effect);   
                }
                else if (stageObjectTypesToDisplay.HasFlag(StageObjectType.Scenery) && stageObject is SceneryStageObjectData)
                {
                    AddNewObjectButton(stageObject, StageObjectType.Scenery);   
                }
                else if (stageObjectTypesToDisplay.HasFlag(StageObjectType.Light) && stageObject is LightStageObjectData)
                {
                    AddNewObjectButton(stageObject, StageObjectType.Light);   
                }
            }
            
            propListUIParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250.0f * objDataList.Count);
            
            UpdateAllFlagButtons();
        }

        private void TogglePropFlag()
        {
            ToggleStageObjectTypeFlag(StageObjectType.Prop);
            togglePropDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Prop) ? Color.white : Color.gray;
        }

        private void ToggleActorFlag()
        {
            ToggleStageObjectTypeFlag(StageObjectType.Actor);
            toggleActorDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Actor) ?  Color.white : Color.gray;
        }

        private void ToggleOrchestraFlag()
        {
            ToggleStageObjectTypeFlag(StageObjectType.Orchestra);
            toggleOrchestraDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Orchestra) ?  Color.white : Color.gray;
        }

        private void ToggleEffectFlag()
        {
            ToggleStageObjectTypeFlag(StageObjectType.Effect);
            toggleEffectDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Effect) ?  Color.white : Color.gray;
        }

        private void ToggleSceneryFlag()
        {
            ToggleStageObjectTypeFlag(StageObjectType.Scenery);
            toggleSceneryDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Scenery) ?  Color.white : Color.gray;
        }

        private void ToggleLightFlag()
        {
            ToggleStageObjectTypeFlag(StageObjectType.Light);
            toggleLightDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Light) ?  Color.white : Color.gray;
        }

        private void UpdateAllFlagButtons()
        {
            togglePropDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Prop) ?  Color.white : Color.gray;
            toggleActorDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Actor) ?  Color.white : Color.gray;
            toggleOrchestraDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Orchestra) ?  Color.white : Color.gray;
            toggleEffectDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Effect) ?  Color.white : Color.gray;
            toggleSceneryDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Scenery) ?  Color.white : Color.gray;
            toggleLightDisplayButton.GetComponent<Image>().color = stageObjectTypesToDisplay.HasFlag(StageObjectType.Light) ?  Color.white : Color.gray;
        }

        private void ToggleStageObjectTypeFlag(StageObjectType typeToToggle)
        {
            if (stageObjectTypesToDisplay.HasFlag(typeToToggle))
            {
                stageObjectTypesToDisplay &= (~typeToToggle);
            }
            else
            {
                stageObjectTypesToDisplay |= typeToToggle;
            }
            
            InitializeStageEditingUIList(StorageManager.Instance.StageObjectDataList);
        }
        
        private void EndStageEditingUI()
        {
            this.gameObject.SetActive(false);
            stageEditingPanel.gameObject.SetActive(false);
            propListUIParent.gameObject.SetActive(false);
            backStageViewUI.BackStageViewHomePage();
        }

        private void DestroyAllUIButtons()
        {
            int numChildren = propListUIParent.childCount;
            for (int i = 0; i < numChildren; ++i)
            {
                Destroy(propListUIParent.GetChild(i).gameObject);
            }
        }
        
        private void AddNewObjectButton(BaseStageObjectData objectData, StageObjectType objectType = StageObjectType.Prop)
        {
            var obj = Instantiate(stageObjectButtonUIObject, propListUIParent).GetComponent<StageObjectButtonUIObject>();
            switch (objectType)
            {
                case StageObjectType.Prop:
                    Assert.IsNotNull((PropStageObjectData)objectData);
                    obj.InitializeAsPropObject((PropStageObjectData)objectData);
                    break;
                case StageObjectType.Actor:
                    obj.InitializeAsActor((ActorStageObjectData)objectData);
                    break;
                case StageObjectType.Orchestra:
                    obj.InitializeAsOrchestra((OrchestraStageObjectData)objectData);
                    break;
                case StageObjectType.Effect:
                    obj.InitializeAsEffectObject((EffectStageObjectData)objectData);
                    break;
                case StageObjectType.Scenery:
                    obj.InitializeAsSceneryObject((SceneryStageObjectData)objectData);
                    break;
                case StageObjectType.Light:
                    obj.InitializeAsLightObject((LightStageObjectData)objectData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
            }
            obj.enabled = true;
            obj.gameObject.SetActive(true);
        }
    }
}