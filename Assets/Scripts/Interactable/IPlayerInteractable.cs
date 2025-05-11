using JetBrains.Annotations;

public interface IPlayerInteractable : IInteractable
{
    [CanBeNull] protected PopupController Popup { get; }
    
    void IInteractable.OnInteractionEnter() => Popup?.EnablePopup();

    void IInteractable.OnInteractionExit() => Popup?.DisablePopup();
}