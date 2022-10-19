using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Testing
{
    [Serializable]
    public class BasePropDecorator
    {
        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private Sprite _sprite;

        public virtual bool Initialized => _spriteRenderer;
        
        [FormerlySerializedAs("_customSpriteColor")] 
        [SerializeField] private Color customSpriteColor = Color.white;

        // [SerializeField] private List<int> testIntList = new List<int>();

        public BasePropDecorator()
        {
            _spriteRenderer = null;
            _materialPropertyBlock = null;
        }
        
        public BasePropDecorator(SpriteRenderer renderer)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            
            this._spriteRenderer = renderer;
            renderer.GetPropertyBlock(_materialPropertyBlock);
        }
        
        public BasePropDecorator(SpriteRenderer renderer, Color customSpriteColor)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            
            this._spriteRenderer = renderer;
            renderer.GetPropertyBlock(_materialPropertyBlock);
            this.customSpriteColor = customSpriteColor;
        }

        public void SetupRenderer(SpriteRenderer renderer)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            this._spriteRenderer = renderer;
            renderer.GetPropertyBlock(_materialPropertyBlock);
        }

        /// <summary>
        /// This function will directly change the material properties / sprite color
        /// of the sprite renderer. For customizable effects, please derive this class and implement
        /// Custom properties, colors, scalar values and the 'ChangeMaterialProperties()' function.
        /// </summary>
        public void ApplyDecoration()
        {
            // Change the material properties
            ChangeMaterialProperties();
            // Change the sprite color
            _spriteRenderer.color = this.customSpriteColor;
            _spriteRenderer.SetPropertyBlock(this._materialPropertyBlock);
        }

        protected virtual void ChangeMaterialProperties()
        {
            // not implemented here, for derived classes.
        }
    }
}