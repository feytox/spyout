using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingComponent : MonoBehaviour
{
    [SerializeField] private SoundMixerManager _mixerManager;
    [SerializeField] private VolumeGroup _volumeGroup;

    private Slider _volumeSlider;

    private void Start()
    {
        _volumeSlider = GetComponentInChildren<Slider>();
        SetCurrentVolume();
    }

    public void SetVolume(float percent)
    {
        switch (_volumeGroup)
        {
            case VolumeGroup.Master:
                _mixerManager.SetMasterVolume(percent);
                break;
            case VolumeGroup.Music:
                _mixerManager.SetMusicVolume(percent);
                break;
            case VolumeGroup.SoundFX:
                _mixerManager.SetSoundFXVolume(percent);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetCurrentVolume()
    {
        var percent = _volumeGroup switch
        {
            VolumeGroup.Master => _mixerManager.MasterVolume,
            VolumeGroup.Music => _mixerManager.MusicVolume,
            VolumeGroup.SoundFX => _mixerManager.SoundFXVolume,
            _ => throw new ArgumentOutOfRangeException()
        };

        _volumeSlider.value = percent;
    }
}

public enum VolumeGroup : byte
{
    Master = 0,
    Music = 1,
    SoundFX = 2
}