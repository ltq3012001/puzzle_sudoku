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
        _bgSprite.color = _themeColor.CellColor;
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
        _bgSprite.color = _themeColor.CellColor;

        if (value == 0)
        {
            IsLocked = false;
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
        if(IsIncorrect)
        {
            _bgSprite.color = _themeColor.WrongCellColor;
        }
        else
        {
            _bgSprite.color = _themeColor.HighlightCorrectCellColor;
        }
        ResetTextColor();
    }

    public void Select()
    {
        _bgSprite.color = _themeColor.SelectedCellColor;
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
            _bgSprite.color = _themeColor.CellColor;
        }
        ResetTextColor();
    }

    private void ResetTextColor()
    {
        if (IsLocked)
        {
            _valueText.color = _themeColor.LockedTextColor;
        }
        else
        {
            if (IsIncorrect)
            {
                _valueText.color = _themeColor.WrongTextColor;
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
        _bgSprite.color = _themeColor.CellColor;
        _valueText.color = _themeColor.LockedTextColor;
    }
}
