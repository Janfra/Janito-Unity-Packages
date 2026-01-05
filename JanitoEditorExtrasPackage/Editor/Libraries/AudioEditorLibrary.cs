using System;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Janito.EditorExtras.Editor
{
    public static class AudioEditorLibrary
    {
        private const string AudioUtilName = "UnityEditor.AudioUtil";

        public static void PlayPreviewClip(AudioClip clip, Int32 startSample, bool loop)
        {
            TryExecuteMethod(nameof(PlayPreviewClip), clip, startSample, loop);
        }

        public static void PausePreviewClip()
        {
            TryExecuteMethod(nameof(PausePreviewClip));
        }

        public static void ResumePreviewClip()
        {
            TryExecuteMethod(nameof(ResumePreviewClip));
        }

        public static void LoopPreviewClip(bool isLooping)
        {
            TryExecuteMethod(nameof(LoopPreviewClip), isLooping);
        }

        public static bool IsPreviewClipPlaying()
        {
            return TryExecuteAndReturnMethod<bool>(nameof(IsPreviewClipPlaying));
        }

        public static void StopAllPreviewClips()
        {
            TryExecuteMethod(nameof(StopAllPreviewClips));
        }

        public static Single GetPreviewClipPosition()
        {
            return TryExecuteAndReturnMethod<Single>(nameof(GetPreviewClipPosition));
        }

        public static Int32 GetPreviewClipSamplePosition()
        {
            return TryExecuteAndReturnMethod<Int32>(nameof(GetPreviewClipSamplePosition));
        }

        public static void SetPreviewClipSamplePosition(AudioClip clip, Int32 samplePosition)
        {
            TryExecuteMethod(nameof(SetPreviewClipSamplePosition), clip, samplePosition);
        }

        public static Int32 GetSampleCount(AudioClip clip)
        {
            return TryExecuteAndReturnMethod<Int32, AudioClip>(nameof(GetSampleCount), clip); 
        }

        public static bool HasPreview(AudioClip clip)
        {
            return TryExecuteAndReturnMethod<bool, AudioClip>(nameof(HasPreview), clip);
        }

        public static double GetDuration(AudioClip clip)
        {
            return TryExecuteAndReturnMethod<double, AudioClip>(nameof(GetDuration), clip);
        }

        private static void TryExecuteMethod<T1>(string methodName, T1 paramOne)
        {
            Type[] parameters = { typeof(T1) };
            if (TryGetMethod(out MethodInfo method, methodName, parameters))
            {
                Invoke(method, new object[] { paramOne });
            }
        }

        private static void TryExecuteMethod<T1, T2>(string methodName, T1 paramOne, T2 paramTwo)
        {
            Type[] parameters = { typeof(T1), typeof(T2) };
            if (TryGetMethod(out MethodInfo method, methodName, parameters))
            {
                Invoke(method, new object[] { paramOne, paramTwo });
            }
        }

        private static void TryExecuteMethod<T1, T2, T3>(string methodName, T1 paramOne, T2 paramTwo, T3 paramThree)
        {
            Type[] parameters = { typeof(T1), typeof(T2), typeof(T3) };
            if (TryGetMethod(out MethodInfo method, methodName, parameters))
            {
                Invoke(method, new object[] { paramOne, paramTwo, paramThree });
            }
        }

        private static TReturn TryExecuteAndReturnMethod<TReturn, T1>(string methodName, T1 paramOne)
        {
            Type[] parameters = { typeof(T1) };
            if (TryGetMethod(out MethodInfo method, methodName, parameters))
            {
                Invoke<TReturn>(method, new object[] { paramOne });
            }
            return default;
        }

        private static TReturn TryExecuteAndReturnMethod<TReturn, T1, T2>(string methodName, T1 paramOne, T2 paramTwo)
        {
            Type[] parameters = { typeof(T1), typeof(T2) };
            if (TryGetMethod(out MethodInfo method, methodName, parameters))
            {
                Invoke<TReturn>(method, new object[] { paramOne, paramTwo });
            }
            return default;
        }

        private static TReturn TryExecuteAndReturnMethod<TReturn, T1, T2, T3>(string methodName, T1 paramOne, T2 paramTwo, T3 paramThree)
        {
            Type[] parameters = { typeof(T1), typeof(T2), typeof(T3) };
            if (TryGetMethod(out MethodInfo method, methodName, parameters))
            {
                Invoke<TReturn>(method, new object[] { paramOne, paramTwo, paramThree });
            }
            return default;
        }

        private static void TryExecuteMethod(string methodName)
        {
            if (TryGetMethod(out MethodInfo method, methodName))
            {
                Invoke(method);
            }
        }

        private static T TryExecuteAndReturnMethod<T>(string methodName)
        {
            if (TryGetMethod(out MethodInfo method, methodName))
            {
                return Invoke<T>(method);
            }
            return default;
        }

        private static Type GetAudioUtilType()
        {
            Assembly audioEditorAssembly = typeof(AudioImporter).Assembly;
            return audioEditorAssembly.GetType(AudioUtilName);
        }

        private static bool TryGetMethod(out MethodInfo method, string methodName, Type[] parameters = null)
        {
            if (parameters == null) parameters = Type.EmptyTypes;
            method = GetAudioUtilType()?.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, parameters, null);
            if (method == null)
            {
                PrintNotFound(methodName);
            }
            return method != null;
        }

        private static void PrintNotFound(string methodName)
        {
            Debug.LogError($"Unable to find method {methodName}.");
        }

        private static T Invoke<T>(MethodInfo method, object[] arguments = null)
        {
            try
            {
                return (T)method.Invoke(null, arguments);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return default;
            }
        }

        private static void Invoke(MethodInfo method, object[] arguments = null)
        {
            try
            {
                method.Invoke(null, arguments);
            }
            catch (Exception exception) 
            {
                Debug.LogException(exception);
            }
        }

        [MenuItem("Tools/Janito/Logging/Print Audio Methods")]
        private static void PrintAudioUtilsMethods()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type audioUtilClass = GetAudioUtilType();
            if (audioUtilClass == null)
            {
                Debug.LogError($"Unable to find AudioUtil class!");
                return;
            }

            stringBuilder.AppendLine($"List of methods for {audioUtilClass.Name}:");
            foreach (var item in audioUtilClass.GetMethods())
            {
                if (item.IsPublic && item.IsStatic)
                {
                    var parameters = item.GetParameters();
                    stringBuilder.AppendLine($"- Name: {item.Name}");
                    if (parameters.Length > 0)
                    {
                        stringBuilder.AppendLine($"  - Parameters:");
                        foreach (var parameter in parameters)
                        {
                            stringBuilder.AppendLine($"        {parameter.Name}: {parameter.ParameterType}");
                        }
                    }
                    stringBuilder.AppendLine($"  - Returns: {item.ReturnType}");
                    stringBuilder.AppendLine("");
                }
            }

            Debug.Log(stringBuilder.ToString());
        }
    }
}
