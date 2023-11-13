using UnityEngine;

namespace AS.Runtime.Models
{
    public class DefaultBoardModel : Model
    {
        public DefaultBoardModel(ItemGenerator generator) : base(generator)
        {
        }

        protected override bool IsCanSwap(ref Vector2Int first, ref Vector2Int second)
        {
            return IsNeighborIndex(first.x, second.x) && InLine(first.y, second.y) ||
                IsNeighborIndex(first.y, second.y) && InLine(first.x, second.x);
        }
        
        private bool IsNeighborIndex(int x1, int x2)
        {
            return x1 >= 0 && x2 >= 0 &&
                Mathf.Abs(x1 - x2) == 1;
        }

        private bool InLine(int x1, int x2)
        {
            return x1 >= 0 && x2 >= 0 && x1 == x2;
        }
    }
}