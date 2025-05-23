#nullable enable
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class CustomSpriteComponent : MonoBehaviour // TODO: maybe use better solution, idk
{
    protected abstract Sprite? Sprite { get; }

    private SpriteRenderer? _renderer;

    void OnEnable() => RefreshSprite();

    void OnValidate() => RefreshSprite();

    protected virtual void BeforeSpriteRefresh()
    {
    }

    private void RefreshSprite()
    {
        BeforeSpriteRefresh();
        if (Sprite != null)
            GetRenderer().sprite = Sprite;
    }

    private SpriteRenderer GetRenderer()
    {
        if (_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();
        Debug.Assert(_renderer != null);
        return _renderer!;
    }
}