using System;
using System.Collections.Generic;
using Runtime.UserInterface;
using UnityEngine;

namespace Runtime.ScriptableObjects
{
    /// <summary>
    /// The Stage Prop Blueprint SO contains data describing the necessary infos to build a prop object on stage.
    /// A prop built from the corresponding prop blueprint can customize certain parts, e.g., color, mask texture, etc.
    /// For a flexible prop building and customizing system, the customizing part should be described by a list of
    /// "Decorators", each decorator can store textures, color values, or scalar values, which will all connect to the
    /// Material / Sprite Object of the Stage Prop.
    ///
    /// Therefore, the blueprint only stores the sprite and resources used for this prop
    /// </summary>
    [CreateAssetMenu(fileName = "StagePropBlueprint_0", menuName = "GoteborgProject/Blueprint/StagePropBlueprint", order = 0)]
    public class StagePropBlueprintSO : BaseStageObjectBlueprintSO
    {
        [SerializeField] private Vector2 propScale = Vector2.one;
        public Vector2 PropScale => propScale;

        [SerializeField] Sprite propSprite;

        public Sprite PropSprite => propSprite;
        
        public override Sprite PreviewSprite => propSprite;
        
        [SerializeField] private Color spriteTint = Color.white;

        public Color SpriteTint => spriteTint;

        public override StageObjectType ObjectDataType => StageObjectType.Prop;
    }
}