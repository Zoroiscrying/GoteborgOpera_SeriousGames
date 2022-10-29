namespace Runtime.SubfunctionObject
{
    public class StagePropLockingButtonObject : BaseStagePropSubfunctionButtonObject
    {
        protected override void DetermineInteractable()
        {
            canBeActivated = true;
            base.DetermineInteractable();
        }
        
        protected override void OnFunctionButtonDown()
        {
            base.OnFunctionButtonDown();

            TargetPropObject.IsLocked = !TargetPropObject.IsLocked;
        }
    }
}