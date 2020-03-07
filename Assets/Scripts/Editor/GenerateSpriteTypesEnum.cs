#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace SpaceOrigin.MahjongConnect
{
    // this class is not generic at the moment, I will rewrite and create editor class for this
    public class GenerateSpriteTypesEnum
    {
        [MenuItem("Tools/GenerateSpriteTypesEnum")]
        public static void Go()
        {
            string enumName = "SpriteTypes";
            string spritesPathName = "Assets/Sprites/SpriteTiles/Faces";

            DirectoryInfo dir = new DirectoryInfo(spritesPathName);
            FileInfo[] info = dir.GetFiles("*.*");

            List<string> fileNames = new List<string>();
            foreach (FileInfo f in info)
            {
                if (f.Extension == ".png")
                {
                    fileNames.Add(Path.GetFileNameWithoutExtension(f.ToString()));
                }
                // Debug.Log(Path.GetFileNameWithoutExtension(f.ToString()));
            }

            string filePathAndName = "Assets/Scripts/Sprite/" + enumName + ".cs";

            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                for (int i = 0; i < fileNames.Count; i++)
                {
                    streamWriter.WriteLine("\t" + fileNames[i] + ",");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();
        }
    }
}
#endif
