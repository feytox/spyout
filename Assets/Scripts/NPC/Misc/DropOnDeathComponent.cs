using UnityEngine;

public class DropOnDeathComponent : MonoBehaviour
{
    [SerializeField] private GroundItem _emptyItemPrefab;
    [SerializeField] private Item _item;

    [SerializeField, Range(1, Item.DefaultMaxCount)]
    private int _customCount;

    public void OnDeath(float deathTime) => StartCoroutine(SpawnDrop(deathTime));

    private async Awaitable SpawnDrop(float delay)
    {
        await Awaitable.WaitForSecondsAsync(delay);
        var item = Instantiate(_emptyItemPrefab, transform.position, Quaternion.identity);
        item.Stack = new ItemStack(_item, _customCount);
        item.RefreshSprite();
    }
}