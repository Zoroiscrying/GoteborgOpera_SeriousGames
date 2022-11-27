using System;
using System.Collections.Generic;
using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.Testing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    /// <summary>
    /// Manage the purchasing UI of Blueprints, Props, Actors and Orchestras.
    /// - Updating the item in the build prop list ui scroll view.
    /// - Updating the resources cost in the build prop list ui scroll view.
    /// - Produce prop via blueprint
    /// - Recycle prop among produced props
    /// - Upgrade Machine levels / Buy resources (Maybe move/copy to other UI)
    /// </summary>
    public class BackstageProductionUI : MonoBehaviour
    {
        // Prop List UI
        [SerializeField] private RectTransform propListTransform;
        [SerializeField] private GameObject propListItemPrefab;

        // Machine Levels UI
        [SerializeField] private TextMeshProUGUI woodMachineLevel;
        [SerializeField] private TextMeshProUGUI metalMachineLevel;
        [SerializeField] private TextMeshProUGUI clothMachineLevel;
        [SerializeField] private TextMeshProUGUI paintMachineLevel;
        
        // Resource Requirements UI
        [SerializeField] private TextMeshProUGUI woodRequirementText;
        [SerializeField] private TextMeshProUGUI metalRequirementText;
        [SerializeField] private TextMeshProUGUI clothRequirementText;
        [SerializeField] private TextMeshProUGUI paintRequirementText;

        // Resource Bar UI
        [SerializeField] private PlayerResourcesBarUI resourceBarUI;
        [SerializeField] private TextMeshProUGUI moneyCostWhenProduce;

        // Backstage View UI - For Quitting Production UI
        [SerializeField] private BackStageViewUI backStageViewUI;
        
        // Button Interactions
        [SerializeField] private Button producePropButton;
        [SerializeField] private Button recycleButton;
        [SerializeField] private Button backButton;
        
        // UI Object Data List
        private List<StageObjectListItemUIObject> _propListItems = new List<StageObjectListItemUIObject>();
        private StageObjectListItemUIObject _selectedStageObject;

        // Scene Preview Update
        [SerializeField] private Camera previewShotCamera;
        [SerializeField] private Transform stagePreviewSceneObjects;
        [SerializeField] private Transform stageObjectPreviewParent;

        private void OnEnable()
        {
            previewShotCamera.enabled = false;
            Assert.IsNotNull(propListTransform);
            Assert.IsNotNull(propListItemPrefab);
            Assert.IsNotNull(woodMachineLevel);
            Assert.IsNotNull(metalMachineLevel);
            Assert.IsNotNull(clothMachineLevel);
            Assert.IsNotNull(paintMachineLevel);
            Assert.IsNotNull(producePropButton);
            Assert.IsNotNull(recycleButton);
            Assert.IsNotNull(backButton);
            Assert.IsNotNull(woodRequirementText);
            Assert.IsNotNull(metalRequirementText);
            Assert.IsNotNull(clothRequirementText);
            Assert.IsNotNull(paintRequirementText);
            // register button events
            producePropButton.onClick.AddListener(ProduceSelectedProp);
            recycleButton.onClick.AddListener(RecycleSelectedProp);
            backButton.onClick.AddListener(SwitchToBackstageView);
        }

        private void OnDisable()
        {
            // unregister button events
            producePropButton.onClick.RemoveListener(ProduceSelectedProp);
            recycleButton.onClick.RemoveListener(RecycleSelectedProp);
            backButton.onClick.RemoveListener(SwitchToBackstageView);
        }

        public void StartPropProducing()
        {
            this.gameObject.SetActive(true);
            UpdateBlueprintList();
            UpdateMachineLevelUI();
            UpdateCurrentResourcesUI();
            // clear preview objects
            for (int i = 0; i < stageObjectPreviewParent.childCount; i++)
            {
                Destroy(stageObjectPreviewParent.GetChild(i).gameObject);
            }
            // render the preview at start
            previewShotCamera.enabled = true;
            stagePreviewSceneObjects.gameObject.SetActive(true);
            //previewShotCamera.Render();
            //previewShotCamera.enabled = false;
        }

        private void SwitchToBackstageView()
        {
            this.gameObject.SetActive(false);
            
            if (_selectedStageObject)
            {
                _selectedStageObject.HintOnDeselected();
            }
            _selectedStageObject = null;
            
            backStageViewUI.BackStageViewHomePage();
            
            previewShotCamera.enabled = false;
            ClearPreviewObjects();
            stagePreviewSceneObjects.gameObject.SetActive(false);
        }
        
        private void UpdateMachineLevelUI()
        {
            if (_selectedStageObject)
            {
                UpdateMachineLevelText(metalMachineLevel,
                    _selectedStageObject.Blueprint.MachineLevelRequirement[GResourceType.Metal],
                    StorageManager.Instance.MachineLevels[GResourceType.Metal]);
                UpdateMachineLevelText(woodMachineLevel,
                    _selectedStageObject.Blueprint.MachineLevelRequirement[GResourceType.Wood],
                    StorageManager.Instance.MachineLevels[GResourceType.Wood]);
                UpdateMachineLevelText(clothMachineLevel,
                    _selectedStageObject.Blueprint.MachineLevelRequirement[GResourceType.Cloth],
                    StorageManager.Instance.MachineLevels[GResourceType.Cloth]);
                UpdateMachineLevelText(paintMachineLevel,
                    _selectedStageObject.Blueprint.MachineLevelRequirement[GResourceType.Paint],
                    StorageManager.Instance.MachineLevels[GResourceType.Paint]);
            }
            else
            {
                woodMachineLevel.text = StorageManager.Instance.MachineLevels[GResourceType.Wood].ToString();
                metalMachineLevel.text = StorageManager.Instance.MachineLevels[GResourceType.Metal].ToString();
                clothMachineLevel.text = StorageManager.Instance.MachineLevels[GResourceType.Cloth].ToString();
                paintMachineLevel.text = StorageManager.Instance.MachineLevels[GResourceType.Paint].ToString();   
            }
        }

        private void UpdateCurrentResourcesUI()
        {
            resourceBarUI.UpdateResourcesBarUI();
            
            if (_selectedStageObject)
            {
                woodRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Wood]}(-{_selectedStageObject.Blueprint.ResourceConsumes[GResourceType.Wood]})";
                metalRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Metal]}(-{_selectedStageObject.Blueprint.ResourceConsumes[GResourceType.Metal]})";
                clothRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Cloth]}(-{_selectedStageObject.Blueprint.ResourceConsumes[GResourceType.Cloth]})";
                paintRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Paint]}(-{_selectedStageObject.Blueprint.ResourceConsumes[GResourceType.Paint]})";

            }
        }
        
        /// <summary>
        /// Update the prop list UI view, including:
        /// - The blueprints that the player has
        /// - The number of produced prop for each blueprint
        /// </summary>
        private void UpdateBlueprintList()
        {
            // the blueprints the player have is stored in the storage manager
            var blueprintStageObjectDict = StorageManager.Instance.BlueprintObjectDict;
            var ownedBlueprints = StorageManager.Instance.OwnedBlueprints;
            
            _selectedStageObject = null;
            producePropButton.interactable = false;
            recycleButton.interactable = false;
            
            if (_propListItems.Count != ownedBlueprints.Count)
            {
                // first destroy all the prop list item game objects
                foreach (var propListItem in _propListItems)
                {
                    Destroy(propListItem.gameObject);
                }
                
                _propListItems.Clear();
                
                // re-generate the list item objects
                foreach (var blueprint in ownedBlueprints)
                {
                    AddNewStageBlueprintListItem(blueprint,
                        blueprintStageObjectDict.ContainsKey(blueprint)
                            ? blueprintStageObjectDict[blueprint].Count
                            : 0);
                }
            }
        }

        private void ProduceSelectedProp()
        {
            Assert.IsNotNull(_selectedStageObject);
            
            // add the produced prop data to the storage manager
            if (StorageManager.Instance.TryProduceNewStageObjectViaBlueprint(_selectedStageObject.Blueprint, true))
            {
                // update the prop list item curNum UI
                _selectedStageObject.ProducedOneItem();
                // update the prop list item
                SelectOnTargetBlueprint(_selectedStageObject);
                // update the resources
            }
            else
            {
                if (StorageManager.Instance.ProducedStageObjectCount >= StorageManager.Instance.MaxStagePropNum)
                {
                    TempUIHintManager.Instance.HintText("Storage is Full!");   
                }
            }
        }

        private void RecycleSelectedProp()
        {
            Assert.IsNotNull(_selectedStageObject);
            
            if (_selectedStageObject)
            {
                if (StorageManager.Instance.TryRecycleStageObjectOfSpecificBlueprint(_selectedStageObject.Blueprint))
                {
                    // update the prop list item curNum UI
                    _selectedStageObject.RecycledOneItem();
                    // update the prop list item
                    SelectOnTargetBlueprint(_selectedStageObject);
                }
            }
        }

        private void AddNewStageBlueprintListItem(BaseStageObjectBlueprintSO blueprint, int curNum)
        {
             var newPropListItemUIObject = (Instantiate(propListItemPrefab, propListTransform).GetComponent<StageObjectListItemUIObject>());
             
             _propListItems.Add(newPropListItemUIObject);
             
             var height = Mathf.Max(750.0f, _propListItems.Count * 120.0f);
             propListTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
             
             newPropListItemUIObject.HintOnDeselected();
             newPropListItemUIObject.UpdateItemUI(blueprint, curNum, SelectOnTargetBlueprint);
        }
        
        private void SelectOnTargetBlueprint(StageObjectListItemUIObject uiObjectToSelect)
        {
            if (_selectedStageObject)
            {
                _selectedStageObject.HintOnDeselected();   
            }
            uiObjectToSelect.HintOnSelected();
            _selectedStageObject = uiObjectToSelect;
            
            // also update the stage preview panel.
            UpdateScenePreviewObject();
            
            // also update the produce button (can click or not)
            if (PurchaseManager.Instance.CanProduceBlueprintProp(uiObjectToSelect.Blueprint, out var requireMoney))
            {
                moneyCostWhenProduce.text = $"Cost {requireMoney} money";
                producePropButton.interactable = true;
            }
            else
            {
                moneyCostWhenProduce.text = "Not enough money!";
                producePropButton.interactable = false;
            }
            
            // also update the machine levels panel
            UpdateMachineLevelUI();

            // also update the recycle button
            if (StorageManager.Instance.BlueprintObjectDict[uiObjectToSelect.Blueprint].Count <= 0)
            {
                recycleButton.interactable = false;
            }
            else
            {
                recycleButton.interactable = true;
            }
            
            // also update the resources requirement texts
            UpdateCurrentResourcesUI();
        }

        private void UpdateScenePreviewObject()
        {
            ClearPreviewObjects();

            if (!_selectedStageObject) return;
            
            switch (_selectedStageObject.Blueprint.ObjectDataType)
            {
                case StageObjectType.Prop:
                    if (_selectedStageObject.Blueprint is StagePropBlueprintSO propBlueprintSo)
                    {
                        var newObj = new GameObject("PropObject")
                        {
                            layer = LayerMask.NameToLayer("PreviewStage"),
                            transform =
                            {
                                parent = stageObjectPreviewParent.transform,
                                localPosition = Vector3.zero,
                            }
                        };
                        var newRenderer = newObj.AddComponent<SpriteRenderer>();
                        newRenderer.color = propBlueprintSo.SpriteTint;
                        newRenderer.sprite = propBlueprintSo.PropSprite;
                        newRenderer.transform.localScale =
                            new Vector3(propBlueprintSo.PropScale.x, propBlueprintSo.PropScale.y, 1);
                        newRenderer.sortingLayerName = "Preview";
                        newRenderer.sortingOrder = 5;
                    }
                    break;
                case StageObjectType.Actor:
                    // Instantiate()
                    break;
                case StageObjectType.Orchestra:
                    if (_selectedStageObject.Blueprint is StageOrchestraBlueprintSO orchestraBlueprintSo)
                    {
                        if (orchestraBlueprintSo.MainStageBgm)
                        {
                            var orchestraPrefab = Instantiate(SharedAssetsManager.Instance.PreviewStageObjectPrefab,
                                stageObjectPreviewParent);
                            orchestraPrefab.GetComponent<SpriteRenderer>().sprite =
                                SharedAssetsManager.Instance.CustomOrchestraSprite;
                            var audioSource = orchestraPrefab.AddComponent<AudioSource>();
                            audioSource.clip = orchestraBlueprintSo.MainStageBgm;
                            audioSource.Play();
                        }
                    }
                    break;
                case StageObjectType.Effect:
                    if (_selectedStageObject.Blueprint is StageEffectBlueprintSO stageEffectBlueprintSo)
                    {
                        if (stageEffectBlueprintSo.EffectObjectPrefab)
                        {
                            var effectPrefab = Instantiate(stageEffectBlueprintSo.EffectObjectPrefab, stageObjectPreviewParent);   
                        }
                        else
                        {
                            var effectPrefab = Instantiate(SharedAssetsManager.Instance.PreviewStageObjectPrefab,
                                stageObjectPreviewParent);
                            effectPrefab.GetComponent<SpriteRenderer>().sprite =
                                stageEffectBlueprintSo.EffectSprite;
                            effectPrefab.transform.localScale =
                                new Vector3(stageEffectBlueprintSo.EffectScale.x, stageEffectBlueprintSo.EffectScale.y, 1);
                        }
                    }
                    break;
                case StageObjectType.Scenery:
                    if (_selectedStageObject.Blueprint is StageSceneryBlueprintSO sceneryBlueprintSo)
                    {
                        var sceneryPrefab = Instantiate(sceneryBlueprintSo.SceneryObjectPrefab, stageObjectPreviewParent);
                    }
                    break;
                case StageObjectType.Light:
                    if (_selectedStageObject.Blueprint is StageLightSettingBlueprintSO lightBlueprintSo)
                    {
                        var sceneryPrefab = Instantiate(lightBlueprintSo.LightObjectPrefab, stageObjectPreviewParent);
                    }
                    break; 
            }
        }

        private void ClearPreviewObjects()
        {
            for (int i = 0; i < stageObjectPreviewParent.childCount; i++)
            {
                Destroy(stageObjectPreviewParent.GetChild(i).gameObject);
            }
        }

        private void UpdateMachineLevelText(TextMeshProUGUI textMeshProUGUI, int requireLevel, int curLevel)
        {
            textMeshProUGUI.text = $"{requireLevel}/{curLevel}";
            if (requireLevel > curLevel)
            {
                textMeshProUGUI.color = Color.red;
            }
            else
            {
                textMeshProUGUI.color = Color.green;
            }
        }
    }
}