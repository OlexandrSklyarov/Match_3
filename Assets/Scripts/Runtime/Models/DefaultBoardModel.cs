using UnityEngine;

namespace AS.Runtime.Models
{
    public class DefaultBoardModel : Model
    {
        public DefaultBoardModel(ItemGenerator generator) : base(generator)
        {
        }

        protected override bool IsCanSwap(Vector2Int first, Vector2Int second)
        {
            return IsNeighbor(first, second);
        }

        private static bool IsNeighbor(Vector2Int first, Vector2Int second)
        {
            return Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y) == 1;
        }
    }
}