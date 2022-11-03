using System;
using Runtime.ScriptableObjects;
using Runtime.Testing;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.StageDataObjects
{
    public enum CustomEffectLayer
    {
        WholeSceneParticleSystem,
        ObjectParticleSystem,
        BackgroundDecal,
        ObjectDecal,
    }

    [Serializable]
    public class EffectStageObjectData : BaseStageObjectData
    {
        public override StageObjectType ObjectDataType => StageObjectType.Effect;
    }
}