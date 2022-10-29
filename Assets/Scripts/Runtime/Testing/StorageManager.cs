using System;
using System.Collections.Generic;
using Runtime.DataStructures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace Runtime.Testing
{
    /// <summary>
    /// Manage the storage of a player, including resources, prop list, actors, and so on.
    /// Manage storage maximum of props, actors, and orchestra (?)
    /// 
    /// - Resources - Numbers
    /// - Prop List - List<StageObjectData>
    /// - Actor List - List<ActorData>
    /// - Orchestra List - List<OrchestraData>
    /// - All other data belonged to the whole server are represented by Scriptable Objects.
    /// </summary>
    public class StorageManager : MonoBehaviour
    {
        #region Singleton

        public static StorageManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<StorageManager>();
                }

                if (!_instance)
                {
                    Debug.LogError("No Stage Editing Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }
        private static StorageManager _instance;

        #endregion

        #region Variables and Properties

        public IDictionary<GResourceType, int> ResourceStorage => resourceStorage;
        [SerializeField] private GenericDictionary<GResourceType, int> resourceStorage;

        public IDictionary<GResourceType, int> MachineLevels => machineLevels;

        [SerializeField] private GenericDictionary<GResourceType, int> machineLevels;
        /*
         *  new()
            {
                { GResourceType.Money, 1},
                { GResourceType.Wood, 0},
                { GResourceType.Metal, 0},
                { GResourceType.Cloth, 0},
                { GResourceType.Paint, 0},
            }
         */

        [SerializeField] private int maxStagePropNum = 10;
        public int MaxStagePropNum => maxStagePropNum;

        // This list should be serialized for players
        [SerializeField] private List<StagePropBlueprintScriptableObject> ownedBlueprints;
        public List<StagePropBlueprintScriptableObject> OwnedBlueprints => ownedBlueprints;
        
        // This list should be serialized for players
        [SerializeField] private List<BaseStageObjectData> stageObjectDataList = new List<BaseStageObjectData>();
        public List<BaseStageObjectData> StageObjectDataList => stageObjectDataList;

        private Dictionary<StagePropBlueprintScriptableObject, List<BaseStageObjectData>> _blueprintObjectDict = 
            new Dictionary<StagePropBlueprintScriptableObject, List<BaseStageObjectData>>();
        public Dictionary<StagePropBlueprintScriptableObject, List<BaseStageObjectData>> BlueprintObjectDict => _blueprintObjectDict;

        private StageEditingUI StageEditingUI
        {
            get
            {
                if (_stageEditingUI == null)
                {
                    _stageEditingUI = FindObjectOfType<StageEditingUI>();
                }
                Assert.IsNotNull(_stageEditingUI);
                return _stageEditingUI;
            }
        }
        private StageEditingUI _stageEditingUI;
        
        #endregion

        // TODO:: Save and Load Interface
        private void Awake()
        {
            _instance = this;
            foreach (var stageObject in stageObjectDataList)
            {
                AddBlueprintPropDict(stageObject);
            }

            foreach (var blueprint in ownedBlueprints)
            {
                if (!_blueprintObjectDict.ContainsKey(blueprint))
                {
                    _blueprintObjectDict.Add(blueprint, new List<BaseStageObjectData>());
                }
            }
        }

        #region Public Function Interfaces

        public void AddStageObjectData(BaseStageObjectData stageObjectData)
        {
            stageObjectDataList.Add(stageObjectData);
            stageObjectDataList.Sort();
            
            StageEditingUI.InitializeStageEditingUI(stageObjectDataList);
        }
        
        public void RemoveStageObjectData(BaseStageObjectData stageObjectData)
        {
            stageObjectDataList.Remove(stageObjectData);
            stageObjectDataList.Sort();
            
            StageEditingUI.InitializeStageEditingUI(stageObjectDataList);
        }

        /// <summary>
        /// Add a new stage object to the storage list
        /// </summary>
        /// <param name="newStageObjectBluePrint"></param>
        /// <param name="useMoneyIfResourcesLack"></param>
        /// <returns>Whether or not adding prop is succeeded</returns>
        public bool TryProduceNewProp(
            StagePropBlueprintScriptableObject newStageObjectBluePrint, 
            bool useMoneyIfResourcesLack = false)
        {
            if (stageObjectDataList.Count >= maxStagePropNum)
            {
                return false;
            }

            int requireExtraMoneyToBuyResource = EnoughResourceToProduceStageObject(newStageObjectBluePrint);

            // have enough machine level
            int requireExtraMoneyToUpgradeMachine = EnoughMachineLevelToProduceStageObject(newStageObjectBluePrint);

            int requireMoney = requireExtraMoneyToBuyResource + requireExtraMoneyToUpgradeMachine;
            
            // have enough money
            if (requireMoney <= resourceStorage[GResourceType.Money])
            {
                if (useMoneyIfResourcesLack)
                {
                    UpdateResourceBuildStageObject(newStageObjectBluePrint);
                    UpdateMachineLevelBuildStageObject(newStageObjectBluePrint);
                }
                else
                {
                    //todo:: notify that money can be used to build the prop
                    return false;   
                }
            }
            
            // add default initialized stage object data
            BaseStageObjectData newData = new BaseStageObjectData
            {
                objectName = newStageObjectBluePrint.BlueprintName + System.Guid.NewGuid().ToString(),
                stagePropBlueprintScriptableObject = newStageObjectBluePrint
            };
            // add the new data to the list and sort
            AddStageObjectData(newData);
            // update the blueprint dictionary to help with the prop building system ui
            AddBlueprintPropDict(newData);
            
            // todo:: update UI accordingly.
            return true;
        }
        
        public bool TryUpgradeWoodMachine() { return TryUpgradeMachineLevel(GResourceType.Wood); }
        public bool TryUpgradeMetalMachine() { return TryUpgradeMachineLevel(GResourceType.Metal); }
        public bool TryUpgradeClothMachine() { return TryUpgradeMachineLevel(GResourceType.Cloth); }
        public bool TryUpgradePaintMachine() { return TryUpgradeMachineLevel(GResourceType.Paint); }

        public bool TryBuyBlueprint(StagePropBlueprintScriptableObject blueprint)
        {
            int moneyToBuy = blueprint.MoneyToBuy;

            // have enough money
            if (moneyToBuy <= resourceStorage[GResourceType.Money])
            {
                resourceStorage[GResourceType.Money] -= moneyToBuy;
                // add blueprint to storage
                if (!ownedBlueprints.Contains(blueprint))
                {
                    ownedBlueprints.Add(blueprint);
                    ownedBlueprints.Sort();
                    // todo:: update UI accordingly.
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TryBuyNewActor()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TryBuyNewOrchestra()
        {
            return false;
        }

        #endregion

        /// <summary>
        /// Check whether or not player has enough resources to buy this prop
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns>The extra money needed to buy the prop</returns>
        public int EnoughResourceToProduceStageObject(StagePropBlueprintScriptableObject blueprint)
        {
            int requireMoney = 0;
            foreach (var resourceType in Enum.GetValues(typeof(GResourceType)))
            {
                GResourceType type = (GResourceType)resourceType;
                if (type == GResourceType.Money)
                {
                    requireMoney += blueprint.ResourceConsumes[GResourceType.Money];
                }
                else
                {
                    if (resourceStorage[type] < blueprint.ResourceConsumes[type])
                    {
                        requireMoney += (blueprint.ResourceConsumes[type] - resourceStorage[type]) * 
                                        PurchaseManager.Instance.ExchangePriceDictSo.ResourceExchangeDict[type];
                    }   
                }
            }
            return requireMoney;
        }
        
        public int EnoughMachineLevelToProduceStageObject(StagePropBlueprintScriptableObject blueprint)
        {
            int requireMoney = 0;
            foreach (var resourceType in Enum.GetValues(typeof(GResourceType)))
            {
                GResourceType type = (GResourceType)resourceType;
                if (type != GResourceType.Money)
                {
                    if (machineLevels[type] < blueprint.MachineLevelRequirement[type])
                    {
                        int tempMachineLevel = machineLevels[type];
                        // if the maximum machine level is reachable
                        if (blueprint.MachineLevelRequirement[type] <= 
                            PurchaseManager.Instance.MachineUpgradePriceDictSo.MachineUpgradeCost[type].Count)
                        {
                            // add up the machine levels and calculate the overall cost to upgrade
                            while (tempMachineLevel < blueprint.MachineLevelRequirement[type])
                            {
                                requireMoney +=
                                    PurchaseManager.Instance.MachineUpgradePriceDictSo.MachineUpgradeCost[type][
                                        tempMachineLevel];
                                tempMachineLevel++;
                            }   
                        }
                    }   
                }
            }
            return requireMoney;
        }

        private bool TryUpgradeMachineLevel(GResourceType resourceType)
        {
            int curMachineLevel = machineLevels[resourceType];
            int moneyRequired =
                PurchaseManager.Instance.MachineUpgradePriceDictSo.CalculateUpgradeCost(resourceType, curMachineLevel);
            // enough money to upgrade
            if (resourceStorage[GResourceType.Money] >= moneyRequired)
            {
                machineLevels[resourceType]++;
                resourceStorage[GResourceType.Money] -= moneyRequired;
                return true;    
            }

            return false;
        }
        private void AddBlueprintPropDict(BaseStageObjectData objectData)
        {
            if (_blueprintObjectDict.ContainsKey(objectData.stagePropBlueprintScriptableObject))
            {
                if (!_blueprintObjectDict[objectData.stagePropBlueprintScriptableObject].Contains(objectData))
                {
                    _blueprintObjectDict[objectData.stagePropBlueprintScriptableObject].Add(objectData);   
                }
            }
            else
            {
                _blueprintObjectDict.Add(objectData.stagePropBlueprintScriptableObject, 
                    new List<BaseStageObjectData>{objectData});
            }
        }
        
        private void RemoveBlueprintPropDict(BaseStageObjectData objectData)
        {
            if (_blueprintObjectDict.ContainsKey(objectData.stagePropBlueprintScriptableObject))
            {
                if (!_blueprintObjectDict[objectData.stagePropBlueprintScriptableObject].Contains(objectData))
                {
                    _blueprintObjectDict[objectData.stagePropBlueprintScriptableObject].Remove(objectData);   
                }
            }
        }
        
        /// <summary>
        /// Buy one prop with the corresponding blueprint and reduce the resources accordingly
        /// </summary>
        /// <param name="blueprint"></param>
        private void UpdateResourceBuildStageObject(StagePropBlueprintScriptableObject blueprint)
        {
            int requireMoney = 0;
            foreach (var resourceType in Enum.GetValues(typeof(GResourceType)))
            {
                GResourceType type = (GResourceType)resourceType;
                if (resourceStorage[type] < blueprint.ResourceConsumes[type])
                {
                    requireMoney += (blueprint.ResourceConsumes[type] - resourceStorage[type]) * 
                                    PurchaseManager.Instance.ExchangePriceDictSo.ResourceExchangeDict[type];
                }
                resourceStorage[type] = Mathf.Max(0, resourceStorage[type] - blueprint.ResourceConsumes[type]);
            }
            resourceStorage[GResourceType.Money] -= requireMoney;
        }
        
        private void UpdateMachineLevelBuildStageObject(StagePropBlueprintScriptableObject blueprint)
        {
            int requireMoney = 0;
            foreach (var resourceType in Enum.GetValues(typeof(GResourceType)))
            {
                GResourceType type = (GResourceType)resourceType;
                if (type != GResourceType.Money)
                {
                    if (machineLevels[type] < blueprint.MachineLevelRequirement[type])
                    {
                        // if the maximum machine level is reachable
                        if (blueprint.MachineLevelRequirement[type] <= 
                            PurchaseManager.Instance.MachineUpgradePriceDictSo.MachineUpgradeCost[type].Count)
                        {
                            // add up the machine levels and calculate the overall cost to upgrade
                            while (machineLevels[type] < blueprint.MachineLevelRequirement[type])
                            {
                                requireMoney +=
                                    PurchaseManager.Instance.MachineUpgradePriceDictSo.MachineUpgradeCost[type][
                                        machineLevels[type]];
                                machineLevels[type]++;
                            }   
                        }
                    }   
                }
            }
            resourceStorage[GResourceType.Money] -= requireMoney;
        }
    }
}