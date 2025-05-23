using UnityEngine;

public class DropOnDeathComponent : MonoBehaviour
{
    [SerializeField] private GroundItem _itemPrefab;

    [SerializeField, Range(0, Item.DefaultMaxCount), Tooltip("Кастомное количество. 0 = оставить из префаба")]
    private int _customCount = -1;

    public void OnDeath(float deathTime) => StartCoroutine(SpawnDrop(deathTime));

    private async Awaitable SpawnDrop(float delay)
    {
        await Awaitable.WaitForSecondsAsync(delay);
        var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity);
        if (_customCount == 0 || item.Stack is null)
            return;

        item.Stack.Count = _customCount;
    }
}