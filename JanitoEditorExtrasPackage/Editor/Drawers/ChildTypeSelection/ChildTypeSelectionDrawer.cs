using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Janito.EditorExtras.TypeLibrary;

namespace Janito.EditorExtras.Editor
{
    [CustomPropertyDrawer(typeof(ChildTypeSelectionAttribute))]
    public class ChildTypeSelectionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Wraps on the serialized property and displays the selection and property GUI
        /// </summary>
        private sealed class ChildTypeSelectionPropertyWrapper
        {
            private const string DropdownFieldName = "DropdownField";
            private const string SearchFieldName = "ToolbarSearchField";

            public readonly VisualElement Root;
            private readonly List<Type> m_ChildTypes;
            private readonly SerializedProperty m_Property;
            private readonly ToolbarSearchField m_SearchField;
            private readonly DropdownField m_DropdownField;
            private readonly PropertyField m_PropertyField;

            private List<string> m_Entries;
            private Dictionary<string, Type> m_NameToType;

            public ChildTypeSelectionPropertyWrapper(VisualElement root, SerializedProperty property, List<Type> childTypes)
            {
                Root = root;
                m_Property = property;
                m_ChildTypes = childTypes;

                m_SearchField = Root.Q<ToolbarSearchField>(SearchFieldName);
                m_SearchField.RegisterValueChangedCallback(OnSearched);

                m_DropdownField = Root.Q<DropdownField>(DropdownFieldName);
                m_Entries = GetChildTypeNamesList(m_Property);
                m_NameToType = new();

                for (int i = 0; i < m_Entries.Count; i++)
                {
                    var entry = m_Entries[i];
                    // First entry is for null and prompt, see GetChildTypeNamesList
                    if (i == 0)
                    {
                        m_NameToType.TryAdd(entry, null);
                    }
                    else
                    {
                        // Offset by 1 due to prompt/null entry
                        m_NameToType.TryAdd(entry, m_ChildTypes[i - 1]);
                    }
                    m_DropdownField.choices.Add(entry);
                }

                m_DropdownField.RegisterValueChangedCallback(OnSelection);
                m_DropdownField.value = m_Entries[GetSelectionIndex(m_Property)];

                m_PropertyField = Root.Q<PropertyField>();
                m_PropertyField.BindProperty(m_Property);

                // Listen for removal
                Root.RegisterCallback<DetachFromPanelEvent>(OnDettach);
            }

            private void OnSearched(ChangeEvent<string> evt)
            {
                if (string.IsNullOrEmpty(evt.newValue) && m_DropdownField.choices.Count < m_Entries.Count)
                {
                    m_DropdownField.choices = new(m_Entries);
                    return;
                }
                if (evt.newValue == evt.previousValue) return;

                var searched = evt.newValue.ToLower();
                var choices = m_DropdownField.choices;
                for (int i = 1; i < m_Entries.Count; i++)
                {
                    var entry = m_Entries[i];
                    if (!entry.ToLower().Contains(searched) && choices.Contains(entry))
                    {
                        choices.Remove(entry);
                    }
                    else if (entry.ToLower().Contains(searched) && !choices.Contains(entry))
                    {
                        choices.Add(entry);
                    }
                }
            }

            private List<string> GetChildTypeNamesList(SerializedProperty property)
            {
                string firstValue;
                if (property.isArray)
                {
                    firstValue = $"Select type to add to {property.name}";
                }
                else
                {
                    firstValue = property.managedReferenceValue == null ? $"Select type to set" : $"Set {property.name} to null";
                }

                List<string> list = new()
                {
                    firstValue
                };

                string displayName;
                foreach (Type type in m_ChildTypes)
                {
                    displayName = string.IsNullOrEmpty(type.Namespace) ? type.Name : $"{type.Name} - {type.Namespace}";
                    list.Add(displayName);
                }

                return list;
            }

            private int GetSelectionIndex(SerializedProperty property)
            {
                var value = property.managedReferenceValue;
                if (value == null)
                {
                    return 0;
                }

                for (int i = 0; i < m_ChildTypes.Count; i++)
                {
                    if (m_ChildTypes[i].Name == value.GetType().Name)
                    {
                        return i + 1;
                    }
                }

                return 0;
            }

            private void OnSelection(ChangeEvent<string> evt)
            {
                if (evt.previousValue == evt.newValue)
                {
                    return;
                }

                Type type = m_NameToType.GetValueOrDefault(evt.newValue, null);
                if (m_Property.isArray)
                {
                    AddReferenceToArray(type);
                }
                else
                {
                    AssignReference(type);
                }
            }

            private void AddReferenceToArray(Type type)
            {
                if (type == null) return;

                try
                {
                    int newIndex = m_Property.arraySize;
                    m_Property.InsertArrayElementAtIndex(newIndex);

                    SerializedProperty elementProperty = m_Property.GetArrayElementAtIndex(newIndex);

                    elementProperty.managedReferenceValue = Activator.CreateInstance(type);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to insert new instance of {type.Name} in array: {exception.Message}");
                    return;
                }

                m_Property.serializedObject.ApplyModifiedProperties();
            }

            private void AssignReference(Type type)
            {
                if (type == null)
                {
                    m_Property.managedReferenceValue = null;
                }
                else
                {
                    try
                    {
                        m_Property.managedReferenceValue = Activator.CreateInstance(type);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Failed to create instance of {type.Name}: {exception.Message}");
                        return;
                    }
                }

                m_Property.serializedObject.ApplyModifiedProperties();
            }

            /// <summary>
            /// Executed when the root visual element is removed from Inspector
            /// </summary>
            private void OnDettach(DetachFromPanelEvent _evt)
            {
                m_PropertyField.Unbind();
            }
        }

        [SerializeField]
        private VisualTreeAsset m_DrawerAsset;

        /// <summary>
        /// Criteria used to retrieve the displayed child types. Abstract and interface types are excluded due to not being compatible with CreateInstance.
        /// </summary>
        private readonly TypeCriteria m_TypeCriteria = new(TypeCriteria.TypeRequirementFlags.NotAbstract | TypeCriteria.TypeRequirementFlags.NotInterface);
        private ChildTypeSelectionAttribute m_ChildTypeAttribute => (ChildTypeSelectionAttribute)attribute;
        private List<Type> m_ChildTypes;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!HasSerializedReferenceAttribute())
            {
                throw new CustomAttributeFormatException($"[ChildTypeSelection] {nameof(ChildTypeSelectionAttribute)} must have the {nameof(SerializeField)} attribute in order to work properly. Please add it.");
            }

            if (!IsFieldCompatible(property))
            {
                throw new ArgumentException($"[ChildTypeSelection] The type {m_ChildTypeAttribute.BaseType.Name} is not assignable to {fieldInfo.FieldType.Name} type in field {fieldInfo.Name}. Please assign compatible types.");
            }

            m_ChildTypes = new(GetCachedEnumerableOfTypeChildren(m_ChildTypeAttribute.BaseType, m_TypeCriteria, true));
            FilterInvalidTypes();

            var propertyDrawer = new ChildTypeSelectionPropertyWrapper(m_DrawerAsset.Instantiate(), property, m_ChildTypes);
            return propertyDrawer.Root;
        }


        private bool HasSerializedReferenceAttribute()
        {
            foreach (CustomAttributeData attributeData in fieldInfo.CustomAttributes)
            {
                if (attributeData.AttributeType == typeof(SerializeReference))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsFieldCompatible(SerializedProperty property)
        {
            Type parentType = m_ChildTypeAttribute.BaseType;
            Type fieldType = fieldInfo.FieldType;
            if (property.isArray || fieldType.IsArray || fieldType.IsGenericType && fieldType.GenericTypeArguments.Length > 0)
            {
                Type elementType = fieldType.IsArray ? fieldType.GetElementType() : fieldType.GenericTypeArguments[0];
                return elementType.IsAssignableFrom(parentType);
            }
            else
            {
                return fieldType.IsAssignableFrom(parentType);
            }
        }

        /// <summary>
        /// Removes from <c>m_ChildTypes</c> all types that are not compatible with managed reference and parameterless constructors
        /// </summary>
        private void FilterInvalidTypes()
        {
            for (int i = m_ChildTypes.Count - 1; i >= 0; i--)
            {
                Type type = m_ChildTypes[i];
                if (!IsValidForAssignment(type))
                {
                    m_ChildTypes.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// Returns if a type is an Unity Object or does not have a parameterless constructor.
        /// </summary>
        /// <param name="type">Type being checked</param>
        /// <returns>Is the type valid for being assigned to the serialized reference</returns>
        private bool IsValidForAssignment(Type type)
        {
            return !typeof(UnityEngine.Object).IsAssignableFrom(type) && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null) != null;
        }
    }
}
