using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    protected abstract void Init(); // instead of awake

    protected static T _singleton;

    void Awake()
    {
        Debug.Assert(
            _singleton == null,
            $"{gameObject.name} tried to awake {typeof(T).Name} second time!"
        );
        _singleton = this as T;
        Init();
    }

    protected static T GetInstance()
    {
        Debug.Assert(
            _singleton != null,
            $"Tried to access {typeof(T).Name} before it was initialized!"
        );
        return _singleton;
    }
}
