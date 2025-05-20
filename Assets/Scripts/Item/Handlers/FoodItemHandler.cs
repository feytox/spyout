using JetBrains.Annotations;

public class FoodItemHandler : ItemHandler
{
    private readonly float _healAmount;
    
    public FoodItemHandler([NotNull] ItemStack stack, float healAmount) : base(stack)
    {
        _healAmount = healAmount;
    }

    public override bool UseItem(ICharacter character)
    {
        character.Health?.Heal(_healAmount);
        Stack.Decrement();
        return true;
    }
}