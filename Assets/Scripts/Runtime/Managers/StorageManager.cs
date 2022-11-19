using System;
using System.Collections.Generic;
using Runtime.DataStructures;
using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.UserInterface;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Runtime.Testing
{
    /// <summary>
    /// Manage the storage of a player, including resources, prop list, actors, and so on.
    /// Manage storage maximum of props, actors, and orchestra (?)
    /// 
    /// - Resources - Numbers
    /// - Stage Data List - List<BaseStageObjectData>
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
        [SerializeField] private List<BaseStageObjectBlueprintSO> ownedBlueprints;
        public List<BaseStageObjectBlueprintSO> OwnedBlueprints => ownedBlueprints;
        
        // This list should be serialized for players
        [SerializeField] private List<BaseStageObjectData> stageObjectDataList = new List<BaseStageObjectData>();
        public List<BaseStageObjectData> StageObjectDataList => stageObjectDataList;
        public int ProducedStageObjectCount =>
            stageObjectDataList.Count + StageEditingManager.Instance.StageObjectsInstantiated.Count;

        /// <summary>
        /// For producing stage objects through blueprints
        /// </summary>
        private Dictionary<BaseStageObjectBlueprintSO, List<BaseStageObjectData>> _blueprintObjectDict = 
            new Dictionary<BaseStageObjectBlueprintSO, List<BaseStageObjectData>>();
        public Dictionary<BaseStageObjectBlueprintSO, List<BaseStageObjectData>> BlueprintObjectDict => _blueprintObjectDict;

        private StageEditingUI StageEditingUI
        {
            get
            {
                if (stageEditingUI == null)
                {
                    stageEditingUI = FindObjectOfType<StageEditingUI>();
                }
                Assert.IsNotNull(stageEditingUI);
                return stageEditingUI;
            }
        } 
        [SerializeField] private StageEditingUI stageEditingUI;
        
        #endregion

        // TODO:: Save and Load Interface
        private void Awake()
        {
            _instance = this;
            foreach (var stageObject in stageObjectDataList)
            {
                if (stageObject is not PropStageObjectData propStageObjectData)
                {
                    continue;   
                }
                AddBlueprintStageObjectDict(propStageObjectData);
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
            
            StageEditingUI.InitializeStageEditingUIList(stageObjectDataList);
        }
        
        public void RemoveStageObjectData(BaseStageObjectData stageObjectData)
        {
            stageObjectDataList.Remove(stageObjectData);
            stageObjectDataList.Sort();
            
            StageEditingUI.InitializeStageEditingUIList(stageObjectDataList);
        }

        /// <summary>
        /// Add a new stage object to the storage list
        /// </summary>
        /// <param name="newStageObjectBluePrint"></param>
        /// <param name="useMoneyIfResourcesLack"></param>
        /// <returns>Whether or not adding prop is succeeded</returns>
        public bool TryProduceNewStageObjectViaBlueprint(
            BaseStageObjectBlueprintSO newStageObjectBluePrint, 
            bool useMoneyIfResourcesLack = false)
        {
            if (ProducedStageObjectCount >= maxStagePropNum)
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
                    TempUIHintManager.Instance.HintText("Storage is Full!");   
                    return false;   
                }
            }
            
            // add default initialized stage object data
            BaseStageObjectData newData = null;
            switch (newStageObjectBluePrint.ObjectDataType)
            {
                case StageObjectType.None:
                    Debug.LogError("This object is declared as None Type!");
                    break;
                case StageObjectType.Prop:
                    newData = new PropStageObjectData()
                    {
                        objectName = newStageObjectBluePrint.BlueprintName + "_" + System.Guid.NewGuid().ToString(),
                        baseStageObjectBlueprintSO = newStageObjectBluePrint
                    };
                    break;
                case StageObjectType.Actor:
                    newData = new ActorStageObjectData()
                    {
                        objectName = newStageObjectBluePrint.BlueprintName + "_" + System.Guid.NewGuid().ToString(),
                        baseStageObjectBlueprintSO = newStageObjectBluePrint
                    };
                    break;
                case StageObjectType.Orchestra:
                    newData = new OrchestraStageObjectData()
                    {
                        objectName = newStageObjectBluePrint.BlueprintName + "_" + System.Guid.NewGuid().ToString(),
                        baseStageObjectBlueprintSO = newStageObjectBluePrint
                    };
                    break;
                case StageObjectType.Effect:
                    newData = new EffectStageObjectData()
                    {
                        objectName = newStageObjectBluePrint.BlueprintName + "_" + System.Guid.NewGuid().ToString(),
                        baseStageObjectBlueprintSO = newStageObjectBluePrint
                    };
                    break;
                case StageObjectType.Scenery:
                    newData = new SceneryStageObjectData()
                    {
                        objectName = newStageObjectBluePrint.BlueprintName + "_" + System.Guid.NewGuid().ToString(),
                        baseStageObjectBlueprintSO = newStageObjectBluePrint
                    };
                    break;
                case StageObjectType.Light:
                    newData = new LightStageObjectData()
                    {
                        objectName = newStageObjectBluePrint.BlueprintName + "_" + System.Guid.NewGuid().ToString(),
                        baseStageObjectBlueprintSO = newStageObjectBluePrint
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (newData != null)
            {
                // add the new data to the list and sort
                AddStageObjectData(newData);
                // update the blueprint dictionary to help with the prop building system ui
                AddBlueprintStageObjectDict(newData);   
                // todo:: update UI accordingly.
                return true;
            }

            Debug.LogError($"Blueprint Failed to Initialize: {newStageObjectBluePrint.BlueprintName}");
            return false;
        }
        
        public bool TryUpgradeWoodMachine() { return TryUpgradeMachineLevel(GResourceType.Wood); }
        public bool TryUpgradeMetalMachine() { return TryUpgradeMachineLevel(GResourceType.Metal); }
        public bool TryUpgradeClothMachine() { return TryUpgradeMachineLevel(GResourceType.Cloth); }
        public bool TryUpgradePaintMachine() { return TryUpgradeMachineLevel(GResourceType.Paint); }

        public bool TryBuyBlueprint(BaseStageObjectBlueprintSO blueprint)
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
        #endregion

        /// <summary>
        /// Check whether or not player has enough resources to buy this prop
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns>The extra money needed to buy the prop</returns>
        public int EnoughResourceToProduceStageObject(BaseStageObjectBlueprintSO blueprint)
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
        
        public int EnoughMachineLevelToProduceStageObject(BaseStageObjectBlueprintSO blueprint)
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
        private void AddBlueprintStageObjectDict(BaseStageObjectData objectData)
        {
            if (_blueprintObjectDict.ContainsKey(objectData.baseStageObjectBlueprintSO))
            {
                if (!_blueprintObjectDict[objectData.baseStageObjectBlueprintSO].Contains(objectData))
                {
                    _blueprintObjectDict[objectData.baseStageObjectBlueprintSO].Add(objectData);   
                }
            }
            else
            {
                _blueprintObjectDict.Add(objectData.baseStageObjectBlueprintSO, 
                    new List<BaseStageObjectData>{objectData});
            }
        }
        
        private void RemoveBlueprintStageObjectDict(BaseStageObjectData objectData)
        {
            if (_blueprintObjectDict.ContainsKey(objectData.baseStageObjectBlueprintSO))
            {
                if (!_blueprintObjectDict[objectData.baseStageObjectBlueprintSO].Contains(objectData))
                {
                    _blueprintObjectDict[objectData.baseStageObjectBlueprintSO].Remove(objectData);   
                }
            }
        }
        
        /// <summary>
        /// Buy one prop with the corresponding blueprint and reduce the resources accordingly
        /// </summary>
        /// <param name="blueprint"></param>
        private void UpdateResourceBuildStageObject(BaseStageObjectBlueprintSO blueprint)
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
        
        private void UpdateMachineLevelBuildStageObject(BaseStageObjectBlueprintSO blueprint)
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