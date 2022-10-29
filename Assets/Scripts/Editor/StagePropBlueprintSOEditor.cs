using System;
using System.Collections.Generic;
using Runtime.Testing;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(StagePropBlueprintScriptableObject))]
    public class StagePropBlueprintSOEditor : UnityEditor.Editor
    {
        private StagePropBlueprintScriptableObject _instance;
        private SerializedProperty _spriteProperty;
        private SerializedProperty _resourceConsumeProperty;
        private SerializedProperty _machineLevelRequirementProperty;
        private SerializedProperty _blueprintNameProperty;
        private SerializedProperty _propScaleProperty;

        private void OnEnable()
        {
            _instance = (StagePropBlueprintScriptableObject)target;
            _spriteProperty = serializedObject.FindProperty("propSprite");
            _resourceConsumeProperty = serializedObject.FindProperty("resourceConsumes");
            _machineLevelRequirementProperty = serializedObject.FindProperty("machineLevelRequirement");
            _blueprintNameProperty = serializedObject.FindProperty("blueprintName");
            _propScaleProperty = serializedObject.FindProperty("propScale");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            serializedObject.Update();
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(_blueprintNameProperty, new GUIContent("Prop Name"));
                EditorGUILayout.PropertyField(_spriteProperty, new GUIContent("Prop Sprite"));
                EditorGUILayout.PropertyField(_propScaleProperty, new GUIContent("Prop Scale"));
                EditorGUILayout.PropertyField(_resourceConsumeProperty, new GUIContent("Resource Consumes"));
                EditorGUILayout.PropertyField(_machineLevelRequirementProperty,
                    new GUIContent("Machine Level Requirements"));
            }
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawResourceConsumeDictionary()
        {
            // Wood, Metal
            if (_instance.ResourceConsumes.ContainsKey(GResourceType.Wood))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    _instance.ResourceConsumes[GResourceType.Wood] = 
                        EditorGUILayout.IntField("Wood", _instance.ResourceConsumes[GResourceType.Wood]);
                    _instance.ResourceConsumes[GResourceType.Metal] = 
                        EditorGUILayout.IntField("Metal", _instance.ResourceConsumes[GResourceType.Metal]);
                }
            
                // Cloth, Paint
                using (new EditorGUILayout.HorizontalScope())
                {
                    _instance.ResourceConsumes[GResourceType.Cloth] = 
                        EditorGUILayout.IntField("Cloth", _instance.ResourceConsumes[GResourceType.Cloth]);
                    _instance.ResourceConsumes[GResourceType.Paint] = 
                        EditorGUILayout.IntField("Paint", _instance.ResourceConsumes[GResourceType.Paint]);
                }   
            }
            else
            {
                _instance.ResourceConsumes.TryAdd(GResourceType.Wood, 0);
                _instance.ResourceConsumes.TryAdd(GResourceType.Metal, 0);
                _instance.ResourceConsumes.TryAdd(GResourceType.Cloth, 0);
                _instance.ResourceConsumes.TryAdd(GResourceType.Paint, 0);
            }
        }
    }
}