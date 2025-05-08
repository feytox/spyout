using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class ItemRenderer : MonoBehaviour
{
    private Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    public void UpdateItem([CanBeNull] ItemStack stack)
    {
        if (stack is null)
        {
            _image.sprite = null;
            _image.enabled = false;
            return;
        }

        _image.sprite = stack.Item.Sprite;
        _image.enabled = true;
    }
}