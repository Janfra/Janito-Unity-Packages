using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Janito.EditorExtras.Editor
{
    /// <summary>
    /// Creates the inspector buttons for the methods and functions marked with <c>ButtonAttribute</c>
    /// </summary>
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButtonEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            var methods = target.GetType().GetMethods(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static
            );

            foreach (var method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                if (buttonAttribute == null) continue;

                var button = new Button(() =>
                {
                    // Check if the button is meant for runtime or editor and log a warning if it's being invoked in the wrong context as a fallback
                    // Logs are not development/editor only since this is an editor script and will not be included in builds
                    try
                    {
                        if (!IsButtonAlwaysExecutable(buttonAttribute))
                        {
                            if (EditorApplication.isPlaying && !buttonAttribute.ExecutionMode.HasFlag(ButtonExecutionMode.PlayMode))
                            {
                                LogLibrary.LogWarningPrefixed<ButtonAttribute>($"Method {method.Name} on {target.name} is marked as an editor button and cannot be invoked during runtime.", target);
                                return;
                            }
                            else if (!EditorApplication.isPlaying && !buttonAttribute.ExecutionMode.HasFlag(ButtonExecutionMode.EditorMode))
                            {
                                LogLibrary.LogWarningPrefixed<ButtonAttribute>($"Method {method.Name} on {target.name} is marked as a runtime button and cannot be invoked in the editor.", target);
                                return;
                            }
                        }

                        method.Invoke(target, null);
                    }
                    catch (Exception e)
                    {
                        LogLibrary.LogErrorPrefixed<ButtonAttribute>($"Failed to invoke method {method.Name} on {target.name} with exception: {e}", target);
                    }
                })
                {
                    text = buttonAttribute.ButtonLabel ?? method.Name,
                    tooltip = buttonAttribute.Tooltip,
                    enabledSelf = CanButtonBeActiveInCurrentApplicationMode(buttonAttribute)
                };

                root.Add(button);
            }

            return root;
        }

        private static bool IsButtonAlwaysExecutable(ButtonAttribute attribute)
        {
            return attribute.ExecutionMode == ButtonExecutionMode.All;
        }

        private static bool CanButtonBeActiveInCurrentApplicationMode(ButtonAttribute attribute)
        {
            return attribute.ExecutionMode.HasFlag(ButtonExecutionMode.PlayMode) && EditorApplication.isPlaying || 
                attribute.ExecutionMode.HasFlag(ButtonExecutionMode.EditorMode) && !EditorApplication.isPlaying;
        }
    }
}
