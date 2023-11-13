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
                    grid[x, y] = -1;                     
                }
            }

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    do
                    {
                       grid[x, y] = GenerateRandomItem(); 
                    }
                    while(IsStreak(x,y, grid));                    
                }
            }

            return grid;
        }

        public bool IsStreak(int row, int col, int[,] grid)
        {
            return IsVerticalStreak(row, col, grid) || IsHorizontalStreak(row, col, grid);
        }

        private bool IsVerticalStreak(int row, int col, int[,] grid)
        {
            var value = grid[row, col];
            var streak = 0;
            var temp = row;

            while(temp > 0 && grid[temp - 1, col] == value)
            {
                streak++;
                temp--;
            }

            temp = row;

            while(temp < grid.GetLength(0) - 1 && grid[temp + 1, col] == value)
            {
                streak++;
                temp++;
            }

            return streak > 1;
        }

        private bool IsHorizontalStreak(int row, int col, int[,] grid)
        {
            var value = grid[row, col];
            var streak = 0;
            var temp = col;

            while(temp > 0 && grid[row, temp - 1] == value)
            {
                streak++;
                temp--;
            }

            temp = row;

            while(temp < grid.GetLength(1) - 1 && grid[row, temp + 1] == value)
            {
                streak++;
                temp++;
            }

            return streak > 1;
        }

        public int GenerateRandomItem()
        {            
            var max = Enum.GetNames(typeof(ItemType)).Length; 
            return UnityEngine.Random.Range(2, max);
        }        
    }
}