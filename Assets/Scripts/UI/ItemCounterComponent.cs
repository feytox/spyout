using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemCounterComponent : MonoBehaviour
{
    [SerializeField] private Item _targetItem;
    [SerializeField] private int _targetCount;
    [SerializeField] private Image _checkMark;

    private ItemRenderer _itemRenderer;
    private TextMeshProUGUI _text;
    private int _count;

    void Start()
    {
        Debug.Assert(_targetItem.ItemType == ItemType.Collectable, "Предмет для счётчика должен быть Collectable");
        
        _itemRenderer = GetComponentInChildren<ItemRenderer>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _itemRenderer.UpdateItem(new ItemStack(_targetItem));

        PlayerController.GetInstance().Inventory.Inventory.OnCollectableItemChange += OnItemCountChange;
        OnCountUpdate();
    }

    private void OnItemCountChange(Item item, int newCount)
    {
        if (item != _targetItem)
            return;

        _count = newCount;
        OnCountUpdate();
    }

    private void OnCountUpdate()
    {
        _text.text = $"{_count}/{_targetCount}";
        _checkMark.enabled = _count >= _targetCount;
    }
}