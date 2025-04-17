#nullable enable
using System;
using System.Collections.Generic;
using Classes.Items;
using UnityEngine;

namespace Classes.Character
{
    public class Inventory : MonoBehaviour
    {
        public int Capacity { get; private set; } = 8; // default
        private readonly Dictionary<int, Item> slots = new(); // no need to init

        // Note: inventory slots must be used as equipment slots
        // maybe add reserved slots count later

        public event Action<Item, int>? OnItemAdded;
        public event Action<Item, int>? OnItemRemoved;

        public Item? this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < Capacity, "Index out of range");
                slots.TryGetValue(index, out var item);
                return item;
            }
            set
            {
                Debug.Assert(index >= 0 && index < Capacity, "Index out of range");
                slots.TryGetValue(index, out var oldItem);
                slots.Remove(index);
                if (oldItem != null)
                    OnItemRemoved?.Invoke(oldItem, index);

                if (value != null)
                {
                    slots[index] = value;
                    OnItemAdded?.Invoke(value, index);
                }
            }
        }

        /// <summary>
        /// Tries to insert item into the first available slot <br />
        /// Complexity: O(n - min_slot)
        /// </summary>
        /// <param name="min_slot">Minimum slot index for insertion. Useful for slots reserving</param>
        /// <returns>Index of the inserted slot, -1 if failed</returns>
        public int TryInsert(Item item, int min_slot = 0)
        {
            for (var i = min_slot; i < Capacity; i++)
            {
                if (this[i] == null)
                {
                    this[i] = item;
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Tries to remove item matching the predicate <br />
        /// Complexity: O(n - min_slot)
        /// </summary>
        /// <param name="min_slot">Minimum slot index for insertion. Useful for slots reserving</param>
        public bool RemoveFirstWhich(Func<Item, bool> predicate, int min_slot = 0)
        {
            var (item, index) = FindFirstWhich(predicate, min_slot);
            if (item != null)
            {
                this[index] = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if contains specified item by name <br />
        /// Complexity: O(n - min_slot)
        /// </summary>
        /// <param name="min_slot">Minimum slot index for insertion. Useful for slots reserving</param>
        public bool Contains(string name, int min_slot = 0) =>
            ContainsWhich(x => x.Name == name, min_slot);

        /// <summary>
        /// Check if any item in the inventory matches the predicate <br />
        /// Complexity: O(n - min_slot)
        /// </summary>
        /// <param name="min_slot">Minimum slot index for insertion. Useful for slots reserving</param>
        public bool ContainsWhich(Func<Item, bool> predicate, int min_slot = 0) =>
            FindFirstWhich(predicate, min_slot).Item1 != null;

        /// <summary>
        /// Returns first (item, index) in the inventory matches the predicate <br />
        /// Complexity: O(n - min_slot)
        /// </summary>
        /// <param name="min_slot">Minimum slot index for insertion. Useful for slots reserving</param>
        /// <returns>Tuple of (item, index) or (null, -1) if not found</returns>
        public (Item?, int) FindFirstWhich(Func<Item, bool> predicate, int min_slot = 0)
        {
            for (var i = min_slot; i < slots.Count; i++)
            {
                var item = this[i];
                if (item != null && predicate(item))
                    return (item, i);
            }

            return (null, -1);
        }

        /// <summary>
        /// Resize inventory to new capacity <br />
        /// All items with index >= newCapacity will be removed
        /// </summary>
        /// <param name="newCapacity"></param>
        public void Resize(int newCapacity)
        {
            Debug.Assert(newCapacity > 0, "Capacity must be greater than 0");
            for (var i = newCapacity; i < Capacity; i++)
                this[i] = null;
            Capacity = newCapacity;
        }
    }
}
