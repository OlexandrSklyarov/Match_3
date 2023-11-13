using System;
using AS.Runtime.Models;
using UnityEngine;

namespace AS.Runtime.ViewModels
{
    public abstract class ViewModel
    {
        public event Action<int[,]> ChangeGridViewEvent;
        public event Action ResetSelectedEvent;
        public event Action<bool, Vector2Int, Vector2Int> TryChangeEvent;

        protected Model _model;

        public ViewModel(Model model)
        {
            _model = model;
            _model.ChangeGridEvent += OnChangeBoardModel;
            _model.SwapItemsEvent += OnSwapItemsResult;
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