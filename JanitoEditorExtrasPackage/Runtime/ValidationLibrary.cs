using System;
using UnityEngine;

namespace Janito.EditorExtras
{
    public static class ValidationLibrary
    {
        public static void SetAndValidateReference<T>(this UnityEngine.Object @object, ref T reference, T value) where T : class
        {
            if (value == null)
            {
                throw new NullReferenceException($"Unable to set reference `{nameof(reference)}`. Object: {@object}.");
            }

            reference = value;
        }

        public static void SetAndValidateReference<T>(ref T reference, T value) where T : class
        {
            if (value == null)
            {
                throw new NullReferenceException($"Unable to set reference '{nameof(reference)}'.");
            }

            reference = value;
        }
    }
}
