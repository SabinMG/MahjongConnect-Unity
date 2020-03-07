using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    [System.Serializable]
    public class LevelData // data links to level layout file
    {
        public string levelName;
        public string levelFileName;   
    }

    public class LevelManager : MonoBehaviour
    {
        public LevelData[] levels;
        public bool visualizeGrid;
        public Grid2DVisualizer grid2DVisualizer;

        private Grid2D _currentLevelGrid;

        void Start() { }
        void Update()
        {
            if (visualizeGrid) grid2DVisualizer.VisualizeGrid(_currentLevelGrid);
        }
        
        public LevelSession GetLevelSession(int levelIndex, Vector3 levelCenterPostion)
        {
            LevelParserData levelParserData = LevelParser.LoadLevelData(levels[levelIndex].levelFileName);
            int rows = levelParserData.rows;
            int columns = levelParserData.columns;
            int totlaNumberOFX = levelParserData.filledCellCount; // number of filled blocks

            if (totlaNumberOFX % 2 != 0)
            {
                Debug.LogError("levele contains even tiles"); // we expect all even filled blocks
                Debug.Break();
            }

            List<Vector2> filledCellIndexList = new List<Vector2>();
            for (int i = 0; i < rows; i++)
            {
                List<bool> currentRow = levelParserData.levelInfoList[i];
                for (int j = 0; j < currentRow.Count; j++)
                {
                    if (currentRow[j] == true)
                    {
                        filledCellIndexList.Add(new Vector2(i,j));
                    }
                }
            }

            int numberOfTileSNeeded= totlaNumberOFX / 2;
            int noofAvaialbleSprites = Enum.GetNames(typeof(SpriteTypes)).Length;
            int numberOfCloseByExpected = UnityEngine.Random.Range(2, 4); // expected close by pairs

            Dictionary<SpriteTypes, int> cellSpriteTypesDict = new Dictionary<SpriteTypes, int>(); // dictionary will track the count of used ons
            List<SpriteTypes> cellSpriteTypesList = new List<SpriteTypes>(); // list as key for dictionary
            cellSpriteTypesDict = GetCellSpriteTyes(numberOfTileSNeeded, noofAvaialbleSprites, ref cellSpriteTypesList); // type of sprite that we are gonna show it on the screnn

            // we create grid with one extra 2 row and 2 column. this helps to find the outer path beyond the grid dimentions
            // session cell starts at (1,1) and end at ( row -1, column-1)

            Grid2D grid2D = new Grid2D(rows+2, columns+2, 1.0f, 1.0f, levelCenterPostion); // ading extra column and row
            Dictionary<SpriteTypes, List<Cell>> connectionMap = new Dictionary<SpriteTypes, List<Cell>>();

            //TODO: close by pairs

            for (int i = 0; i < filledCellIndexList.Count; i++)
            {
                Vector2 currentIndex = filledCellIndexList[i];
                Vector2 cellIndex = new Vector2(currentIndex.x+1, currentIndex.y+1);
                Cell cell = grid2D.Cells[(int)cellIndex.x, (int)cellIndex.y];
                cell.CellPositionIndex = cellIndex;

                int randomSpriteNo = UnityEngine.Random.Range(0, cellSpriteTypesList.Count);
                SpriteTypes randomSprite = cellSpriteTypesList[randomSpriteNo];

                if (cellSpriteTypesDict.ContainsKey(randomSprite)) // we expecting the diction contins the key
                {
                    cellSpriteTypesDict[randomSprite] -= 1;
                    if (cellSpriteTypesDict[randomSprite] == 0)
                    {
                        cellSpriteTypesList.Remove(randomSprite);
                        cellSpriteTypesDict.Remove(randomSprite);
                    }
                }

                SpriteObject spriteObject = new SpriteObject(randomSprite);
                cell.CellObj = spriteObject;
                spriteObject.ParentCell = cell;
                if (connectionMap.ContainsKey(randomSprite))
                {
                    connectionMap[randomSprite].Add(cell);
                }
                else
                {
                    connectionMap.Add(randomSprite, new List<Cell>() { cell });
                }

               float randomeValue = UnityEngine.Random.value;
            }

            _currentLevelGrid = grid2D;
            LevelSession levelSession = new LevelSession(grid2D, connectionMap);
            levelSession.RemainigSpritObjCnt  = totlaNumberOFX; // init max sprite count
            levelSession.SpriteObjCount = totlaNumberOFX;
            return levelSession;
        }

        private Dictionary<SpriteTypes, int> GetCellSpriteTyes(int maxCount, int noofAvaialbleSprites, ref List<SpriteTypes> cellSpriteTypesList) // returns sprite types for cells
        {
            HashSet<int> availableSpritesHash = new HashSet<int>();
            for (int i = 0; i < noofAvaialbleSprites; i++)
            {
                availableSpritesHash.Add(i);
            }

            Dictionary<SpriteTypes, int> spriteTypes = new Dictionary<SpriteTypes, int>(); //TODO : introduce duplciates 
            for (int i = 0; i < maxCount; i++) 
            {
                int randomRange = 0;
                if (availableSpritesHash.Count > 0) // maxCount < number of availabe 
                {
                    randomRange = UnityEngine.Random.Range(0, availableSpritesHash.Count);
                }
                else
                {
                    randomRange = UnityEngine.Random.Range(0, noofAvaialbleSprites); // creats duplicate values

                    SpriteTypes spriteType = (SpriteTypes)randomRange;
                    if (spriteTypes.ContainsKey(spriteType))
                    {
                        spriteTypes[spriteType] = spriteTypes[spriteType] + 2;
                    }
                    else
                    {
                        cellSpriteTypesList.Add((SpriteTypes)randomRange);
                        spriteTypes.Add((SpriteTypes)randomRange, 2);
                    }

                    continue; // continue to next iteration
                }

                int j = 0;
                int randomSprite = 0;
                foreach (int hashValue in availableSpritesHash) // might need to re write
                {
                    if (j == randomRange)
                    {
                        randomSprite = hashValue;
                        break;
                    }
                    j++;
                }

                cellSpriteTypesList.Add((SpriteTypes)randomSprite);
                spriteTypes.Add((SpriteTypes)randomSprite, 2); // two here represents number of pairs
                availableSpritesHash.Remove(randomSprite);
            }

            return spriteTypes;
        }
    }
}
