using Classes;
using UnityEngine;

public class ThetaPather : Singleton<ThetaPather>
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

    protected override void Init()
    {
        DontDestroyOnLoad(gameObject); // Optional

        // Your code goes here
    }
}
