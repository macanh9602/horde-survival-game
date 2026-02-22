
using UnityEngine;

namespace DucDevGame
{
    public class UnitDragView : MonoBehaviour, IDraggable
    {
        [Header("Drag Feel")]
        [SerializeField] private float dragSmoothTime = 0.05f;
        [SerializeField] private float dropSmoothTime = 0.1f;
        [SerializeField] private float pickUpHeight = 0.45f;
        [SerializeField] private float scaleMultiplier = 1.08f;
        [SerializeField] private float tiltStrength = 6f;
        [SerializeField] private float tiltSpeed = 12f;

        private Vector3 _originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _velocity;
        private Vector3 _originalScale;
        private bool _isDragging;
        private float _currentHeight;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public Transform GetTransform()
        {
            return this.transform;
        }

        public void OnDragStart()
        {
            _isDragging = true;
            _originalPosition = transform.position;
            _targetPosition = _originalPosition;
            transform.localScale = _originalScale * scaleMultiplier;
            _velocity = Vector3.zero;
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            _targetPosition = worldPos;
        }

        public void OnDrop(Vector3 finalPos)
        {
            _isDragging = false;
            _targetPosition = finalPos;
            transform.localScale = _originalScale;
        }

        private void Update()
        {
            if (!_isDragging) return;
            float targetHeight = _isDragging ? pickUpHeight : 0f;
            _currentHeight = Mathf.Lerp(_currentHeight, targetHeight, Time.deltaTime * 12f);

            Vector3 targetPosWithHeight = _targetPosition + Vector3.up * _currentHeight;
            float smoothTime = _isDragging ? dragSmoothTime : dropSmoothTime;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosWithHeight,
                ref _velocity,
                smoothTime
            );

        }

        public void ResetPosition()
        {
            _isDragging = false;
            transform.position = _originalPosition;
            _targetPosition = _originalPosition;
            _velocity = Vector3.zero;
            transform.localScale = _originalScale;
        }
    }
}