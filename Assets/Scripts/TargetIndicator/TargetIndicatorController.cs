using JetBrains.Annotations;
using UnityEngine;

public class TargetIndicatorController : MonoBehaviour
{
    [SerializeField] private Waypoint _targetPoint;
    [SerializeField] [CanBeNull] private PlayerTriggerCollider _disableTrigger;
    [SerializeField] private TargetIndicatorIcon _indicatorIcon;
    [SerializeField] [CanBeNull] private TargetIndicatorController _nextTarget;
    [SerializeField] private bool _disableOnVisible;

    private Vector3 _targetPosition;
    private Transform _playerTransform;
    private Camera _camera;

    private void Start()
    {
        _targetPosition = _targetPoint.Position;
        
        var player = PlayerController.GetInstance();
        _playerTransform = player.transform;
        _camera = player.Camera;

        if (_disableTrigger is not null)
            _disableTrigger.OnPlayerEnter += DisableIndicator;
    }

    private void DisableIndicator()
    {
        _nextTarget?.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        var playerPos = _playerTransform.position;
        var directionVec = _targetPosition - playerPos;
        var ray = new Ray(playerPos, directionVec);

        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        var targetDistance = directionVec.magnitude;
        var minDistance = targetDistance;

        foreach (var plane in planes)
            if (plane.Raycast(ray, out var distance) && distance < minDistance)
                minDistance = distance;

        var worldPos = ray.GetPoint(minDistance);
        var screenPos = _camera.WorldToScreenPoint(worldPos);
        var isTargetVisible = Mathf.Approximately(minDistance, targetDistance);
        
        _indicatorIcon.SetVisible(!isTargetVisible);
        _indicatorIcon.SetPosition(directionVec, screenPos);
        
        if (isTargetVisible && _disableOnVisible)
            DisableIndicator();
    }
}