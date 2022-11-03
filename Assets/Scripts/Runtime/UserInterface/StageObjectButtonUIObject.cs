using System;
using Runtime.Managers;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.Testing;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UserInterface
{
    [Flags]
    public enum StageObjectType
    {
        None = 0,
        
        Prop = 1 << 0,
        Actor = 1 << 1,
        Orchestra = 1 << 2,
        Effect = 1 << 3,
        Scenery = 1 << 4,
        Light = 1 << 5,
        
        // All = ~(~0 << 6)
    }
    
    public class StageObjectButtonUIObject : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image propIcon;
        [SerializeField] private RawImage previewTexture;
        [SerializeField] private TextMeshProUGUI objectName;
        
        private StageObjectType _stageObjectType = StageObjectType.Prop;

        public StageObjectType StageObjectType
        {
            get => _stageObjectType;
            set => _stageObjectType = value;
        }

        private BaseStageObjectData _objectData;

        public BaseStageObjectData StageObjectData => _objectData;

        private void OnEnable()
        {
            Assert.IsNotNull(propIcon);
        }

        public void InitializeAsPropObject(PropStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Prop;
            _objectData = objectData;
            objectName.text = objectData.objectName;
            var stagePropBlueprintSo = (objectData.baseStageObjectBlueprintSO) as StagePropBlueprintSO;
            if (stagePropBlueprintSo != null)
            {
                propIcon.sprite = stagePropBlueprintSo.PropSprite ? stagePropBlueprintSo.PropSprite
                    : SharedAssetsManager.Instance.CustomPropObjectSprite;   
            }
        }
        
        public void InitializeAsActor(ActorStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Actor;
        }

        public void InitializeAsOrchestra(OrchestraStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Orchestra;
        }

        public void InitializeAsEffectObject(EffectStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Effect;
            _objectData = objectData;
            objectName.text = objectData.objectName;
            var stageEffectBlueprintSo = (objectData.baseStageObjectBlueprintSO) as StageEffectBlueprintSO;
            if (stageEffectBlueprintSo != null)
            {
                propIcon.sprite = stageEffectBlueprintSo.EffectSprite ? stageEffectBlueprintSo.EffectSprite
                    : SharedAssetsManager.Instance.CustomParticleSystemEffectSprite;   
            }
        }
        
        public void InitializeAsSceneryObject(SceneryStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Scenery;
            _stageObjectType = StageObjectType.Effect;
            _objectData = objectData;
            objectName.text = objectData.objectName;
            var stageSceneryBlueprintSo = (objectData.baseStageObjectBlueprintSO) as StageSceneryBlueprintSO;
            if (stageSceneryBlueprintSo != null)
            {
                previewTexture.texture = stageSceneryBlueprintSo.SceneryPreviewTexture;
            }
        }
        
        public void InitializeAsLightObject(LightStageObjectData objectData)
        {
            _stageObjectType = StageObjectType.Scenery;
            _stageObjectType = StageObjectType.Effect;
            _objectData = objectData;
            objectName.text = objectData.objectName;
            // var stageSceneryBlueprintSo = (objectData.baseStageObjectBlueprintSO) as lightso;
            // if (stageSceneryBlueprintSo != null)
            // {
            //     previewTexture.texture = stageSceneryBlueprintSo.SceneryPreviewTexture;
            // }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // add this prop to the scene via stage manager, remove this prop from the list of storage
            bool canAddToScene = true;
            StorageManager.Instance.RemoveStageObjectData(StageObjectData);
            switch (_stageObjectType)
            {
                case StageObjectType.Prop:
                    // update the UI to remove this obj
                    StageEditingManager.Instance.InstantiateNewPropToStage(StageObjectData, Vector2.zero);
                    break;
                case StageObjectType.Actor:
                    StageEditingManager.Instance.InstantiateNewActorToStage(StageObjectData, Vector2.zero);
                    break;
                case StageObjectType.Orchestra:
                    StageEditingManager.Instance.InstantiateNewOrchestraToStage(StageObjectData, Vector2.zero);
                    break;
                case StageObjectType.Effect:
                    StageEditingManager.Instance.InstantiateNewEffectToStage(StageObjectData, Vector2.zero);
                    break;
                case StageObjectType.Scenery:
                    StageEditingManager.Instance.InstantiateNewSceneryToStage(StageObjectData, Vector2.zero);
                    break;
                default:
                    Debug.LogError("Undefined Stage Object Type");
                    canAddToScene = false;
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