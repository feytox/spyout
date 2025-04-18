using UnityEngine;

namespace Classes.Character
{
    [DisallowMultipleComponent]
    public class Manager : MonoBehaviour
    {
        [SerializeField]
        private Appearance _appearance;
        public Appearance Appearance => _appearance;

        [SerializeField]
        private Transform _body;
        public Transform Body => _body;

        // Taken from body
        private Movement _movement;
        public Movement Movement => _movement;

        [SerializeField]
        private Health _health;
        public Health Health => _health;

        [SerializeField]
        private Inventory _inventory;
        public Inventory Inventory => _inventory;

        public Vector2 LocalPosition
        {
            get => _body.localPosition;
            set => _body.localPosition = value;
        }

        public Vector2 Position
        {
            get => _body.position;
            set => _body.position = value;
        }

        void Awake()
        {
            Debug.Assert(_body != null, "Character Manager must have body");
            _movement = _body.GetComponent<Movement>();
        }
    }
}
