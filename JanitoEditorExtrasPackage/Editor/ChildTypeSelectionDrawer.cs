using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Janito.EditorExtras.EditorExtrasLibrary;

namespace Janito.EditorExtras.Editor
{
    [CustomPropertyDrawer(typeof(ChildTypeSelectionAttribute))]
    public class ChildTypeSelectionDrawer : PropertyDrawer
    {
        private ChildTypeSelectionAttribute _childTypeAttribute => (ChildTypeSelectionAttribute)attribute;
        private List<Type> _childTypes;
        private SerializedProperty _property;
        private DropdownField _selectionField;

        /// <summary>
        /// Criteria used to retrieve the displayed child types. Abstract and interface types are excluded due to not being compatible with CreateInstance.
        /// </summary>
        private TypeCriteria _typeCriteria = new(TypeCriteria.TypeRequirementFlags.NotAbstract | TypeCriteria.TypeRequirementFlags.NotInterface);

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!HasSerializedReferenceAttribute())
            {
                throw new CustomAttributeFormatException($"[ChildTypeSelection] {nameof(ChildTypeSelectionAttribute)} must have the {nameof(SerializeField)} attribute in order to work properly. Please add it.");
            }

            if (!fieldInfo.FieldType.IsAssignableFrom(_childTypeAttribute.BaseType))
            {
                throw new ArgumentException($"[ChildTypeSelection] The type {_childTypeAttribute.BaseType.Name} is not assignable to {fieldInfo.FieldType.Name} type in field {fieldInfo.Name}. Please assign compatible types.");
            }


            VisualElement root = new();
            _property = property;
            _childTypes = new(GetEnumerableOfTypeChildren(_childTypeAttribute.BaseType, _typeCriteria));
            FilterInvalidTypes();
  
            // Add to the inspector the dropfield to select type and a property field to display the serialized reference
            AddTypeSelectionDropdown(root, property);
            PropertyField propertyField = new(property);
            root.Add(propertyField);

            return root;
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
            return typeof(UnityEngine.Object).IsAssignableFrom(type) && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null) != null;
        }

        private void AddTypeSelectionDropdown(VisualElement root, SerializedProperty property)
        {
            var typeOptions = GetChildTypeNamesList(property);
            _selectionField = new DropdownField(typeOptions, GetSelectionIndex(property));
            _selectionField.RegisterValueChangedCallback(OnSelection);
            root.Add(_selectionField);
        }

        private List<string> GetChildTypeNamesList(SerializedProperty property)
        {
            string firstValue = property.managedReferenceValue == null ? $"Select type to set on {property.name}" : $"Set {property.name} to null";
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
            AssignReference(type);
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
                catch(Exception exception)
                {
                    Debug.LogError($"Failed to create instance of {type.Name}: {exception.Message}");
                    return;
                }
            }

            _property.serializedObject.ApplyModifiedProperties();
        }
    }
}
