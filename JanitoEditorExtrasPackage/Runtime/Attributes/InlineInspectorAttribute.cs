using System;
using System.Diagnostics;
using UnityEngine;

namespace Janito.EditorExtras
{
    /// <summary>
    /// Adds an inline inspector as a foldout to an object field in the Unity Inspector. This allows you to view and edit the properties of the object directly within the inspector, without having to navigate to a separate inspector window.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InlineInspectorAttribute : PropertyAttribute
    {
        /// <summary>
        /// Forces the inline inspector to fallback to the default inspector if the object does not have support for <c>CreateInspectorGUI</c>. This is useful for objects if you want to see the default inspector instead of nothing.
        /// </summary>
        public readonly bool ForceInspector;

        public InlineInspectorAttribute()
        {
            ForceInspector = false;
        }

        public InlineInspectorAttribute(bool forceInspector)
        {
            ForceInspector = forceInspector;
        }
    }
}
