using System;
using UnityEngine;

namespace AS.Runtime.Models
{
    public abstract class Model
    {
        protected ItemGenerator _generator;
        protected int[,] _grid;

        public event Action<int[,]> ChangeGridEvent;
        public event Action<bool, Vector2Int, Vector2Int> SwapItemsEvent;

        public Model(ItemGenerator generator)
        {
            _generator = generator;
            _grid = generator.GenerateRandomGrid();
        }

        public void SetCellType(Vector2Int index, int value)
        {
            SetItem(index, value);
            ForceChangeData();
        }

        public int[,] GetGrid() => _grid;

        public void ForceChangeData() => ChangeGridEvent?.Invoke(_grid);

        public void TryChangeItems(Vector2Int first, Vector2Int second)
        {
            if (IsCanSwap(first, second))
            {
                Swap(first, second);

                if (IsStreak(first, second))
                {
                    SuccessSwap(first, second);
                    Debug.Log("IsStreak!!!");
                }
                else
                {
                    Swap(second, first);
                    FailureSwap(first, second);
                }
            }
            else
            {
                FailureSwap(first, second);
            }
        }

        private bool IsStreak(Vector2Int first, Vector2Int second)
        {
            return _generator.IsStreak(first.x, first.y, _grid) ||
                _generator.IsStreak(second.x, second.y, _grid);
        }

        private void SuccessSwap(Vector2Int first, Vector2Int second) => SwapItemsEvent(true, first, second);

        private void FailureSwap(Vector2Int first, Vector2Int second) => SwapItemsEvent(false, first, second);

        private void Swap(Vector2Int first, Vector2Int second)
        {
            var temp = GetItem(first);
            SetItem(first, GetItem(second));
            SetItem(second, temp);
        }

        public int GetItem(Vector2Int index) => _grid[index.x, index.y];

        private int SetItem(Vector2Int index, int value) => _grid[index.x, index.y] = value;

        protected abstract bool IsCanSwap(Vector2Int first, Vector2Int second);
    }
}