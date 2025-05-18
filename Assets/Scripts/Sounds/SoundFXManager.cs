using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundFXObject;
    
    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(SoundFXManager)} second time!"
        );
        _singleton = this;
    }

    public void PlayRandomSound(AudioClip[] clips, Transform spawnTransform)
    {
        if (clips.Length == 0)
            return;
        
        var soundIndex = Random.Range(0, clips.Length);
        PlaySound(clips[soundIndex], spawnTransform);
    }

    private void PlaySound(AudioClip clip, Transform spawnTransform)
    {
        var source = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity, transform);
        source.clip = clip;
        source.volume = 1.0f;
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }

    #region Singleton

    private static SoundFXManager _singleton;

    public static SoundFXManager Instance
    {
        get
        {
            Debug.Assert(
                _singleton != null,
                $"Tried to access {nameof(SoundFXManager)} before it was initialized!"
            );
            return _singleton;
        }
    }

    #endregion
}