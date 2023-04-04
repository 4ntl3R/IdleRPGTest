using System;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Models
{
    public class MovingModel
    {
        public event Action OnTargetReached;
        public event Action<Vector3> OnPositionChanged;
        
        private Vector3 _targetPosition;
        private Vector3 _currentPosition;
        private Vector3 _movementVector;
        private readonly float _speed;
        private readonly float _targetReachRange;

        public MovingModel(float speed, float targetReachRange = 0.5f)
        {
            _speed = speed;
            _targetReachRange = targetReachRange;
        }

        public void SetPositions(Vector3 start, Vector3 end)
        {
            _targetPosition = end;
            _currentPosition = start;
            UpdatePosition();
            CheckForTargetReach();
        }

        public void MovingIteration(float deltaTime)
        {
            _movementVector = (_targetPosition - _currentPosition).normalized;
            _currentPosition += _movementVector * (_speed * deltaTime);
            UpdatePosition();
            CheckForTargetReach();
        }

        private void CheckForTargetReach()
        {
            if (Vector2.Distance(_targetPosition, _currentPosition) < _targetReachRange)
            {
                OnTargetReached?.Invoke();
            }
        }

        private void UpdatePosition()
        {
            OnPositionChanged?.Invoke(_currentPosition);
        }
    }
}
