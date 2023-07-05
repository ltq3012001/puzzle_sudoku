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

    [SerializeField] private Animator _sceneTransitionAnimator;
    [Header("Button Image")]
    [Space]
    [Header("Top Bar")]
    [SerializeField] private Image _optionButtonImg;
    [SerializeField] private Image _themeButtonImg;

    [Header("Menu Scene - Play button group")]
    [SerializeField] private Image _resumeButtonImg;
    [SerializeField] private Image _easyButtonImg;
    [SerializeField] private Image _mediumButtonImg;
    [SerializeField] private Image _hardButtonImg;


    [Header("Bottom Bar")]
    [SerializeField] private Image _mainButtonImg;
    [SerializeField] private Image _statisticsButtonImg;
    [SerializeField] private Image _storeButtonImg;
    [SerializeField] private Button _mainButton;
    [SerializeField] private Button _statisticButton;
    [SerializeField] private Button _storeButton;

    [SerializeField] private Color _buttonThemeColor;
    [SerializeField] private Color _currentStateButtonThemeColor;

    private const float OPEN_SCENE_TIME = 1f;
    private const float CLOSE_SCENE_TIME = 0.84f;
    private const float LOAD_GAME_TIME = 1f;

    [SerializeField] private GameObject _mainScene;
    [SerializeField] private GameObject _storeScene;
    [SerializeField] private GameObject _statisticsScene;

    [SerializeField] private Animator _mainSceneAnimator;
    [SerializeField] private Animator _storeSceneAnimator;
    [SerializeField] private Animator _statisticSceneAnimator;

    private GameObject _currentScene;
    private Animator _currentSceneAnimator;

    //Play Game Button group
    private void Start()
    {
        _currentState = MainMenuState.Main;
        _mainButtonImg.color = _currentStateButtonThemeColor;
        LoadScene(_mainScene, _mainSceneAnimator);
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
    }

    private void EnableBottomBarButton()
    {
        _mainButton.interactable = true;
        _storeButton.interactable = true;
        _statisticButton.interactable = true;
    }

    private IEnumerator LoadGameScene()
    {
        _sceneTransitionAnimator.SetTrigger("LoadScene");
        yield return new WaitForSeconds(LOAD_GAME_TIME);
        SceneManager.LoadScene(1);
    }

    private IEnumerator CloseCurrentScene(GameObject scene, Animator animator)
    {
        DisableBottomBarButton();
        _currentSceneAnimator.SetTrigger("CloseScene");
        yield return new WaitForSeconds(CLOSE_SCENE_TIME);
        _currentScene.SetActive(false);
        _currentScene = null;
        _currentSceneAnimator = null;
        LoadScene(scene, animator);
    }
    private void LoadScene(GameObject scene, Animator animator)
    {
        _currentScene = scene;
        _currentSceneAnimator = animator;
        _currentScene.SetActive(true);
        EnableBottomBarButton();
    }

    public void OnResumeButtonPressed()
    {
        DataManager.SetNewPuzzleLevel(Generator.DifficultyLevel.RELOAD);
        StartCoroutine(LoadGameScene());
    }

    public void OnEasyGameButtonPressed()
    {
        DataManager.SetNewPuzzleLevel(Generator.DifficultyLevel.EASY);
        StartCoroutine(LoadGameScene());
    }

    public void OnMediumGameButtonPressed()
    {
        DataManager.SetNewPuzzleLevel(Generator.DifficultyLevel.MEDIUM);
        StartCoroutine(LoadGameScene());
    }

    public void OnHardGameButtonPressed()
    {
        DataManager.SetNewPuzzleLevel(Generator.DifficultyLevel.HARD);
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
        //First change another button to default color
        _storeButtonImg.color = _buttonThemeColor;
        _statisticsButtonImg.color = _buttonThemeColor;
        //Then change current state button to current state color
        _mainButtonImg.color = _currentStateButtonThemeColor;
        StartCoroutine(CloseCurrentScene(_mainScene, _mainSceneAnimator));
    }

    public void OnStatisticButtonPressed()
    {
        if (_currentState == MainMenuState.Statistics)
        {
            Debug.Log("Nothing hapeen");
            return;
        }
        _currentState = MainMenuState.Statistics;
        //First change another button to default color
        _mainButtonImg.color = _buttonThemeColor;
        _storeButtonImg.color = _buttonThemeColor;
        //Then change current state button to current state color
        _statisticsButtonImg.color = _currentStateButtonThemeColor;
        StartCoroutine(CloseCurrentScene(_statisticsScene, _statisticSceneAnimator));
    }

    public void OnStoreButtonPressed()
    {
        if (_currentState == MainMenuState.Store)
        {
            Debug.Log("Nothing hapeen");
            return;
        }
        _currentState = MainMenuState.Store;
        //First change another button to default color
        _mainButtonImg.color = _buttonThemeColor;
        _statisticsButtonImg.color = _buttonThemeColor;
        //Then change current state button to current state color
        _storeButtonImg.color = _currentStateButtonThemeColor;
        StartCoroutine(CloseCurrentScene(_storeScene, _storeSceneAnimator));
    }



}
