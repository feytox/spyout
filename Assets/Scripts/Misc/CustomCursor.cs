using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;

    public static bool Visible
    {
        set => Cursor.visible = value;
    }

    private void Awake() => Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
}