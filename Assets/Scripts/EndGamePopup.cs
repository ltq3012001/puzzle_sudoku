using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePopup : MonoBehaviour
{
    public Generator.DifficultyLevel difficultyLevel;
    [SerializeField] private TMPro.TMP_Text _newGameButtonText;
    public void OnNextLevelPressed()
    {
        if (difficultyLevel == Generator.DifficultyLevel.HARD)
        {
            difficultyLevel = Generator.DifficultyLevel.EASY;
        }else
        {
            difficultyLevel++;
        }
        ReloadUIValue();
    }

    public void OnPreviousLevelPressed()
    {
        if (difficultyLevel == Generator.DifficultyLevel.EASY)
        {
            difficultyLevel = Generator.DifficultyLevel.HARD;
        }
        else
        {
            difficultyLevel--;
        }
        ReloadUIValue();
    }
    public void ReloadUIValue()
    {
        _newGameButtonText.text = string.Format("New Game {0}", difficultyLevel.ToString());
    }
}
