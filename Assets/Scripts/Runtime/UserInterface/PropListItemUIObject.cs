using System;
using Runtime.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    public class PropListItemUIObject : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image listItemBg;
        [SerializeField] private TextMeshProUGUI blueprintName;
        [SerializeField] private TextMeshProUGUI stageObjectNum;
        [SerializeField] private Image blueprintIcon;

        public BaseStageObjectBlueprintSO Blueprint => _blueprint;
        private BaseStageObjectBlueprintSO _blueprint;
        private Action<PropListItemUIObject> _blueprintSelected;

        public void ProducedOneItem()
        {
            stageObjectNum.text = (int.Parse(stageObjectNum.text)+1).ToString();
        }
        
        public void RecycledOneItem()
        {
            stageObjectNum.text = (int.Parse(stageObjectNum.text)-1).ToString();
        }
        
        public void UpdateItemUI(BaseStageObjectBlueprintSO blueprint, int curNum, 
            Action<PropListItemUIObject> onBlueprintSelected)
        {
            blueprintName.text = blueprint.BlueprintName;
            stageObjectNum.text = curNum.ToString();
            blueprintIcon.sprite = blueprint.PreviewSprite;
            _blueprint = blueprint;
            _blueprintSelected = onBlueprintSelected;
        }

        public void HintOnSelected()
        {
            listItemBg.color = new Color(0.8f, 0.4f, 0.2f);
        }

        public void HintOnDeselected()
        {
            listItemBg.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _blueprintSelected.Invoke(this);
        }
    }
}