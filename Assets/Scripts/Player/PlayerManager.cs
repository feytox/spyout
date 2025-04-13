using System.Runtime.CompilerServices;
using UnityEngine;

// To use this template, replace all occurrences of "CLASSNAME" with your class name

[DisallowMultipleComponent]
public class CLASSNAME : MonoBehaviour
{
    public static PlayerController PlayerController { get; private set; }
    public static CameraController CameraController { get; private set; }
    private int field = 1000;
    public static int Field
    {
        get => GetInstance().field;
        set => GetInstance().field = value;
    }

    public static void Method()
    {
        var instance = GetInstance();
        // Your code goes here, use instance to avoid unnecessary null rechecks
    }

    void Awake()
    {
        Debug.Assert(
            singleton == null,
            $"{gameObject.name} tried to awake {typeof(CLASSNAME).Name} second time!"
        );
        singleton = this;

        PlayerController = GetComponentInChildren<PlayerController>();
        CameraController = GetComponentInChildren<CameraController>();

        Debug.Assert(PlayerController != null);
        Debug.Assert(CameraController != null);
    }

    private static CLASSNAME singleton;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static CLASSNAME GetInstance() // Use it everywhere
    {
        Debug.Assert(
            singleton != null,
            $"Tried to access {typeof(CLASSNAME).Name} before it was initialized!"
        );
        return singleton;
    }
}
