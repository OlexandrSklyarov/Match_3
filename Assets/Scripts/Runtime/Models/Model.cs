using System;

namespace AS.Runtime.Models
{
    public abstract class Model
    {
        protected Cell[,] _cells;

        public event Action<Cell[,]> ChangeGridEvent;

        public Model(Cell[,] cells)
        {
            _cells = cells;
        }

        public void SetCellType(int x, int y, ItemType type)
        {
            _cells[x,y].Type = type;
            ChangeGridEvent?.Invoke(_cells);
        }
    }
}