using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Generator;

public class GameOverPopup : EndGamePopup
{
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _previousLevelButton;

    [SerializeField] private Button _secondChanceButton;
    [SerializeField] private Button _restartThisGameButton;
    [SerializeField] private Button _startNewGameButton;

    public void Initialize(GameManager gameManager, Generator.DifficultyLevel difficulty)
    {
        difficultyLevel = difficulty;
        _secondChanceButton.onClick.AddListener(gameManager.OnSecondChancePressed);
        _restartThisGameButton.onClick.AddListener(gameManager.OnRestartThisGamePressed);
        _startNewGameButton.onClick.AddListener(gameManager.OnStartNewGamePressed);
        _nextLevelButton.onClick.AddListener(OnNextLevelPressed);
        _previousLevelButton.onClick.AddListener(OnPreviousLevelPressed);
        ReloadUIValue();
    }
}
