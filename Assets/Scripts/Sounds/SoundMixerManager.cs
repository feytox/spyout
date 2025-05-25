using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    public float MasterVolume => GetVolume("masterVolume");
    public float MusicVolume => GetVolume("musicVolume");
    public float SoundFXVolume => GetVolume("soundFXVolume");

    public void SetMasterVolume(float percent) => SetVolume("masterVolume", percent);

    public void SetMusicVolume(float percent) => SetVolume("musicVolume", percent);

    public void SetSoundFXVolume(float percent) => SetVolume("soundFXVolume", percent);

    private void SetVolume(string group, float percent)
    {
        _mixer.SetFloat(group, Mathf.Log10(percent) * 20f);
    }

    private float GetVolume(string group)
    {
        if (_mixer.GetFloat(group, out var volume))
            return Mathf.Pow(10, volume / 20f);

        Debug.LogError($"Unknown sound type {group}");
        return 0;
    }
}