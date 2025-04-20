using JetBrains.Annotations;
using UnityEngine;

public class AxeItemHandler : ItemHandler
{
    public AxeItemHandler([NotNull] ItemStack stack) : base(stack)
    {
    }

    public override bool UseItem()
    {
        Debug.Log("топорр");
        return false;
    }
}