using System;
using AS.Runtime.Models;
using UnityEngine;

namespace AS.Runtime.ViewModels
{
    public abstract class ViewModel
    {
        public event Action<Cell[,]> ChangeGridViewEvent;

        protected Model _model;
        private Vector2Int _size;

        public ViewModel(Model model, Vector2Int size)
        {
            _model = model;
            _model.ChangeGridEvent += OnChangeBoardModel;

            _size = size;

        }

        private void OnChangeBoardModel(Cell[,] cells)
        {
            ChangeGridViewEvent?.Invoke(cells);
        }

        public void GenerateBoard()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    _model.SetCellType(x, y, GetRandomCellData());
                }
            }
        }

        private ItemType GetRandomCellData()
        {            
            var max = Enum.GetNames(typeof(ItemType)).Length; 
            return (ItemType)UnityEngine.Random.Range(1, max);
        }
    }
}