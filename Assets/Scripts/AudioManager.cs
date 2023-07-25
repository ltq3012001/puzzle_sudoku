using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    [SerializeField] private AudioSource _sound;
    private void Awake()
    {
        _sound.volume = 1;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip audio)
    {
        _sound.PlayOneShot(audio);
    }

    public void SoundVolume(bool isMute)
    {
        if (isMute)
        {
            _sound.volume = 0;
        }
        else
        {
            _sound.volume = 1;
        }
    }
}
