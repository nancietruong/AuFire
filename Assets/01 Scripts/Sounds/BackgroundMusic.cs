using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : Singleton<BackgroundMusic>
{
    AudioSource audioSource;
    public AudioClip backgroundMusic;
    public Slider BGMSlider;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }

        BGMSlider.onValueChanged.AddListener(delegate { SetVolume(BGMSlider.value); });
    }


    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip clip = null)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else if (audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public void PauseBackgroundMusic()
    {
        audioSource.Pause();
    }
}
