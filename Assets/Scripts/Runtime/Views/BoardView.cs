using System.Linq;
using AS.Runtime.Models;
using UnityEngine;

namespace AS.Runtime.Views
{
    public class BoardView : View
    {
        protected override void OnUpdateBoard(Cell[,] cells)
        {
            if (_cellViews == null)
            {
                GenerateViews(cells);
            }

            UpdateImage(cells);
        }

        private void UpdateImage(Cell[,] cells)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    SetImage(cells, x, y, _cellViews[x, y]);
                }
            }
        }

        private void GenerateViews(Cell[,] cells)
        {
            _cellViews = new CellView[cells.GetLength(0), cells.GetLength(1)];

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var cell = Instantiate(_cellViewPrefab, _rect);

                    var pos = GetCellPosition(x, y, cell.GetSize());
                    cell.SetPosition(pos);

                    SetImage(cells, x, y, cell);

                    _cellViews[x, y] = cell;
                }
            }
        }

        private void SetImage(Cell[,] cells, int x, int y, CellView cell)
        {
            if (TryGetImage(cells[x, y].Type, out var image))
            {
                cell.SetImage(image);
            }
        }

        private Vector2 GetCellPosition(int x, int y, float cellSize)
        {
            return new Vector2
            (
                cellSize * 0.5f + cellSize * x,
                -cellSize * 0.5f - cellSize * y
            );
        }

        private bool TryGetImage(ItemType type, out Sprite image) 
        {
            image = null;
            var item = _data.FirstOrDefault(x => x.Type == type);

            if (item != null)
            {
                image = item.Image;
                return true;
            }

            return false;
        }
    }
}