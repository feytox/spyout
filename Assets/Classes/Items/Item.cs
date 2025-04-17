using System;
using System.Collections.Generic;
using UnityEngine;

namespace Classes.Items
{
    /// <summary>
    /// Base class for all items
    /// Note: may be optimized by using name as a hashcode
    /// </summary>
    public abstract class Item
    {
        public abstract string Name { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract Sprite Icon { get; protected set; }
        public abstract void OnUse(); // not sure how it will work
        // maybe add something like
    }
}
