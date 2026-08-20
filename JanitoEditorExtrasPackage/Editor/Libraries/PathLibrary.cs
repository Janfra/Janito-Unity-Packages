using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Janito.EditorExtras.Editor
{
    public static class PathLibrary
    {
        public const string k_EditorProjectAssetsRoot = "Assets";

        /// <summary>
        /// Opens a folder selection window to the user, returns the selected path if it is within the project assets folder
        /// </summary>
        /// <param name="destinationPath">On success it contains a path within the project assets folder, otherwise empty</param>
        /// <param name="folder">Folder to initially open</param>
        /// <param name="title">Title of the folder selection window</param>
        /// <param name="defaultName">Default name to name the folder</param>
        /// <returns>Was a valid path within the project assets folder selected</returns>
        public static bool TryGetProjectPathFromUser(out string destinationPath, string folder = "Assets", string title = "Select Destination", string defaultName = "")
        {
            string path = EditorUtility.OpenFolderPanel(title, folder, defaultName);

            if (TryGetProjectRelativePath(path, out destinationPath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Opens a save folder selection window to the user, returns the selected path if it is within the project assets folder
        /// </summary>
        /// <param name="destinationPath">On success it contains a path within the project assets folder, otherwise empty</param>
        /// <param name="folder">Folder to initially open</param>
        /// <param name="title">Title of the folder selection window</param>
        /// <param name="defaultName">Default name to name the folder</param>
        /// <returns>Was a valid path within the project assets folder selected</returns>
        public static bool TryGetProjectSavePathFromUser(out string destinationPath, string folder = "Assets", string title = "Select Destination", string defaultName = "")
        {
            destinationPath = EditorUtility.SaveFolderPanel(title, folder, defaultName);
            return IsPathInAssetsPath(destinationPath);
        }

        /// <summary>
        /// Attempts to converts an absolute path to a relative path to the project. 
        /// </summary>
        /// <param name="absolutePath">Aboslute path to be converted</param>
        /// <param name="relativePath">Path converted to be relative to the project</param>
        /// <returns>Is the absolute path within the project</returns>
        public static bool TryGetProjectRelativePath(string absolutePath, out string relativePath)
        {
            if (IsPathInAssetsPath(absolutePath))
            {
                string sanitizedPath = absolutePath.Replace('\\', '/'); // Ensure forward slashes, FileUtil fails otherwise
                relativePath = FileUtil.GetProjectRelativePath(absolutePath);
                return true;
            }

            relativePath = string.Empty;
            return false;
        }

        /// <summary>
        /// Returns if an absolute path is located within the project assets folder.
        /// </summary>
        /// <param name="absolutePath">Absolute path being checked</param>
        /// <returns>True if the absolute path located within the assets folder, otherwise false</returns>
        public static bool IsPathInAssetsPath(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath)) return false;
            return absolutePath.StartsWith(Application.dataPath);
        }

        /// <summary>
        /// Creates a folder at the provided absolute path if the folder does not exist.
        /// </summary>
        /// <param name="absolutePath">Path to create folder at</param>
        public static void CreateFolderIfMissing(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath))
            {
                Debug.LogError("Unable to create folder with missing path.");
                return;
            }

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Creates any missing folders from the provided <c>relativePath</c> in the project.
        /// </summary>
        /// <param name="relativePath">Folders path to be created</param>
        /// <exception cref="ArgumentException"><c>relativePath</c> does not start with 'Assets' as the root folder</exception>
        public static void CreateFoldersInProject(string relativePath)
        {
            if (!relativePath.StartsWith(k_EditorProjectAssetsRoot))
            {
                throw new ArgumentException(message: $"Relative path must be rooted at '{k_EditorProjectAssetsRoot}' location in order to be able to create a folder in project.");
            }

            string[] folders = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string parentFolder = k_EditorProjectAssetsRoot; // We assume it is at the root already as required and skip the first folder
            for (int i = 1; i < folders.Length; i++)
            {
                string folder = folders[i];
                if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrEmpty(folder))
                {
                    continue;
                }

                string currentFolder = Path.Combine(parentFolder, folder);
                if (!AssetDatabase.IsValidFolder(currentFolder))
                {
                    AssetDatabase.CreateFolder(parentFolder, folder);
                }
            }
        }
    }
}
