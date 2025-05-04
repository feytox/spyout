using System;

public enum ItemHandlerType : byte
{
    None = 0,
    Weapon = 1,
    
    // examples
    Milk = 98, 
    Axe = 99
}

public static class ItemHandlerTypeExt
{
    public static ItemHandler CreateHandler(this ItemHandlerType handlerType, ItemStack stack)
    {
        return handlerType switch
        {
            ItemHandlerType.None => null,
            ItemHandlerType.Weapon => new WeaponItemHandler(stack),
            ItemHandlerType.Milk => new MilkItemHandler(stack),
            ItemHandlerType.Axe => new AxeItemHandler(stack),
            _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
        };
    }
}