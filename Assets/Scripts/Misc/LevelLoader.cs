using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator _fadeAnimator;

    [SerializeField]
    private float _transitionTime;
    
    private static readonly int StartTrigger = Animator.StringToHash("Start");

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelAsync(sceneName));
    }
    
    private async Awaitable LoadLevelAsync(string sceneName)
    {
        _fadeAnimator.SetTrigger(StartTrigger);
        await Awaitable.WaitForSecondsAsync(_transitionTime);
        await SceneManager.LoadSceneAsync(sceneName);
    }
}