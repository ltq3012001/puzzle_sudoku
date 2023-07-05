using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int Value;
    public int Row;
    public int Col;
    public bool IsLocked;
    public bool IsIncorrect;

    //   

    [SerializeField] private SpriteRenderer _bgSprite;
    [SerializeField] private TMPro.TMP_Text _valueText;

    [Header("Color")]
    [Space]
    [Header("Start Color")]

    [SerializeField] private Color _startUnlockedCellColor;
    [SerializeField] private Color _startUnlockedTextColor;
    [SerializeField] private Color _startLockedCellColor;
    [SerializeField] private Color _startLockedTextColor;

    [Header("Highlight Color")]

    [SerializeField] private Color _highlightUnlockedCellColor;
    [SerializeField] private Color _highlightUnlockedTextColor;
    [SerializeField] private Color _highlightLockedCellColor;
    [SerializeField] private Color _highlightLockedTextColor;
    [SerializeField] private Color _highlightWrongCellColor;
    [SerializeField] private Color _highlightWrongTextColor;

    [Header("Selected Color")]

    [SerializeField] private Color _selectedCellColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _selectedWrongCellColor;
    [SerializeField] private Color _selectedWrongTextColor;

    [Header("Reset Color")]

    [SerializeField] private Color _resetCellColor;
    [SerializeField] private Color _resetTextColor;
    [SerializeField] private Color _resetWrongCellColor;
    [SerializeField] private Color _resetWrongTextColor;

    public void Init(int value)
    {
        IsIncorrect = false;
        Value = value;

        if(value == 0)
        {
            IsLocked = false;
            _bgSprite.color = _startUnlockedCellColor;
            _valueText.color = _startUnlockedTextColor;
            _valueText.text = "";
        }
        else
        {
            IsLocked = true;
            _bgSprite.color = _startLockedCellColor;
            _valueText.color = _startLockedTextColor;
            _valueText.text = Value.ToString();
        }
    }

    public void Highlight()
    {
        if (IsLocked)
        {
            _bgSprite.color = _highlightLockedCellColor;
            _valueText.color = _highlightLockedTextColor;
        }
        else
        {
            if(IsIncorrect)
            {
                _bgSprite.color= _highlightWrongCellColor;
                _valueText.color= _highlightWrongTextColor;
            }
            else
            {
                _bgSprite.color = _highlightUnlockedCellColor;
                _valueText.color = _highlightUnlockedTextColor;
            }
        }
    }

    public void Select()
    {
        if(!IsIncorrect)
        {
            _bgSprite.color=_selectedCellColor;
            _valueText.color= _selectedTextColor;
        }
        else
        {
            _bgSprite.color = _selectedWrongCellColor;
            _valueText.color = _selectedWrongTextColor;
        }
    }

    public void Reset()
    {
        if (IsLocked)
        {
            _bgSprite.color = _startLockedCellColor;
            _valueText.color = _startLockedTextColor;
        }
        else
        {
            if (IsIncorrect)
            {
                _bgSprite.color = _resetWrongCellColor;
                _valueText.color = _resetWrongTextColor;
            }
            else
            {
                _bgSprite.color = _resetCellColor;
                _valueText.color = _resetTextColor;
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
        _bgSprite.color = _startLockedCellColor;
        _valueText.color = _startLockedTextColor;
    }
}
