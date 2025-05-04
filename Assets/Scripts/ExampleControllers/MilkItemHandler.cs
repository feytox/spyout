using JetBrains.Annotations;

public class MilkItemHandler : ItemHandler
{
    public MilkItemHandler([NotNull] ItemStack stack) : base(stack)
    {
    }
    

    public override bool UseItem(ICharacter character)
    {
        return true;
    }
}