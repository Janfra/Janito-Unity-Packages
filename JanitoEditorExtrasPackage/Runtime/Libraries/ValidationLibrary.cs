using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Janito.EditorExtras
{
    public static class ValidationLibrary
    {
        private const string REQUIRED_REFERENCE_MISSING_PREFIX = "Unable to set required reference";

        public static void SetAndValidateReference<T>(this Object @object, ref T reference, T value) where T : class
        {
            if (value == null)
            {
                Debug.Break();
                throw new NullReferenceException($"{GetRequiredReferenceMissingPrefix()}. {GetObjectSuffix(@object)}");
            }

            reference = value;
        }

        public static void SetAndValidateReference<T>(ref T reference, T value) where T : class
        {
            if (value == null)
            {
                Debug.Break();
                throw new NullReferenceException($"{GetRequiredReferenceMissingPrefix()}.");
            }

            reference = value;
        }

        public static void SetAndValidateReference<T>(this Object @object, ref T reference, string referenceName, T value) where T : class
        {
            if (value == null)
            {
                Debug.Break();
                throw new NullReferenceException($"{GetRequiredReferenceMissingPrefix()} `{referenceName}`. {GetObjectSuffix(@object)}");
            }

            reference = value;
        }

        public static void SetAndValidateReference<T>(ref T reference, string referenceName, T value) where T : class
        {
            if (value == null)
            {
                Debug.Break();
                throw new NullReferenceException($"{GetRequiredReferenceMissingPrefix()} `{referenceName}`.");
            }

            reference = value;
        }

        private static string GetRequiredReferenceMissingPrefix()
        {
            return REQUIRED_REFERENCE_MISSING_PREFIX;
        }

        private static string GetObjectSuffix(Object @object)
        {
            return $"Object: {@object.name}";
        }
    }
}
