using JetBrains.Annotations;
using UnityEngine;

public class NPCInventoryController : InventoryController
{
    [SerializeField] [CanBeNull] 
    private Item _defaultItem;

    void Start()
    {
        if (_defaultItem is null)
            return;

        var stack = new ItemStack(_defaultItem);
        Inventory?.SetStack(stack, 0);
    }
}