using TMPro;
using UnityEngine;

public class EndMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _deathsText;
    [SerializeField] private AudioSource _endSoundSource;
    [SerializeField] private LevelLoader _levelLoader;

    public void Start()
    {
        _endSoundSource.Play();

        var counters = GetComponentsInChildren<ItemCounterComponent>();
        foreach (var counter in counters)
            counter.ForceShowItemIcon();
    }

    public void SetResult(float time, int score, int deaths)
    {
        var floorTime = (int)time;
        var seconds = (floorTime % 60).ToString().PadLeft(2, '0');
        var minutes = floorTime / 60;
        _timeText.text = $"{minutes}:{seconds}";
        _scoreText.text = score.ToString();
        _deathsText.text = deaths.ToString();
    }

    public void QuitToMainMenu()
    {
        _levelLoader.LoadLevel("MainMenu", 0.3f);
        CustomCursor.Visible = true;
    }
}