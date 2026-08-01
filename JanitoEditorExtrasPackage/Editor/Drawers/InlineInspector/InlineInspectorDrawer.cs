using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Janito.EditorExtras.Editor
{
    [CustomPropertyDrawer(typeof(InlineInspectorAttribute))]
    public class InlineInspectorDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = base.CreatePropertyGUI(property);
            if (root == null)
            {
                root = new VisualElement();
            }

            var propertyField = new PropertyField(property);
            root.Add(propertyField);
            propertyField.RegisterValueChangeCallback((changeEvent) =>
            {
                // Ensure we are not adding new inspectors without clearing old ones first
                RemoveExistingInlineInspector(changeEvent.changedProperty, root);
                AddInlineInspector(changeEvent.changedProperty, root);
            });

            // Add any initial inline inspector if needed
            AddInlineInspector(property, root);
            return root;
        }

        private void AddInlineInspector(SerializedProperty property, VisualElement root)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                LogLibrary.LogErrorInDevelopment<InlineInspectorAttribute>($"Inline Inspector must be used on object references. Inline is not required for {property.name}.");
                return;
            }

            var foldout = new Foldout();
            foldout.name = GetPropertyInlineInspectorRootName(property);
            if (property.isArray)
            {
                for (int i = 0; i < property.arraySize; i++)
                {
                    var arrayElement = property.GetArrayElementAtIndex(i);
                    if (!TryAddInspector(arrayElement, foldout))
                    {
                        continue;
                    }
                }

                root.Add(foldout);
            }
            else
            {
                if (TryAddInspector(property, foldout))
                {
                    root.Add(foldout);
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
            var GUI = UnityEditor.Editor.CreateEditor(property.objectReferenceValue).CreateInspectorGUI();
            GUI.Bind(serialisedObject);
            foreach (var item in GUI.Children())
            {
                item.Bind(serialisedObject);
            }
            root.Add(GUI);
            return true;
        }

        private string GetPropertyInlineInspectorRootName(SerializedProperty property)
        {
            return $"InlineInspectorRootFor{property.name}";
        }

        private void RemoveExistingInlineInspector(SerializedProperty property, VisualElement root)
        {
            var element = root.Q(GetPropertyInlineInspectorRootName(property));
            if (element != null)
            {
                root.Remove(element);
            }
        }
    }
}
