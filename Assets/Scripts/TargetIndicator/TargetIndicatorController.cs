using JetBrains.Annotations;
using UnityEngine;

public class TargetIndicatorController : MonoBehaviour
{
    [SerializeField] private TargetIndicatorIcon _indicatorIcon;
    [SerializeField] [CanBeNull] private PlayerTriggerCollider _disableTrigger;
    [SerializeField] [CanBeNull] private TargetIndicatorController _nextTarget;

    private Transform _playerTransform;
    private Camera _camera;

    void Start()
    {
        var player = PlayerController.GetInstance();
        _playerTransform = player.transform;
        _camera = player.Camera;

        if (_disableTrigger is not null)
            _disableTrigger.OnPlayerEnter += OnDisableTrigger;
    }

    private void OnDisableTrigger()
    {
        _nextTarget?.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        var playerPos = _playerTransform.position;
        var directionVec = transform.position - playerPos;
        var ray = new Ray(playerPos, directionVec);

        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        var targetDistance = directionVec.magnitude;
        var minDistance = targetDistance;

        foreach (var plane in planes)
            if (plane.Raycast(ray, out var distance) && distance < minDistance)
                minDistance = distance;
        
        var worldPos = ray.GetPoint(minDistance);
        var screenPos = _camera.WorldToScreenPoint(worldPos);
        
        _indicatorIcon.SetVisible(!Mathf.Approximately(minDistance, targetDistance));
        _indicatorIcon.SetPosition(directionVec, screenPos);
    }
}