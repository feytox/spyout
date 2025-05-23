using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class ItemRenderer : MonoBehaviour
{
    [SerializeField] private bool _displayCount;
    [SerializeField] private TextMeshProUGUI _text;

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
            _text.enabled = false;
            return;
        }

        _image.sprite = stack.Item.Sprite;
        _image.enabled = true;
        _text.text = stack.Count.ToString();
        _text.enabled = _displayCount && stack.Count > 1;
    }
}