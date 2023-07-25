using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator
{
    public enum DifficultyLevel
    {
        RELOAD = 0,
        RESTART = 1,
        EASY = 2,
        MEDIUM = 3,
        HARD = 4
    }

    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;
    private const int BOARD_SIZE = 9;
    private const int MAX_SQUARE_REMOVED = 60;
    private const int MIN_SQUARE_REMOVED = 20;

    private static void InitializeGrid(int[,] grid)
    {
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Shuffle(numbers);
        for (int i = 0; i < GRID_SIZE; i++)
        {
            grid[0, i] = numbers[i];
        }
        FillGrid(1, 0, grid);
    }

    private static bool FillGrid(int r, int c, int[,] grid)
    {
        if (r == GRID_SIZE)
        {
            return true;
        }

        if (c == GRID_SIZE)
        {
            return FillGrid(r + 1, 0, grid);
        }

        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Shuffle(numbers);
        foreach (var num in numbers)
        {
            if (IsValid(grid, r, c, num))
            {
                grid[r, c] = num;
                if (FillGrid(r, c + 1, grid))
                {
                    return true;
                }
            }
        }
        grid[r, c] = 0;

        return false;
    }

    private static bool IsValid(int[,] board, int row, int col, int val)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[row, i] == val)
            {
                return false;
            }

        }
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[i, col] == val)
            {
                return false;
            }
        }

        int subGridRow = row / SUBGRID_SIZE * SUBGRID_SIZE;
        int subGridCol = col / SUBGRID_SIZE * SUBGRID_SIZE;

        for (int r = subGridRow; r < subGridRow + SUBGRID_SIZE; r++)
        {
            for (int c = subGridCol; c < subGridCol + SUBGRID_SIZE; c++)
            {
                if (board[r, c] == val)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public static int[,] GeneratePuzzle(DifficultyLevel level)
    {
        var grid = new int[GRID_SIZE, GRID_SIZE];
        int squareToRemove = 0;
        switch (level)
        {
            case DifficultyLevel.EASY:
                {
                    squareToRemove = Random.Range(MIN_SQUARE_REMOVED, MIN_SQUARE_REMOVED + 5);
                    break;
                }
            case DifficultyLevel.MEDIUM:
                {
                    squareToRemove = Random.Range(MIN_SQUARE_REMOVED + 10, MIN_SQUARE_REMOVED + 20);
                    break;
                }
            case DifficultyLevel.HARD:
                {
                    squareToRemove = Random.Range(MIN_SQUARE_REMOVED + 20, MAX_SQUARE_REMOVED);
                    break;
                }
        }
        InitializeGrid(grid);
        while (squareToRemove > 0)
        {
            int randRow = Random.Range(0, BOARD_SIZE);
            int randCol = Random.Range(0, BOARD_SIZE);
            if (grid[randCol, randRow] != 0)
            {
                int temp = grid[randCol, randRow];
                grid[randCol, randRow] = 0;
                if (Solver.HasUniqueSolution(grid))
                {
                    squareToRemove--;
                }
                else
                {
                    grid[randCol, randRow] = temp;
                }
            }
        }
        return grid;
    }
}
