using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator _fadeAnimator;

    [SerializeField]
    private float _transitionTime;
    
    private static readonly int StartTrigger = Animator.StringToHash("Start");

    public void LoadLevel(string sceneName) => LoadLevel(sceneName, _transitionTime);

    public void LoadLevel(string sceneName, float transitionTime)
    {
        StartCoroutine(LoadLevelAsync(sceneName, transitionTime));
    }
    
    private async Awaitable LoadLevelAsync(string sceneName, float transitionTime)
    {
        _fadeAnimator.SetTrigger(StartTrigger);
        await Awaitable.WaitForSecondsAsync(transitionTime);
        await SceneManager.LoadSceneAsync(sceneName);
    }
}