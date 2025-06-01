using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerController))]
public class PlayerDataSaverController : MonoBehaviour
{
    private PlayerController _player;

    private Vector3 _checkpoint;
    private float _health;

    public void Save(Vector3 checkpoint)
    {
        _checkpoint = checkpoint;
        _health = (_player as ICharacter).Health.Health;
    }

    public void Load()
    {
        _player.transform.position = _checkpoint;
        (_player as ICharacter).Health.Health = _health;
    }

    private void Start()
    {
        _player = GetComponent<PlayerController>();
        Save(_player.Position);
    }
}