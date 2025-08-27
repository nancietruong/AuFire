using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    static AudioSource audioSource;
    static SoundEffectLibrary soundEffectLibrary;

    public Slider sfxSlider;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        soundEffectLibrary = GetComponent<SoundEffectLibrary>();
    }

    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void PlaySound(TypeOfSoundEffect type)
    {
        AudioClip clip = soundEffectLibrary.GetRandomClip(type);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"No sound found for: {type}");
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
