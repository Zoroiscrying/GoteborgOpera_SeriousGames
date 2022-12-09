using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Testing
{
    public class BlueprintPackUIObject : MonoBehaviour
    {
        [SerializeField] private BlueprintPackSO blueprintPackSo;

        [SerializeField] private TextMeshProUGUI packNameUI;
        [SerializeField] private TextMeshProUGUI packDescriptionUI;
        
        [SerializeField] private Button selectPackButton;
        
        private void OnEnable()
        {
            selectPackButton.onClick.AddListener(SelectThisBlueprint);
            InitializeUI();
        }

        private void OnDisable()
        {
            selectPackButton.onClick.RemoveListener(SelectThisBlueprint);
        }

        private void InitializeUI()
        {
            packNameUI.text = blueprintPackSo.PackName;
            packDescriptionUI.text = blueprintPackSo.PackDescription;
        }

        private void SelectThisBlueprint()
        {
            if (StorageManager.Instance.TempUsedBlueprintPackPoint > 0)
            {
                if (blueprintPackSo)
                {
                    StorageManager.Instance.AddBlueprintPackToStorage(blueprintPackSo);   
                }
                Destroy(this.gameObject);
                StorageManager.Instance.TempUsedBlueprintPackPoint--;
            }
            else
            {
                TempUIHintManager.Instance.HintText("You cannot purchase more blueprint packs!");
            }
        }
    }
}