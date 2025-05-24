using UnityEngine;

public class SkeletonActivatorComponent : MonoBehaviour
{
    [SerializeField] private NPCController _skeleton;
    [SerializeField] private AudioClip _spawnSound;
    [SerializeField] private GroundItem _trigger;

    void Start()
    {
        _trigger.OnInteract += ActivateSkeleton;
    }

    private void ActivateSkeleton()
    {
        SoundFXManager.Instance.PlaySound(_spawnSound, transform);
        _skeleton.gameObject.SetActive(true);
    }
}