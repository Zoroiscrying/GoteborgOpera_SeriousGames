using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Testing
{
    public enum GResourceType
    {
        Money,
        Wood,
        Metal,
        Cloth,
        Paint
    }

    [Serializable]
    public class GResourceConsume
    {
        public GResourceType type;
        [Min(0)]   
        public int consumeNum;

        public GResourceConsume(GResourceType type, int consumeNum)
        {
            this.type = type;
            this.consumeNum = consumeNum;
        }
    }
    
    /// <summary>
    /// The Stage Prop Blueprint SO contains data describing the necessary infos to build a prop object on stage.
    /// A prop built from the corresponding prop blueprint can customize certain parts, e.g., color, mask texture, etc.
    /// For a flexible prop building and customizing system, the customizing part should be described by a list of
    /// "Decorators", each decorator can store textures, color values, or scalar values, which will all connect to the
    /// Material / Sprite Object of the Stage Prop.
    ///
    /// Therefore, the blueprint only stores the sprite and resources used for this prop
    /// </summary>
    [CreateAssetMenu(fileName = "StagePropBlueprint_0", menuName = "GoteborgProject/CreatePropBlueprint", order = 0)]
    public class StagePropBlueprintScriptableObject : ScriptableObject
    {
        public Vector2 propScale = Vector2.one;
        
        public Sprite propSprite;

        [Header("Resource Consumes")]
        public SerializedDictionary<GResourceType, int> resourceConsumes =
            new SerializedDictionary<GResourceType, int>();
    }
}