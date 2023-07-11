using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgSprite;
    [SerializeField] private TMPro.TMP_Text _valueText;
    [SerializeField] private TMPro.TMP_Text[] _noteValueText;

    private ThemeColor _themeColor;

    private List<int> noteValues;
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
        noteValues = new List<int>();
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

    public void Init(int value, bool isIncorrect, bool isLocked, List<int> noteValueList, ThemeColor themeColor)
    {
        _themeColor = themeColor;
        Value = value;
        IsIncorrect = isIncorrect;
        IsLocked = isLocked;
        _bgSprite.color = _themeColor.CellColor;
        noteValues = new List<int>(noteValueList);
        if (value == 0)
        {
            IsLocked = false;
            _valueText.color = _themeColor.UnlockedTextColor;
            _valueText.text = "";
            if (noteValues.Count > 0)
            {
                foreach (int noteValue in noteValues.ToList())
                {
                    ShowNoteValue(noteValue);
                }
            }
        }
        else
        {
            Reset();
            _valueText.text = Value.ToString();
        }
    }

    public void Highlight()
    {
        if (IsIncorrect)
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

    private void ShowNoteValue(int value)
    {
        
        _noteValueText[value - 1].color= _themeColor.NoteTextColor;
    }

    private void HideNoteValue(int value)
    {
        
        _noteValueText[value - 1].color = _themeColor.HideNoteTextColor;
    }

    public void UpdateValue(int value)
    {
        Value = value;
        if(Value != 0)
        {
            _valueText.text = Value.ToString();
            if(noteValues.Count> 0)
            {
                foreach (int noteValue in noteValues.ToString())
                {
                    HideNoteValue(noteValue);
                }
            }
        }
        else
        {
            _valueText.text = "";
            if (noteValues.Count > 0)
            {
                foreach (int noteValue in noteValues.ToString())
                {
                    ShowNoteValue(noteValue);
                }
            }
        }
    }

    public void UpdateNoteValue(int value)
    {
        if (noteValues.Contains(value))
        {
            noteValues.Remove(value);
            HideNoteValue(value);
        }
        else
        {
            noteValues.Add(value);
            ShowNoteValue(value);
        }
    }

    public void UpdateWin()
    {
        _bgSprite.color = _themeColor.CellColor;
        _valueText.color = _themeColor.LockedTextColor;
    }

    public List<int> GetNoteValue()
    {
        return noteValues;
    }
}
