using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public int InventorySize = 8;
    
    public Inventory Inventory { get; private set; }

    void Start()
    {
        Inventory = new Inventory(InventorySize);
        OnStart();
    }

    protected virtual void OnStart() {}
}