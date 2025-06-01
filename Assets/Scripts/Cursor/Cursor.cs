using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private bool visible = true;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Cursor.visible = visible;
    }
}