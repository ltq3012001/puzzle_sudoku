using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : MonoBehaviour
{
    [SerializeField] private Sprite _muteSound;
    [SerializeField] private Sprite _unmuteSound;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _removeAdsButton;
    [SerializeField] private Button _helpButton;

    [SerializeField] private Image _layoutGroup;
    public void Initialize(MainMenuManager manager, ThemeColor themeColor, bool isMute)
    {
        _closeButton.onClick.AddListener(manager.OptionPopup_ClosePopup);
        _soundButton.onClick.AddListener(manager.OptionPopup_OnSoundButtonPressed);
        _removeAdsButton.onClick.AddListener(manager.OptionPopup_OnRemoveAdButtonPressed);
        _helpButton.onClick.AddListener(manager.OptionPopup_OnHelMeButtonPressed);

        ChangeSoundSprite(isMute);

        _closeButton.GetComponentInChildren<Image>().color = themeColor.ButtonColor;
        _soundButton.GetComponentInChildren<Image>().color = themeColor.ButtonColor;
        _removeAdsButton.GetComponentInChildren<Image>().color = themeColor.ButtonColor;
        _helpButton.GetComponentInChildren<Image>().color = themeColor.ButtonColor;

        _soundButton.GetComponentInChildren<TMP_Text>().color = themeColor.ButtonColor;
        _removeAdsButton.GetComponentInChildren<TMP_Text>().color = themeColor.ButtonColor;
        _helpButton.GetComponentInChildren<TMP_Text>().color = themeColor.ButtonColor;

        this.GetComponent<Image>().color = themeColor.ButtonColor;
        _layoutGroup.color = themeColor.ThemeMainColor;

        //ReloadUIValue();
    }

    public void ChangeSoundSprite(bool isMute)
    {
        if (isMute)
        {
            _soundButton.GetComponentInChildren<Image>().sprite = _muteSound;
            _soundButton.GetComponentInChildren<TMP_Text>().text = "Mute";

        }
        else
        {
            _soundButton.GetComponentInChildren<Image>().sprite = _unmuteSound;
            _soundButton.GetComponentInChildren<TMP_Text>().text = "Unmute";
        }
    }
}
