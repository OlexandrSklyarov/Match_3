using AS.Runtime.Data;
using AS.Runtime.Models;
using AS.Runtime.ViewModels;
using AS.Runtime.Views;
using UnityEngine;

namespace AS.Runtime
{
    public class StarterGame : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;
        [SerializeField] private View _view;

        private BoardModel _model;
        private BoardViewModel _viewModel;

        private void Start() 
        {
            var gridTool = new ItemGridTool(_config.Board.Size, _config.Board.AnimationData);
            _model = new BoardModel(gridTool);
            _viewModel = new BoardViewModel(_model);

            _view.Init(_viewModel, _config.Board.ViewData, _config.Board.AnimationData);  

            _model.SendUpdateGridData();
        }
     }
}
