using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DeathMenuController : MonoBehaviour
{
    private static readonly int Visible = Animator.StringToHash("Visible");

    private Animator _animator;

    void Start() => _animator = GetComponent<Animator>();

    public void TriggerDeath()
    {
        _animator.SetBool(Visible, true);
    }
}