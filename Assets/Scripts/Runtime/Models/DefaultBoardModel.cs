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
            return IsNeighbor(ref first, ref second) && IsCanMerge();
        }

        private static bool IsNeighbor(ref Vector2Int first, ref Vector2Int second)
        {
            return Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y) == 1;
        }

        private bool IsCanMerge()
        {
            return true;
        }
    }
}