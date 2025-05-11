using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PopupController : MonoBehaviour
{
    private static readonly int Visible = Animator.StringToHash("Visible");
    
    private Animator _animator;

    void Start() => _animator = GetComponent<Animator>();

    public void EnablePopup() => _animator.SetBool(Visible, true);

    public void DisablePopup() => _animator.SetBool(Visible, false);
}