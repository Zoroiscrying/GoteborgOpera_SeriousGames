using System;
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
    [RequireComponent(
        typeof(SpriteRenderer), 
        typeof(StageTouchableObject))]
    [ExecuteInEditMode]
    public class StagePropObject : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private StageTouchableObject _stageTouchableObject;

        public BaseStageObjectData StageObjectData => stageObjectData;
        
        [SerializeField] private BaseStageObjectData stageObjectData;
        [SerializeField] private BasePropDecorator basePropDecorator;

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

        public void UpdateRendererOrderInLayer(int newOrder)
        {
            this._spriteRenderer.sortingOrder = newOrder;
        }
        
        /// <summary>
        /// This function is to be called when a prop from a prop list is instantiated.
        /// </summary>
        /// <param name="propDecorator"></param>
        public void InjectPropDecorator(BasePropDecorator propDecorator)
        {
            this.basePropDecorator = propDecorator;
        }

        /// <summary>
        /// Injects the blueprint scriptable obj to this object
        /// </summary>
        /// <param name="stageObjectData"></param>
        public void InitializeFromStageObjectData(BaseStageObjectData stageObjectData)
        {
            this.stageObjectData = stageObjectData;
            OnEnable();
            ApplyPropDecorators();
            GetComponent<PolygonCollider2D>().TryUpdateShapeToAttachedSprite();
        }

        private void OnValidate()
        {
            if (_spriteRenderer && _stageTouchableObject && basePropDecorator.Initialized)
            {
                ApplyPropDecorators();   
            }
            else
            {
                OnEnable();
                
            }
        }

        private void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _stageTouchableObject = GetComponent<StageTouchableObject>();
            
            // TODO:: This should be changed to a more flexible version, where properties can be changed in the editor
            // TODO:: Might just add a list of 'customize material variable' to the base prop decorator...
            basePropDecorator.SetupRenderer(_spriteRenderer);
        }

        private void ApplyPropDecorators()
        {
            // pass in list of decorators
            basePropDecorator.ApplyDecoration();
            
            if (stageObjectData.stagePropBlueprintScriptableObject)
            {
                _spriteRenderer.sprite = stageObjectData.stagePropBlueprintScriptableObject.PropSprite;
                this.transform.localScale = new Vector3(stageObjectData.stagePropBlueprintScriptableObject.PropScale.x,
                    stageObjectData.stagePropBlueprintScriptableObject.PropScale.y, 1.0f);
            }
        }
    }
}