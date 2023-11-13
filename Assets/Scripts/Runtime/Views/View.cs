using AS.Runtime.Data;
using AS.Runtime.ViewModels;
using UnityEngine;

namespace AS.Runtime.Views
{
    public abstract class View : MonoBehaviour
    {
        protected Vector2Int? FirstItem;
        protected Vector2Int? SecondItem;
        
        protected RectTransform _rect;
        protected ViewModel _viewModel;
        protected CellViewData _data;

        private void Awake() 
        {
            _rect = GetComponent<RectTransform>();             
        }       

        public void Init(ViewModel viewModel, CellViewData viewData)
        {
            _viewModel = viewModel;
            _data = viewData;
            
            viewModel.ChangeGridViewEvent += OnUpdateBoard;
            viewModel.TryChangeEvent += OnChangeItems;
        }

        protected abstract void OnChangeItems(bool isChangeSuccess, Vector2Int first, Vector2Int second);

        protected abstract void OnResetSelectedItems();

        protected abstract void OnUpdateBoard(int[,] cells);  
    }
}