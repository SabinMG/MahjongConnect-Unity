using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    public class LevelSession 
    {
        private int _sessionScore;
        private int _sessionHighScore; // prevous High Score
        private int _spriteObjCount; // total active tiles in the begiing
        private int _remainigSpritObjCnt; // remaing tiles at present, if this reaches 0 game over

        private Grid2D _grid2D;
        private Dictionary<SpriteTypes, List<Cell>> _connetionMap; // stores the same types for sprites for easy acces
        // Stack<SpriteTypes> _hintStack;  // for later use                                                        

        private Vector2Int _upDirection = new Vector2Int(-1, 0); // row is upwards i index
        private Vector2Int _downDirection = new Vector2Int(1, 0);
        private Vector2Int _rightDirection = new Vector2Int(0, 1);
        private Vector2Int _LeftDirection = new Vector2Int(0, -1); // colum left and right j index

        public Grid2D Grid { get => _grid2D; set => _grid2D = value; }
        public Dictionary<SpriteTypes, List<Cell>> ConnetionMap { get => _connetionMap; set => _connetionMap = value; }
        public int SpriteObjCount { get => _spriteObjCount; set => _spriteObjCount = value; }
        public int RemainigSpritObjCnt { get => _remainigSpritObjCnt; set => _remainigSpritObjCnt = value; }
        public Vector2Int UpDirection { get => _upDirection; set => _upDirection = value; }
        public Vector2Int DownDirection { get => _downDirection; set => _downDirection = value; }
        public Vector2Int RightDirection { get => _rightDirection; set => _rightDirection = value; }
        public Vector2Int LeftDirection { get => _LeftDirection; set => _LeftDirection = value; }
        public int SessionScore { get => _sessionScore; set => _sessionScore = value; }
        public int SessionHighScore { get => _sessionHighScore; set => _sessionHighScore = value; }

        public LevelSession(Grid2D grid2D, Dictionary<SpriteTypes, List<Cell>> connectionMap)
        {
            _grid2D = grid2D;
            _connetionMap = connectionMap;
        }

        public void RemoveCellFromConnectionMap(SpriteObject spriteObj)
        {
            Cell lastObjCell = spriteObj.ParentCell;
            SpriteTypes lastObjSpriteType = spriteObj.SpriteType;
            RemoveCellFromConnectionMap(lastObjSpriteType, lastObjCell);
        }

        public void RemoveCellFromConnectionMap(SpriteTypes type, Cell cell)
        {
            if (_connetionMap.ContainsKey(type))
            {
                for (int i = 0; i < _connetionMap[type].Count; i++)
                {
                    if (_connetionMap[type][i] == cell)
                    {
                        _connetionMap[type].Remove(cell);
                        break;
                    }
                }
            }
            else { Debug.Log("Map connection doent exist"); }
        }

        public bool CheckIfNearBy(SpriteObject startObject, SpriteObject endObject)
        {
            Vector2 inputObectIndex = new Vector2(startObject.ParentCell.CellPositionIndex.x, startObject.ParentCell.CellPositionIndex.y);
            Vector2 targetObjectIndex = new Vector2(endObject.ParentCell.CellPositionIndex.x, endObject.ParentCell.CellPositionIndex.y);
            float distane = Vector2.Distance(inputObectIndex, targetObjectIndex);
            if (distane < 1.5f) return true; // they are near by
            return false;
        }

        // recursion // there coulbe be other better aprach. bcs shortage of time I am going wiht this aproeach
        // may be also can appy memoization
        public bool CalculateValidTurns(Vector2 currentPositionIndex, Vector2 targetPostionIndex, Vector2Int intDirection, int turnCount)
        {
            if (turnCount > 2) return false;
            Vector2 nextIndex = new Vector2(currentPositionIndex.x + intDirection.x, currentPositionIndex.y + intDirection.y);
            if (nextIndex == targetPostionIndex) // we reached destination 
            {
                if (turnCount <= 2) return true;
                else return false;
            }

            if (nextIndex.x >= 0 && nextIndex.y >= 0 && nextIndex.x < _grid2D.Rows && nextIndex.y < _grid2D.Columns)
            {
                if (_grid2D.Cells[(int)nextIndex.x, (int)nextIndex.y].CellObj == null)
                {
                    if (turnCount > 2) return false;

                    if (intDirection.Equals(UpDirection) || intDirection.Equals(DownDirection))
                    {
                        bool value1 = CalculateValidTurns(nextIndex, targetPostionIndex, intDirection, turnCount);
                        bool value2 = CalculateValidTurns(nextIndex, targetPostionIndex, LeftDirection, turnCount + 1);
                        bool value3 = CalculateValidTurns(nextIndex, targetPostionIndex, RightDirection, turnCount + 1);
                        return value1 || value2 || value3;
                    }
                    else if (intDirection.Equals(LeftDirection) || intDirection.Equals(RightDirection))
                    {
                        bool value1 = CalculateValidTurns(nextIndex, targetPostionIndex, intDirection, turnCount);
                        bool value2 = CalculateValidTurns(nextIndex, targetPostionIndex, UpDirection, turnCount + 1);
                        bool value3 = CalculateValidTurns(nextIndex, targetPostionIndex, DownDirection, turnCount + 1);
                        return value1 || value2 || value3;
                    }
                }
                else //path is block by a closed cell return false
                {
                    return false;
                }
            }
            else // next index is outside of the grid dimention, retun false
            {
                return false;
            }
            return false;
        }

        // This part need to be optimized 
        public List<SpriteObject> GetHintSpriteObjects()
        {
            List<SpriteObject> hintObjects = new List<SpriteObject>();
            List<List<Cell>> distancePairCellsList = new List<List<Cell>>();

            // all close by objects will be checked first then other far objects, becaus the other is more expensive
            // also can cache this info while building the session TODO: optimize
            foreach (KeyValuePair<SpriteTypes, List<Cell>> item in _connetionMap)
            {
                List<Cell> listCells = item.Value;
                for (int i = 0; i < listCells.Count; i++)
                {
                    for (int j = 0; j < listCells.Count; j++)
                    {
                        if (i != j)
                        {
                            SpriteObject startObject = (SpriteObject)listCells[i].CellObj;
                            SpriteObject endObject = (SpriteObject)listCells[j].CellObj;
                            bool validPair = CheckIfNearBy(startObject, endObject);

                            if (validPair)
                            {
                                hintObjects.Add(startObject);
                                hintObjects.Add(endObject);
                                return hintObjects;
                            }
                        }
                    }
                }
                distancePairCellsList.Add(listCells);
            }

            // entering into expensive stack 
            foreach (List<Cell> item in distancePairCellsList)
            {
                List<Cell> listCells = item;
                for (int i = 0; i < listCells.Count; i++)
                {
                    for (int j = 0; j < listCells.Count; j++)
                    {
                        if (i != j)
                        {
                            SpriteObject startObject = (SpriteObject)listCells[i].CellObj;
                            SpriteObject endObject = (SpriteObject)listCells[j].CellObj;

                            bool statusUp = CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, UpDirection, 0);
                            bool statusDown = CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, DownDirection, 0);
                            bool statusRight = CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, RightDirection, 0);
                            bool statusLeft = CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, LeftDirection, 0);
                            bool validPair = (statusUp || statusDown || statusRight || statusLeft);
                            if (validPair)
                            {
                                hintObjects.Add(startObject);
                                hintObjects.Add(endObject);
                                return hintObjects;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}

