using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _blur;

    private PlayerInputController _playerInputs;

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {nameof(SoundFXManager)} second time!"
        );
        _singleton = this;
    }

    void Start()
    {
        _playerInputs = PlayerController.Inputs;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        _blur.SetActive(true);
        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        if (_settings.activeSelf)
        {
            _settings.SetActive(false);
            _pauseMenu.SetActive(true);
            return;
        }
        
        Time.timeScale = 1.0f;
        _blur.SetActive(false);
        _pauseMenu.SetActive(false);
        _playerInputs.Unpause();
    }

    #region Singleton

    private static PauseManager _singleton;

    public static PauseManager Instance
    {
        get
        {
            Debug.Assert(
                _singleton != null,
                $"Tried to access {nameof(PauseManager)} before it was initialized!"
            );
            return _singleton;
        }
    }

    #endregion
}