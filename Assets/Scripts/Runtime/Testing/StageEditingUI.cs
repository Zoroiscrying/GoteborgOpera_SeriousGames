using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Testing
{
    public class StageEditingUI : MonoBehaviour
    {
        [SerializeField] private Transform stageEditingPanel;
        [SerializeField] private Transform propListUIParent;
        [SerializeField] private GameObject stageObjectButtonUIObject;

        public void StartStageEditingUI()
        {
            stageEditingPanel.gameObject.SetActive(true);
            propListUIParent.gameObject.SetActive(true);
        }

        public void EndStageEditingUI()
        {
            stageEditingPanel.gameObject.SetActive(false);
            propListUIParent.gameObject.SetActive(false);
        }
        
        public void InitializeStageEditingUI(List<BaseStageObjectData> objDataList)
        {
            // destroy the children under uiParent
            // TODO:: Use pooling
            
            int numChildren = propListUIParent.childCount;
            for (int i = 0; i < numChildren; ++i)
            {
                Destroy(propListUIParent.GetChild(i).gameObject);
            }
            
            // create new object buttons
            foreach (var stageObject in objDataList)
            {
                AddNewObjectButton(stageObject);
            }
            
            // other things...
            
            // 
        }
        
        private void AddNewObjectButton(BaseStageObjectData objectData)
        {
            var obj = Instantiate(stageObjectButtonUIObject, propListUIParent).GetComponent<StageObjectButtonUIObject>();
            obj.InitializeAsPropObject(objectData);
            obj.enabled = true;
            obj.gameObject.SetActive(true);
        }
    }
}