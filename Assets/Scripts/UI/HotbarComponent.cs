using JetBrains.Annotations;
using UnityEngine;

public class HotbarComponent : MonoBehaviour
{
    private InventoryController _playerInventory;
    private SlotComponent[] _slots;

    private void Start()
    {
        _playerInventory = PlayerController.GetInstance().Inventory;
        _slots = GetComponentsInChildren<SlotComponent>();

        InitSlots();
        _playerInventory.Inventory!.OnSlotUpdated += OnSlotUpdate;
        _playerInventory.OnActiveSlotChange += OnActiveSlotChange;
    }

    private void OnDestroy()
    {
        _playerInventory.Inventory!.OnSlotUpdated -= OnSlotUpdate;
        _playerInventory.OnActiveSlotChange -= OnActiveSlotChange;
    }

    private void InitSlots()
    {
        _slots[_playerInventory.ActiveSlot].ChangeSlotStatus(true);

        for (var slot = 0; slot < _playerInventory.InventorySize; slot++)
            _slots[slot].OnSlotUpdate(_playerInventory.Inventory[slot]);
    }

    private void OnActiveSlotChange(int prevSlot, int slot)
    {
        _slots[prevSlot].ChangeSlotStatus(false);
        _slots[slot].ChangeSlotStatus(true);
    }

    private void OnSlotUpdate(int slot, [CanBeNull] ItemStack stack)
    {
        _slots[slot].OnSlotUpdate(stack);
    }
}