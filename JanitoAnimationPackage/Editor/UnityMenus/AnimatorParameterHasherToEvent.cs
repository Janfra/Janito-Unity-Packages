using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Janito.Animations.Editor
{
    public static class AnimatorParameterHasherToEvent
    {
        private const string MenuPath = "Assets/Janito/Create Parameter Event From Parameter Hasher";

        [MenuItem(MenuPath, true)]
        public static bool ValidateCreateEvent()
        {
            foreach(var obj in Selection.objects)
            {
                // If any is valid, allow the menu item to be selected. We will handle invalid selections in the CreateEvent method.
                if (IsValidSelection(obj))
                {
                    return true;
                }
            }
            return false;
        }

        [MenuItem(MenuPath)]
        public static void CreateEvent()
        {
            List<Object> newSelection = new List<Object>();
            foreach (var obj in Selection.objects)
            {
                if (IsValidSelection(obj))
                {
                    newSelection.Add(GenerateEventFromHasher((AnimatorParameterHasher)obj));
                }
            }

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.objects = newSelection.ToArray();
        }

        private static Object GenerateEventFromHasher(AnimatorParameterHasher hasher)
        {
            var eventAsset = GetMatchingEventFromHasher(hasher);
            eventAsset.Initialise(hasher);
            var path = AssetDatabase.GetAssetPath(hasher);
            var directory = System.IO.Path.GetDirectoryName(path);
            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(path);
            var newPath = System.IO.Path.Combine(directory, $"{fileNameWithoutExtension}_Event.asset");

            AssetDatabase.CreateAsset(eventAsset, newPath);
            return eventAsset;
        }

        private static bool IsValidSelection(Object selection)
        {
            return selection is AnimatorParameterHasher hasher && hasher.Type != 0;
        }
        private static AnimatorParameterEvent GetMatchingEventFromHasher(AnimatorParameterHasher hasher)
        {
            switch (hasher.Type)
            {
                case AnimatorControllerParameterType.Bool:
                    return ScriptableObject.CreateInstance<AnimatorBoolParameterEvent>();
                case AnimatorControllerParameterType.Trigger:
                    return ScriptableObject.CreateInstance<AnimatorTriggerParameterEvent>();

                case AnimatorControllerParameterType.Float:
                case AnimatorControllerParameterType.Int:
                default:
                    throw new NotImplementedException($"No event type implemented for parameter type {hasher.Type}");
            }
        }
    }
}
