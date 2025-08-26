using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    static AudioSource audioSource;
    static SoundEffectLibrary soundEffectLibrary;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        soundEffectLibrary = GetComponent<SoundEffectLibrary>();
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

}
