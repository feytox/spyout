using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemCounterComponent : MonoBehaviour
{
    [SerializeField] private Item _targetItem;
    [SerializeField] private int _targetCount;
    [SerializeField] private Image _checkMark;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private ItemRenderer _itemRenderer;
    
    private int _count;

    void Start()
    {
        Debug.Assert(_targetItem.ItemType == ItemType.Collectable, "Предмет для счётчика должен быть Collectable");
        
        _itemRenderer.UpdateItem(new ItemStack(_targetItem));
        PlayerController.GetInstance().Inventory.Inventory.OnCollectableItemChange += OnItemCountChange;
        OnCountUpdate();
    }

    public void ForceShowItemIcon() => _itemRenderer.Image.material = null;

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
        if (_count > 0)
            _itemRenderer.Image.material = null;
    }
}