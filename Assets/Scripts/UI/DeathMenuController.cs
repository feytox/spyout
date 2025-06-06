using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DeathMenuController : MonoBehaviour
{
    private static readonly int Visible = Animator.StringToHash("Visible");

    [SerializeField] private CheckpointManager _checkpointManager;

    private Animator _animator;

    private void Start() => _animator = GetComponent<Animator>();

    public void TriggerDeath()
    {
        _animator.SetBool(Visible, true);
        CustomCursor.Visible = true;
    }

    public void ReloadGame()
    {
        _animator.SetBool(Visible, false);
        _checkpointManager.ReloadGame();
        CustomCursor.Visible = false;
    }
}