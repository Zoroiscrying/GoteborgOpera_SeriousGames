using System;
using Runtime.ScriptableObjects;
using Runtime.UserInterface;

namespace Runtime.StageDataObjects
{
    [Serializable]
    public class BaseStageObjectData : IComparable
    {
        public string objectName = "NewStageObject";
        
        public BaseStageObjectBlueprintSO baseStageObjectBlueprintSO;
        public string baseStageObjectBlueprintAssetPath;

        public virtual StageObjectType ObjectDataType => StageObjectType.None;
        
        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is BaseStageObjectData otherObjData)
                return otherObjData.baseStageObjectBlueprintSO.MoneyToBuy.CompareTo(
                    otherObjData.baseStageObjectBlueprintSO.MoneyToBuy);
            else
                throw new ArgumentException("Object is not a BaseStageObjectData!");
        }
    }
}