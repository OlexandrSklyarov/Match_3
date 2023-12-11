using System;
using AS.Runtime.Models;
using UnityEngine;

namespace AS.Runtime.ViewModels
{
    public class BoardViewModel
    {
        public event Action<int[,]> ChangeGridViewEvent;
        public event Action<bool, Vector2Int, Vector2Int> TryChangeEvent;
        public event Action<Vector2Int, Vector2Int> MoveItemEvent;
        public event Action<int> UpdateTotalPoints;
        public event Action<string> BoardChangeStateEvent;

        protected BoardModel _model;

        public BoardViewModel(BoardModel model)
        {
            _model = model;
            _model.ChangeGridEvent += OnChangeBoardModel;
            _model.SwapItemsEvent += OnSwapItemsResult;
            _model.MoveItemEvent += OnMoveItem;
            _model.UpdateTotalPointsEvent += OnUpdateTotalPoints;
            _model.ChangeBoardStateEvent += OnBoardChangeState;
        }

        private void OnBoardChangeState(string state)
        {
            BoardChangeStateEvent?.Invoke(state);
        } 
        
        private void OnUpdateTotalPoints(int totalPoints)
        {
            UpdateTotalPoints?.Invoke(totalPoints);
        }

        private void OnMoveItem(Vector2Int oldPos, Vector2Int newPos)
        {
            MoveItemEvent?.Invoke(oldPos, newPos);
        }

        private void OnSwapItemsResult(bool success, Vector2Int first, Vector2Int second)
        {
            TryChangeEvent?.Invoke(success, first, second);
        }

        private void OnChangeBoardModel(int[,] cells)
        {
            ChangeGridViewEvent?.Invoke(cells);
        }       
       
        public void SelectItemPair(Vector2Int first, Vector2Int second)
        {
            _model.TryChangeItems(first, second);            
        }        
    }
}