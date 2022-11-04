using System;
using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Testing;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    public class QuestPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject questUIObjectPrefab;
        [SerializeField] private RectTransform questPanelParent;

        [SerializeField] private Button submitQuestButton;
        [SerializeField] private Button findNewQuestButton;

        private Dictionary<QuestItemData, QuestItemUIObject> _questItemDictionary =
            new Dictionary<QuestItemData, QuestItemUIObject>();

        private bool _initialized = false;

        private void OnEnable()
        {
            submitQuestButton.onClick.AddListener(TrySubmitQuest);
            findNewQuestButton.onClick.AddListener(TryFindNewQuest);
            if (!_initialized)
            {
                foreach (var existingQuest in QuestManager.Instance.PlayerQuests)
                {
                    AddNewQuest(existingQuest);
                }
            }
        }

        private void OnDisable()
        {
            submitQuestButton.onClick.RemoveListener(TrySubmitQuest);
            findNewQuestButton.onClick.RemoveListener(TryFindNewQuest);
        }

        private void TrySubmitQuest()
        {
            QuestManager.Instance.SubmitChosenQuest();
        }

        private void TryFindNewQuest()
        {
            QuestManager.Instance.FindQuest();
        }

        public bool AddNewQuest(QuestItemData newQuestData)
        {
            if (_questItemDictionary.Count >= 3)
            {
                return false;
            }

            if (_questItemDictionary.ContainsKey(newQuestData))
            {
                return false;
            }

            var newQuestItemUIObject =
                Instantiate(questUIObjectPrefab, questPanelParent).GetComponent<QuestItemUIObject>();
            if (newQuestItemUIObject == null)
            {
                return false;
            }
            newQuestItemUIObject.InitializeUIObject(newQuestData);
            _questItemDictionary.Add(newQuestData, newQuestItemUIObject);
            return true;
        }

        public bool RemoveExistingQuest(QuestItemData existingQuestData)
        {
            if (!_questItemDictionary.ContainsKey(existingQuestData))
            {
                return false;
            }

            // remove the quest from the dictionary, destroy the ui object accordingly
            _questItemDictionary.Remove(existingQuestData, out var questItemUIObject);
            Destroy(questItemUIObject.gameObject);

            foreach (var questDataUIPair in _questItemDictionary)
            {
                questDataUIPair.Value.IndicateQuestCanBeChoose();
            }

            return true;
        }

        public bool ChooseExistingQuest(QuestItemData questItemData)
        {
            if (!_questItemDictionary.ContainsKey(questItemData))
            {
                return false;
            }

            foreach (var pair in _questItemDictionary)
            {
                if (pair.Key == questItemData)
                {
                    pair.Value.IndicateThisQuestInProgress();
                }
                else
                {
                    pair.Value.IndicateOtherQuestInProgress();
                }
            }

            return true;
        }
    }
}