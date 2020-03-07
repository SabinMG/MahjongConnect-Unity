using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    public class Cell
    {
        private CellObject _cellObject; // objject assigned to the cell
        private Vector3 _cellCenterPosition; // cell center psoition
        private Vector2 _cellPositionIndex; // position index inside grid
        private float _height;
        private float _width;

        public Vector3 CellCenterPosition { get => _cellCenterPosition; set => _cellCenterPosition = value; }
        public CellObject CellObj { get => _cellObject; set => _cellObject = value; }
        public float Height { get => _height; set => _height = value; }
        public float Width { get => _width; set => _width = value; }
        public Vector2 CellPositionIndex { get => _cellPositionIndex; set => _cellPositionIndex = value; }
    }

    public class Grid2D
    {
        private int _rows; 
        private int _columns; 
        private float _cellWidth; 
        private float _cellHeight;
        private Cell[,] _cells;
   
        public Cell[,] Cells
        {
            get { return _cells; }
        }

        public int Rows { get => _rows; set => _rows = value; }
        public int Columns { get => _columns; set => _columns = value; }

        public Grid2D(int rows, int columns, float cellWidth, float cellHeight, Vector3 gridCenterPosition)
        {
            Rows = rows;
            Columns = columns;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            _cells = new Cell[Rows, Columns];

            Vector3 centrePosition = gridCenterPosition;
            centrePosition.y += (float)rows / 2.0f;
            centrePosition.x -= (float)columns / 2.0f- (float)_cellWidth/2.0f;


            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cell cell = new Cell();
                    cell.Height = cellHeight;
                    cell.Width = cellWidth;

                    Vector2 cellPos = new Vector2((float)j * cell.Width, (float)i * -cell.Height);
                    cell.CellCenterPosition = centrePosition + new Vector3(cellPos.x, cellPos.y, 0);
                    _cells[i, j] = cell;
                }
            }
        }
    }
}
