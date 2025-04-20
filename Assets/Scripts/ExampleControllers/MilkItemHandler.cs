using JetBrains.Annotations;
using UnityEngine;

public class MilkItemHandler : ItemHandler
{
    public MilkItemHandler([NotNull] ItemStack stack) : base(stack)
    {
    }

    public override bool UseItem()
    {
        Debug.Log($"Молока у тебя {Stack.Count} штук!");
        return false;
    }
}