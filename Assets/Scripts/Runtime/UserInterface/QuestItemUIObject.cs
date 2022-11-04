using System;
using Runtime.Managers;
using Runtime.Testing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    public class QuestItemUIObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questTitle;
        [SerializeField] private TextMeshProUGUI questDescription;
        [SerializeField] private TextMeshProUGUI questMechanicsDescription;

        [SerializeField] private Button chooseQuestButton;

        [SerializeField] private RectTransform thisQuestChosenPanel;
        [SerializeField] private RectTransform otherQuestChosenPanel;

        public void InitializeUIObject(QuestItemData questItemData)
        {
            questTitle.text = questItemData.questTitle;
            questDescription.text = questItemData.questDescription;
            questMechanicsDescription.text = string.Empty;
            // this is not good design pattern, as this ui object is directly accessing the manager's properties.
            if (QuestManager.Instance.CurrentChosenQuest != null)
            {
                IndicateOtherQuestInProgress();
            }
            else
            {
                IndicateQuestCanBeChoose();
            }
            
            // register choose quest event 
            chooseQuestButton.onClick.RemoveAllListeners();
            chooseQuestButton.onClick.AddListener(()=>
            {
                QuestManager.Instance.TryChooseQuest(questItemData);
            });
        }

        public void IndicateQuestCanBeChoose()
        {
            thisQuestChosenPanel.gameObject.SetActive(false);
            otherQuestChosenPanel.gameObject.SetActive(false);
        }
        
        public void IndicateThisQuestInProgress()
        {
            thisQuestChosenPanel.gameObject.SetActive(true);
            otherQuestChosenPanel.gameObject.SetActive(false);
        }

        public void IndicateOtherQuestInProgress()
        {
            thisQuestChosenPanel.gameObject.SetActive(false);
            otherQuestChosenPanel.gameObject.SetActive(true);
        }
    }
}