using UnityEngine;

namespace Runtime.SubfunctionObject
{
    public class StagePropLockingButtonObject : BaseStagePropSubfunctionButtonObject
    {
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;
        
        protected override void DetermineInteractable()
        {
            canBeActivated = true;
            base.DetermineInteractable();
        }
        
        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();

            TargetPropObject.IsLocked = !TargetPropObject.IsLocked;
            buttonSpriteRenderer.sprite = TargetPropObject.IsLocked ? unlockedSprite : lockedSprite;
        }
    }
}