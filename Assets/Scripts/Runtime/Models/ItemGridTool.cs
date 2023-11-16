using System;
using AS.Runtime.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AS.Runtime.Models
{
    public class ItemGridTool
    {
        private Vector2Int _size;
        private AnimationData _animationData;

        public ItemGridTool(Vector2Int size, AnimationData animationData)
        {
            _size = size;
            _animationData = animationData;
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

        public bool AnalyzeGrid(int[,] grid, Action<int, int> addPoint, Action<int> calculateFinalPoints)
        {            
            var groupCount = 0;
            var nchBlock = 0;
            var chBlock = 0;

            //hor
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                nchBlock = 0;

                for(int x = 0; x < grid.GetLength(0); x++)
                {
                    if (x == 0) chBlock = Mathf.Abs(grid[x, y]);

                    if (Mathf.Abs(grid[x, y]) == chBlock)
                    {
                        nchBlock++;
                    }
                    else
                    {
                        if(nchBlock > 2)
                        {
                            addPoint?.Invoke(chBlock, nchBlock); //save type
                            groupCount++;

                            for (int l = 0; l < nchBlock; l++)
                            {
                                grid[x - nchBlock + l, y] = -1 * Mathf.Abs(grid[x - nchBlock + l, y]);
                            }
                        }

                        chBlock = Mathf.Abs(grid[x,y]);
                        nchBlock = 1;
                    }

                    if (x == grid.GetLength(0)-1 && nchBlock > 2)
                    {
                        addPoint?.Invoke(chBlock, nchBlock); //save type
                        groupCount++;

                        for (int l = 0; l < nchBlock; l++)
                        {
                            grid[x - nchBlock + l, y] = -1 * Mathf.Abs(grid[x - nchBlock + l, y]);
                        }
                    }
                }
            }  

            //vert
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                nchBlock = 0;

                for(int y = 0; y < grid.GetLength(1); y++)
                {
                    if (y == 0) chBlock = Mathf.Abs(grid[x, y]);

                    if (Mathf.Abs(grid[x, y]) == chBlock)
                    {
                        nchBlock++;
                    }
                    else
                    {
                        if(nchBlock > 2)
                        {
                            addPoint?.Invoke(chBlock, nchBlock); //save type
                            groupCount++;

                            for (int l = 0; l < nchBlock; l++)
                            {
                                grid[x, y - nchBlock + l] = -1 * Mathf.Abs(grid[x, y - nchBlock + l]);
                            }
                        }

                        chBlock = Mathf.Abs(grid[x,y]);
                        nchBlock = 1;
                    }

                    if (y == grid.GetLength(1)-1 && nchBlock > 2)
                    {
                        addPoint?.Invoke(chBlock, nchBlock); //save type
                        groupCount++;

                        for (int l = 0; l < nchBlock; l++)
                        {
                            grid[x, y - nchBlock + l] = -1 * Mathf.Abs(grid[x, y - nchBlock + l]);
                        }
                    }
                }
            }             

            if (groupCount > 0) calculateFinalPoints?.Invoke(groupCount);

            return groupCount > 0;
        }

        public async UniTask TryGridMoveDownAsync(int[,] grid, Action<Vector2Int, Vector2Int> onMoveItem)
        {
            var nHole = 0;
            var yHole = 0;

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                nHole = 0;

                for (int y = grid.GetLength(1) -1 ; y >= 0; y--)
                {
                    if (grid[x,y] < 0)
                    {
                        nHole++;

                        if (nHole == 1) yHole = y;
                    }

                    if (grid[x,y] > 0 && nHole > 0)
                    {
                        onMoveItem?.Invoke(new Vector2Int(x, yHole), new Vector2Int(x,y));
                        grid[x, yHole] = grid[x,y];
                        yHole--;
                        grid[x,y] = -1;

                        await UniTask.Delay(TimeSpan.FromSeconds(_animationData.FastSwapDuration));
                    }
                }
            }
        }

        public void ReplaceGrid(int[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for(int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x,y] < 0) grid[x,y] = GenerateRandomItem(); 
                }
            }
        }
    }
}