using System;
using System.Diagnostics;
using UnityEngine;

namespace Janito.EditorExtras
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CreateButtonAttribute : PropertyAttribute 
    {
        public const string k_DefaultSavePath = "Assets"; // Requires 'Assets' to be the root for valid save paths
        public readonly string NamingFormat;
        public readonly string SavePath;

        public CreateButtonAttribute(string namingFormat = null, string savePath = k_DefaultSavePath)
        {
            savePath.Trim();
            if (string.IsNullOrEmpty(savePath) || string.IsNullOrWhiteSpace(savePath))
            {
                savePath = k_DefaultSavePath;
            }
            else if(!savePath.StartsWith(k_DefaultSavePath))
            {
                savePath = $"{k_DefaultSavePath}/{savePath}";
                LogLibrary.LogWarningInDevelopment<CreateButtonAttribute>($"Create Button save path must start with '{k_DefaultSavePath}' as the root.");
            }

            NamingFormat = namingFormat;
            SavePath = savePath;
        }

        public CreateButtonAttribute()
        {
            SavePath = k_DefaultSavePath;
            NamingFormat = null;
        }
    }
}
