using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generator;

public class DataManager
{
    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;

    public struct LevelInfo
    {
        public Cell[,] grid;
        public Generator.DifficultyLevel level; 
        public int lifeRemain; 
        public int adUsed;
        public LevelInfo(Cell[,] grid, Generator.DifficultyLevel level, int lifeRemain, int adUsed)
        {
            this.grid = grid;
            this.level = level;
            this.lifeRemain = lifeRemain;
            this.adUsed = adUsed;
        }
    }
    public static void SaveCurrentLevel(Cell[,] grid, Generator.DifficultyLevel level, int lifeRemain, int adUsed)
    {
        PlayerPrefs.SetString("Level", level.ToString());
        PlayerPrefs.SetInt("LifeRemain", lifeRemain);
        PlayerPrefs.SetInt("AdUsed", adUsed);
    }

    //public static LevelInfo GetCurrentLevel()
    //{
    //    //Generator.DifficultyLevel level = Generator.DifficultyLevel.RELOAD;
    //    //int[,] tempGrid = new int[GRID_SIZE, GRID_SIZE];
    //    //string arrayString = PlayerPrefs.GetString("Grid");
    //    //string[] arrayValue = arrayString.Split(",");
    //    //int index = 0;
    //    //for (int i = 0; i < GRID_SIZE; i++)
    //    //{
    //    //    for (int j = 0; j < GRID_SIZE; j++)
    //    //    {
    //    //        tempGrid[i, j] = int.Parse(arrayValue[index]);
    //    //        index++;
    //    //    }
    //    //}
    //    //Enum.TryParse<Generator.DifficultyLevel>(PlayerPrefs.GetString("Level"),out level);
    //    //LevelInfo levelInfo = new LevelInfo(
    //    //    tempGrid,
    //    //    level,
    //    //    PlayerPrefs.GetInt("LifeRemain"),
    //    //    PlayerPrefs.GetInt("AdUsed")
    //    //    );
    //    //return levelInfo;
    //}

    public static void SaveNewPuzzleLevel(Generator.DifficultyLevel level)
    {
        PlayerPrefs.SetString("NewPuzzleLevel", level.ToString());
    }

    public static void SaveGameStats()
    {

    }
}
