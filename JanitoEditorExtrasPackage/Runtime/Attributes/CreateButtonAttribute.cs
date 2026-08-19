using System;
using System.Diagnostics;
using UnityEngine;

namespace Janito.EditorExtras
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CreateButtonAttribute : PropertyAttribute 
    {
        public const string k_DefaultSavePath = "Assets";
        public readonly string NamingFormat;
        public readonly string SavePath;

        public CreateButtonAttribute(string namingFormat = null, string savePath = k_DefaultSavePath)
        {
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
