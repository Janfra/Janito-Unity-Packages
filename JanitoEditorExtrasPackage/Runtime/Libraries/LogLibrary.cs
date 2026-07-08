using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Janito.EditorExtras
{
    /// <summary>
    /// Collection of logging extension methods and functions to log messages with a prefix of the type of the object for easy identification. It includes editor and development build only versions of the methods to avoid unnecessary logs in production builds, and non <c>UnityEngine.Object</c> contexts versions that require the type to be specified as a generic parameter.
    /// </summary>
    /// <remarks>
    /// Non <c>UnityEngine.Object</c> contexts do not include an extension method the same way <c>UnityEngine.Object</c> classes do to avoid ambiguity when accessing extension method. For non <c>UnityEngine.Object</c> contexts, use the generic version of the method and specify the type as the generic parameter using <c>LogLibrary</c>.
    /// </remarks>
    /// <example>
    /// LogLibrary.LogInDevelopment<SomeClass>("This is a log message from SomeClass."); // Without context
    /// LogLibrary.LogInDevelopment<SomeClass>("This is a log message from SomeClass with context.", someObject); // With context
    /// this.LogInDevelopment("This is a log message from this object."); // Logging with extension method in any <c>UnityEngine.Object</c> class.
    /// </example>
    public static class LogLibrary
    {
        #region Editor/Development Only
        /// <summary>
        /// Logs a message only in editor and in development builds while appending as a prefix the type of the object for easy identification.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="message"></param>
        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogInDevelopment(this Object @object, string message)
        {
            @object.LogPrefixed(message);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarningInDevelopment(this Object @object, string message)
        {
            @object.LogWarningPrefixed(message);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogErrorInDevelopment(this Object @object, string message)
        {
            @object.LogErrorPrefixed(message);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogAssertionInDevelopment(this Object @object, string message)
        {
            @object.LogAssertionPrefixed(message);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogInDevelopment<T>(string message, Object context = null)
        {
            LogPrefixed<T>(message, context);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarningInDevelopment<T>(string message, Object context = null)
        {
            LogWarningPrefixed<T>(message, context);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogErrorInDevelopment<T>(string message, Object context = null)
        {
            LogErrorPrefixed<T>(message, context);
        }

        [HideInCallstack]
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogAssertionInDevelopment<T>(string message, Object context = null)
        {
            LogAssertionPrefixed<T>(message, context);
        }

        #endregion

        /// <summary>
        /// Logs a message while appending as a prefix the type of the object for easy identification.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="message"></param>
        [HideInCallstack]
        public static void LogPrefixed(this Object @object, string message)
        {
            Debug.Log($"[{@object.GetType().Name}] " + message, @object);
        }

        [HideInCallstack]
        public static void LogWarningPrefixed(this Object @object, string message)
        {
            Debug.LogWarning($"[{@object.GetType().Name}] " + message, @object);
        }

        [HideInCallstack]
        public static void LogErrorPrefixed(this Object @object, string message)
        {
            Debug.LogError($"[{@object.GetType().Name}] " + message, @object);
        }

        [HideInCallstack]
        public static void LogAssertionPrefixed(this Object @object, string message)
        {
            Debug.LogAssertion($"[{@object.GetType().Name}] " + message, @object);
        }

        [HideInCallstack]
        public static void LogPrefixed<T>(string message, Object context = null)
        {
            Debug.Log($"[{typeof(T).Name}] " + message, context);
        }

        [HideInCallstack]
        public static void LogWarningPrefixed<T>(string message, Object context = null)
        {
            Debug.LogWarning($"[{typeof(T).Name}] " + message, context);
        }

        [HideInCallstack]
        public static void LogErrorPrefixed<T>(string message, Object context = null)
        {
            Debug.LogError($"[{typeof(T).Name}] " + message, context);
        }

        [HideInCallstack]
        public static void LogAssertionPrefixed<T>(string message, Object context = null)
        {
            Debug.LogAssertion($"[{typeof(T).Name}] " + message, context);
        }
    }
}
