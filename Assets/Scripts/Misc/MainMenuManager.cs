using UnityEngine;

[DisallowMultipleComponent]
public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private LevelLoader _levelLoader;
    
    public void OnClickPlay() => _levelLoader.LoadLevel("Town");

    public void OnClickQuit() => Application.Quit();
}