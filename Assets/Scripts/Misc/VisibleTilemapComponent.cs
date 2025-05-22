using UnityEngine;

public class VisibleTilemapComponent : MonoBehaviour
{
    [SerializeField] private bool _canSeeThroughTiles = true;

    public bool CanSeeThroughTiles => _canSeeThroughTiles;
}