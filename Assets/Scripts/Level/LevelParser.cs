using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    public class LevelParserData
    {
        public List<List<bool>> levelInfoList;
        public int filledCellCount;
        public int rows;
        public int columns;
    }

    public class LevelParser : MonoBehaviour
    {
        private static string _basePathName = "LevelLayouts/";

        public static LevelParserData LoadLevelData(string fileName)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(_basePathName + fileName);
            string fileContent = textAsset.ToString();
            char[] charArray = fileContent.ToCharArray();

            List<List<bool>> levelInfo = new List<List<bool>>();
            List<bool> rowData = new List<bool>();
            int filledCellCount = 0;

            for (int i = 0; i < charArray.Length; i++)
            {
                if (charArray[i] == 'X')
                {
                    rowData.Add(true);
                    filledCellCount++;
                }
                else if (charArray[i] == '0')
                {
                    rowData.Add(false);
                }
                else if (charArray[i] == '\n')
                {
                    levelInfo.Add(rowData);
                    rowData = new List<bool>();
                }
            }
            levelInfo.Add(rowData);

            if (levelInfo.Count == 0)
            {
                Debug.LogError("Parsing error : check the level file is in correct format");
                Debug.Break();
            }

            // Debug.Log("rows: " + levelInfo.Count + " columns :" + levelInfo[0].Count);

            LevelParserData levelParserData = new LevelParserData();
            levelParserData.rows = levelInfo.Count;
            levelParserData.columns = levelInfo[0].Count;
            levelParserData.levelInfoList = levelInfo;
            levelParserData.filledCellCount = filledCellCount;

            return levelParserData;
        }
    }
}


// example of input data
/*
X0000000X00
0X000000X00
00X00000X00
000X0000X00
0000X000X00
00000X00X00
000000X0X00
0000000XXXX
XXXXXXXXXXX
0000000XXXX
0000000XXXX
*/


