using JetBrains.Annotations;

public interface IWalker
{
    [CanBeNull] public IDoorPermission DoorPermission { get; }
}