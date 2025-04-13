using System.Runtime.CompilerServices;
using UnityEngine;

// To use this template, replace all occurrences of "ThetaPather" with your class name

[DisallowMultipleComponent]
public class ThetaPather : MonoBehaviour
{
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
            $"{gameObject.name} tried to awake {typeof(ThetaPather).Name} second time!"
        );
        singleton = this;
        // DontDestroyOnLoad(gameObject); // Optional

        // Your code goes here
    }

    private static ThetaPather singleton;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ThetaPather GetInstance() // Use it everywhere
    {
        Debug.Assert(
            singleton != null,
            $"Tried to access {typeof(ThetaPather).Name} before it was initialized!"
        );
        return singleton;
    }
}
