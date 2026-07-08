using System;
using UnityEngine;

namespace Janito.EditorExtras
{
    /// <summary>
    /// Determines in which application mode this button can be executed.
    /// </summary>
    [Flags]
    public enum ButtonExecutionModes
    {
        PlayMode = 1 << 0,
        EditorMode = 1 << 1,
        All = PlayMode | EditorMode,
    }

    /// <summary>
    /// Marks the method to have a button drawn in the inspector that will attempt to execute on clicked.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute 
    {
        public readonly string ButtonLabel;
        public readonly ButtonExecutionModes ExecutionMode;
        public readonly string Tooltip;

        public ButtonAttribute(string buttonLabel = null, ButtonExecutionModes executionMode = ButtonExecutionModes.All, string tooltip = null)
        {
            ButtonLabel = buttonLabel;
            ExecutionMode = executionMode;
            Tooltip = tooltip;
        }

        public ButtonAttribute(string buttonLabel)
        {
            ButtonLabel = buttonLabel;
            ExecutionMode = ButtonExecutionModes.All;
            Tooltip = string.Empty;
        }

        public ButtonAttribute(ButtonExecutionModes executionMode)
        {
            ExecutionMode = executionMode;
            ButtonLabel = string.Empty;
            Tooltip = string.Empty;
        }
    }
}
