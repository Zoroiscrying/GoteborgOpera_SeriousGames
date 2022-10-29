using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Testing
{
    public class PropProduceMachineLevelsUI : MonoBehaviour
    {
        [SerializeField] private PlayerResourcesBarUI resourceBarUI;
        
        [SerializeField] private TextMeshProUGUI woodMachineLevel;
        [SerializeField] private TextMeshProUGUI metalMachineLevel;
        [SerializeField] private TextMeshProUGUI clothMachineLevel;
        [SerializeField] private TextMeshProUGUI paintMachineLevel;

        [SerializeField] private TextMeshProUGUI moneyToUpgradeWoodMachine;
        [SerializeField] private TextMeshProUGUI moneyToUpgradeMetalMachine;
        [SerializeField] private TextMeshProUGUI moneyToUpgradeClothMachine;
        [SerializeField] private TextMeshProUGUI moneyToUpgradePaintMachine;
        
        [SerializeField] private Button upgradeWoodMachineButton;
        [SerializeField] private Button upgradeMetalMachineButton;
        [SerializeField] private Button upgradeClothMachineButton;
        [SerializeField] private Button upgradePaintMachineButton;

        private void OnEnable()
        {
            upgradeWoodMachineButton.onClick.AddListener(TryUpgradeWoodMachine);
            upgradeMetalMachineButton.onClick.AddListener(TryUpgradeMetalMachine);
            upgradeClothMachineButton.onClick.AddListener(TryUpgradeClothMachine);
            upgradePaintMachineButton.onClick.AddListener(TryUpgradePaintMachine);
            UpdateMachineLevels();
        }

        private void OnDisable()
        {
            upgradeWoodMachineButton.onClick.RemoveListener(TryUpgradeWoodMachine);
            upgradeMetalMachineButton.onClick.RemoveListener(TryUpgradeMetalMachine);
            upgradeClothMachineButton.onClick.RemoveListener(TryUpgradeClothMachine);
            upgradePaintMachineButton.onClick.RemoveListener(TryUpgradePaintMachine);
        }

        private void TryUpgradeWoodMachine()
        {
            if (StorageManager.Instance.TryUpgradeWoodMachine())
            {
                UpdateMachineLevels();
            }
            else
            {
                IndicateNotEnoughMoney();
            }
        }
        
        private void TryUpgradeMetalMachine()
        {
            if (StorageManager.Instance.TryUpgradeMetalMachine())
            {
                UpdateMachineLevels();
            }
            else
            {
                IndicateNotEnoughMoney();
            }
        }
        
        private void TryUpgradeClothMachine()
        {
            if (StorageManager.Instance.TryUpgradeClothMachine())
            {
                UpdateMachineLevels();
            }
            else
            {
                IndicateNotEnoughMoney();
            }
        }
        
        private void TryUpgradePaintMachine()
        {
            if (StorageManager.Instance.TryUpgradePaintMachine())
            {
                UpdateMachineLevels();
            }
            else
            {
                IndicateNotEnoughMoney();
            }
        }

        private void IndicateNotEnoughMoney()
        {
            // not implemented here, should be implemented else where...
        }

        private void UpdateMachineLevels()
        {
            int woodLevel = StorageManager.Instance.MachineLevels[GResourceType.Wood];
            int metalLevel = StorageManager.Instance.MachineLevels[GResourceType.Metal];
            int clothLevel = StorageManager.Instance.MachineLevels[GResourceType.Cloth];
            int paintLevel = StorageManager.Instance.MachineLevels[GResourceType.Paint];
            
            woodMachineLevel.text = woodLevel.ToString();
            metalMachineLevel.text = metalLevel.ToString();
            clothMachineLevel.text = clothLevel.ToString();
            paintMachineLevel.text = paintLevel.ToString();

            // if it's max level, disable the money cost of upgrading machine
            if (PurchaseManager.Instance.MachineUpgradePriceDictSo.IsMaximumMachineLevel(GResourceType.Wood, woodLevel))
            {
                moneyToUpgradeWoodMachine.text = "";
            }
            else
            {
                moneyToUpgradeWoodMachine.text =
                    PurchaseManager.Instance.MachineUpgradePriceDictSo.CalculateUpgradeCost(GResourceType.Wood,
                        StorageManager.Instance.MachineLevels[GResourceType.Wood]).ToString();   
            }
            
            if (PurchaseManager.Instance.MachineUpgradePriceDictSo.IsMaximumMachineLevel(GResourceType.Metal, metalLevel))
            {
                moneyToUpgradeMetalMachine.text = "";
            }
            else
            {
                moneyToUpgradeMetalMachine.text =
                    PurchaseManager.Instance.MachineUpgradePriceDictSo.CalculateUpgradeCost(GResourceType.Metal,
                        StorageManager.Instance.MachineLevels[GResourceType.Metal]).ToString();
            }
            
            if (PurchaseManager.Instance.MachineUpgradePriceDictSo.IsMaximumMachineLevel(GResourceType.Cloth, clothLevel))
            {
                moneyToUpgradeClothMachine.text = "";
            }
            else
            {
                moneyToUpgradeClothMachine.text =
                    PurchaseManager.Instance.MachineUpgradePriceDictSo.CalculateUpgradeCost(GResourceType.Cloth,
                        StorageManager.Instance.MachineLevels[GResourceType.Cloth]).ToString();
            }
            
            if (PurchaseManager.Instance.MachineUpgradePriceDictSo.IsMaximumMachineLevel(GResourceType.Paint, paintLevel))
            {
                moneyToUpgradePaintMachine.text = "";
            }
            else
            {
                moneyToUpgradePaintMachine.text =
                    PurchaseManager.Instance.MachineUpgradePriceDictSo.CalculateUpgradeCost(GResourceType.Paint,
                        StorageManager.Instance.MachineLevels[GResourceType.Paint]).ToString();
            }
            
            resourceBarUI.UpdateResourcesBarUI();
        }
    }
}