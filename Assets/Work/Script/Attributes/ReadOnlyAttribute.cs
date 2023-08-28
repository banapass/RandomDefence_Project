using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{
    public ReadOnlyAttribute()
    {
    }
}

#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
#endif