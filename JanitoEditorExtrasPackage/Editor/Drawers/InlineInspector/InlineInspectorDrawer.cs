using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Janito.EditorExtras.Editor
{
    [CustomPropertyDrawer(typeof(InlineInspectorAttribute))]
    public class InlineInspectorDrawer : PropertyDrawer
    {
        [SerializeField]
        private VisualTreeAsset m_DrawerAsset;

        private const string m_InlineInspectorRoorName = "InlineInspectorRoot";
        private const string m_ContainerName = "Container";
        private InlineInspectorAttribute m_InlineInspectorAttribute => (InlineInspectorAttribute)attribute;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                LogLibrary.LogErrorInDevelopment<InlineInspectorAttribute>($"Inline Inspector must be used on object references. Inline is not required for the property `{property.displayName}`.", property.serializedObject.targetObject);
                return new PropertyField(property);
            }

            VisualElement root = m_DrawerAsset != null ? m_DrawerAsset.Instantiate() : new VisualElement();
            PropertyField propertyField = GetPropertyField(property, root);

            AddInlineInspector(property, root);

            // Register a callback to handle changes in the property value
            propertyField.RegisterValueChangeCallback((changeEvent) =>
            {
                UpdateInlineInspector(changeEvent.changedProperty, root);
            });

            return root;
        }

        private PropertyField GetPropertyField(SerializedProperty property, VisualElement root)
        {
            var propertyField = root.Q<PropertyField>();
            if (propertyField == null)
            {
                propertyField = new PropertyField(property);
                root.Add(propertyField);
            }
            else
            {
                propertyField.BindProperty(property);
            }

            propertyField.RegisterCallback<DetachFromPanelEvent>((detachEvent) =>
            {
                propertyField.Unbind();
            });

            return propertyField;
        }

        private void UpdateInlineInspector(SerializedProperty property, VisualElement root)
        {
            var container = root.Q<Foldout>(m_ContainerName);
            if (container == null)
            {
                LogLibrary.LogErrorInDevelopment<InlineInspectorAttribute>($"Inline Inspector container not found for property `{property.displayName}` in {property.serializedObject.targetObject.name}. Ensure that the property is correctly set up. Otherwise, report this issue.", property.serializedObject.targetObject);
                return;
            }

            container.Clear();
            AddInlineInspectorContents(property, container);
        }

        private void AddInlineInspector(SerializedProperty property, VisualElement root)
        {
            Foldout container = root.Q<Foldout>(m_ContainerName);
            if (container == null)
            {
                container = new Foldout();
                container.name = m_ContainerName;
                container.text = "Inline Inspector";
                root.Add(container);    
            }

            AddInlineInspectorContents(property, container);
        }

        private void AddInlineInspectorContents(SerializedProperty property, Foldout container)
        {
            if (property.isArray)
            {
                for (int i = 0; i < property.arraySize; i++)
                {
                    var arrayElement = property.GetArrayElementAtIndex(i);
                    if (!TryAddInspector(arrayElement, container))
                    {
                        continue;
                    }
                }
            }
            else
            {
                if (!TryAddInspector(property, container))
                {
                    container.SetEnabled(false);
                }
                else
                {
                    container.SetEnabled(true);
                }
            }
        }

        private bool TryAddInspector(SerializedProperty property, VisualElement root)
        {
            if (property.objectReferenceValue == null)
            {
                return false;
            }

            var serialisedObject = new SerializedObject(property.objectReferenceValue);
            var editor = UnityEditor.Editor.CreateEditor(property.objectReferenceValue);
            var container = editor.CreateInspectorGUI();
            if (container == null)
            {
                if (m_InlineInspectorAttribute.ForceInspector)
                {
                    container = new VisualElement();
                    InspectorElement.FillDefaultInspector(container, serialisedObject, editor);
                }
                else
                {
                    return false;
                }
            }

            // Bind GUI to properly display and update values
            container.Bind(serialisedObject);
            foreach (var element in container.Children())
            {
                element.Bind(serialisedObject);
            }

            // Setup clean up of elements on removal
            container.RegisterCallback<DetachFromPanelEvent>((detachEvent) =>
            {
                container.Unbind();
                foreach (var element in container.Children())
                {
                    element.Unbind();
                }
                serialisedObject.Dispose();
            });

            root.Add(container);
            return true;
        }
    }
}
