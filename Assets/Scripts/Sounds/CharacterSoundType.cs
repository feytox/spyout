using System;
using UnityEngine;

public enum CharacterSoundType : byte
{
    Idle = 0,
    Death = 1,
    Attack = 2
}

[Serializable]
public class CharacterSoundStorage
{
    [SerializeField] public CharacterSoundType SoundType;

    [SerializeField] public AudioClip[] Clips;
}