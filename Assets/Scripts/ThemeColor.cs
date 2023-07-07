using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorTheme", menuName = "Data/ColorTheme", order = 1)]

public class ThemeColor : ScriptableObject
{
    //CellColor
    public Color CorrectCellColor;
    public Color WrongCellColor;
    public Color SelectedCorrectCellColor;
    public Color SelectedWrongCellColor;
    public Color HighlightColor;

    //TextColor
    public Color LockedTextColor;
    public Color UnlockedTextColor;
    public Color WrongTextColor;

}
