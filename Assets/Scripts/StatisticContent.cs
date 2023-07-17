using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticContent : MonoBehaviour
{
    [SerializeField] private TMP_Text _contentTitle;
    [SerializeField] private TMP_Text _descTitle;
    [SerializeField] private TMP_Text _desc;

    public void Initialize(Generator.DifficultyLevel difficultyLevel, int gamePlayed, int gameWon, int winRate, int perfectGame, string time, ThemeColor themeColor)
    {
        _contentTitle.text = difficultyLevel.ToString();
        string descString = string.Format("{0}\n{1}\n{2}\n{3}\n{4}"
            , gamePlayed
            , gameWon
            , string.Format("{0}%", winRate)
            , perfectGame
            , time);
        _desc.text = descString;
        _contentTitle.color = themeColor.ButtonColor;
        _desc.color = themeColor.ButtonColor;
        _descTitle.color = themeColor.ButtonColor;
    }
}
