using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private Button[] _inputButton;

    //Note
    [SerializeField] private TMP_Text _noteModeNotiText;
    [SerializeField] private Image _noteModeInside;
    [SerializeField] private Image _noteModeBorder;

    [SerializeField] private TMP_Text _noteButtonText;
    [SerializeField] private Image _noteButton;
    private bool isNoteMode;
    //Hint
    [SerializeField] private TMP_Text _hintNotiText;
    [SerializeField] private Image _hintModeInside;
    [SerializeField] private Image _hintModeBorder;

    [SerializeField] private TMP_Text _hintButtonText;
    [SerializeField] private Image _hintButton;
    //Undo
    [SerializeField] private TMP_Text _undoText;
    [SerializeField] private Image _undoButton;
    //Erase
    [SerializeField] private TMP_Text _eraseText;
    [SerializeField] private Image _eraseButton;

    [SerializeField] private GameObject _gameOverPopupPrefabs;
    [SerializeField] private GameObject _gameWonPoupPrefabs;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private SpriteRenderer _backGround;
    [SerializeField] private Image _optionButton;
    [SerializeField] private Image _mainMenuButton;


    private GameObject currentPopup;

    private bool hasGameFinished;
    private Cell[,] cells;
    [SerializeField] private Cell selectedCell;
    private List<Cell> lastUpdatedCell;
    private List<int> lastUpdatedCellValue;


    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;

    private float timer;
    private int life;
    private int hintRemain;
    private int adTimer;
    [SerializeField] private Generator.DifficultyLevel difficulty;


    private void Start()
    {
        Enum.TryParse<Generator.DifficultyLevel>(PlayerPrefs.GetString("NewPuzzleLevel"), out difficulty);
        SpawnCell(difficulty);

        foreach(Button inputButton in _inputButton)
        {
            inputButton.GetComponentInChildren<TMP_Text>().color = _themeColor.ButtonColor;
        }
        _optionButton.color = _themeColor.ButtonColor;
        _mainMenuButton.color = _themeColor.ButtonColor;

        _noteModeBorder.color = _themeColor.ButtonColor;
        _noteModeInside.color = _themeColor.ThemeMainColor;
        _noteModeNotiText.color = _themeColor.ButtonColor;

        _noteButtonText.color = _themeColor.ButtonColor;
        _noteButton.color = _themeColor.ButtonColor;

        _hintModeBorder.color = _themeColor.ButtonColor;
        _hintModeInside.color = _themeColor.ThemeMainColor;
        _hintNotiText.color = _themeColor.ButtonColor;

        _hintButtonText.color = _themeColor.ButtonColor;
        _hintButton.color = _themeColor.ButtonColor;

        _undoButton.color = _themeColor.ButtonColor;
        _undoText.color = _themeColor.ButtonColor;

        _eraseButton.color = _themeColor.ButtonColor;
        _eraseText.color = _themeColor.ButtonColor;

        _backGround.color = _themeColor.ButtonColor;
    }

    private void Update()
    {
        if (!hasGameFinished)
        {
            timer += Time.deltaTime;
            _timeText.text = string.Format("{0:00}:{1:00}", math.round(timer / 60), math.round(timer % 60));
        }

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

    //Game Flow

    private void SpawnCell(Generator.DifficultyLevel difficultyLevel)
    {
        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;
        lastUpdatedCell = null;
        lastUpdatedCellValue = new List<int>();
        lastUpdatedCell = new List<Cell>();
        if (difficultyLevel == Generator.DifficultyLevel.RELOAD)
        {
            GetCurrentLevel();
        }
        else if (difficultyLevel == Generator.DifficultyLevel.RESTART)
        {
            RestartCurrentLevel();
        }
        else
        {
            CreateNewLevel(difficultyLevel);
        }
    }

    private void CreateNewLevel(Generator.DifficultyLevel level)
    {
        int[,] tempGrid = Generator.GeneratePuzzle(level);
        for (int i = 0; i < GRID_SIZE; i++)
        {
            Vector3 spawnPos = _startPos + i % 3 * _offsetX * Vector3.right + i / 3 * _offsetY * Vector3.up;
            SubGrid subGrid = Instantiate(_subGridPrefabs, spawnPos, Quaternion.identity);
            subGrid.Initialize(_themeColor.ContentBackground);
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
                PlayerPrefs.SetInt(string.Format("New Puzzle [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col), subGridCells[j].Value);
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
        PlayerPrefs.SetInt("SelectedCellRow", -1);
        PlayerPrefs.SetInt("SelectedCellCol", -1);
        int listCount = PlayerPrefs.GetInt("LastUpdateCellCount", -1);
        if (listCount != -1)
        {
            for (int i = 0; i < listCount; i++)
            {
                PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellRow_{0}", i));
                PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellCol_{0}", i));
                PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellValue_{0}", i));
            }
            PlayerPrefs.DeleteKey("LastUpdateCellCount");
        }
        ShowUIValue();
    }

    private void RestartCurrentLevel()
    {
        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;
        lastUpdatedCell = null;
        lastUpdatedCellValue = new List<int>();
        lastUpdatedCell = new List<Cell>();
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
                int cellValue = PlayerPrefs.GetInt(string.Format("New Puzzle [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col));
                subGridCells[j].Init(cellValue, _themeColor);
                cells[subGridCells[j].Row, subGridCells[j].Col] = subGridCells[j];
                PlayerPrefs.SetInt(string.Format("Value [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col), subGridCells[j].Value);
            }
        }
        life = 3;
        timer = 0;
        adTimer = 0;
        hintRemain = 1;
        Enum.TryParse<Generator.DifficultyLevel>(PlayerPrefs.GetString("Difficulty"), out difficulty);
        PlayerPrefs.SetInt("Life", life);
        PlayerPrefs.SetInt("HintRemain", hintRemain);
        PlayerPrefs.SetInt("SelectedCellRow", -1);
        PlayerPrefs.SetInt("SelectedCellCol", -1);
        int listCount = PlayerPrefs.GetInt("LastUpdateCellCount", -1);
        if (listCount != -1)
        {
            for (int i = 0; i < listCount; i++)
            {
                PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellRow_{0}", i));
                PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellCol_{0}", i));
                PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellValue_{0}", i));
            }
            PlayerPrefs.DeleteKey("LastUpdateCellCount");
        }
        ShowUIValue();
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

                List<int> noteList = new List<int>();
                int listCount = PlayerPrefs.GetInt(string.Format("NoteValueCount [{0}],[{1}]", subGridCells[j].Row, subGridCells[j].Col), 0);
                if (listCount > 0)
                {
                    for (int count = 0; count < listCount; count++)
                    {
                        noteList.Add(PlayerPrefs.GetInt(string.Format("NoteValue [{0}],[{1}]_{2}", subGridCells[j].Row, subGridCells[j].Col, count)));
                    }
                }
                subGridCells[j].Init(cellValue, Convert.ToBoolean(cellIsIncorrect), Convert.ToBoolean(cellIsLocked), noteList, _themeColor);
                cells[subGridCells[j].Row, subGridCells[j].Col] = subGridCells[j];
            }
        }
        int selectedCellRow = PlayerPrefs.GetInt("SelectedCellRow", -1);
        int selectedCellCol = PlayerPrefs.GetInt("SelectedCellCol", -1);

        bool isReset = false;
        int lastUpdateCellCount = PlayerPrefs.GetInt("LastUpdatedCellCount", -1);
        if (lastUpdateCellCount != -1)
        {
            for (int count = 0; count < lastUpdateCellCount; count++)
            {
                int lastUpdateCellRow = PlayerPrefs.GetInt(string.Format("LastUpdatedCellRow_{0}", count), -1);
                int lastUpdateCellCol = PlayerPrefs.GetInt(string.Format("LastUpdatedCellCol_{0}", count), -1);
                if (lastUpdateCellRow != -1 && lastUpdateCellCol != -1)
                {
                    lastUpdatedCell.Add(cells[lastUpdateCellRow, lastUpdateCellCol]);
                    lastUpdatedCellValue.Add(PlayerPrefs.GetInt(string.Format("LastUpdatedCellValue_{0}", count), 0));
                }
            }
        }
        Enum.TryParse<Generator.DifficultyLevel>(PlayerPrefs.GetString("Difficulty"), out difficulty);
        life = PlayerPrefs.GetInt("Life");
        hintRemain = PlayerPrefs.GetInt("HintRemain");
        timer = PlayerPrefs.GetFloat("Timer");
        if (isReset)
        {
            ResetGrid();
            Highlight();
        }
        ShowUIValue();
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
                List<int> noteValues = new List<int>(cell.GetNoteValue());
                if (noteValues.Count > 0)
                {
                    PlayerPrefs.SetInt(string.Format("NoteValueCount [{0}],[{1}]", cell.Row, cell.Col), noteValues.Count);
                    for (int count = 0; count < noteValues.Count; count++)
                    {
                        PlayerPrefs.SetInt(string.Format("NoteValue [{0}],[{1}]_{2}", cell.Row, cell.Col, count), noteValues[count]);
                    }
                }
            }
        }
        PlayerPrefs.SetInt("Life", life);
        PlayerPrefs.SetInt("HintRemain", hintRemain);
        PlayerPrefs.SetFloat("Timer", timer);
        if (lastUpdatedCell != null)
        {
            int listCount = lastUpdatedCell.Count;
            PlayerPrefs.SetInt("LastUpdatedCellCount", listCount);
            for (int i = 0; i < listCount; i++)
            {
                PlayerPrefs.SetInt(string.Format("LastUpdatedCellRow_{0}", i), lastUpdatedCell[i].Row);
                PlayerPrefs.SetInt(string.Format("LastUpdatedCellCol_{0}", i), lastUpdatedCell[i].Col);
                PlayerPrefs.SetInt(string.Format("LastUpdatedCellValue_{0}", i), lastUpdatedCellValue[i]);
            }
        }
        if (selectedCell != null)
        {
            PlayerPrefs.SetInt("SelectedCellRow", selectedCell.Row);
            PlayerPrefs.SetInt("SelectedCellCol", selectedCell.Col);
        }
    }

    private void GameOver()
    {
        ShowUIValue();
        SaveStatistics(false);

        currentPopup = GameObject.Instantiate(_gameOverPopupPrefabs, _canvas.transform);
        currentPopup.GetComponent<GameOverPopup>().Initialize(this, difficulty);
    }

    private void GameWin()
    {
        ShowUIValue();
        bool isNewRecord = SaveStatistics(true);

        currentPopup = GameObject.Instantiate(_gameWonPoupPrefabs, _canvas.transform);
        currentPopup.GetComponent<GameWinPopup>().Initialize(this, difficulty, timer, life, isNewRecord);
    }


    //UI Function

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
                if (cells[i, j].Value != 0 && cells[i, j].Value == selectedCell.Value)
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

    private void ShowUIValue()
    {
        HideInput();
        if (hasGameFinished)
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    cells[i, j].UpdateWin();
                }
            }
            return;
        }

        _difficultyText.text = difficulty.ToString();
        _lifeText.text = string.Format("{0} / 3", life);
        if (isNoteMode)
        {
            _noteModeBorder.color = _themeColor.NotiBoxColor;
            _noteModeNotiText.color = _themeColor.NotiBoxColor;
            _noteModeNotiText.text = "ON";
        }
        else
        {
            _noteModeBorder.color = _themeColor.DisableNotiColor;
            _noteModeNotiText.color = _themeColor.DisableNotiColor;
            _noteModeNotiText.text = "OFF";
        }
        if (hintRemain > 0)
        {
            _hintNotiText.text = hintRemain.ToString();
        }
        else
        {
            _hintNotiText.text = "AD";
        }
    }

    private void HideInput()
    {
        int[] inputArray = new int[9];
        for(int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] = 0;
        }
        for(int i =0; i< GRID_SIZE; i++)
        {
            for(int j = 0;j < GRID_SIZE; j++)
            {
                if (cells[i,j].Value!=0 && !cells[i,j].IsIncorrect)
                {
                    inputArray[cells[i, j].Value - 1]++;
                }
            }
        }
        for(int i =0; i< inputArray.Length; i++)
        {
            if (inputArray[i] ==9)
            {
                _inputButton[i].interactable = false;
                _inputButton[i].GetComponentInChildren<TMP_Text>().color = _themeColor.HideNoteTextColor;
            }
        }
    }

    //Game Logic

    private void UpdateCellValue(int value)
    {
        if (selectedCell.Value == value) return;
        lastUpdatedCell.Add(selectedCell);
        lastUpdatedCellValue.Add(selectedCell.Value);

        selectedCell.UpdateValue(value);
        Highlight();
        if (!IsValid(selectedCell, cells))
        {
            DecreaseLife();
        }else
        {

            CheckWin();
        }
        ShowUIValue();
    }

    private void UpdateNoteValue(int value)
    {
        selectedCell.UpdateNoteValue(value);
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

    private void DecreaseLife()
    {
        life--;
        if (life == 0)
        {
            GameOver();
        }
    }

    private void DeleteLastUpdateCellKey()
    {
        int lastIndex = lastUpdatedCellValue.Count - 1;
        lastUpdatedCell.RemoveAt(lastIndex);
        lastUpdatedCellValue.RemoveAt(lastIndex);

        PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellRow_{0}", lastIndex));
        PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellCol_{0}", lastIndex));
        PlayerPrefs.DeleteKey(string.Format("LastUpdatedCellValue_{0}", lastIndex));

        PlayerPrefs.SetInt("LastUpdatedCellCount", lastIndex - 1);
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
        GameWin();
    }

    private bool SaveStatistics(bool isWon)
    {
        bool isNewRecord = false;
        int gameStart = PlayerPrefs.GetInt(string.Format("GameStart_{0}", difficulty.ToString()), 0);
        gameStart++;
        PlayerPrefs.SetInt(string.Format("GameStart_{0}", difficulty.ToString()), gameStart);
        if (isWon)
        {
            int gameWon = PlayerPrefs.GetInt(string.Format("GameWon_{0}", difficulty.ToString()), 0);
            gameWon++;
            PlayerPrefs.SetInt(string.Format("GameWon_{0}", difficulty.ToString()), gameWon);

            float highTimer = PlayerPrefs.GetFloat(string.Format("HighTimer_{0}", difficulty.ToString()), 0f);
            if (timer < highTimer || highTimer == 0f)
            {
                PlayerPrefs.SetFloat(string.Format("HighTimer_{0}", difficulty.ToString()), timer);
                isNewRecord = true;
            }
            if (life == 3)
            {
                int perfectGame = PlayerPrefs.GetInt(string.Format("PerfectGame_{0}", difficulty.ToString()), 0);
                perfectGame++;
                PlayerPrefs.SetInt(string.Format("PerfectGame_{0}", difficulty.ToString()), perfectGame);
            }
            PlayerPrefs.DeleteKey("Difficulty");
        }
        
        
        return isNewRecord;
    }


    //Scene Manager

    private IEnumerator ReloadCurrentScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator LoadMainMenuScene()
    {
        _sceneTransitionAnimator.SetTrigger("LoadScene");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }

    public void InputButtonPressed(int value)
    {
        if (hasGameFinished || selectedCell == null) return;
        if (!selectedCell.IsLocked)
        {
            if (isNoteMode)
            {
                UpdateNoteValue(value);
            }
            else
            {
                UpdateCellValue(value);
            }
        }
    }

    //Feature Button Group

    public void EraseCellValue()
    {
        if (hasGameFinished || selectedCell == null) return;
        if (!selectedCell.IsLocked)
        {
            if (selectedCell.Value == 0) return;
            selectedCell.UpdateValue(0);
            Highlight();
        }
    }

    public void HintButtonPressed()
    {
        if (hintRemain > 0)
        {
            if (hasGameFinished || selectedCell == null) return;
            if (!selectedCell.IsLocked || selectedCell.IsIncorrect)
            {
                Cell tempCell = new Cell();
                tempCell.Row = selectedCell.Row;
                tempCell.Col = selectedCell.Col;
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    tempCell.Value = i;
                    if (IsValid(tempCell, cells))
                    {
                        UpdateCellValue(i);
                    }
                }
            }
            hintRemain--;
            ShowUIValue();
        }
    }

    public void UndoButtonPressed()
    {
        if (hasGameFinished || selectedCell == null) return;
        if (!selectedCell.IsLocked && lastUpdatedCell.Count > 0)
        {
            selectedCell = lastUpdatedCell[lastUpdatedCell.Count - 1];
            selectedCell.UpdateValue(lastUpdatedCellValue[lastUpdatedCellValue.Count - 1]);
            DeleteLastUpdateCellKey();
            Highlight();
        }
    }

    public void NoteModeButtonPressed()
    {
        isNoteMode = !isNoteMode;
        ShowUIValue();
    }

    //Game Over Popup

    public void OnSecondChancePressed()
    {
        life++;
        Destroy(currentPopup);
        currentPopup = null;
        ShowUIValue();
    }

    public void OnRestartThisGamePressed()
    {
        DataManager.SaveNewPuzzleLevel(Generator.DifficultyLevel.RESTART);
        Destroy(currentPopup);
        currentPopup = null;
        StartCoroutine(ReloadCurrentScene());
    }

    public void OnStartNewGamePressed()
    {
        Generator.DifficultyLevel level = currentPopup.GetComponent<EndGamePopup>().difficultyLevel;
        Destroy(currentPopup);
        currentPopup = null;
        DataManager.SaveNewPuzzleLevel(level);
        StartCoroutine(ReloadCurrentScene());
    }

    //Top Bar Button

    public void ExitGame()
    {
        SaveCurrentGameStatus();
        StartCoroutine(LoadMainMenuScene());
    }

}
