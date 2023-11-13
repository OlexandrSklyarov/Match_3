using AS.Runtime.Data;
using AS.Runtime.ViewModels;
using UnityEngine;

namespace AS.Runtime.Views
{
    public abstract class View : MonoBehaviour
    {
        public Vector2Int? FirstItem;
        public Vector2Int? SecondItem;

        protected RectTransform _rect;
        protected BoardViewModel _viewModel;
        protected CellViewData _data;

        private void Awake() 
        {
            _rect = GetComponent<RectTransform>();             
        }       


        public void Init(BoardViewModel viewModel, CellViewData viewData)
        {
            _viewModel = viewModel;
            _data = viewData;
            
            viewModel.ChangeGridViewEvent += OnUpdateBoard;
            viewModel.ResetSelectedEvent += OnResetSelectedItems;
            viewModel.TryChangeEvent += OnChangeItems;
        }

        protected abstract void OnChangeItems(bool isChangeSuccess, Vector2Int first, Vector2Int second);

        protected abstract void OnResetSelectedItems();

        protected abstract void OnUpdateBoard(int[,] cells);  
    }
}