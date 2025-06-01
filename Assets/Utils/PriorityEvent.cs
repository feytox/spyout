using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// Представляет систему событий, которая выполняет обработчики в порядке приоритета.
    /// </summary>
    /// <typeparam name="T">Тип данных, передаваемых обработчикам событий.</typeparam>
    public class PriorityEvent<T>
    {
        /// <summary>
        /// Отсортированный список обработчиков событий с их приоритетами.
        /// </summary>
        private readonly List<Listener> _listeners = new();

        /// <summary>
        /// Подписывает обработчик на событие с указанным приоритетом.
        /// </summary>
        /// <param name="priority">Приоритет. Чем больше, тем позже будет выполнен</param>
        /// <param name="listener">Функция, которая обрабатывает данные события и возвращает true,
        /// если нужно прервать остальные события</param>
        public void Subscribe(int priority, Func<T, bool> listener)
        {
            var item = new Listener(priority, listener);
            var index = ~_listeners.BinarySearch(item);
            _listeners.Insert(index, item);
        }

        /// <summary>
        /// Выполняет все обработчики событий в порядке возрастания приоритета.
        /// Выполнение останавливается, если какой-либо обработчик возвращает false.
        /// </summary>
        /// <param name="data">Данные, передаваемые каждому обработчику события.</param>
        public void ExecuteEvents(T data)
        {
             foreach (var listener in _listeners)
                if (listener.Execute(data))
                    return;
        }

        private class Listener : IComparable<Listener>
        {
            private readonly int _priority;
            private readonly Func<T, bool> _func;

            public Listener(int priority, Func<T, bool> func)
            {
                _priority = priority;
                _func = func;
            }

            public bool Execute(T data) => _func(data);

            public int CompareTo(Listener other)
            {
                if (ReferenceEquals(this, other)) 
                    return 0;
                return other is null ? 1 : _priority.CompareTo(other._priority);
            }
        }
    }
}