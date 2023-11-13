using System;
using UnityEngine;

namespace AS.Runtime.Models
{
    public class ItemGenerator
    {
        private Vector2Int _size;

        public ItemGenerator(Vector2Int size)
        {
            _size = size;
        }

        public int[,] GenerateRandomGrid()
        {
            var grid = new int[_size.x, _size.y];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y] = GenerateRandomItem();
                }
            }

            return grid;
        }

        public int GenerateRandomItem()
        {            
            var max = Enum.GetNames(typeof(ItemType)).Length; 
            return UnityEngine.Random.Range(2, max);
        }
    }
}