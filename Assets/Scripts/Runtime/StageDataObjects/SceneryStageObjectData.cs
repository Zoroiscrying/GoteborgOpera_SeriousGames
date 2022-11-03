using System;
using Runtime.ScriptableObjects;
using Runtime.StageDataObjects;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.Testing
{
    [Serializable]
    public class SceneryStageObjectData : BaseStageObjectData
    {
        public override StageObjectType ObjectDataType => StageObjectType.Scenery;
        /// <summary>
        /// The scenery object prefab is a collection of objects that fits the whole stage.
        /// </summary>
        // public StageSceneryBlueprintSO sceneryObjectBlueprint;
        // public string sceneryObjectBlueprintAssetPath;
    }
}