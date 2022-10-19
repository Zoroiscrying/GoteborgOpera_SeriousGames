using System;
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
        private SerializedProperty _propScaleProperty;

        private void OnEnable()
        {
            _instance = (StagePropBlueprintScriptableObject)target;
            _spriteProperty = serializedObject.FindProperty("propSprite");
            _resourceConsumeProperty = serializedObject.FindProperty("resourceConsumes");
            _propScaleProperty = serializedObject.FindProperty("propScale");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            serializedObject.Update();
            
            using (new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(_spriteProperty, new GUIContent("Prop Sprite"));
                EditorGUILayout.PropertyField(_propScaleProperty, new GUIContent("Prop Scale"));
                // EditorGUILayout.PropertyField(_resourceConsumeProperty, new GUIContent("Resource Consumes"));
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Resource Consumes: ");   
                    DrawResourceConsumeDictionary();
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawResourceConsumeDictionary()
        {
            // Wood, Metal
            if (_instance.resourceConsumes.ContainsKey(GResourceType.Wood))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    _instance.resourceConsumes[GResourceType.Wood] = 
                        EditorGUILayout.IntField("Wood", _instance.resourceConsumes[GResourceType.Wood]);
                    _instance.resourceConsumes[GResourceType.Metal] = 
                        EditorGUILayout.IntField("Metal", _instance.resourceConsumes[GResourceType.Metal]);
                }
            
                // Cloth, Paint
                using (new EditorGUILayout.HorizontalScope())
                {
                    _instance.resourceConsumes[GResourceType.Cloth] = 
                        EditorGUILayout.IntField("Cloth", _instance.resourceConsumes[GResourceType.Cloth]);
                    _instance.resourceConsumes[GResourceType.Paint] = 
                        EditorGUILayout.IntField("Paint", _instance.resourceConsumes[GResourceType.Paint]);
                }   
            }
            else
            {
                _instance.resourceConsumes.Add(GResourceType.Wood, 0);
                _instance.resourceConsumes.Add(GResourceType.Metal, 0);
                _instance.resourceConsumes.Add(GResourceType.Cloth, 0);
                _instance.resourceConsumes.Add(GResourceType.Paint, 0);
            }
        }
    }
}