using JetBrains.Annotations;

public class FoodItemHandler : ItemHandler
{
    
    public FoodItemHandler([NotNull] ItemStack stack) : base(stack)
    {
    }

    public override bool UseItem(ICharacter character)
    {
        character.Health?.Heal(Stack.EffectiveHealAmount);
        Stack.Decrement();
        return true;
    }
}
