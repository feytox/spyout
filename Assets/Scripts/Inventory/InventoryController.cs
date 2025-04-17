using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public int InventorySize = 8;
    
    public Inventory Inventory { get; private set; }

    private void Start() => Inventory = new Inventory(InventorySize);
}