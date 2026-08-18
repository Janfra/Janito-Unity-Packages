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

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var field = new PropertyField(property);
            var fieldType = fieldInfo.FieldType.GetCoreType(); // Handle lists and arrays, assuming that the property returns the items inside the list/array instead of itself.
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
            var fieldType = fieldInfo.FieldType.GetCoreType();
            string destinationPath = EditorUtility.SaveFilePanelInProject(
                "Create New Asset",
                $"New {fieldType.Name}", 
                "asset", 
                "Save Location"
                );

            if (string.IsNullOrEmpty(destinationPath))
            {
                return;
            }

            var asset = ScriptableObject.CreateInstance(fieldType);
            AssetDatabase.CreateAsset(asset, destinationPath);
            AssetDatabase.SaveAssets();
            property.objectReferenceValue = asset;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
