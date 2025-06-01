using UnityEngine;

public class TargetIndicatorIcon : MonoBehaviour
{
    private static readonly int Visible = Animator.StringToHash("Visible");

    [SerializeField] private Animator _iconAnimator;

    private bool _visible;

    public void SetVisible(bool state)
    {
        if (_visible == state)
            return;

        _visible = state;
        _iconAnimator.SetBool(Visible, state);
    }

    public void SetPosition(Vector3 directionVec, Vector3 screenPosition)
    {
        transform.position = screenPosition;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, directionVec);
    }
}