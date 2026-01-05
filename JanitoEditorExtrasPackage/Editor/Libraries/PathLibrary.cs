using UnityEditor;
using UnityEngine;

namespace Janito.EditorExtras.Editor
{
    public static class PathLibrary
    {
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
            string path = EditorUtility.SaveFolderPanel(title, folder, defaultName);

            if (TryGetProjectRelativePath(path, out destinationPath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to converts an absolute path to a relative path to the project. 
        /// </summary>
        /// <param name="absolutePath">Aboslute path to be converted</param>
        /// <param name="relativePath">Path converted to be relative to the project</param>
        /// <returns>Is the absolute path within the project</returns>
        public static bool TryGetProjectRelativePath(string absolutePath, out string relativePath)
        {
            if (!string.IsNullOrEmpty(absolutePath))
            {
                string sanitizedPath = absolutePath.Replace('\\', '/'); // Ensure forward slashes, FileUtil fails otherwise
                if (absolutePath.StartsWith(Application.dataPath))
                {
                    relativePath = FileUtil.GetProjectRelativePath(absolutePath);
                    return true;
                }
            }

            relativePath = string.Empty;
            return false;
        }
    }
}
