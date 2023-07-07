using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorTheme", menuName = "Data/ColorTheme", order = 1)]

public class ThemeColor : ScriptableObject
{
    //CellColor
    public Color CellColor;
    public Color SelectedCellColor;
    public Color HighlightCorrectCellColor;
    public Color WrongCellColor;

    //TextColor
    public Color LockedTextColor;
    public Color UnlockedTextColor;
    public Color WrongTextColor;

}
