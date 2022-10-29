using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Testing
{
    public enum StageObjectType
    {
        Prop = 0,
        Actor = 1,
        Orchestra = 2,
    }
    
    public class StageObjectButtonUIObject : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image propIcon;
        
        private StageObjectType _stageObjectType = StageObjectType.Prop;

        public StageObjectType StageObjectType
        {
            get => _stageObjectType;
            set => _stageObjectType = value;
        }

        private BaseStageObjectData _objectData;

        public BaseStageObjectData PropObjectData => _objectData;

        private void OnEnable()
        {
            Assert.IsNotNull(propIcon);
        }

        public void InitializeAsPropObject(BaseStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Prop;
            _objectData = objectData;
            propIcon.sprite = objectData.stagePropBlueprintScriptableObject.PropSprite;
            
            // todo:: add customization hint to the prop ui (color bar, etc)
            // todo:: add stage type hint (prop, actor, orchestra, light, etc.)
        }
        
        // todo:: actor data
        public void InitializeAsActor()
        {
            _stageObjectType = StageObjectType.Actor;
        }
        
        // todo:: orchestra data
        public void InitializeAsOrchestra()
        {
            _stageObjectType = StageObjectType.Orchestra;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // add this prop to the scene via stage manager, remove this prop from the list of storage
            bool canAddToScene = true;
            // can 
            switch (_stageObjectType)
            {
                case StageObjectType.Prop:
                    StorageManager.Instance.RemoveStageObjectData(PropObjectData);
                    StageEditingManager.Instance.InstantiateNewPropToStage(Vector2.zero, PropObjectData);
                    // update the UI to remove this obj
                    
                    break;
                case StageObjectType.Actor:
                    // var objData = StorageManager.Instance.StageObjectDataList[_listIndex];
                    break;
                case StageObjectType.Orchestra:
                    // var objData = StorageManager.Instance.StageObjectDataList[_listIndex];
                    break;
                default:
                    Debug.LogError("Undefined Stage Object Type");
                    break;
            }
            // remove this button as it is added
            if (canAddToScene)
            {
                Destroy(this.gameObject);
            }
        }
    }
}