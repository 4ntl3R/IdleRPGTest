using System;
using System.Collections.Generic;
using IdleRPG.Scripts.Data;
using IdleRPG.Scripts.Runtime.Controllers;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Managers;
using IdleRPG.Scripts.Runtime.Models;
using UnityEngine;
using UnityEngine.UI;

namespace IdleRPG.Scripts.Runtime.Views
{
    public class EnemyView : MonoBehaviour, IHittable, IMovable, IDeathReceiver
    {
        public event Action<float> OnHitReceived;
        public event Action<Vector3, Vector3> OnResetPositions;

        [SerializeField] 
        private Slider healthBar;

        [SerializeField] 
        private Transform visualization;
        
        //todo: switch to local variables
        private MovingController _movingController;
        private EnemyAttackController _enemyAttackController;
        private HealthController _healthController;

        private MovingModel _movingModel;
        private CharacterParametersModel _characterParametersModel;

        private IIterator _damageIterator;
        private IIterator _movingIterator;
        private ITargetProvider _targetProvider;

        private List<IDisposable> _disposables;

        public Transform Target { get; private set; }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        public void Inject(ITargetProvider targetProvider, CharacterParametersData data)
        {
            _targetProvider = targetProvider;

            _damageIterator = new SingleCoroutineManager(this);
            _movingIterator = new SingleCoroutineManager(this);
            
            Target = transform;
            _characterParametersModel = new CharacterParametersModel(data);
            _movingModel = new MovingModel(data.MovementSpeed);
            
            _movingController = new MovingController(_movingModel, this, _movingIterator);
            _enemyAttackController = new EnemyAttackController(_movingModel, _characterParametersModel, _damageIterator, _targetProvider);
            _healthController = new HealthController(healthBar, this, _characterParametersModel, 
                new List<IDeathReceiver>{_movingController, _enemyAttackController});

            _disposables = new List<IDisposable>
                {_movingController, _enemyAttackController, _healthController};
        }

        public void ResetEnemy(Vector3 position)
        {
            OnResetPositions?.Invoke(position, _targetProvider.GetTarget().Target.transform.position);
        }

        public void ReceiveHit(float hitPower)
        {
            OnHitReceived?.Invoke(hitPower);
        }
        
        public void MoveToPosition(Vector3 position)
        {
            Target.position = position;
        }

        public void LookAt(Vector3 target)
        {
            visualization.LookAt(target);
        }

        public void ResolveDeath()
        {
            Debug.Log("dead");
        }
    }
}
