#nullable enable
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class CustomSpriteComponent : MonoBehaviour // TODO: maybe use better solution, idk
{
    protected abstract Sprite? Sprite { get; }
    protected abstract Material? Material { get; }

    private SpriteRenderer? _renderer;

    void OnEnable() => RefreshSprite();

    void OnValidate() => RefreshSprite();

    protected virtual void BeforeSpriteRefresh()
    {
    }

    public void RefreshSprite()
    {
        BeforeSpriteRefresh();
        if (Sprite == null)
            return;
        
        var spriteRenderer = GetRenderer();
        spriteRenderer.sprite = Sprite;
        spriteRenderer.material = Material;
    }

    private SpriteRenderer GetRenderer()
    {
        if (_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();
        Debug.Assert(_renderer != null);
        return _renderer!;
    }
}