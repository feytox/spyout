using TMPro;
using UnityEngine;

public class EndMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Start()
    {
        var counters = GetComponentsInChildren<ItemCounterComponent>();
        foreach (var counter in counters)
            counter.ForceShowItemIcon();
    }

    public void SetResult(float time, int score)
    {
        var floorTime = (int)time;
        var seconds = (floorTime % 60).ToString().PadRight(2, '0');
        var minutes = floorTime / 60;
        _timeText.text = $"{minutes}:{seconds}";
        _scoreText.text = score.ToString();
    }
}