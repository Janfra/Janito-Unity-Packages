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
    /// <seealso cref="ButtonAttribute"/> 
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButtonEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Name of the root VisualElement that contains all the buttons created for methods marked with <c>ButtonAttribute</c>
        /// </summary>
        public const string ButtonEditorRootName = "ButtonEditorRoot";  

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            root.Add(GetButtonsForMethodsFromObject(target));
            return root;
        }

        /// <summary>
        /// Creates a VisualElement containing buttons for all methods of the given object that are marked with the <c>ButtonAttribute</c>
        /// </summary>
        /// <param name="object">The target object whose methods will be inspected for <c>ButtonAttribute</c>s</param>
        /// <returns>A VisualElement containing buttons for all methods marked with <c>ButtonAttribute</c></returns>
        /// <seealso cref="ButtonAttribute"/>
        public static VisualElement GetButtonsForMethodsFromObject(UnityEngine.Object @object)
        {
            var root = new VisualElement();
            root.name = ButtonEditorRootName;
            var methods = @object.GetType().GetMethods(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static
            );

            foreach (var method in methods)
            {
                if (TryCreateButtonForMethod(method, @object, out var button))
                {
                    root.Add(button);
                }
            }

            return root;
        }

        /// <summary>
        /// Creates a button for the given method if it has a <c>ButtonAttribute</c>. The button will invoke the method when clicked, and its label and tooltip will be set based on the <c>ButtonAttribute</c>'s properties. The button's enabled state will also be determined by the current application mode (editor or play mode) and the <c>ButtonAttribute</c>'s ExecutionMode
        /// </summary>
        /// <param name="method">The method info of the target method</param>
        /// <param name="object">The target object on which the method is invoked</param>
        /// <param name="button">The created button if the method has a <c>ButtonAttribute</c>; otherwise, null</param>
        /// <returns>True if the button was created; otherwise, false</returns>
        /// <seealso cref="ButtonAttribute"/>
        public static bool TryCreateButtonForMethod(MethodInfo method, UnityEngine.Object @object, out Button button)
        {
            button = null;
            var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
            if (buttonAttribute == null) return false;

            if (method.GetParameters().Length > 0)
            {
                LogLibrary.LogErrorPrefixed<ButtonAttribute>($"Method {method.Name} on {@object.name} has parameters and cannot be invoked by a button. Buttons can only be created for parameterless methods.", @object);
                return false;
            }

            button = new Button(() =>
            {
                try
                {
                    // Performs logging if not valid, acts as a fallback in case somehow the button is invoked in the wrong context
                    if (!IsButtonExecutionValid(buttonAttribute, method, @object))
                    {
                        return;
                    }

                    method.Invoke(@object, null);
                }
                catch (Exception e)
                {
                    LogLibrary.LogErrorPrefixed<ButtonAttribute>($"Failed to invoke method {method.Name} on {@object.name} with exception: {e}", @object);
                }
            })
            {
                text = buttonAttribute.ButtonLabel ?? method.Name,
                tooltip = buttonAttribute.Tooltip,
                enabledSelf = CanButtonBeActiveInCurrentApplicationMode(buttonAttribute)
            };
            return true;
        }

        /// <summary>
        /// Verifies that the button can be executed in the current application mode (editor or play mode) based on the <c>ButtonAttribute</c>'s <c>ExecutionMode</c> and logs a warning if the button is not valid for the current mode
        /// </summary>
        /// <param name="attribute">The <c>ButtonAttribute</c> associated with the button</param>
        /// <param name="method">The method info of the button's target method</param>
        /// <param name="target">The target object on which the method is invoked</param>
        /// <returns>Is the button execution valid in the current application mode</returns>
        private static bool IsButtonExecutionValid(ButtonAttribute attribute, MethodInfo method, UnityEngine.Object @object)
        {
            if (!IsButtonAlwaysExecutable(attribute))
            {
                if (EditorApplication.isPlaying && !attribute.ExecutionMode.HasFlag(ButtonExecutionMode.PlayMode))
                {
                    LogLibrary.LogWarningPrefixed<ButtonAttribute>($"Method {method.Name} on {@object.name} is marked as an editor button and cannot be invoked during runtime.", @object);
                    return false;
                }
                else if (!EditorApplication.isPlaying && !attribute.ExecutionMode.HasFlag(ButtonExecutionMode.EditorMode))
                {
                    LogLibrary.LogWarningPrefixed<ButtonAttribute>($"Method {method.Name} on {@object.name} is marked as a runtime button and cannot be invoked in the editor.", @object);
                    return false;
                }
            }

            return true;
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
