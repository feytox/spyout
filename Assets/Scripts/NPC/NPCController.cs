using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NPCTaskManager))]
public class NPCController : MonoBehaviour
{   
    [SerializeField] private float _movementSpeed = 4f;
    
    private Rigidbody2D _body;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Перемещает NPC к указанной цели с заданной точностью.
    /// </summary>
    /// <param name="target">Целевая позиция.</param>
    /// <param name="targetSqrAccuracy">Точность в квадрате, с которой NPC должен достичь цели.</param>
    /// <returns>Возвращает true, если цель достигнута, иначе false.</returns>
    public bool MoveToTarget(Vector2 target, float targetSqrAccuracy)
    {
        var moveVec = target - (Vector2)transform.position;
        if (moveVec.sqrMagnitude <= targetSqrAccuracy)
            return true;
        
        MoveInDirection(moveVec);
        return false;
    }
    
    /// <summary>
    /// Перемещает NPC в указанном направлении.
    /// </summary>
    /// <param name="moveVec">Вектор направления движения.</param>
    public void MoveInDirection(Vector2 moveVec) => _body.linearVelocity = moveVec * _movementSpeed;
}