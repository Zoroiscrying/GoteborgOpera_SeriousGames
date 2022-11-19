using Runtime.Testing;
using UnityEngine;
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
        [SerializeField] private Button shareButton;
        // [SerializeField] private Button unknownButton;

        [SerializeField] private RectTransform sideExpandedPanel;

        private void OnEnable()
        {
            sidePanelExpandButton.onClick.AddListener(ToggleExpandedPanel);
            personalProfileButton.onClick.AddListener(OpenProfilePage);
            visitOperasButton.onClick.AddListener(VisitOtherOpera);
            goToBackstageButton.onClick.AddListener(SwitchToBackstage);
            questsButton.onClick.AddListener(OpenQuestsPage);
            settingsButton.onClick.AddListener(SettingsPanel);
            shareButton.onClick.AddListener(ShareGame);
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
            shareButton.onClick.RemoveListener(ShareGame);
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
            TempUIHintManager.Instance.HintText("Content to be developed :)");   
        }

        private void VisitOtherOpera()
        {
            TempUIHintManager.Instance.HintText("Content to be developed :)");   
        }

        private void SwitchToBackstage()
        {
            frontStageViewingPanel.gameObject.SetActive(false);
            backStageViewUI.SwitchFrontStageToBackStage();
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
            TempUIHintManager.Instance.HintText("Content to be developed :)");   
        }

        private void ShareGame()
        {
            TempUIHintManager.Instance.HintText("Content to be developed :)");   
        }
    }
}