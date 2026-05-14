using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace Janito.EditorExtras.Editor
{
    /// <summary>
    /// Custom drawer for the ReadOnly attribute
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        // New unity UI method
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement element = base.CreatePropertyGUI(property);
            if (element == null)
            {
                element = new PropertyField(property);
            }

            element?.SetEnabled(false);
            return element;
        }

        // Fallback for older versions
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool previousGUIState = GUI.enabled;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = previousGUIState;
        }
    }
}
