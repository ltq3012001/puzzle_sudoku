using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private enum MainMenuState
    {
        Main,
        Statistics,
        Store
    }


    private MainMenuState _currentState;

    [SerializeField] private ThemeColor _darkThemeColor;
    [SerializeField] private ThemeColor _lightThemeColor;
    private ThemeColor currentThemeColor;
    private string currentThemeColorState;

    [SerializeField] private Animator _sceneTransitionAnimator;
    [SerializeField] private Camera _camera;
    [Header("Button Image")]
    [Space]
    [Header("Top Bar")]
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _themeButton;

    [Header("Menu Scene - Play button group")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _easyButton;
    [SerializeField] private Button _mediumButton;
    [SerializeField] private Button _hardButton;

    [Header("Bottom Bar")]
    [SerializeField] private Button _mainButton;
    [SerializeField] private Button _statisticButton;
    [SerializeField] private Button _storeButton;

    private const float LOAD_GAME_TIME = 1f;

    [SerializeField] private GameObject _mainScene;
    [SerializeField] private GameObject _storeScene;
    [SerializeField] private GameObject _statisticsScene;

    [SerializeField] private GameObject _statisticsContentPrefabs;
    [SerializeField] private GameObject _statisticsContentCanvas;

    private GameObject _currentScene;
    private Animator _currentSceneAnimator;

    //Play Game Button group
    private void Start()
    {
        currentThemeColorState = PlayerPrefs.GetString("ThemeColor", "light");
        UpdateThemeColor(currentThemeColorState);

        _currentState = MainMenuState.Main;


        string level = PlayerPrefs.GetString("Difficulty", string.Empty);
        if (level != string.Empty)
        {
            _resumeButton.interactable = true;
        }

        LoadStatisticContent();

        SelectSceneButton(_currentState);

        LoadScene(_mainScene);
    }
    private void Update()
    {

    }

    private void UpdateThemeColor(string theme)
    {
        switch (theme)
        {
            case "light":
                {
                    currentThemeColor = _lightThemeColor;
                    break;
                }
            case "dark":
                {
                    currentThemeColor = _darkThemeColor;
                    break;
                }
        }
        PlayerPrefs.SetString("ThemeColor", theme);
        _mainButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _statisticButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _storeButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;

        _camera.backgroundColor = currentThemeColor.ThemeMainColor;

        _optionButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _themeButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;

        _resumeButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _resumeButton.GetComponentInChildren<TMP_Text>().color = currentThemeColor.ThemeMainColor;

        _easyButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _easyButton.GetComponentInChildren<TMP_Text>().color = currentThemeColor.ThemeMainColor;

        _mediumButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _mediumButton.GetComponentInChildren<TMP_Text>().color = currentThemeColor.ThemeMainColor;

        _hardButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
        _hardButton.GetComponentInChildren<TMP_Text>().color = currentThemeColor.ThemeMainColor;
    }

    private void CallOptionPopup()
    {

    }

    private void CallThemePopup()
    {

    }

    private void LoadStatisticContent()
    {
        for (Generator.DifficultyLevel difficuly = Generator.DifficultyLevel.EASY; difficuly <= Generator.DifficultyLevel.HARD; difficuly++)
        {
            int gamePlayed = PlayerPrefs.GetInt(string.Format("GameStart_{0}", difficuly.ToString()), 0);
            int gameWon = PlayerPrefs.GetInt(string.Format("GameWon_{0}", difficuly.ToString()), 0);
            int winRate = (int)math.round(gameWon * 100 / gamePlayed);
            int perfectGame = PlayerPrefs.GetInt(string.Format("PerfectGame_{0}", difficuly.ToString()), 0);

            float time = PlayerPrefs.GetFloat(string.Format("HighTimer_{0}", difficuly.ToString()), 0);
            string timeString = string.Format("{0:00}:{1:00}", math.round(time / 60), math.round(time % 60));
            GameObject statisticContent = GameObject.Instantiate(_statisticsContentPrefabs, _statisticsContentCanvas.transform);
            statisticContent.GetComponent<StatisticContent>().Initialize(difficuly, gamePlayed, gameWon, winRate, perfectGame, timeString, currentThemeColor);
        }
    }

    private void DisableBottomBarButton()
    {
        _mainButton.interactable = false;
        _storeButton.interactable = false;
        _statisticButton.interactable = false;
        _optionButton.interactable = false;
        _themeButton.interactable = false;
    }

    private void EnableBottomBarButton()
    {
        _mainButton.interactable = true;
        _storeButton.interactable = true;
        _statisticButton.interactable = true;
        _optionButton.interactable = true;
        _themeButton.interactable = true;
    }

    private IEnumerator LoadGameScene()
    {
        _sceneTransitionAnimator.SetTrigger("LoadScene");
        yield return new WaitForSeconds(LOAD_GAME_TIME);
        SceneManager.LoadScene(1);
    }

    private void CloseCurrentScene(GameObject scene)
    {
        DisableBottomBarButton();
        _currentScene.SetActive(false);
        _currentScene = null;
        LoadScene(scene);
    }
    private void LoadScene(GameObject scene)
    {
        _currentScene = scene;
        _currentScene.SetActive(true);
        EnableBottomBarButton();
    }

    private void SelectSceneButton(MainMenuState state)
    {
        switch (state)
        {
            case MainMenuState.Main:
                {
                    _mainButton.GetComponent<Image>().color = currentThemeColor.SelectedStateColor;

                    _statisticButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
                    _storeButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
                    break;
                }
            case MainMenuState.Statistics:
                {
                    _statisticButton.GetComponent<Image>().color = currentThemeColor.SelectedStateColor;

                    _mainButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
                    _storeButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
                    break;
                }
            case MainMenuState.Store:
                {
                    _storeButton.GetComponent<Image>().color = currentThemeColor.SelectedStateColor;

                    _statisticButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
                    _mainButton.GetComponent<Image>().color = currentThemeColor.ButtonColor;
                    break;
                }
        }
    }

    public void OnResumeButtonPressed()
    {
        DataManager.SaveNewPuzzleLevel(Generator.DifficultyLevel.RELOAD);
        StartCoroutine(LoadGameScene());
    }

    public void OnEasyGameButtonPressed()
    {
        DataManager.SaveNewPuzzleLevel(Generator.DifficultyLevel.EASY);
        StartCoroutine(LoadGameScene());
    }

    public void OnMediumGameButtonPressed()
    {
        DataManager.SaveNewPuzzleLevel(Generator.DifficultyLevel.MEDIUM);
        StartCoroutine(LoadGameScene());
    }

    public void OnHardGameButtonPressed()
    {
        DataManager.SaveNewPuzzleLevel(Generator.DifficultyLevel.HARD);
        StartCoroutine(LoadGameScene());
    }
    //Top bar
    public void OnOptionButtonPressed()
    {
        CallOptionPopup();
    }

    public void OnThemeButtonPressed()
    {
        //CallThemePopup();
        if (currentThemeColorState == "light")
        {
            currentThemeColorState = "dark";
            UpdateThemeColor(currentThemeColorState);
        }
        else
        {
            currentThemeColorState = "light";
            UpdateThemeColor(currentThemeColorState);
        }

    }

    //Bottom bar

    public void OnMainButtonPressed()
    {
        if (_currentState == MainMenuState.Main)
        {
            Debug.Log("Nothing hapeen");
            return;
        }
        _currentState = MainMenuState.Main;
        SelectSceneButton(_currentState);
        CloseCurrentScene(_mainScene);
    }

    public void OnStatisticButtonPressed()
    {
        if (_currentState == MainMenuState.Statistics)
        {
            Debug.Log("Nothing hapeen");
            return;
        }
        _currentState = MainMenuState.Statistics;
        SelectSceneButton(_currentState);
        CloseCurrentScene(_statisticsScene);
    }

    public void OnStoreButtonPressed()
    {
        if (_currentState == MainMenuState.Store)
        {
            Debug.Log("Nothing hapeen");
            return;
        }
        _currentState = MainMenuState.Store;
        SelectSceneButton(_currentState);
        CloseCurrentScene(_storeScene);
    }



}
