using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.Testing
{
    public class PlayerResourcesBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyStorageText;
        [SerializeField] private TextMeshProUGUI woodStorageText;
        [SerializeField] private TextMeshProUGUI metalStorageText;
        [SerializeField] private TextMeshProUGUI clothStorageText;
        [SerializeField] private TextMeshProUGUI paintStorageText;

        [SerializeField] private TextMeshProUGUI storageCountText;
        
        [SerializeField] private Button buyWoodButton;
        [SerializeField] private Button buyMetalButton;
        [SerializeField] private Button buyClothButton;
        [SerializeField] private Button buyPaintButton;

        private void OnEnable()
        {
            Assert.IsNotNull(moneyStorageText);
            Assert.IsNotNull(woodStorageText);
            Assert.IsNotNull(metalStorageText);
            Assert.IsNotNull(clothStorageText);
            Assert.IsNotNull(paintStorageText);
            Assert.IsNotNull(storageCountText);
            Assert.IsNotNull(buyWoodButton);
            Assert.IsNotNull(buyMetalButton);
            Assert.IsNotNull(buyClothButton);
            Assert.IsNotNull(buyPaintButton);
            
            buyWoodButton.onClick.AddListener(TryBuyWood);
            buyMetalButton.onClick.AddListener(TryBuyMetal);
            buyClothButton.onClick.AddListener(TryBuyCloth);
            buyPaintButton.onClick.AddListener(TryBuyPaint);
            
            UpdateResourcesBarUI();
        }

        private void OnDisable()
        {
            buyWoodButton.onClick.RemoveListener(TryBuyWood);
            buyMetalButton.onClick.RemoveListener(TryBuyMetal);
            buyClothButton.onClick.RemoveListener(TryBuyCloth);
            buyPaintButton.onClick.RemoveListener(TryBuyPaint);
        }

        private void TryBuyWood() { TryBuyResources(GResourceType.Wood); }
        private void TryBuyMetal() { TryBuyResources(GResourceType.Metal); }
        private void TryBuyCloth() { TryBuyResources(GResourceType.Cloth); }
        private void TryBuyPaint() { TryBuyResources(GResourceType.Paint); }

        private void TryBuyResources(GResourceType resourceType)
        {
            // have enough money
            if (PurchaseManager.Instance.TryPurchaseResource(resourceType))
            {
                UpdateResourcesBarUI();
            }
            else
            {
                IndicateNotEnoughMoney();
            }
        }

        private void IndicateNotEnoughMoney()
        {
            moneyStorageText.color = Color.gray;
        }

        public void UpdateResourcesBarUI()
        {
            moneyStorageText.text =
                $"Money:{StorageManager.Instance.ResourceStorage[GResourceType.Money]}";
            woodStorageText.text =
                $"Wood:{StorageManager.Instance.ResourceStorage[GResourceType.Wood]}";
            metalStorageText.text =
                $"Metal:{StorageManager.Instance.ResourceStorage[GResourceType.Metal]}";
            clothStorageText.text =
                $"Cloth:{StorageManager.Instance.ResourceStorage[GResourceType.Cloth]}";
            paintStorageText.text =
                $"Paint:{StorageManager.Instance.ResourceStorage[GResourceType.Paint]}";
            storageCountText.text =
                $"Storage {StorageManager.Instance.StageObjectDataList.Count}/{StorageManager.Instance.MaxStagePropNum}";
            storageCountText.color = 
                StorageManager.Instance.StageObjectDataList.Count >= StorageManager.Instance.MaxStagePropNum ? Color.red : Color.black;
        }
    }
}