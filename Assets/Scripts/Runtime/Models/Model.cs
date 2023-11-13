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
            if (IsCanSwap(ref first, ref second))
            {
                Swap(first, second);
                SwapItemsEvent(true, first, second);
            }
            else
            {
                SwapItemsEvent(false, first, second);
            }
        }       
        
        private void Swap(Vector2Int first, Vector2Int second)
        {
            var temp = GetItem(first);
            SetItem(first, GetItem(second));
            SetItem(second, temp);
        }

        public int GetItem(Vector2Int index) => _grid[index.x, index.y];

        private int SetItem(Vector2Int index, int value) => _grid[index.x, index.y] = value;

        protected abstract bool IsCanSwap(ref Vector2Int first, ref Vector2Int second);
    }
}