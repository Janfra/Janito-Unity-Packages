using System;
using System.Diagnostics;
using UnityEngine;

namespace Janito.EditorExtras
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InlineInspectorAttribute : PropertyAttribute
    {
    
    }
}
