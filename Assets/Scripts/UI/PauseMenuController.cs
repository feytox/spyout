using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private LevelLoader _levelLoader;
    
    public void QuitToMainMenu()
    {
        ResumeGame();
        _levelLoader.LoadLevel("MainMenu");
        CustomCursor.Visible = true;
    }

    public void ResumeGame() => _pauseManager.ResumeGame();
}