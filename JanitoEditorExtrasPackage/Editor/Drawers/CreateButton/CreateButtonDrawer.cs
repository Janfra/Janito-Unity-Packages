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
        private const string k_DefaultNullValueFallbackName = "Null";

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
            var createButton = new Button(() => TryCreateAsset(property))
            {
                text = "Create",
                tooltip = $"Create a new {ObjectNames.NicifyVariableName(fieldInfo.FieldType.GetCoreType().Name)} asset and assign it to this field.",
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

        private void TryCreateAsset(SerializedProperty property)
        {
            var savePath = GetDefaultSavePath();

            // Prompt folder creation if missing
            if (!AssetDatabase.IsValidFolder(savePath))
            {
                TryCreateSaveFolder(savePath);
            }

            string destinationPath = EditorUtility.SaveFilePanelInProject(
                "Create New Asset",
                GetDefaultName(property.serializedObject.targetObject),
                "asset",
                "Save Location",
                savePath
                );

            if (string.IsNullOrEmpty(destinationPath))
            {
                return;
            }

            CreateAsset(destinationPath, property);
        }

        private void TryCreateSaveFolder(string folderPath)
        {
            bool shouldCreateFolder = EditorUtility.DisplayDialog(
            "Missing Target Directory",
            $"The assigned default save folder does not exist: \n\n{folderPath}",
            "Create Folder",
            "Continue Without Folder"
            );

            if (shouldCreateFolder)
            {
                PathLibrary.CreateFoldersInProject(folderPath);
            }
        }

        private string GetDefaultName(Object parent)
        {
            if (string.IsNullOrEmpty(m_CreateButtonAttribute.NamingFormat))
            {
                return $"New {fieldInfo.FieldType.GetCoreType().Name}";
            }
            else
            {
                return FormatPathWithObjectTypeInfo(m_CreateButtonAttribute.NamingFormat, parent);
            }
        }

        private string FormatPathWithObjectTypeInfo(string format, Object typeInstance)
        {
            var formattedName = format.FormatWithReflectionValues(typeInstance, k_DefaultNullValueFallbackName);

            // Sanitise path
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                formattedName.Replace(invalidChar.ToString(), "");
            }

            return formattedName;
        }

        private string GetDefaultSavePath()
        {
            return string.IsNullOrEmpty(m_CreateButtonAttribute.SavePath) ? CreateButtonAttribute.k_DefaultSavePath : m_CreateButtonAttribute.SavePath;
        }

        private void CreateAsset(string destinationPath, SerializedProperty property)
        {
            var fieldType = fieldInfo.FieldType.GetCoreType();
            var asset = ScriptableObject.CreateInstance(fieldType);
            AssetDatabase.CreateAsset(asset, destinationPath);
            AssetDatabase.SaveAssets();
            property.objectReferenceValue = asset;
            property.serializedObject.ApplyModifiedProperties();
            EditorGUIUtility.PingObject(asset);
        }
    }
}
