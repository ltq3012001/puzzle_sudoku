using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgSprite;
    [SerializeField] private TMPro.TMP_Text _valueText;

    private ThemeColor _themeColor;

    public int Value;
    public int Row;
    public int Col;
    public bool IsLocked;
    public bool IsIncorrect;

    public void Init(int value, ThemeColor themeColor)
    {
        _themeColor = themeColor;
        IsIncorrect = false;
        Value = value;
        _bgSprite.color = _themeColor.CorrectCellColor;
        if (value == 0)
        {
            IsLocked = false;
            _valueText.color = _themeColor.UnlockedTextColor;
            _valueText.text = "";
        }
        else
        {
            IsLocked = true;
            _valueText.color = _themeColor.LockedTextColor;
            _valueText.text = Value.ToString();
        }
    }

    public void Init(int value, bool isIncorrect, bool isLocked, ThemeColor themeColor)
    {
        _themeColor = themeColor;
        Value = value;
        IsIncorrect = isIncorrect;
        IsLocked = isLocked;

        if (value == 0)
        {
            IsLocked = false;
            _bgSprite.color = _themeColor.CorrectCellColor;
            _valueText.color = _themeColor.UnlockedTextColor;
            _valueText.text = "";
        }
        else
        {
            Reset();
            _valueText.text = Value.ToString();
        }
    }

    public void Highlight()
    {
        _bgSprite.color = _themeColor.HighlightColor;
        ResetTextColor();
    }

    public void Select()
    {
        if (IsIncorrect)
        {
            _bgSprite.color = _themeColor.SelectedWrongCellColor;
        }
        else
        {
            _bgSprite.color = _themeColor.SelectedCorrectCellColor;
        }
        ResetTextColor();
    }

    public void Reset()
    {
        if (IsIncorrect)
        {
            _bgSprite.color = _themeColor.WrongCellColor;
        }
        else
        {
            _bgSprite.color = _themeColor.CorrectCellColor;
        }
        ResetTextColor();
    }

    private void ResetTextColor()
    {
        if (IsIncorrect)
        {
            _valueText.color = _themeColor.WrongTextColor;
        }
        else
        {
            if (IsLocked)
            {
                _valueText.color = _themeColor.LockedTextColor;
            }
            else
            {
                _valueText.color = _themeColor.UnlockedTextColor;
            }
        }
    }

    public void UpdateValue(int value)
    {
        Value = value;
        _valueText.text = Value == 0 ? "" : Value.ToString();
    }

    public void UpdateWin()
    {
        _bgSprite.color = _themeColor.CorrectCellColor;
        _valueText.color = _themeColor.LockedTextColor;
    }
}
