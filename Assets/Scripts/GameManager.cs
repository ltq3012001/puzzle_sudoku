using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ThemeColor _themeColor;

    [SerializeField] private Animator _sceneTransitionAnimator;

    [SerializeField] private Vector3 _startPos;
    [SerializeField] private float _offsetX, _offsetY;
    [SerializeField] private SubGrid _subGridPrefabs;

    [Header("UI")]
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _lifeText;
    [SerializeField] private TMP_Text _difficultyText;

    private bool hasGameFinished;
    private Cell[,] cells;
    [SerializeField] private Cell selectedCell;

    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;

    private float timer;
    private int life;
    private int hintRemain;
    private int adTimer;
    private Generator.DifficultyLevel difficulty;


    private void Start()
    {
        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;
        Enum.TryParse<Generator.DifficultyLevel>(PlayerPrefs.GetString("NewPuzzleLevel"), out difficulty);
        SpawnCell(difficulty);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        _timeText.text = string.Format("{0:00}:{1:00}", math.round(timer / 60), math.round(timer % 60));

        if (hasGameFinished || !Input.GetMouseButton(0)) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Cell tempCell;
        if (hit
            && hit.collider.TryGetComponent(out tempCell)
            && tempCell != selectedCell
            )
        {
            ResetGrid();
            selectedCell = tempCell;
            Highlight();
        }

    }
    private void SpawnCell(Generator.DifficultyLevel difficultyLevel)
    {
        if (difficultyLevel != Generator.DifficultyLevel.RELOAD)
        {
            CreateNewLevel(difficultyLevel);
        }
        else
        {
            GetCurrentLevel();
        }
    }
    private void CreateNewLevel(Generator.DifficultyLevel level)
    {
        int[,] tempGrid = Generator.GeneratePuzzle(level);
        for (int i = 0; i < GRID_SIZE; i++)
        {
            Vector3 spawnPos = _startPos + i % 3 * _offsetX * Vector3.right + i / 3 * _offsetY * Vector3.up;
            SubGrid subGrid = Instantiate(_subGridPrefabs, spawnPos, Quaternion.identity);
            List<Cell> subGridCells = subGrid.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                subGridCells[j].Row = startRow + j / 3;
                subGridCells[j].Col = startCol + j % 3;
                int cellValue = tempGrid[subGridCells[j].Row, subGridCells[j].Col];
                subGridCells[j].Init(cellValue, _themeColor);
                cells[subGridCells[j].Row, subGridCells[j].Col] = subGridCells[j];
                PlayerPrefs.SetInt(string.Format("Value [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col), subGridCells[j].Value);
            }
        }
        life = 3;
        timer = 0;
        adTimer = 0;
        hintRemain = 1;
        difficulty = level;
        PlayerPrefs.SetString("Difficulty", level.ToString());
        PlayerPrefs.SetInt("Life", life);
        PlayerPrefs.SetInt("HintRemain", hintRemain);
    }
    private void GetCurrentLevel()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            Vector3 spawnPos = _startPos + i % 3 * _offsetX * Vector3.right + i / 3 * _offsetY * Vector3.up;
            SubGrid subGrid = Instantiate(_subGridPrefabs, spawnPos, Quaternion.identity);
            List<Cell> subGridCells = subGrid.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                subGridCells[j].Row = startRow + j / 3;
                subGridCells[j].Col = startCol + j % 3;
                int cellValue = PlayerPrefs.GetInt(string.Format("Value [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col));
                int cellIsIncorrect = PlayerPrefs.GetInt(string.Format("IsIncorrect [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col));
                int cellIsLocked = PlayerPrefs.GetInt(string.Format("IsLocked [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col));
                subGridCells[j].Init(cellValue, Convert.ToBoolean(cellIsIncorrect), Convert.ToBoolean(cellIsLocked), _themeColor);
                cells[subGridCells[j].Row, subGridCells[j].Col] = subGridCells[j];
            }
        }
        Enum.TryParse<Generator.DifficultyLevel>(PlayerPrefs.GetString("Level"), out difficulty);
        life = PlayerPrefs.GetInt("Life");
        hintRemain = PlayerPrefs.GetInt("HintRemain");
        timer = PlayerPrefs.GetFloat("Timer");
        ShowUIValue();
    }

    private void ShowUIValue()
    {
        _difficultyText.text = difficulty.ToString();
        _lifeText.text = string.Format("{0} / 3", life);
    }
    private void ResetGrid()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].Reset();
            }
        }
    }

    private void Highlight()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if(cells[i, j].Value != 0 && cells[i,j].Value == selectedCell.Value)
                {
                    cells[i, j].Highlight();
                }
                cells[i, j].IsIncorrect = !IsValid(cells[i, j], cells);
            }
        }

        int currentRow = selectedCell.Row;
        int currentCol = selectedCell.Col;
        int subGridRow = currentRow - currentRow % SUBGRID_SIZE;
        int subGridCol = currentCol - currentCol % SUBGRID_SIZE;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            cells[i, currentCol].Highlight();
            cells[currentRow, i].Highlight();
            cells[subGridRow + i % SUBGRID_SIZE, subGridCol + i / SUBGRID_SIZE].Highlight();
        }

        cells[currentRow, currentCol].Select();
    }

    private bool IsValid(Cell cell, Cell[,] cells)
    {
        int row = cell.Row;
        int col = cell.Col;
        int value = cell.Value;
        cell.Value = 0;

        if (value == 0) return true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (cells[row, i].Value == value || cells[i, col].Value == value)
            {
                cell.Value = value;
                return false;
            }
        }

        int subGridRow = row - row % SUBGRID_SIZE;
        int subGridCol = col - col % SUBGRID_SIZE;

        for (int r = subGridRow; r < subGridRow + SUBGRID_SIZE; r++)
        {
            for (int c = subGridCol; c < subGridCol + SUBGRID_SIZE; c++)
            {
                if (cells[r, c].Value == value)
                {
                    cell.Value = value;
                    return false;
                }
            }
        }
        cell.Value = value;
        return true;
    }

    private void CheckWin()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].Value == 0 || cells[i, j].IsIncorrect) return;
            }
        }
        hasGameFinished = true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].UpdateWin();
            }
        }
    }

    private void DecreaseLife()
    {
        if (life == 1)
        {
            GameOver();
            return;
        }
        life--;
        _lifeText.text = string.Format("{0} / 3", life);
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void SaveCurrentGameStatus()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                int row = startRow + j / 3;
                int col = startCol + j % 3;
                Cell cell = cells[row, col];
                PlayerPrefs.SetInt(string.Format("Value [{0}],[{1}]", cell.Row, cell.Col), cell.Value);
                PlayerPrefs.SetInt(string.Format("IsIncorrect [{0}],[{1}]", cell.Row, cell.Col), cell.IsIncorrect.GetHashCode());
                PlayerPrefs.SetInt(string.Format("IsLocked [{0}],[{1}]", cell.Row, cell.Col), cell.IsLocked.GetHashCode());
            }
        }
        PlayerPrefs.SetInt("Life", life);
        PlayerPrefs.SetInt("HintRemain", hintRemain);
        PlayerPrefs.SetFloat("Timer", timer);
    }

    public void UpdateCellValue(int value)
    {
        if (hasGameFinished || selectedCell == null) return;
        if (!selectedCell.IsLocked)
        {
            if (selectedCell.Value == value) return;
            selectedCell.UpdateValue(value);
            Highlight();
            if (!IsValid(selectedCell, cells))
            {
                DecreaseLife();
            }
            CheckWin();
        }
    }
    public void ExitGame()
    {
        SaveCurrentGameStatus();
        StartCoroutine(LoadMainMenuScene());
    }


    IEnumerator LoadMainMenuScene()
    {
        _sceneTransitionAnimator.SetTrigger("LoadScene");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
