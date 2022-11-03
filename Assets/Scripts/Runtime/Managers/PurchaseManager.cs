using System;
using System.Collections.Generic;
using Runtime.ScriptableObjects;
using Runtime.UserInterface;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Runtime.Testing
{
    /// <summary>
    /// Managing the purchasing of props, actors, and orchestras
    /// May also manage codes redeems
    /// </summary>
    public class PurchaseManager : MonoBehaviour
    {
        #region Singleton

        public static PurchaseManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<PurchaseManager>();
                }

                if (!_instance)
                {
                    Debug.LogError("No Stage Editing Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }

        private static PurchaseManager _instance;

        #endregion

        private void Awake()
        {
            _instance = this;

            Assert.IsNotNull(storageManager);
        }

        #region Variables and Properties

        // resource price is immutable in this design
        // this so is immutable, however the list of purchasable blueprints are computed at runtime (load time)
        [Tooltip("This SO should only be set in the editor and not runtime.")] [SerializeField]
        private ResourceExchangePriceDictSO exchangePriceDictSo;

        [SerializeField] private MachineUpgradePriceDictSO machineUpgradePriceDictSo;

        public ResourceExchangePriceDictSO ExchangePriceDictSo => exchangePriceDictSo;

        public MachineUpgradePriceDictSO MachineUpgradePriceDictSo => machineUpgradePriceDictSo;

        // reference to the storage
        [SerializeField] private StorageManager storageManager;

        // list of purchasable stuff
        [Tooltip("This SO should only be set in the editor and not runtime.")] 
        [SerializeField] private ServerBlueprintsSO allBlueprints;

        [FormerlySerializedAs("backstagePurchaseUI")] [SerializeField] private BackstageProductionUI backstageProductionUI;

        [SerializeField] private GameObject purchasePropUIPanel;
        
        // private purchasableBlueprints
        // [SerializeField] private List<ActorData>
        // [SerializeField] private List<Orchestra>

        #endregion

        #region Public Functions

        // functions to buy new stuff

        // buy resources
        public bool TryPurchaseResource(GResourceType resourceType)
        {
            if (StorageManager.Instance.ResourceStorage[GResourceType.Money] >
                PurchaseManager.Instance.ExchangePriceDictSo.ResourceExchangeDict[resourceType])
            {
                storageManager.ResourceStorage[GResourceType.Money] -=
                    ExchangePriceDictSo.ResourceExchangeDict[resourceType];
                storageManager.ResourceStorage[resourceType]++;
                return true;
            }

            return false;
        }
        
        // buy new blueprint
        public void TryPurchaseBlueprint(BaseStageObjectBlueprintSO blueprint)
        {
            if (storageManager.TryBuyBlueprint(blueprint))
            {
                // update UIs, Lists, etc.
            }
        }

        // produce new prop
        public void TryPurchaseStageProp(BaseStageObjectBlueprintSO blueprint)
        {
            if (storageManager.TryProduceNewStageObjectViaBlueprint(blueprint))
            {
                // update UIs, Lists, etc.
            }
        }

        // TODO::recruit new actors
        // (actors may require special properties, such as you need to perform a show and earn more than 50 money in one go)
        // TODO::recruit new orchestras (also may require special properties)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns></returns>
        public bool CanBuyBlueprint(BaseStageObjectBlueprintSO blueprint)
        {
            return blueprint.MoneyToBuy <= storageManager.ResourceStorage[GResourceType.Money];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blueprint"></param>
        /// /// <param name="requireMoney"></param>
        /// <returns></returns>
        public bool CanProduceBlueprintProp(BaseStageObjectBlueprintSO blueprint, out int requireMoney)
        {
            requireMoney = storageManager.EnoughResourceToProduceStageObject(blueprint) + 
                           storageManager.EnoughMachineLevelToProduceStageObject(blueprint);
            return requireMoney <= storageManager.ResourceStorage[GResourceType.Money];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanRecruitNewActor()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanRecruitNewOrchestra()
        {
            return false;
        }
        
        #endregion

        #region Private Functions
        
        

        #endregion
    }
}