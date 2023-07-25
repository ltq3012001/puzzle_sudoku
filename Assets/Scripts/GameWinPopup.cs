using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameWinPopup : EndGamePopup
{
    [SerializeField] private TMP_Text _popupTitle;
    [SerializeField] private TMP_Text _popupDesc;

    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _previousLevelButton;
    [SerializeField] private Button _startNewGameButton;

    [SerializeField] private Image _layoutGroup;

    public void Initialize(GameManager gameManager, Generator.DifficultyLevel difficulty, float time, int life, bool isNewHighScore, ThemeColor themeColor)
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

        _popupTitle.color = themeColor.ButtonColor;
        _popupDesc.color = themeColor.ButtonColor;

        _nextLevelButton.GetComponent<Image>().color = themeColor.ButtonColor;
        _previousLevelButton.GetComponent<Image>().color = themeColor.ButtonColor;

        _startNewGameButton.GetComponent<Image>().color = themeColor.ButtonColor;
        _startNewGameButton.GetComponentInChildren<TMP_Text>().color = themeColor.ThemeMainColor;

        this.GetComponent<Image>().color = themeColor.ButtonColor;
        _layoutGroup.color = themeColor.ThemeMainColor;

        ReloadUIValue();
    }

}
