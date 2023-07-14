using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPopup : EndGamePopup
{
    [SerializeField] private TMPro.TMP_Text _popupDesc;

    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _previousLevelButton;
    [SerializeField] private Button _startNewGameButton;

    public void Initialize(GameManager gameManager, Generator.DifficultyLevel difficulty, float time, int life, bool isNewHighScore)
    {
        difficultyLevel = difficulty;
        _startNewGameButton.onClick.AddListener(gameManager.OnStartNewGamePressed);
        _nextLevelButton.onClick.AddListener(OnNextLevelPressed);
        _previousLevelButton.onClick.AddListener(OnPreviousLevelPressed);
        string mistakeString = "Mistake " + life.ToString();
        if (life == 3)
        {
            mistakeString = "Perfect Game!";
        }

        string popupDescString = string.Format("Difficulty {0}\n{1}\nTime {2}"
            , difficulty.ToString()
            , mistakeString
            , string.Format("{0:00}:{1:00}", math.round(time / 60), math.round(time % 60)));

        if (isNewHighScore)
        {
            popupDescString += "\nNew Record!";
        }

        _popupDesc.text = popupDescString;
        ReloadUIValue();
    }

}
