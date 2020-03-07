using UnityEngine;

namespace SpaceOrigin.MahjongConnect
{
    public class Grid2DVisualizer : MonoBehaviour
    {
        public int rows; 
        public int columns; 
        public float cellWidth; 
        public float cellHeight;
       
        private Grid2D _grid2D;
        private Vector3 _lastTransPosition;

        public void VisualizeGrid(Grid2D grid2D) // this enabled you to show esternal created grid
        {
            _grid2D = grid2D;
        }

        private void OnValidate()
        {
            _grid2D = null;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (_grid2D == null | transform.position != _lastTransPosition )
                {
                    _lastTransPosition = transform.position;
                    _grid2D = new Grid2D(rows, columns, cellWidth, cellHeight, transform.position);
                }
                DrawGridCells();
            }
            else
            {
                if (_grid2D != null)
                {
                    DrawGridCells();
                }
            }
        }

        private void DrawGridCells()
        {
            if (_grid2D == null) return;

            for (int i = 0; i < _grid2D.Rows; i++)
            {
                for (int j = 0; j < _grid2D.Columns; j++)
                {
                    Cell cell = _grid2D.Cells[i,j];
                  
                    Vector3 bottomLeftPos = new Vector3(cell.CellCenterPosition.x - cell.Width / 2, cell.CellCenterPosition.y - cell.Height / 2, cell.CellCenterPosition.z);
                    Vector3 bottomRightPos = new Vector3(cell.CellCenterPosition.x + cell.Width / 2, cell.CellCenterPosition.y - cell.Height / 2, cell.CellCenterPosition.z);
                    Vector3 topLeftPos = new Vector3(cell.CellCenterPosition.x - cell.Width / 2, cell.CellCenterPosition.y + cell.Height / 2, cell.CellCenterPosition.z);
                    Vector3 topRightPos = new Vector3(cell.CellCenterPosition.x + cell.Width / 2, cell.CellCenterPosition.y + cell.Height / 2, cell.CellCenterPosition.z);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(bottomLeftPos, bottomRightPos);
                    Gizmos.DrawLine(bottomRightPos, topRightPos);
                    Gizmos.DrawLine(topRightPos, topLeftPos);
                    Gizmos.DrawLine(topLeftPos, bottomLeftPos);

                    Gizmos.DrawSphere(cell.CellCenterPosition, .02f);
                }
            }
        }
    }
}
