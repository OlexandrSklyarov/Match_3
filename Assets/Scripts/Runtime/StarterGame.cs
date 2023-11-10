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

        private void Start() 
        {
            var model = new BoardModel(new Cell[_config.Board.Size.x, _config.Board.Size.y]);
            var viewModel = new BoardViewModel(model, _config.Board.Size);

            _view.Init(viewModel, _config.Board.CellPrefab, _config.Board.Views);  

            viewModel.GenerateBoard();
        }
    }
}
