using System.Linq;
using AS.Runtime.Models;
using UnityEngine;

namespace AS.Runtime.Views
{
    public class UIBoardView : View
    {        
        private CellViewContainer[,] _cellViews;

        protected override void OnUpdateBoard(int[,] cells)
        {
            if (_cellViews == null)
            {
                GenerateViews(cells);
            }

            UpdateImage(cells);
        }

        private void UpdateImage(int[,] cells)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    SetImage((ItemType)cells[x, y], _cellViews[x, y]);
                }
            }
        }

        private void GenerateViews(int[,] cells)
        {
            _cellViews = new CellViewContainer[cells.GetLength(0), cells.GetLength(1)];

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var cell = Instantiate(_data.CellPrefab, _rect);

                    var pos = GetCellPosition(x, y, cell.GetSize());
                    cell.SetPosition(pos);

                    var type = (ItemType)cells[x, y];

                    SetImage(type, cell);

                    TrySubscribeOnItem(x, y, cell, type);

                    _cellViews[x, y] = cell;
                }
            }
        }

        private void TrySubscribeOnItem(int x, int y, CellViewContainer cell, ItemType type)
        { 

            if (type == ItemType.None || type == ItemType.Empty) return;
            
            cell.PointerDownEvent += () => FirstSelectItem(x, y);
            cell.SelectEvent += () => TrySelectSecondItem(x, y);       
        }

        private void FirstSelectItem(int x, int y) => FirstItem = new Vector2Int(x, y);       

        private void TrySelectSecondItem(int x, int y)
        {
            if (!FirstItem.HasValue) return;

            SecondItem = new Vector2Int(x, y); 

            TrySelectPairItems();
        }

        private void SetImage(ItemType type, CellViewContainer cell)
        {
            if (TryGetImage(type, out var image))
            {
                cell.SetImage(image);
            }
            else
            {
                cell.SetImage(_data.DefaultImage);
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
            var item = _data.Items.FirstOrDefault(x => x.Type == type);

            if (item != null)
            {
                image = item.Image;
                return true;
            }

            Debug.LogWarning("image not found...");

            return false;
        }

        protected override void OnResetSelectedItems()
        {            
            if (!FirstItem.HasValue) return;
            if (!SecondItem.HasValue) return;            

            FirstItem = null;
            SecondItem = null;
        }

        private CellViewContainer GetItem(Vector2Int index) => _cellViews[index.x, index.y];

        protected override void OnChangeItems(bool isChangeSuccess, Vector2Int first, Vector2Int second)
        {
            if (isChangeSuccess)
            {
                Swap(GetItem(first), GetItem(second));
            }
            else
            {
                FakeSwap(GetItem(first), GetItem(second));
            }
        }

        private void Swap(CellViewContainer first, CellViewContainer second)
        {
            var f = first.MyView; 
            var s = second.MyView;    

            second.SetView(f);
            first.SetView(s);  
        }

        private void FakeSwap(CellViewContainer first, CellViewContainer second)
        {
            first.ViewMoveAndReturn(second.MyView.MyPosition);
            second.ViewMoveAndReturn(first.MyView.MyPosition);
        }

        private void TrySelectPairItems()
        {
            if (!FirstItem.HasValue) return;
            if (!SecondItem.HasValue) return;

            var first = FirstItem.Value;
            var second = SecondItem.Value;

            OnResetSelectedItems();

            _viewModel.SelectItemPair(first, second);
        }
    }
}