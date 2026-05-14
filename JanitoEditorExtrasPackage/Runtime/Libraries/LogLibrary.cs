using UnityEngine;

namespace Janito.EditorExtras
{
    public static class LogLibrary
    {
        #region Editor/Development Only
        /// <summary>
        /// Logs a message only in editor and in development builds while appending as a prefix the type of the object for easy identification.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="message"></param>
        public static void LogInDevelopment(this Object @object, string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            @object.LogPrefixed(message);
#endif
        }

        public static void LogWarningInDevelopment(this Object @object, string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            @object.LogWarningPrefixed(message);
#endif
        }

        public static void LogErrorInDevelopment(this Object @object, string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            @object.LogErrorPrefixed(message);  
#endif
        }

        public static void LogAssertionInDevelopment(this Object @object, string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            @object.LogAssertionPrefixed(message);
#endif
        }
        #endregion

        /// <summary>
        /// Logs a message while appending as a prefix the type of the object for easy identification.
        /// </summary>
        /// <param name="object"></param>
        /// <param name="message"></param>
        public static void LogPrefixed(this Object @object, string message) 
        {
            Debug.Log($"[{@object.GetType().Name}] " + message, @object);
        }

        public static void LogWarningPrefixed(this Object @object, string message)
        {
            Debug.LogWarning($"[{@object.GetType().Name}] " + message, @object);
        }

        public static void LogErrorPrefixed(this Object @object, string message)
        {
            Debug.LogError($"[{@object.GetType().Name}] " + message, @object);
        }

        public static void LogAssertionPrefixed(this Object @object, string message)
        {
            Debug.LogAssertion($"[{@object.GetType().Name}] " + message, @object);
        }
    }
}
