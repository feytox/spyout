using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class SlotComponent : MonoBehaviour
{
    [SerializeField]
    private Image _itemRenderer;
    private Image _selectImage;
    
    void Start()
    {
        _selectImage = GetComponent<Image>();
    }

    public void OnSlotUpdate([CanBeNull] ItemStack stack)
    {
        if (stack is null)
        {
            _itemRenderer.sprite = null;
            _itemRenderer.enabled = false;
            return;
        }

        _itemRenderer.sprite = stack.Item.Sprite;
        _itemRenderer.enabled = true;
    }

    public void ChangeSlotStatus(bool status) => _selectImage.enabled = status;
}
