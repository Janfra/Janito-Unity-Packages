using System;
using System.Diagnostics;
using UnityEngine;

namespace Janito.EditorExtras
{
    /// <summary>
    /// Adds a dropdown to select and assign one of the types that inherit from the provided abstract or interface class, excluding other abstract or interface classes. Intended to be used along with the <c>SerializeReference</c> attribute.
    /// </summary>
    /// <remarks>
    /// Must be used along with the <c>SerializeReference</c> to work as intended. 
    /// <seealso cref="SerializeReference"/>
    /// </remarks>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ChildTypeSelectionAttribute : PropertyAttribute
    {
        public readonly Type BaseType;

        public ChildTypeSelectionAttribute(Type baseType) 
        {
            if (!baseType.IsAbstract && !baseType.IsInterface)
            {
                throw new ArgumentException($"Type {baseType.Name} is not an abstract or interface class.");
            }

            if (typeof(UnityEngine.Object).IsAssignableFrom(baseType))
            {
                throw new ArgumentException($"Type {baseType.Name} must not inherit from Unity Object class.");
            }

            BaseType = baseType;
        }
    }
}
