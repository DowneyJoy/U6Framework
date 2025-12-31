using System.IO;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace Downey.Setup
{
    public static class Setup
    {
        public static void CreateDefaultFolders()
        {
            Folders.CreateDefault("_Project","Animations","Art","Materials","ScriptableObjects","Scripts","Settings");
            Refresh();
        }
        static class Folders
        {
            public static void CreateDefault(string root,params string[] folders)
            {
                var fullpath = Path.Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Path.Combine(fullpath, folder);
                    if(!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
            }
        }
    }

}