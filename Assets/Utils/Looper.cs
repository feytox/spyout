using UnityEngine;

namespace Utils
{
    public class Looper<T>
    {
        public readonly T[] values;
        private int cursor = 0;

        public Looper(T[] values)
        {
            Debug.Assert(values.Length > 0, "Looper must have at least one value");
            this.values = values;
        }

        public T Current => values[cursor];

        /// <summary>
        /// Moves cursor to the next value and returns it
        /// </summary>
        public T MoveNext() => values[cursor = (cursor + 1) % values.Length];
    }
}
