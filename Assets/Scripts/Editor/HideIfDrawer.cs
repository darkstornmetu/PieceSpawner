using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HideIfAttribute))]
public class HideIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideIfAttribute hideIf = attribute as HideIfAttribute;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(hideIf.ConditionalSourceField);

        if (sourcePropertyValue != null)
        {
            bool conditionMet = sourcePropertyValue.boolValue == hideIf.HideInInspector;
            if (!conditionMet)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true); // Default display if source property is not found
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideIfAttribute hideIf = attribute as HideIfAttribute;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(hideIf.ConditionalSourceField);

        if (sourcePropertyValue != null)
        {
            bool conditionMet = sourcePropertyValue.boolValue == hideIf.HideInInspector;
            return conditionMet ? -EditorGUIUtility.standardVerticalSpacing : EditorGUI.GetPropertyHeight(property, label, true);
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}