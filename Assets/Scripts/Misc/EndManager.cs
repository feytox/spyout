using System.Linq;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    private const int DeathAntiScore = 10;
    private static readonly int GameEnd = Animator.StringToHash("GameEnd");

    [SerializeField] private Animator _fadeAnimator;
    [SerializeField] private float _beforeFadeDelay = 1.5f;
    [SerializeField] private float _endLength = 3.0f;
    [SerializeField] private EndMenuController _endMenu;

    private float _startTime;
    private float? _endTime;
    private int _deaths;

    void Start() => _startTime = Time.time;
    
    public void IncrementDeath() => _deaths++;
    
    public void StopTimer()
    {
        Debug.Assert(_endTime == null);
        _endTime = Time.time;
    }

    public void ScheduleEnd() => StartCoroutine(ExecuteGameEnd());

    private async Awaitable ExecuteGameEnd()
    {
        if (_endTime is null)
            throw new System.ArgumentNullException($"No time measured");
        
        await Awaitable.WaitForSecondsAsync(_beforeFadeDelay);
        _fadeAnimator.SetTrigger(GameEnd);

        await Awaitable.WaitForSecondsAsync(_endLength);

        var score = CountScore();
        _endMenu.gameObject.SetActive(true);
        _endMenu.SetResult(_endTime.Value - _startTime, score, _deaths);
    }

    private int CountScore()
    {
        var score = PlayerController.GetInstance().Inventory.Inventory.CollectableItems
            .Select(pair => pair.Key.CollectableScore * pair.Value)
            .Sum();

        return Mathf.Max(0, score - _deaths * DeathAntiScore);
    }
}