using Classes;
using Classes.Character;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField]
        private Manager _playerManager;
        private Transform _cameraTransform;
        private Vector3 _offset;

        protected override void Init()
        {
            Debug.Assert(_playerManager != null, "CameraController must have player manager");
        }

        void Start()
        {
            Debug.Assert(_playerManager.Appearance != null, "Player manager must have appearance");
            var appearanceOffset = _playerManager.Appearance.Offset;
            _cameraTransform = transform;
            _offset = _cameraTransform.localPosition;
            _offset.x += appearanceOffset.x;
            _offset.y += appearanceOffset.y;
        }

        void LateUpdate()
        {
            _cameraTransform.localPosition = (Vector3)_playerManager.Position + _offset;
        }
    }
}
