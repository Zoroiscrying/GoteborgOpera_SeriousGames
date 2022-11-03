using System;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.UserInterface;
using UnityEngine.Serialization;

namespace Runtime.Testing
{
    [Serializable]
    public class PropStageObjectData : BaseStageObjectData
    {
        public override StageObjectType ObjectDataType => StageObjectType.Prop;
        // TODO:: Consider list of decorators and their serialization.
        // public List<>
        
        // this data is retrieved at runtime via StagePropBlueprintAssetPath on Load.
        // public string stagePropBlueprintAssetPath = "";
        // [FormerlySerializedAs("stagePropBlueprintScriptableObject")] public StagePropBlueprintSO stagePropBlueprintSo;
    }
}