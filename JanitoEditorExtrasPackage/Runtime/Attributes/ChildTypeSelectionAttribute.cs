using System;
using System.Diagnostics;
using UnityEngine;

namespace Janito.EditorExtras
{
    /// <summary>
    /// Adds a dropdown to select and assign one of the types that inherit from the provided abstract class, excluding other abstract classes. Intended to be used along with the SerializeReference attribute.
    /// </summary>
    /// <remarks>Must be used along with the SerializeReference to work as intended</remarks>
    /// <seealso cref="SerializeReference"/>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    public class ChildTypeSelectionAttribute : PropertyAttribute
    {
        public readonly Type BaseType;

        public ChildTypeSelectionAttribute(Type baseType) 
        {
            if (!baseType.IsAbstract)
            {
                throw new ArgumentException($"Type {baseType.Name} is not an abstract class.");
            }

            if (typeof(UnityEngine.Object).IsAssignableFrom(baseType)) 
            {
                throw new ArgumentException($"Type {baseType.Name} must not inherit from Unity Object class.");
            }

            BaseType = baseType;
        }
    }
}
