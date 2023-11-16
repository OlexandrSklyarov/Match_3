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

            Debug.Log("[UpdateImage completed!!!]");
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
            var item = _data.Items.FirstOrDefault(x => x.Type == type);

            if (item != null)
            {
                cell.SetImage(item.Image);
                return;
            }

            cell.SetImageWithShake(_data.DefaultImage, _animationData.ShakeDuration);
        }

        private Vector2 GetCellPosition(int x, int y, float cellSize)
        {
            return new Vector2
            (
                cellSize * 0.5f + cellSize * x,
                -cellSize * 0.5f - cellSize * y
            );
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
                Swap(GetItem(first), GetItem(second), _animationData.DefaultSwapDuration);
            }
            else
            {
                FakeSwap(GetItem(first), GetItem(second), _animationData.DefaultSwapDuration);
            }
        }

        private void Swap(CellViewContainer first, CellViewContainer second, float duration)
        {
            var f = first.MyView; 
            var s = second.MyView;    

            second.SetView(f, duration);
            first.SetView(s, duration);  
        }

        private void FakeSwap(CellViewContainer first, CellViewContainer second, float duration)
        {
            first.ViewMoveAndReturn(second.MyView.MyPosition, duration);
            second.ViewMoveAndReturn(first.MyView.MyPosition, duration);
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

        protected override void OnMoveItem(Vector2Int oldPos, Vector2Int newPos)
        {
            Swap(GetItem(oldPos), GetItem(newPos), _animationData.FastSwapDuration);
        }
    }
}