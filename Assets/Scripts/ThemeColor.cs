using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ColorTheme", menuName = "Data/ColorTheme", order = 1)]

public class ThemeColor : ScriptableObject
{
    [Header("General")]
    public Color ThemeMainColor;
    public Color ButtonColor;

    [Header("Main Menu")]
    public Color ContentBackground;
    public Color SelectedStateColor;


    [Header("Game Scene")]

    //CellColor
    public Color CellColor;
    public Color SelectedCellColor;
    public Color HighlightCorrectCellColor;
    public Color WrongCellColor;

    //TextColor
    public Color LockedTextColor;
    public Color UnlockedTextColor;
    public Color WrongTextColor;
    public Color NoteTextColor;
    public Color HideNoteTextColor;

    //ButtonColor
    public Color NotiBackgroundColor;
    public Color NotiBoxColor;
    public Color DisableNotiColor;

}
