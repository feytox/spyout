using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class SlotComponent : MonoBehaviour
{
    [SerializeField] private Image _selectImage;
    [SerializeField] private ItemRenderer _itemRenderer;

    public void OnSlotUpdate([CanBeNull] ItemStack stack) => _itemRenderer.UpdateItem(stack);

    public void ChangeSlotStatus(bool status) => _selectImage.enabled = status;
}