using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.Testing;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.UserInterface
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
                // indicate money change
                TempUIHintManager.Instance.HintResourceChange(GResourceType.Money,
                    -PurchaseManager.Instance.ExchangePriceDictSo.ResourceExchangeDict[resourceType]);
                // indicate resource change
                TempUIHintManager.Instance.HintResourceChange(resourceType, 1);
            }
            else
            {
                IndicateNotEnoughMoney();
            }
        }

        private void IndicateNotEnoughMoney()
        {
            TempUIHintManager.Instance.HintText("Not Enough Money!");
            moneyStorageText.color = Color.gray;
        }

        public void UpdateResourcesBarUI()
        {
            moneyStorageText.text =
                $"{StorageManager.Instance.ResourceStorage[GResourceType.Money]}";
            woodStorageText.text =
                $"{StorageManager.Instance.ResourceStorage[GResourceType.Wood]}";
            metalStorageText.text =
                $"{StorageManager.Instance.ResourceStorage[GResourceType.Metal]}";
            clothStorageText.text =
                $"{StorageManager.Instance.ResourceStorage[GResourceType.Cloth]}";
            paintStorageText.text =
                $"{StorageManager.Instance.ResourceStorage[GResourceType.Paint]}";
            storageCountText.text =
                $"{StorageManager.Instance.ProducedStageObjectCount}/{StorageManager.Instance.MaxStagePropNum}";
            storageCountText.color = 
                StorageManager.Instance.ProducedStageObjectCount >= StorageManager.Instance.MaxStagePropNum ? 
                    SharedAssetsManager.Instance.PinkContrastColor : SharedAssetsManager.Instance.TextColor;
        }
    }
}