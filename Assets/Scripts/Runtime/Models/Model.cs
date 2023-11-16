using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AS.Runtime.Models
{
    public abstract class Model
    {
        protected ItemGridTool _gridTool;
        protected int[,] _grid;
        protected Dictionary<int, int> _itemPoints = new();
        protected bool _isBlockInput;

        public event Action<int[,]> ChangeGridEvent;
        public event Action<bool, Vector2Int, Vector2Int> SwapItemsEvent;
        public event Action<Vector2Int, Vector2Int> MoveItemEvent;

        public Model(ItemGridTool gridTool)
        {
            _gridTool = gridTool;
            _grid = _gridTool.GenerateRandomGrid();
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
            if (_isBlockInput) return;

            if (IsCanSwap(first, second))
            {
                Swap(first, second);                

                if (TryAnalyze())
                {     
                    SuccessSwap(first, second);

                    MoveGridAsync();
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

        private async void MoveGridAsync()
        {
            Debug.Log("Destroy items...");

            _isBlockInput = true;
            var time = TimeSpan.FromSeconds(0.5f);

            await UniTask.Delay(time);
           
            ForceChangeData();

            await Task.Delay(1000);

            //fall items
            await _gridTool.TryGridMoveDownAsync(_grid, OnMoveItem);
            ForceChangeData();

            await UniTask.Delay(time);

            //replace items
            _gridTool.ReplaceGrid(_grid);
            ForceChangeData();

            await UniTask.Delay(time);            

            //analyze
            if (TryAnalyze())
            {                
                MoveGridAsync();
                return;
            }

            _isBlockInput = false;
        }

        private void OnMoveItem(Vector2Int oldPos, Vector2Int newPos) => MoveItemEvent?.Invoke(oldPos, newPos);

        private bool TryAnalyze()
        {
            _itemPoints.Clear();
            return _gridTool.AnalyzeGrid(_grid, OnAddPoints, CalculateFinalPoints);
        }

        private void CalculateFinalPoints(int groupCount)
        {
            var total = 0;

            foreach(var item in _itemPoints)
            {
                total += GetCoast(item.Key) * item.Value * groupCount;
            }

            Debug.Log($"Total points: {total}");
        }

        private int GetCoast(int key)
        {
            return ((ItemType)key) switch
            {
                ItemType.Item_1 => 2,
                ItemType.Item_2 => 4,
                ItemType.Item_3 => 5,
                ItemType.Item_4 => 3,
                ItemType.Item_5 => 1,
                _=> 0
            };
        }

        private void OnAddPoints(int type, int groupLength)
        {
            Debug.Log($"type {(ItemType)type} length {groupLength}");

            if (_itemPoints.ContainsKey(type))
            {
                _itemPoints[type] += groupLength;
            } 
            else
            {
                _itemPoints.Add(type, groupLength);
            }
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