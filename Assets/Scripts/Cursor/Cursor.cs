using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    [SerializeField] private bool visible = true;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        Cursor.visible = visible;
    }
}