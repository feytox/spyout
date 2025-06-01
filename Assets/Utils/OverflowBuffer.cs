using System;
using UnityEngine;

// TODO: maybe use something else
/// <summary>
/// Самописный зацикленный буфер c расширенным функционалом.
/// Для простоты при удалении объекты остаются в буфере, поэтому и должны быть структурой.
/// </summary>
/// <typeparam name="T"></typeparam>
public class OverflowBuffer<T> where T : struct
{
    public int Count { get; private set; }

    private const int InitialCapacity = 4;

    private int _head;
    private int _capacity = InitialCapacity;
    private T[] _array = new T[InitialCapacity];

    /// <summary>
    /// Уменьшает размер буфера до указанного значения.
    /// </summary>
    /// <param name="newSize">Новый размер буфера.</param>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если <paramref name="newSize"/> меньше нуля.</exception>
    public void Trim(int newSize)
    {
        if (newSize < 0)
            throw new ArgumentOutOfRangeException(nameof(newSize), "Размер не может быть меньше нуля.");
        
        if (newSize >= Count)
            return;

        Count = newSize;
    }
    
    /// <summary>
    /// Пытается удалить и вернуть элемент из начала буфера.
    /// </summary>
    /// <param name="result">Удаленный элемент или значение по умолчанию, если буфер пуст.</param>
    /// <returns><c>true</c>, если элемент был успешно удален; иначе <c>false</c>.</returns>
    public bool TryDequeue(out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = Dequeue();
        return true;
    }
    
    /// <summary>
    /// Удаляет и возвращает элемент из начала буфера.
    /// </summary>
    /// <returns>Элемент из начала буфера.</returns>
    /// <exception cref="IndexOutOfRangeException">Выбрасывается, буфер пуст.</exception>
    public T Dequeue()
    {
        if (Count == 0)
            throw new IndexOutOfRangeException();

        var result = _array[_head];
        _head = (_head + 1) % _capacity;
        Count--;
        return result;
    }

    /// <summary>
    /// Добавляет элемент в конец буфера. При необходимости увеличивает емкость.
    /// </summary>
    /// <param name="item">Элемент для добавления.</param>
    public void Enqueue(T item)
    {
        ResizeIfNeeded(Count + 1);
        var index = (_head + Count - 1) % _capacity;
        _array[index] = item;
    }

    /// <summary>
    /// Добавляет коллекцию элементов в конец буфера. При необходимости увеличивает емкость.
    /// </summary>
    /// <param name="items">Массив элементов для добавления.</param>
    public void EnqueueRange(T[] items)
    {
        var prevSize = Count;
        ResizeIfNeeded(Count + items.Length);

        var index = (_head + prevSize) % _capacity;
        foreach (var item in items)
        {
            _array[index] = item;
            index = (index + 1) % _capacity;
        }
    }
    
    /// <summary>
    /// Пытается вернуть элемент из начала буфера без его удаления.
    /// </summary>
    /// <param name="result">Элемент из начала буфера или значение по умолчанию, если буфер пуст.</param>
    /// <returns><c>true</c>, если элемент был успешно получен; иначе <c>false</c>.</returns>
    public bool TryPeek(out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = _array[_head];
        return true;
    }

    /// <summary>
    /// Возвращает элемент из начала буфера без его удаления.
    /// </summary>
    /// <returns>Элемент из начала буфера.</returns>
    /// <exception cref="IndexOutOfRangeException">Выбрасывается, если буфер пуст.</exception>
    public T Peek()
    {
        if (Count == 0)
            throw new IndexOutOfRangeException();

        return _array[_head];
    }
    
    /// <summary>
    /// Пытается вернуть последний элемент без его удаления.
    /// </summary>
    /// <param name="result">Последний элемент буфера или значение по умолчанию, если буфер пуст.</param>
    /// <returns><c>true</c>, если элемент был успешно получен; иначе <c>false</c>.</returns>
    public bool TryPeekLast(out T result)
    {
        if (Count == 0)
        {
            result = default;
            return false;
        }

        result = _array[(_head + Count - 1) % _capacity];
        return true;
    }

    public bool TryGetOrLast(int index, out T result)
    {
        index = Mathf.Min(index, Count - 1);
        if (index < 0)
        {
            result = default;
            return false;
        }
        
        result = _array[(_head + index) % _capacity];
        return true;
    }

    /// <summary>
    /// Пытается вернуть последний элемент без его удаления.
    /// </summary>
    /// <returns>Последний элемент буфера.</returns>
    /// <exception cref="IndexOutOfRangeException">Выбрасывается, если буфер пуст.</exception>
    public T PeekLast()
    {
        if (Count == 0)
            throw new IndexOutOfRangeException();

        return _array[(_head + Count - 1) % _capacity];
    }
    
    /// <summary>
    /// Очищает буфер, устанавливая его размер в 0.
    /// </summary>
    public void Clear() => Trim(0);

    private void ResizeIfNeeded(int newSize)
    {
        if (newSize < _capacity)
        {
            Count = newSize;
            return;
        }

        var newCapacity = Math.Max(newSize, _capacity + _capacity / 2);
        var newArray = new T[newCapacity];
        for (var i = 0; i < Count; i++)
        {
            var index = (_head + i) % _capacity;
            newArray[i] = _array[index];
        }

        _head = 0;
        Count = newSize;
        _capacity = newArray.Length;
        _array = newArray;
    }
}