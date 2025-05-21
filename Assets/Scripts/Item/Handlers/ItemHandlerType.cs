using System;

public enum ItemHandlerType : byte
{
    None = 0,
    Weapon = 1,
    Meat = 2,
    Wine = 3
}

public static class ItemHandlerTypeExt
{
    public static ItemHandler CreateHandler(this ItemHandlerType handlerType, ItemStack stack)
    {
        return handlerType switch
        {
            ItemHandlerType.None => null,
            ItemHandlerType.Weapon => new WeaponItemHandler(stack),
            ItemHandlerType.Meat => new FoodItemHandler(stack),
            ItemHandlerType.Wine => new FoodItemHandler(stack),
            _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
        };
    }
}
