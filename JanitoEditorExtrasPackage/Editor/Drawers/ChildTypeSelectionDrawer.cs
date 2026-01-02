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
        private sealed class ChildTypeSelectionPropertyWrapper : VisualElement
        {
            private readonly ChildTypeSelectionAttribute _childTypeAttribute;
            private readonly List<Type> _childTypes;
            private readonly SerializedProperty _property;
            private readonly DropdownField _selectionField;
            private readonly PropertyField _propertyField;

            public ChildTypeSelectionPropertyWrapper(SerializedProperty property, ChildTypeSelectionAttribute attribute, List<Type> childTypes)
            {
                _property = property;
                _childTypeAttribute = attribute;
                _childTypes = childTypes;

                // Add dropdown field to select type
                var typeOptions = GetChildTypeNamesList(property);
                _selectionField = new DropdownField(typeOptions, GetSelectionIndex(property));
                _selectionField.RegisterValueChangedCallback(OnSelection);
                Add(_selectionField);

                //Add property field to display the serialized reference
                _propertyField = new(property);
                Add(_propertyField);

                // Listen for removal
                RegisterCallback<DetachFromPanelEvent>(OnDettach);
            }

            /// <summary>
            /// Executed when the root visual element is removed from Inspector
            /// </summary>
            private void OnDettach(DetachFromPanelEvent _evt)
            {
                _propertyField.Unbind();
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

                foreach (Type type in _childTypes)
                {
                    list.Add(type.Name);
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

                for (int i = 0; i < _childTypes.Count; i++)
                {
                    if (_childTypes[i].Name == value.GetType().Name)
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

                Type type = GetMatchingType(evt.newValue);
                if (_property.isArray)
                {
                    AddReferenceToArray(type);
                }
                else
                {
                    AssignReference(type);
                }
            }

            private Type GetMatchingType(string name)
            {
                foreach (Type type in _childTypes)
                {
                    if (name == type.Name)
                    {
                        return type;
                    }
                }

                return null;
            }

            private void AddReferenceToArray(Type type)
            {
                if (type == null) return;

                try
                {
                    int newIndex = _property.arraySize;
                    _property.InsertArrayElementAtIndex(newIndex);

                    SerializedProperty elementProperty = _property.GetArrayElementAtIndex(newIndex);

                    elementProperty.managedReferenceValue = Activator.CreateInstance(type);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to insert new instance of {type.Name} in array: {exception.Message}");
                    return;
                }

                _property.serializedObject.ApplyModifiedProperties();
            }

            private void AssignReference(Type type)
            {
                if (type == null)
                {
                    _property.managedReferenceValue = null;
                }
                else
                {
                    try
                    {
                        _property.managedReferenceValue = Activator.CreateInstance(type);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Failed to create instance of {type.Name}: {exception.Message}");
                        return;
                    }
                }

                _property.serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// Criteria used to retrieve the displayed child types. Abstract and interface types are excluded due to not being compatible with CreateInstance.
        /// </summary>
        private readonly TypeCriteria _typeCriteria = new(TypeCriteria.TypeRequirementFlags.NotAbstract | TypeCriteria.TypeRequirementFlags.NotInterface);
        private ChildTypeSelectionAttribute _childTypeAttribute => (ChildTypeSelectionAttribute)attribute;
        private List<Type> _childTypes;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!HasSerializedReferenceAttribute())
            {
                throw new CustomAttributeFormatException($"[ChildTypeSelection] {nameof(ChildTypeSelectionAttribute)} must have the {nameof(SerializeField)} attribute in order to work properly. Please add it.");
            }

            if (!IsFieldCompatible(property))
            {
                throw new ArgumentException($"[ChildTypeSelection] The type {_childTypeAttribute.BaseType.Name} is not assignable to {fieldInfo.FieldType.Name} type in field {fieldInfo.Name}. Please assign compatible types.");
            }

            _childTypes = new(GetEnumerableOfTypeChildren(_childTypeAttribute.BaseType, _typeCriteria, true));
            FilterInvalidTypes();

            return new ChildTypeSelectionPropertyWrapper(property, _childTypeAttribute, _childTypes);
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
            Type parentType = _childTypeAttribute.BaseType;
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
        /// Removes from <c>_childTypes</c> all types that are not compatible with managed reference and parameterless constructors
        /// </summary>
        private void FilterInvalidTypes()
        {
            for (int i = _childTypes.Count - 1; i >= 0; i--)
            {
                Type type = _childTypes[i];
                if (!IsValidForAssignment(type))
                {
                    _childTypes.RemoveAt(i);
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
