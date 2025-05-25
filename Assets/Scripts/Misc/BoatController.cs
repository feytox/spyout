using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoatController : MonoBehaviour, IPlayerInteractable
{
    private static readonly int Move = Animator.StringToHash("move");
    
    [SerializeField] private PopupController _popup;
    [SerializeField] private SessionController _sessionController;

    private Animator _animator;
    private bool _isInBoat;
    
    private void Start() => _animator = GetComponent<Animator>();

    private void TriggerGameEnd()
    {
        _sessionController.StopTimer();
        var player = PlayerController.GetInstance();

        _isInBoat = true;
        _popup.DisablePopup();
        LockPlayer(player);
        _animator.SetTrigger(Move);
        _sessionController.ScheduleEnd();
    }

    private void LockPlayer(PlayerController player)
    {
        PlayerController.Inputs.DisablePlayer();
        player.transform.SetParent(transform);
        player.transform.localPosition = Vector3.zero;
    }


    #region IPlayerInteractable

    public event Action OnInteract;

    public void Interact()
    {
        TriggerGameEnd();
        OnInteract?.Invoke();
    }

    public bool CanInteract() => !_isInBoat;

    public Vector3 Position => transform.position;

    PopupController IPlayerInteractable.Popup => _popup;

    bool IPlayerInteractable.Interacted { get; set; }

    #endregion
}