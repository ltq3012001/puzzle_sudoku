using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Generator;

public class GameOverPopup : EndGamePopup
{
    [SerializeField] private TMP_Text _popupTitle;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _previousLevelButton;

    [SerializeField] private Button _secondChanceButton;
    [SerializeField] private Button _restartThisGameButton;
    [SerializeField] private Button _startNewGameButton;

    [SerializeField] private Image _layoutGroup;

    public void Initialize(GameManager gameManager, Generator.DifficultyLevel difficulty, ThemeColor themeColor)
    {
        difficultyLevel = difficulty;
        _secondChanceButton.onClick.AddListener(gameManager.OnSecondChancePressed);
        _restartThisGameButton.onClick.AddListener(gameManager.OnRestartThisGamePressed);
        _startNewGameButton.onClick.AddListener(gameManager.OnStartNewGamePressed);
        _nextLevelButton.onClick.AddListener(OnNextLevelPressed);
        _previousLevelButton.onClick.AddListener(OnPreviousLevelPressed);

        _popupTitle.color = themeColor.ButtonColor;

        _nextLevelButton.GetComponent<Image>().color = themeColor.ButtonColor;
        _previousLevelButton.GetComponent<Image>().color = themeColor.ButtonColor;

        _secondChanceButton.GetComponent<Image>().color = themeColor.ButtonColor;
        _restartThisGameButton.GetComponent<Image>().color = themeColor.ButtonColor;
        _startNewGameButton.GetComponent<Image>().color = themeColor.ButtonColor;

        _secondChanceButton.GetComponentInChildren<TMP_Text>().color = themeColor.ThemeMainColor;
        _restartThisGameButton.GetComponentInChildren<TMP_Text>().color = themeColor.ThemeMainColor;
        _startNewGameButton.GetComponentInChildren<TMP_Text>().color = themeColor.ThemeMainColor;

        this.GetComponent<Image>().color = themeColor.ButtonColor;
        _layoutGroup.color = themeColor.ThemeMainColor;

        ReloadUIValue();
    }
}
