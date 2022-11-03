using System;
using System.Collections.Generic;
using Runtime.Testing;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.ScriptableObjects
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
    
    public class BaseStageObjectBlueprintSO : ScriptableObject, IComparable
    {
        [SerializeField] private Sprite previewSprite;

        public virtual Sprite PreviewSprite => previewSprite;

        [SerializeField] private string blueprintName;
        public string BlueprintName => blueprintName;

        [SerializeField] int moneyToBuy = 0;
        public int MoneyToBuy => moneyToBuy;
        
        public virtual StageObjectType ObjectDataType => StageObjectType.None;

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

            StagePropBlueprintSO otherBlueprint = obj as StagePropBlueprintSO;
            if (otherBlueprint != null)
                return this.MoneyToBuy.CompareTo(otherBlueprint.MoneyToBuy);
            else
                throw new ArgumentException("Object is not a Blueprint");
        }
    }
}