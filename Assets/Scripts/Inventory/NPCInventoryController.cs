using JetBrains.Annotations;
using UnityEngine;

public class NpcInventoryController : InventoryController
{
    [SerializeField] [CanBeNull] private Item _defaultItem;

    private void Start()
    {
        if (_defaultItem is null)
            return;

        var stack = new ItemStack(_defaultItem);
        Inventory?.SetStack(stack, 0);
    }
}