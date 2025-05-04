#nullable enable
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public int InventorySize = 8;
    
    public Inventory? Inventory { get; private set; }

    public virtual ItemStack? ActiveItem => Inventory?[0];

    void Start()
    {
        Inventory = new Inventory(InventorySize);
        OnStart();
    }

    protected virtual void OnStart() {}
}