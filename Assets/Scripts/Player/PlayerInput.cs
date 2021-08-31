using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player {
    public class PlayerInput : MonoBehaviour {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _tankAimMask = 0;
        [SerializeField] private float _tankAimMaxDistance = 100;
        [SerializeField] private bool _axisRaw = true;

        public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnAimTurret = new UnityEvent<Vector2>();
        public UnityEvent OnShoot = new UnityEvent();

        private void Awake()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
        }

        private void Update()
        {
            SetMovementDirection();

            if (IsMouseOverUI()) return;

            SetAimPosition();
            Fire();
        }

        private void SetMovementDirection()
        {
            float moveHorizontal = _axisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
            float moveVertical = _axisRaw ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

            Vector2 moveDirection = new Vector2(moveHorizontal, moveVertical);
            OnMoveBody?.Invoke(moveDirection);
        }

        private void SetAimPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, _tankAimMaxDistance, _tankAimMask);
            if (hit.collider == null) return;

            Vector2 pos = new Vector2(hit.point.x, hit.point.z);
            OnAimTurret?.Invoke(pos);
        }

        private void Fire()
        {
            if (Input.GetMouseButtonDown(0)) {
                OnShoot.Invoke();
            }
        }

        public static bool IsMouseOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}