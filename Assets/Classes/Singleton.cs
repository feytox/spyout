using UnityEngine;

namespace Classes
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        // instead of awake
        // does not have default implementation intentionally
        protected abstract void Init();

        protected static T _singleton;

        protected static T GetInstance()
        {
            Debug.Assert(
                _singleton != null,
                $"Tried to access {typeof(T).Name} before it was initialized!"
            );
            return _singleton;
        }

        void Awake()
        {
            Debug.Assert(
                _singleton == null,
                $"{gameObject.name} tried to awake {typeof(T).Name} second time!"
            );
            _singleton = this as T;
            Init();
        }
    }
}
