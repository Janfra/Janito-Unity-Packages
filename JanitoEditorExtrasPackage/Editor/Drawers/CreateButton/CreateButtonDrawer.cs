using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Janito.EditorExtras.Editor
{
    [CustomPropertyDrawer(typeof(CreateButtonAttribute))]
    public class CreateButtonDrawer : PropertyDrawer
    {
        private CreateButtonAttribute m_CreateButtonAttribute => (CreateButtonAttribute)attribute;
        private const string k_DefaultNullFieldFallbackName = "Null";
        private const BindingFlags k_FieldBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var field = new PropertyField(property);
            var fieldType = fieldInfo.FieldType.GetCoreType(); // Handle lists and array, we assume that the property returns the items inside the list/array instead of the collection itself, so we just need to confirm the type
            if (property.propertyType != SerializedPropertyType.ObjectReference || !typeof(ScriptableObject).IsAssignableFrom(fieldType))
            {
                LogLibrary.LogErrorInDevelopment<CreateButtonAttribute>($"CreateButtonAttribute can only be used on `ScriptableObject` types. Field '{property.displayName}' is of type '{fieldType.Name}'.", property.serializedObject.targetObject);
                return field;
            }

            if (fieldType.IsAbstract || fieldType.IsInterface)
            {
                LogLibrary.LogErrorInDevelopment<CreateButtonAttribute>($"CreateButtonAttribute cannot be used on abstract classes or interfaces. Field '{property.displayName}' is not compatible.", property.serializedObject.targetObject);
                return field;
            }

            var container = new VisualElement();
            SetContainerStyle(container);
            SetFieldStyle(field);
            container.Add(field);

            var createButton = AddCreateButton(container, property);
            field.RegisterValueChangeCallback(evt =>
            {
                SetButtonDisplay(createButton, property);
            });

            return container;
        }

        private PropertyField SetFieldStyle(PropertyField field)
        {
            field.style.flexGrow = 1;
            return field;
        }

        private VisualElement SetContainerStyle(VisualElement container)
        {
            container.style.flexDirection = FlexDirection.Row;
            return container;
        }

        private Button SetButtonStyle(Button button)
        {
            button.style.marginLeft = 5;
            button.style.width = 60;
            button.style.height = 18;
            return button;
        }

        private Button AddCreateButton(VisualElement container, SerializedProperty property)
        {
            var createButton = new Button(() => CreateAsset(property))
            {
                text = "Create",
                tooltip = $"Create a new {fieldInfo.FieldType.Name} asset and assign it to this field.",
            };

            container.Add(createButton);
            SetButtonDisplay(createButton, property);
            return SetButtonStyle(createButton);
        }

        private void SetButtonDisplay(Button button, SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
            {
                button.style.display = DisplayStyle.Flex;
            }
            else
            {
                button.style.display = DisplayStyle.None;
            }
        }

        private void CreateAsset(SerializedProperty property)
        {
            string destinationPath = EditorUtility.SaveFilePanelInProject(
                "Create New Asset",
                GetDefaultName(property.serializedObject.targetObject),
                "asset",
                "Save Location",
                GetDefaultSavePath()
                );

            if (string.IsNullOrEmpty(destinationPath))
            {
                return;
            }

            var fieldType = fieldInfo.FieldType.GetCoreType();
            var asset = ScriptableObject.CreateInstance(fieldType);
            AssetDatabase.CreateAsset(asset, destinationPath);
            AssetDatabase.SaveAssets();
            property.objectReferenceValue = asset;
            property.serializedObject.ApplyModifiedProperties();
        }

        private string GetDefaultName(Object parent)
        {
            if (string.IsNullOrEmpty(m_CreateButtonAttribute.NamingFormat))
            {
                return $"New {fieldInfo.FieldType.GetCoreType().Name}";
            }
            else
            {
                return FormatPathWithObjectFieldInfo(m_CreateButtonAttribute.NamingFormat, parent);
            }
        }

        private string FormatPathWithObjectFieldInfo(string format, object fieldSource)
        {
            int bufferSize = 32;
            int length = format.Length;
            StringBuilder sb = new StringBuilder(length + bufferSize);
            int index = 0;

            while (index < length)
            {
                int openBraceIndex = format.IndexOf('{', index);

                // If no more placeholders are found, append the rest of the string and break
                if (openBraceIndex == -1)
                {
                    sb.Append(format, index, length - index);
                    break;
                }

                // Append the text before the placeholder
                if (openBraceIndex > index)
                {
                    sb.Append(format, index, openBraceIndex - index);
                }

                int closeBraceIndex = format.IndexOf('}', openBraceIndex);
                if (closeBraceIndex == -1)
                {
                    // If there's no closing brace, treat it as a literal '{'. Maybe add a warning here in the future.
                    sb.Append(format, openBraceIndex, length - openBraceIndex);
                    break;
                }

                string fieldName = format.Substring(openBraceIndex + 1, closeBraceIndex - openBraceIndex - 1);

                // Try to fill in the placeholder with the actual field name if it matches, otherwise treat it as a literal
                FieldInfo field = fieldSource.GetType().GetField(fieldName, k_FieldBindingFlags);
                if (field != null)
                {
                    object value = field.GetValue(fieldSource);
                    sb.Append(value != null ? value.ToString() : k_DefaultNullFieldFallbackName);
                }
                else
                {
                    sb.Append(format, openBraceIndex, closeBraceIndex - openBraceIndex + 1);
                }

                index = closeBraceIndex + 1;
            }

            // Sanitise path
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                sb.Replace(invalidChar.ToString(), "");
            }

            return sb.ToString();
        }

        private string GetDefaultSavePath()
        {
            return string.IsNullOrEmpty(m_CreateButtonAttribute.SavePath) ? CreateButtonAttribute.k_DefaultSavePath : m_CreateButtonAttribute.SavePath;
        }
    }
}
