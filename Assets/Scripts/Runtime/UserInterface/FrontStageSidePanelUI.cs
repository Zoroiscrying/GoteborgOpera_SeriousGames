using Runtime.Testing;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    public class FrontStageSidePanelUI : MonoBehaviour
    {
        [SerializeField] private BackStageViewUI backStageViewUI;

        [SerializeField] private RectTransform frontStageViewingPanel;
    
        [SerializeField] private Button sidePanelExpandButton;
        [SerializeField] private Sprite spriteUnexpanded;
        [SerializeField] private Sprite spriteExpanded;
        
        [SerializeField] private Button personalProfileButton;
        [SerializeField] private Button visitOperasButton;
        [SerializeField] private Button goToBackstageButton;
        [SerializeField] private Button questsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button settingsButton;
        [FormerlySerializedAs("shareButton")] [SerializeField] private Button purchaseButton;
        [SerializeField] private Button aboutButton;

        [SerializeField] private RectTransform sideExpandedPanel;

        private void OnEnable()
        {
            sidePanelExpandButton.onClick.AddListener(ToggleExpandedPanel);
            personalProfileButton.onClick.AddListener(OpenProfilePage);
            visitOperasButton.onClick.AddListener(VisitOtherOpera);
            goToBackstageButton.onClick.AddListener(SwitchToBackstage);
            questsButton.onClick.AddListener(OpenQuestsPage);
            settingsButton.onClick.AddListener(SettingsPanel);
            purchaseButton.onClick.AddListener(PurchasePanel);
            aboutButton.onClick.AddListener(AboutPanel);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            sidePanelExpandButton.onClick.RemoveListener(ToggleExpandedPanel);
            personalProfileButton.onClick.RemoveListener(OpenProfilePage);
            visitOperasButton.onClick.RemoveListener(VisitOtherOpera);
            goToBackstageButton.onClick.RemoveListener(SwitchToBackstage);
            questsButton.onClick.RemoveListener(OpenQuestsPage);
            settingsButton.onClick.RemoveListener(SettingsPanel);
            purchaseButton.onClick.RemoveListener(PurchasePanel);
            aboutButton.onClick.RemoveListener(AboutPanel);
            quitButton.onClick.RemoveListener(QuitGame);
        }

        public void OpenFrontStageViewingPanel()
        {
            frontStageViewingPanel.gameObject.SetActive(true);
            sideExpandedPanel.gameObject.SetActive(false);
        }
        
        private void ToggleExpandedPanel()
        {
            bool targetActive = !sideExpandedPanel.gameObject.activeSelf;
            sideExpandedPanel.gameObject.SetActive(targetActive);
            sidePanelExpandButton.GetComponent<Image>().sprite = targetActive ? spriteExpanded : spriteUnexpanded;
        }

        private void OpenProfilePage()
        {
            TempUIHintManager.Instance.HintText("You can customize your profile in this page.");   
        }

        private void VisitOtherOpera()
        {
            TempUIHintManager.Instance.HintText("You can visit other operas in this page and receive rewards accordingly.");   
        }

        private void SwitchToBackstage()
        {
            frontStageViewingPanel.gameObject.SetActive(false);
            backStageViewUI.SwitchFrontStageToBackStage();
        }

        private void AboutPanel()
        {
            // this is going to open up the developers panel
            
        }

        private void OpenQuestsPage()
        {
            // this is implemented via editor button event
        }

        private void QuitGame()
        {
            // save any game data here
            
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SettingsPanel()
        {
            TempUIHintManager.Instance.HintText("You can control the audio/game/other settings in this panel");   
        }

        private void PurchasePanel()
        {
            TempUIHintManager.Instance.HintText("In purchase panel, you can see the blueprints you can buy.");   
        }
    }
}