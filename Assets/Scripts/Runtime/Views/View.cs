using AS.Runtime.Data;
using AS.Runtime.Models;
using AS.Runtime.ViewModels;
using UnityEngine;

namespace AS.Runtime.Views
{
    public abstract class View : MonoBehaviour
    {
        protected RectTransform _rect;
        protected BoardViewModel _viewModel;
        protected CellView _cellViewPrefab;
        protected CellViewData[] _data;
        protected CellView[,] _cellViews;

        private void Awake() 
        {
            _rect = GetComponent<RectTransform>();    
        }

        public void Init(BoardViewModel viewModel, CellView cellPrefab, CellViewData[] viewData)
        {
            _viewModel = viewModel;
            _cellViewPrefab = cellPrefab;
            _data = viewData;
            
            viewModel.ChangeGridViewEvent += OnUpdateBoard;
        }

        protected abstract void OnUpdateBoard(Cell[,] cells);
    }
}