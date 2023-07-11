using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Generator;

public class GameOverPopup : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    public Generator.DifficultyLevel difficultyLevel;

    [SerializeField] private TMPro.TMP_Text _newGameButtonText;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _previousLevelButton;

    [SerializeField] private Button _secondChanceButton;
    [SerializeField] private Button _restartThisGameButton;
    [SerializeField] private Button _startNewGameButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialized(GameManager gameManager, Generator.DifficultyLevel difficulty)
    {
        this.gameManager = gameManager;
        difficultyLevel = difficulty;
        _secondChanceButton.onClick.AddListener(this.gameManager.OnSecondChancePressed);
        _restartThisGameButton.onClick.AddListener(this.gameManager.OnRestartThisGamePressed);
        _startNewGameButton.onClick.AddListener(this.gameManager.OnStartNewGamePressed);
        _nextLevelButton.onClick.AddListener(this.gameManager.OnNextLevelPressed);
        _previousLevelButton.onClick.AddListener(this.gameManager.OnPreviousLevelPressed);
        ReloadUIValue();
    }

    public void ReloadUIValue()
    {
        _newGameButtonText.text = string.Format("New Game ({0})", difficultyLevel.ToString());
    }
}
