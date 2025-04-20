#nullable enable
using System;

public abstract class ItemHandler
{
    protected readonly ItemStack Stack;

    protected ItemHandler(ItemStack stack) => Stack = stack;

    public abstract bool UseItem();
    
    public static ItemHandler? CreateHandler(ItemStack stack, ItemHandlerType handlerType)
    {
        return handlerType switch
        {
            ItemHandlerType.None => null,
            ItemHandlerType.Milk => new MilkItemHandler(stack),
            ItemHandlerType.Axe => new AxeItemHandler(stack),
            _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
        };
    }
}