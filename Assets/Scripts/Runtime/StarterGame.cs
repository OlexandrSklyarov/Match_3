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

        private Model _model;
        private ViewModel _viewModel;

        private void Start() 
        {
            var generator = new ItemGenerator(_config.Board.Size);
            _model = new DefaultBoardModel(generator);
            _viewModel = new DefaultBoardViewModel(_model);

            _view.Init(_viewModel, _config.Board.ViewData);  

            _model.ForceChangeData();
        }
     }
}
