using JetBrains.Annotations;

public interface IPlayerInteractable : IInteractable
{
    [CanBeNull] protected PopupController Popup { get; }
    protected bool Interacted { get; set; }

    void IInteractable.OnInteractionEnter()
    {
        if (Interacted)
            return;
        Popup?.EnablePopup();
        Interacted = true;
    }

    void IInteractable.OnInteractionExit()
    {
        if (!Interacted)
            return;
        Popup?.DisablePopup();
        Interacted = false;
    }
}