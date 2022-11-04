using System;
using UnityEngine;

namespace Runtime.Testing
{
    [CreateAssetMenu(fileName = "QuestItemData", menuName = "GoteborgProject/Quest/NewQuestData", order = 0)]
    public class QuestItemSO : ScriptableObject
    {
        public string questTitle;
        [Multiline]
        public string questDescription;
        // todo:: other properties
    }
}