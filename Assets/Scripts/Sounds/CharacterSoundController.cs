using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class CharacterSoundController : MonoBehaviour
{
    [SerializeField] private CharacterSoundStorage[] _sounds;
    [SerializeField] private float _idleSoundCooldown = 10f;
    [SerializeField] private float _footstepsLength = 0.5f;

    private readonly Dictionary<CharacterSoundType, AudioClip[]> _soundClips = new();
    private Cooldown _footstepsCooldown;
    private Cooldown _idleCooldown;
    private GridController _grid;
    private AudioSource _footstepsSource;
    
    void Start()
    {
        ParseSoundStorages();
        _grid = GridController.GetInstance();
        _footstepsSource = GetComponentInChildren<AudioSource>();
        _footstepsCooldown = new Cooldown(_footstepsLength);
        _idleCooldown = new Cooldown(_idleSoundCooldown);
    }

    public void PlaySound(CharacterSoundType soundType)
    {
        if (!_soundClips.TryGetValue(soundType, out var clips))
            return;
        
        PlayRandomSound(clips);
    }

    public void PlayRandomSound(AudioClip[] clips) => SoundFXManager.Instance.PlayRandomSound(clips, transform);

    public void UpdateIdleSound()
    {
        if (!_idleCooldown.ResetIfExpired())
            return;
        
        PlaySound(CharacterSoundType.Idle);
    }
    
    public void UpdateMovement(Vector3 currentPos, Vector3 moveVec)
    {
        if (_footstepsCooldown.ResetIfExpired())
            OnMove(currentPos, moveVec);
    }

    private void OnMove(Vector3 currentPos, Vector3 moveVec)
    {
        if (_footstepsSource.isPlaying || moveVec == Vector3.zero)
            return;
        
        var clips = _grid.GetFootstepSound(currentPos);
        if (clips is null || clips.Length == 0)
            return;
        
        var soundIndex = Random.Range(0, clips.Length);
        _footstepsSource.PlayOneShot(clips[soundIndex]);
    }
    
    private void ParseSoundStorages()
    {
        foreach (var storage in _sounds)
        {
            if (_soundClips.ContainsKey(storage.SoundType))
                Debug.LogError($"Duplicate sound type {storage.SoundType}");
            
            Debug.Assert(storage.Clips.Length != 0, $"0 AudioClips of {storage.SoundType}");
            _soundClips[storage.SoundType] = storage.Clips;
        }
    }
}