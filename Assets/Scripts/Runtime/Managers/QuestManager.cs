using System.Collections.Generic;
using Runtime.ScriptableObjects;
using Runtime.Testing;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.Managers
{
    public class QuestManager : MonoBehaviour
    {
        #region Singleton

        public static QuestManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<QuestManager>();
                }

                if (!_instance)
                {
                    Debug.LogError("No Quest Manager Exist, Please check the scene.");
                }

                return _instance;
            }
        }

        private static QuestManager _instance;

        #endregion

        [SerializeField] private List<QuestItemSO> possibleQuests;

        [SerializeField] private List<QuestItemData> playerQuests = new List<QuestItemData>();
        public List<QuestItemData> PlayerQuests => playerQuests;

        private QuestItemData _currentChosenQuest;
        public QuestItemData CurrentChosenQuest => _currentChosenQuest;

        [SerializeField] private QuestPanelUI questPanelUI;

        public void FindQuest()
        {
            if (possibleQuests.Count > 0)
            {
                int randIndex = Random.Range(0, possibleQuests.Count - 1);
                var questItemSO = possibleQuests[randIndex];
                var newRuntimeQuestData = new QuestItemData()
                {
                    questTitle = questItemSO.questTitle,
                    questDescription = questItemSO.questDescription,
                };
                
                AddNewQuest(newRuntimeQuestData);
            }
        }
        
        public void AddNewQuest(QuestItemData newQuest)
        {
            if (playerQuests.Contains(newQuest))
            {
                return;
            }

            // max quest num - 3
            if (playerQuests.Count >= 3)
            {
                return;
            }
            
            playerQuests.Add(newQuest);
            questPanelUI.AddNewQuest(newQuest); // ui
        }
        
        public void TryChooseQuest(QuestItemData quest)
        {
            if (_currentChosenQuest != null)
            {
                return;
            }
            _currentChosenQuest = quest;
            questPanelUI.ChooseExistingQuest(quest); // ui
        }

        public void SubmitChosenQuest()
        {
            if (_currentChosenQuest != null)
            {
                // todo:: calculate the income considering more conditions, right now it's just random number
                // todo:: resources update should be represented by ui animations
                int randIncome = Random.Range(8, 16);
                StorageManager.Instance.ResourceStorage[GResourceType.Money] += randIncome;
                TempUIHintManager.Instance.HintText($"Quest Submitted. You received {randIncome} money!");
                questPanelUI.RemoveExistingQuest(_currentChosenQuest); // ui
                playerQuests.Remove(_currentChosenQuest);
                _currentChosenQuest = null;
            }
        }
    }
}
