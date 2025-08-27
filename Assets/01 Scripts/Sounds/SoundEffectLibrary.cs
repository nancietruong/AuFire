using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfSoundEffect
{
    Rifle,
    Pistol,
    Shotgun,
    Hurt,
    Death,
    Roll,
    Key,
    UsePotion,
    DoorOpen,
    DoorClose,
    Victory,
}

[System.Serializable]
public struct SoundEffectGroup
{
    public TypeOfSoundEffect type;
    public List<AudioClip> clips;
}

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField]
    private SoundEffectGroup[] soundEffectGroups;
    Dictionary<TypeOfSoundEffect, List<AudioClip>> soundEffectDict;

    private void Awake()
    {
        InitDictionary();
    }

    public void InitDictionary()
    {
        soundEffectDict = new Dictionary<TypeOfSoundEffect, List<AudioClip>>();
        foreach (SoundEffectGroup group in soundEffectGroups)
        {
            soundEffectDict[group.type] = group.clips;
        }
    }

    public AudioClip GetRandomClip(TypeOfSoundEffect type)
    {
        if (soundEffectDict.ContainsKey(type))
        {
            List<AudioClip> clips = soundEffectDict[type];
            if (clips.Count > 0)
            {
                int randomIndex = Random.Range(0, clips.Count);
                return clips[randomIndex];
            }
            else
            {
                Debug.LogWarning($"No clips available for sound effect group: {type}");
            }
        }
        return null;
    }
}
