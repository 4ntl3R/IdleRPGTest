using System;
using IdleRPG.Scripts.Runtime.Controllers;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Managers;
using IdleRPG.Scripts.Runtime.Models;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Views
{
    public class ProjectileView : MonoBehaviour, IMovable
    {
        public event Action<Vector3, Vector3> OnResetPositions;
        public event Action<ProjectileView> OnHit;

        [SerializeField] 
        private Transform visualization;

        [SerializeField]
        private float speed = 10f;

        [SerializeField] 
        private float targetReach = 0.05f;

        private float _damage;
        
        private MovingController _movingController;
        private ProjectileController _projectileController;
        private MovingModel _movingModel;
        private IIterator _movingIterator;
        private IHittable _currentTarget;

        private bool _isInitiated = false;

        private void OnDestroy()
        {
            _movingController.Dispose();
            _projectileController.Dispose();
            _movingIterator.StopIterating();
        }

        public void MoveToPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void LookAt(Vector3 target)
        {
            visualization.up = target - transform.position;
        }

        public void Launch(Vector3 launchPosition, ITargetProvider target, float damage)
        {
            if (!_isInitiated)
            {
                ManageDependencies();
                _isInitiated = true;
            }
            
            _currentTarget = target.GetTarget();
            _damage = damage;
            OnResetPositions?.Invoke(launchPosition, _currentTarget.Target.position);
        }

        public void ResolveHit()
        {
            if (_currentTarget.Target.gameObject.activeInHierarchy)
            {
                _currentTarget.ReceiveHit(_damage);
            }
            OnHit?.Invoke(this);
        }
        
        private void ManageDependencies()
        {
            _movingModel = new MovingModel(speed, targetReach);
            _movingIterator = new SingleCoroutineManager(this);
            
            _projectileController = new ProjectileController(_movingModel, this);
            _movingController = new MovingController(_movingModel, this, _movingIterator);
        }
    }
}
