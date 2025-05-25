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
    [SerializeField] private Image _image;

    public Image Image => _image;

    public void UpdateItem([CanBeNull] ItemStack stack)
    {
        if (stack is null)
        {
            Image.sprite = null;
            Image.enabled = false;
            _text.enabled = false;
            return;
        }

        Image.sprite = stack.Item.Sprite;
        Image.enabled = true;
        _text.text = stack.Count.ToString();
        _text.enabled = _displayCount && stack.Count > 1;
    }
}