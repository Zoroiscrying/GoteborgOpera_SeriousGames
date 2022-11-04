using System.Collections.Generic;
using Runtime.ScriptableObjects;
using Runtime.Testing;
using TMPro;
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
        [SerializeField] private RectTransform propListTransform;
        [SerializeField] private GameObject propListItemPrefab;

        [SerializeField] private TextMeshProUGUI woodMachineLevel;
        [SerializeField] private TextMeshProUGUI metalMachineLevel;
        [SerializeField] private TextMeshProUGUI clothMachineLevel;
        [SerializeField] private TextMeshProUGUI paintMachineLevel;
        
        [SerializeField] private TextMeshProUGUI woodRequirementText;
        [SerializeField] private TextMeshProUGUI metalRequirementText;
        [SerializeField] private TextMeshProUGUI clothRequirementText;
        [SerializeField] private TextMeshProUGUI paintRequirementText;

        [SerializeField] private PlayerResourcesBarUI resourceBarUI;
        [SerializeField] private TextMeshProUGUI moneyCostWhenProduce;

        [SerializeField] private BackStageViewUI backStageViewUI;
        
        [SerializeField] private Button producePropButton;
        [SerializeField] private Button recycleButton;
        [SerializeField] private Button backButton;
        
        private List<PropListItemUIObject> _propListItems = new List<PropListItemUIObject>();
        private PropListItemUIObject _selectedProp;

        private void OnEnable()
        {
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
        }

        private void SwitchToBackstageView()
        {
            this.gameObject.SetActive(false);
            
            if (_selectedProp)
            {
                _selectedProp.HintOnDeselected();
            }
            _selectedProp = null;
            
            backStageViewUI.BackStageViewHomePage();
        }
        
        private void UpdateMachineLevelUI()
        {
            if (_selectedProp)
            {
                UpdateMachineLevelText(metalMachineLevel,
                    _selectedProp.Blueprint.MachineLevelRequirement[GResourceType.Metal],
                    StorageManager.Instance.MachineLevels[GResourceType.Metal]);
                UpdateMachineLevelText(woodMachineLevel,
                    _selectedProp.Blueprint.MachineLevelRequirement[GResourceType.Wood],
                    StorageManager.Instance.MachineLevels[GResourceType.Wood]);
                UpdateMachineLevelText(clothMachineLevel,
                    _selectedProp.Blueprint.MachineLevelRequirement[GResourceType.Cloth],
                    StorageManager.Instance.MachineLevels[GResourceType.Cloth]);
                UpdateMachineLevelText(paintMachineLevel,
                    _selectedProp.Blueprint.MachineLevelRequirement[GResourceType.Paint],
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
            
            if (_selectedProp)
            {
                woodRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Wood]}(-{_selectedProp.Blueprint.ResourceConsumes[GResourceType.Wood]})";
                metalRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Metal]}(-{_selectedProp.Blueprint.ResourceConsumes[GResourceType.Metal]})";
                clothRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Cloth]}(-{_selectedProp.Blueprint.ResourceConsumes[GResourceType.Cloth]})";
                paintRequirementText.text =
                    $"{StorageManager.Instance.ResourceStorage[GResourceType.Paint]}(-{_selectedProp.Blueprint.ResourceConsumes[GResourceType.Paint]})";

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
            
            _selectedProp = null;
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
            Assert.IsNotNull(_selectedProp);
            
            // add the produced prop data to the storage manager
            if (StorageManager.Instance.TryProduceNewStageObjectViaBlueprint(_selectedProp.Blueprint, true))
            {
                // update the prop list item curNum UI
                _selectedProp.ProducedOneItem();
                // update the prop list item
                SelectOnTargetBlueprint(_selectedProp);
                // update the resources
            }
        }

        private void RecycleSelectedProp()
        {
            Assert.IsNotNull(_selectedProp);
        }

        private void AddNewStageBlueprintListItem(BaseStageObjectBlueprintSO blueprint, int curNum)
        {
             var newPropListItemUIObject = (Instantiate(propListItemPrefab, propListTransform).GetComponent<PropListItemUIObject>());
             
             _propListItems.Add(newPropListItemUIObject);
             
             var height = Mathf.Max(750.0f, _propListItems.Count * 120.0f);
             propListTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
             
             newPropListItemUIObject.HintOnDeselected();
             newPropListItemUIObject.UpdateItemUI(blueprint, curNum, SelectOnTargetBlueprint);
        }
        
        private void SelectOnTargetBlueprint(PropListItemUIObject uiObjectToSelect)
        {
            if (_selectedProp)
            {
                _selectedProp.HintOnDeselected();   
            }
            uiObjectToSelect.HintOnSelected();
            _selectedProp = uiObjectToSelect;
            // also update the stage preview panel.
            
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