using Runtime.Managers;
using Runtime.StageDataObjects;
using UnityEngine;

namespace Runtime.Testing
{
    /// <summary>
    /// Interface Object of a stage prop with other systems
    /// Instantiated by 'Stage Editing Manager', Register Event to the manager for instantiating sub-function buttons
    /// When moved aside, destroyed by the 'Stage Editing Manager', and added to the Prop List instead.
    /// Can be customized, where the decorator's properties are changed in runtime.
    /// The customizing setup should be kept, therefore the base prop decorator is serialized.
    /// The storage's list should also keep a reference to this decorator, so that it can be kept.
    /// </summary>
    [RequireComponent(typeof(StageTouchablePropObject))]
    [ExecuteInEditMode]
    public class BaseStageObject : MonoBehaviour
    {
        protected StageTouchablePropObject _stageTouchablePropObject;

        public BaseStageObjectData StageObjectData => stageObjectData;
        [SerializeField] private BaseStageObjectData stageObjectData;

        private bool _isLocked = false;
        public bool IsLocked
        {
            get => _isLocked;
            set => _isLocked = value;
        }
        
        private LayerZ _curLayerZCoord = LayerZ.StageCenter;
        public LayerZ CurLayerZCoord
        {
            get => _curLayerZCoord;
            set => _curLayerZCoord = value;
        }
        
        public virtual void UpdateRendererOrderInLayer(int newOrder)
        {
            // this._spriteRenderer.sortingOrder = newOrder;
        }

        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public virtual void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            this.stageObjectData = stageObjectData;
            OnEnable();
            RegisterButtonDownEvent();
        }

        public void RegisterButtonDownEvent()
        {
            this.GetComponent<StageTouchablePropObject>().SubscribeOnObjectActivated((OnTouchableActivated));
        }

        private void OnTouchableActivated()
        {
            StageEditingManager.Instance.EditingStageObject(this, 1.0f);
        }

        private void OnValidate()
        {
            if (!_stageTouchablePropObject)
            {
                OnEnable();   
            }
        }

        protected virtual void OnEnable()
        {
            _stageTouchablePropObject = GetComponent<StageTouchablePropObject>();
        }
    }
}