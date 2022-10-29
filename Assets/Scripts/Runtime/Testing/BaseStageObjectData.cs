using System;
using System.Collections.Generic;

namespace Runtime.Testing
{
    [Serializable]
    public class BaseStageObjectData : IComparable
    {
        public string objectName = "NewStageObject";
        public string stagePropBlueprintAssetPath = "";
        
        // TODO:: Consider list of decorators and their serialization.
        // public List<>
        
        // this data is retrieved at runtime
        public StagePropBlueprintScriptableObject stagePropBlueprintScriptableObject;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            BaseStageObjectData otherObjData = obj as BaseStageObjectData;
            if (otherObjData != null)
                return this.stagePropBlueprintScriptableObject.MoneyToBuy.CompareTo(
                    otherObjData.stagePropBlueprintScriptableObject.MoneyToBuy);
            else
                throw new ArgumentException("Object is not a BaseStageObjectData!");
        }
    }
}