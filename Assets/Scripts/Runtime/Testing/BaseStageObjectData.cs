using System;
using System.Collections.Generic;

namespace Runtime.Testing
{
    [Serializable]
    public class BaseStageObjectData
    {
        public string objectName = "NewStageObject";
        public string stagePropBlueprintAssetPath = "";
        
        // TODO:: Consider list of decorators and their serialization.
        // public List<>
        
        // this data is retrieved at runtime
        public StagePropBlueprintScriptableObject stagePropBlueprintScriptableObject;
        
    }
}