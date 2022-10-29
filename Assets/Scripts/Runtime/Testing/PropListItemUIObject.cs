using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Testing
{
    public class PropListItemUIObject : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image listItemBg;
        [SerializeField] private TextMeshProUGUI blueprintName;
        [SerializeField] private TextMeshProUGUI stageObjectNum;
        [SerializeField] private Image blueprintIcon;

        public StagePropBlueprintScriptableObject Blueprint => _blueprint;
        private StagePropBlueprintScriptableObject _blueprint;
        private Action<PropListItemUIObject> _blueprintSelected;

        public void ProducedOneItem()
        {
            stageObjectNum.text = (int.Parse(stageObjectNum.text)+1).ToString();
        }
        
        public void RecycledOneItem()
        {
            stageObjectNum.text = (int.Parse(stageObjectNum.text)-1).ToString();
        }
        
        public void UpdateItemUI(StagePropBlueprintScriptableObject blueprint, int curNum, 
            Action<PropListItemUIObject> onBlueprintSelected)
        {
            blueprintName.text = blueprint.BlueprintName;
            stageObjectNum.text = curNum.ToString();
            blueprintIcon.sprite = blueprint.PropSprite;
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