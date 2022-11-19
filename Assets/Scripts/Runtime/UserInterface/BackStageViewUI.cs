using Runtime.Managers;
using Runtime.Testing;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    public class BackStageViewUI : MonoBehaviour
    {

        [SerializeField] private Button backToFrontStageButton;
        [SerializeField] private Button stageEditingButton;
        [SerializeField] private Button officeButton;
        [SerializeField] private Button actorLoungeButton;
        [SerializeField] private Button propProductionButton;
        [SerializeField] private Button propModificationButton;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button storageButton;

        [SerializeField] private RectTransform backStageParentPanel;
        [SerializeField] private PlayerResourcesBarUI resourcesBarUI;
        [SerializeField] private FrontStageSidePanelUI frontStageViewUI;
        [SerializeField] private StageEditingUI stageEditingUI;
        [SerializeField] private BackstageProductionUI propProducingUI;
        [SerializeField] private RectTransform backStageViewPanel;
        // [SerializeField] private RectTransform actorLoungePanel;
        // [SerializeField] private RectTransform propModificationPanel;
        // [SerializeField] private RectTransform purchasingPanel;
        // [SerializeField] private RectTransform storagePanel;
        
        private void OnEnable()
        {
            backToFrontStageButton.onClick.AddListener(BackToFrontStage);
            stageEditingButton.onClick.AddListener(StartStageEditing);
            officeButton.onClick.AddListener(OpenOfficePage);
            actorLoungeButton.onClick.AddListener(OpenActorLounge);
            propProductionButton.onClick.AddListener(StartPropProducing);
            propModificationButton.onClick.AddListener(StartPropModification);
            purchaseButton.onClick.AddListener(OpenPurchasingPage);
            storageButton.onClick.AddListener(OpenStoragePage);
        }

        private void OnDisable()
        {
            backToFrontStageButton.onClick.RemoveListener(BackToFrontStage);
            stageEditingButton.onClick.RemoveListener(StartStageEditing);
            officeButton.onClick.RemoveListener(OpenOfficePage);
            actorLoungeButton.onClick.RemoveListener(OpenActorLounge);
            propProductionButton.onClick.RemoveListener(StartPropProducing);
            propModificationButton.onClick.RemoveListener(StartPropModification);
            purchaseButton.onClick.RemoveListener(OpenPurchasingPage);
            storageButton.onClick.RemoveListener(OpenStoragePage);
        }

        public void SwitchFrontStageToBackStage()
        {
            backStageParentPanel.gameObject.SetActive(true);
            backStageViewPanel.gameObject.SetActive(true);
            StageEditingManager.Instance.NotifySwitchToBackStage();
        }

        public void BackStageViewHomePage()
        {
            backStageParentPanel.gameObject.SetActive(true);
            backStageViewPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// Close the backstage view panel and the resources UI,
        /// open the front stage viewing UI(Profile, Quest, and Visit others) with editing disabled.
        /// </summary>
        private void BackToFrontStage()
        {
            backStageParentPanel.gameObject.SetActive(false);
            frontStageViewUI.OpenFrontStageViewingPanel();
            StageEditingManager.Instance.ChangeStageEditPermission(false);
        }

        /// <summary>
        /// Close the backstage view panel and the resources UI,
        /// open the front stage editing UI with editing enabled
        /// </summary>
        private void StartStageEditing()
        {
            backStageParentPanel.gameObject.SetActive(false);
            stageEditingUI.StartStageEditingUI();
            stageEditingUI.InitializeStageEditingUIList(StorageManager.Instance.StageObjectDataList);
            StageEditingManager.Instance.ChangeStageEditPermission(true);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        private void OpenOfficePage()
        {
            
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        private void OpenActorLounge()
        {
            
        }

        /// <summary>
        /// Open the prop producing UI and close the backstage view panel
        /// </summary>
        private void StartPropProducing()
        {
            backStageViewPanel.gameObject.SetActive(false);
            propProducingUI.gameObject.SetActive(true);
            propProducingUI.StartPropProducing();
        }

        /// <summary>
        /// Open the prop modification UI and close the backstage view panel
        /// </summary>
        private void StartPropModification()
        {
            // backStageViewPanel.gameObject.SetActive(false);
            //
        }

        /// <summary>
        /// Open the purchasing page UI and close the backstage view panel
        /// </summary>
        private void OpenPurchasingPage()
        {
            // backStageViewPanel.gameObject.SetActive(false);
            //
        }

        /// <summary>
        /// Open the storage page UI and close the backstage view panel
        /// </summary>
        private void OpenStoragePage()
        {
            // backStageViewPanel.gameObject.SetActive(false);
            //
        }
    }
}