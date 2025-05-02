#nullable enable
using UnityEngine;

public class NPCInventoryController : InventoryController
{
    [SerializeField] private Item? _defaultItem;
    
    protected override void OnStart()
    {
        if (_defaultItem is null)
            return;
        
        var stack = new ItemStack(_defaultItem);
        Inventory?.SetStack(stack, 0);
    }
}