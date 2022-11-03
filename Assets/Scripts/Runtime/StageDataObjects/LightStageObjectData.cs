using System;
using Runtime.StageDataObjects;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.Testing
{
    [Serializable]
    public class LightStageObjectData : BaseStageObjectData
    {
        //public GameObject lightsObjectPrefab;
        //public string lightsObjectPrefabLoadPath;
        public override StageObjectType ObjectDataType => StageObjectType.Light;
    }
}