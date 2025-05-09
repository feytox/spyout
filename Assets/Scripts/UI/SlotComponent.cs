using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class SlotComponent : MonoBehaviour
{
    private Image _selectImage;
    private ItemRenderer _itemRenderer;

    void Start()
    {
        _selectImage = GetComponent<Image>();
        _itemRenderer = GetComponentInChildren<ItemRenderer>();
    }

    public void OnSlotUpdate([CanBeNull] ItemStack stack) => _itemRenderer.UpdateItem(stack);

    public void ChangeSlotStatus(bool status) => _selectImage.enabled = status;
}
