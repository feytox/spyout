using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class ItemCounterComponent : MonoBehaviour
{
    [SerializeField] private Item _targetItem;
    [SerializeField] private int _targetCount;

    private ItemRenderer _itemRenderer;
    private TextMeshProUGUI _text;
    private int _count;

    void Start()
    {
        Debug.Assert(_targetItem.ItemType == ItemType.Collectable, "Предмет для счётчика должен быть Collectable");
        
        _itemRenderer = GetComponentInChildren<ItemRenderer>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _itemRenderer.UpdateItem(new ItemStack(_targetItem));
        
        PlayerController.GetInstance().Inventory.Inventory.OnCollectItem += OnCollectItem;
        OnCountUpdate();
    }

    private void OnCollectItem(ItemStack stack)
    {
        if (stack.Item != _targetItem)
            return;

        _count += stack.Count;
        OnCountUpdate();
    }

    private void OnCountUpdate()
    {
        _text.text = $"{_count}/{_targetCount}";
    }
}