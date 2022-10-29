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
    public class StagePropBlueprintScriptableObject : ScriptableObject, IComparable
    {
        [SerializeField] private string blueprintName;
        public string BlueprintName => blueprintName;

        [SerializeField] int moneyToBuy = 0;
        public int MoneyToBuy => moneyToBuy;

        [SerializeField] private Vector2 propScale = Vector2.one;
        public Vector2 PropScale => propScale;

        [SerializeField] Sprite propSprite;

        public Sprite PropSprite => propSprite;

        [Header("Resource Consumes")]
        [SerializeField] private GenericDictionary<GResourceType, int> resourceConsumes =
            new()
            {
                { GResourceType.Money, 0},
                { GResourceType.Wood, 0},
                { GResourceType.Metal, 0},
                { GResourceType.Cloth, 0},
                { GResourceType.Paint, 0},
            };

        public IDictionary<GResourceType, int> ResourceConsumes => resourceConsumes;

        [Header("Machine Level Requirement")]
        [SerializeField] private GenericDictionary<GResourceType, int> machineLevelRequirement =
            new()
            {
                { GResourceType.Wood, 0},
                { GResourceType.Metal, 0},
                { GResourceType.Cloth, 0},
                { GResourceType.Paint, 0},
            };

        public IDictionary<GResourceType, int> MachineLevelRequirement => machineLevelRequirement;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            StagePropBlueprintScriptableObject otherBlueprint = obj as StagePropBlueprintScriptableObject;
            if (otherBlueprint != null)
                return this.MoneyToBuy.CompareTo(otherBlueprint.MoneyToBuy);
            else
                throw new ArgumentException("Object is not a Blueprint");
        }
    }
}