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
        var healthController = character.Health;
        if (healthController is null || healthController.IsMaxHealth)
            return false;
        
        healthController.Heal(_healAmount);
        Stack.Decrement();
        return true;
    }
}