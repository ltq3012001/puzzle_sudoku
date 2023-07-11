using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
    [SerializeField] private ThemeColor _themeColor;
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

    private const float OPEN_SCENE_TIME = 1f;
    private const float CLOSE_SCENE_TIME = 0.84f;
    private const float LOAD_GAME_TIME = 1f;

    [SerializeField] private GameObject _mainScene;
    [SerializeField] private GameObject _storeScene;
    [SerializeField] private GameObject _statisticsScene;

    private GameObject _currentScene;
    private Animator _currentSceneAnimator;

    //Play Game Button group
    private void Start()
    {
        _currentState = MainMenuState.Main;
        _mainButton.GetComponent<Image>().color = _themeColor.ButtonColor;
        _statisticButton.GetComponent<Image>().color = _themeColor.ButtonColor;
        _storeButton.GetComponent<Image>().color = _themeColor.ButtonColor;

        _camera.backgroundColor = _themeColor.ThemeMainColor;

        _optionButton.GetComponent<Image>().color = _themeColor.ButtonColor;
        _themeButton.GetComponent<Image>().color = _themeColor.ButtonColor;

        _resumeButton.GetComponent<Image>().color = _themeColor.ButtonColor;
        _easyButton.GetComponent<Image>().color = _themeColor.ButtonColor;
        _mediumButton.GetComponent<Image>().color = _themeColor.ButtonColor;
        _hardButton.GetComponent<Image>().color = _themeColor.ButtonColor;

        string level = PlayerPrefs.GetString("Difficulty", string.Empty);
        if(level != string.Empty)
        {
            _resumeButton.interactable = false;
        }

        SelectSceneButton(_currentState);

        LoadScene(_mainScene);
    }
    private void Update()
    {

    }

    private void UpdateThemeColor()
    {

    }

    private void CallOptionPopup()
    {

    }

    private void CallThemePopup()
    {

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
                    _mainButton.GetComponent<Image>().color = _themeColor.SelectedStateColor;

                    _statisticButton.GetComponent<Image>().color = _themeColor.ButtonColor;
                    _storeButton.GetComponent<Image>().color = _themeColor.ButtonColor;
                    break;
                }
            case MainMenuState.Statistics:
                {
                    _statisticButton.GetComponent<Image>().color = _themeColor.SelectedStateColor;

                    _mainButton.GetComponent<Image>().color = _themeColor.ButtonColor;
                    _storeButton.GetComponent<Image>().color = _themeColor.ButtonColor;
                    break;
                }
            case MainMenuState.Store:
                {
                    _storeButton.GetComponent<Image>().color = _themeColor.SelectedStateColor;

                    _statisticButton.GetComponent<Image>().color = _themeColor.ButtonColor;
                    _mainButton.GetComponent<Image>().color = _themeColor.ButtonColor;
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
        CallThemePopup();
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
