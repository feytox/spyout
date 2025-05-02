using JetBrains.Annotations;

public class AxeItemHandler : ItemHandler
{
    public AxeItemHandler([NotNull] ItemStack stack) : base(stack)
    {
    }

    public override bool UseItem(ICharacter character)
    {
        return true;
    }
}