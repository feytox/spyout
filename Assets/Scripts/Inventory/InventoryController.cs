using System;
using JetBrains.Annotations;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public int InventorySize = 8;

    public Inventory Inventory { get; private set; }
    public int ActiveSlot { get; private set; }
    [CanBeNull] public ItemStack ActiveItem => Inventory?[ActiveSlot];

    public event Action<int, int> OnActiveSlotChange;

    void Awake()
    {
        Inventory = new Inventory(InventorySize);
    }

    protected void SetActiveSlot(int slot)
    {
        if (slot == ActiveSlot)
            return;

        OnActiveSlotChange?.Invoke(ActiveSlot, slot);
        ActiveSlot = slot;
    }
}