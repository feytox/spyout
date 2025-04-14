using System.Collections.Generic;
using UnityEngine;

// [DisallowMultipleComponent]
public class Singleton : MonoBehaviour
{
    private static readonly Dictionary<string, Singleton> _singletons = new();

    protected virtual void Init() { } // instead of awake

    void Awake()
    {
        var isExist = _singletons.TryGetValue(GetType().Name, out var singleton);
        Debug.Assert(
            !isExist || singleton == null,
            $"{gameObject.name} tried to awake {GetType().Name} second time!"
        );
        _singletons[GetType().Name] = this;
        Init();
    }

    protected static Singleton GetInstance<T>()
    {
        var singleton = _singletons[nameof(T)];
        Debug.Assert(singleton != null, $"Tried to access {nameof(T)} before it was initialized!");
        return singleton;
    }
}
