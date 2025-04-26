#nullable enable
using System;
using System.Collections.Generic;

public static class LinqExtensions
{

    
    // from new c#
    public static TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
        MaxBy(source, keySelector, null);

    // from new c#
    public static TSource? MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer)
    {
        comparer ??= Comparer<TKey>.Default;

        using var e = source.GetEnumerator();

        if (!e.MoveNext())
            if (default(TSource) is null)
                return default;

        var value = e.Current;
        var key = keySelector(value);

        if (default(TKey) is null)
        {
            while (key == null)
            {
                if (!e.MoveNext())
                {
                    return value;
                }

                value = e.Current;
                key = keySelector(value);
            }

            while (e.MoveNext())
            {
                var nextValue = e.Current;
                var nextKey = keySelector(nextValue);
                if (nextKey == null || comparer.Compare(nextKey, key) <= 0)
                    continue;
                key = nextKey;
                value = nextValue;
            }
        }
        else
        {
            if (comparer == Comparer<TKey>.Default)
            {
                while (e.MoveNext())
                {
                    var nextValue = e.Current;
                    var nextKey = keySelector(nextValue);
                    if (Comparer<TKey>.Default.Compare(nextKey, key) <= 0)
                        continue;
                    key = nextKey;
                    value = nextValue;
                }
            }
            else
            {
                while (e.MoveNext())
                {
                    var nextValue = e.Current;
                    var nextKey = keySelector(nextValue);
                    if (comparer.Compare(nextKey, key) <= 0)
                        continue;

                    key = nextKey;
                    value = nextValue;
                }
            }
        }

        return value;
    }
    
    // from new c#
    public static TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
        MinBy(source, keySelector, comparer: null);

    // from new c#
    public static TSource? MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
        IComparer<TKey>? comparer)
    {
        comparer ??= Comparer<TKey>.Default;

        using var e = source.GetEnumerator();

        if (!e.MoveNext())
            if (default(TSource) is null)
                return default;

        var value = e.Current;
        var key = keySelector(value);

        if (default(TKey) is null)
        {
            while (key == null)
            {
                if (!e.MoveNext())
                {
                    return value;
                }

                value = e.Current;
                key = keySelector(value);
            }

            while (e.MoveNext())
            {
                var nextValue = e.Current;
                var nextKey = keySelector(nextValue);
                if (nextKey == null || comparer.Compare(nextKey, key) >= 0) 
                    continue;
                
                key = nextKey;
                value = nextValue;
            }
        }
        else
        {
            if (comparer == Comparer<TKey>.Default)
            {
                while (e.MoveNext())
                {
                    var nextValue = e.Current;
                    var nextKey = keySelector(nextValue);
                    if (Comparer<TKey>.Default.Compare(nextKey, key) >= 0) 
                        continue;
                    
                    key = nextKey;
                    value = nextValue;
                }
            }
            else
            {
                while (e.MoveNext())
                {
                    var nextValue = e.Current;
                    var nextKey = keySelector(nextValue);
                    if (comparer.Compare(nextKey, key) >= 0) 
                        continue;
                    
                    key = nextKey;
                    value = nextValue;
                }
            }
        }

        return value;
    }
}