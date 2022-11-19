using System;
using Runtime.Testing;
using UnityEngine;

namespace Runtime.SubfunctionObject
{
    public class BaseStagePropSubfunctionButtonObject : BaseTouchable2DObject
    {
        [SerializeField] protected SpriteRenderer buttonSpriteRenderer;
        [SerializeField] protected Collider2D buttonCollider;
        // public Sprite ButtonSprite => buttonSprite;

        protected Transform TargetTransform;
        protected BaseStageObject TargetPropObject;

        protected void IndicateNotInteractable()
        {
            buttonSpriteRenderer.color = new Color(.2f, .2f, .2f);
        }
        
        protected void IndicateInteractable()
        {
            buttonSpriteRenderer.color = Color.white;
        }
        
        public void ReactivateButton()
        {
            PointerDown = false;
            // this.canBeActivated = true;
            if (!IfButtonCanAppear()) return; // if this button is not for the type of prop object, quit activating this button
            DetermineInteractable();
            buttonSpriteRenderer.enabled = true;
            buttonCollider.enabled = true;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        public void DeactivateButton()
        {
            PointerDown = false;
            this.canBeActivated = false;
            buttonSpriteRenderer.enabled = false;
            buttonCollider.enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        public void InitializeButtonObject(Transform transform, BaseStageObject propObject)
        {
            this.TargetTransform = transform;
            this.TargetPropObject = propObject;
            
            // inject event
            SubscribeOnObjectButtonDown(OnFunctionButtonDown);
            DetermineInteractable();
        }

        protected virtual void DetermineInteractable()
        {
            if (canBeActivated)
            {
                IndicateInteractable();
            }
            else
            {
                IndicateNotInteractable();
            }
        }

        protected virtual void OnFunctionButtonDown()
        {
            // Debug.Log("On Subfunction Button Down.");
            DeactivateButton();
        }

        protected virtual bool IfButtonCanAppear()
        {
            return true;
        }
    }
}