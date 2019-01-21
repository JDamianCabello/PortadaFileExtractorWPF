using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace CodeExtractor
{
    static class Copy
    {
        /// <summary>
        /// Copy expecific files filtering by extension
        /// </summary>
        /// <param name="sourcePath">Original full directory patch</param>
        /// <param name="destPath">Destination full directory patch</param>
        /// <param name="cs">search all *.cs extensions in the directory</param>
        /// <param name="exe">search all *.exe extensions in the directory</param>
        /// <param name="dll">search all *.dll extensions in the directory</param>
        /// <param name="compress">Determines if the folder is compressed</param>
        /// <param name="deleteAfterCompress">Determines if the compressed directory should be deleted after zip</param>
        public static void ExpecificFiles(string sourcePath, string destPath, bool cs, bool exe, bool dll, bool compress, bool deleteAfterCompress, bool showCreatedFileInExplorer)
        {
            if (sourcePath == "" || !Directory.Exists(sourcePath))
                return;

            if (destPath == "")
                return;

            string[] directorys = Directory.GetDirectories(sourcePath);
            string[] filesToCopy = null;
            string[] extensionsToCopy = CreateExtensionList(cs, exe, dll);

            for (int i = 0; i < directorys.Length; i++)
            {
                filesToCopy = SearchFilesToCopy(directorys[i], extensionsToCopy);
                if (!Directory.Exists(DirectoryEndPatch(directorys[i], destPath )))
                    Directory.CreateDirectory(DirectoryEndPatch(directorys[i], destPath));

                for (int j = 0; j < filesToCopy.Length; j++)
                    File.Copy(filesToCopy[j], FileEndPatch(directorys[i], destPath, Path.GetFileName(filesToCopy[j])), true);
            }
            if(compress)
                Compress(destPath, Path.GetFileName(destPath));
            if (deleteAfterCompress)
                DeleteEntireFolder(destPath);
            if (showCreatedFileInExplorer)
                OpenPatchWindowsExplorer(destPath);
        }

        /// <summary>
        /// Method that makes a zip from entire folder
        /// </summary>
        /// <param name="path">Patch to make the zip</param>
        /// <param name="fileName">Zip name</param>
        private static void Compress (string path, string fileName)
        {
            string rootPathDyrectory = new DirectoryInfo(path).Parent.FullName + Path.DirectorySeparatorChar + fileName + ".zip";
            if (File.Exists(rootPathDyrectory))
                File.Delete(rootPathDyrectory);

            ZipFile.CreateFromDirectory(path, rootPathDyrectory,CompressionLevel.Optimal,false);
        }

        /// <summary>
        /// Open the windows explorer to show a file
        /// </summary>
        /// <param name="path">the patch to the file</param>
        private static void OpenPatchWindowsExplorer(string path)
        {
            Process.Start(new DirectoryInfo(path).Parent.FullName + Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Remove recursively a directory
        /// </summary>
        /// <param name="pathToDelete">Patch to remove</param>
        private static void DeleteEntireFolder(string pathToDelete)
        {
            Directory.Delete(pathToDelete,true);
        }

        /// <summary>
        /// Aux method. Make a end directory patch from source patch and destination patch
        /// </summary>
        /// <param name="sourcePath">Original full directory patch</param>
        /// <param name="destPath">Destination full directory patch</param>
        /// <returns>The end directory pach</returns>
        private static string DirectoryEndPatch(string sourcePath, string destPath)
        {
            return destPath + Path.DirectorySeparatorChar + Path.GetFileName(sourcePath);
        }

        /// <summary>
        /// Aux method. Make a end file patch from source patch and destination patch
        /// </summary>
        /// <param name="sourcePath">Original full file patch</param>
        /// <param name="destPath">Destination full file patch</param>
        /// <param name="fileName">File name and file extesion</param>
        /// <returns>The end file patch</returns>
        private static string FileEndPatch(string sourcePath, string destPath, string fileName)
        {
            return destPath + Path.DirectorySeparatorChar + Path.GetFileName(sourcePath) + Path.DirectorySeparatorChar + fileName;
        }

        /// <summary>
        /// Search all files in folder by extension and exludes directorys or files
        /// </summary>
        /// <param name="folderPath">Full patch to father folder</param>
        /// <param name="extension">Extension to search in father folder</param>
        /// <returns></returns>
        private static string[] SearchFilesToCopy(string folderPath, string[] extension)
        {
            List<string> files = new List<string>();
            string[] excludesPatterns = { Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar + "Debug" + Path.DirectorySeparatorChar, Path.DirectorySeparatorChar +"Properties" + Path.DirectorySeparatorChar, ".vshost." };
            for (int i = 0; i < extension.Length; i++)
            {
                string[] filePaths = Directory.GetFiles(folderPath, extension[i],SearchOption.AllDirectories); //Search in the folder and subfolders
                for (int j = 0; j < filePaths.Length; j++)
                    if (!ExcludedFilesOrFolders(filePaths[j], excludesPatterns))
                        files.Add(filePaths[j]); 
            }

            return files.ToArray();
        }
        /// <summary>
        /// Check if these file or folder should be excluded from copy
        /// </summary>
        /// <param name="patch">Patch to file</param>
        /// <param name="excludedPatterns">All excludes patterns</param>
        /// <returns>A list of patch´s to copy</returns>
        private static bool ExcludedFilesOrFolders(string patch, string[] excludedPatterns)
        {
            for (int i = 0; i < excludedPatterns.Length; i++)
                if (patch.Contains(excludedPatterns[i]))
                    return true;

            return false;


        }

        /// <summary>
        /// Aux method for save extensions to search.
        /// </summary>
        /// <param name="cs">search all *.cs extensions in the directory</param>
        /// <param name="exe">search all *.exe extensions in the directory</param>
        /// <param name="dll">search all *.dll extensions in the directory</param>
        /// <returns>A list of extensions to search</returns>
        private static string[] CreateExtensionList(bool cs, bool exe, bool dll)
        {
            List<string> auxList = new List<string>();

            if (cs)
                auxList.Add("*.cs");
            if (exe)
                auxList.Add("*.exe");
            if (dll)
                auxList.Add("*.dll");

            return auxList.ToArray();
        }
    }


}
