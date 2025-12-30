using System;
using UnityEngine;

namespace Janito.EditorExtras
{
    /// <summary>
    /// Adds a dropdown to select and assign one of the types that inherit from the provided abstract class, excluding other abstract classes. Intended to be used along with the SerializeReference attribute.
    /// </summary>
    /// <remarks>Must be used along with the SerializeReference to work as intended</remarks>
    /// <seealso cref="SerializeReference"/>
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

            BaseType = baseType;
        }
    }
}
