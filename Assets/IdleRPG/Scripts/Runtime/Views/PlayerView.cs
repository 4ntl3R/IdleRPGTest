using System;
using System.Collections.Generic;
using IdleRPG.Scripts.Data;
using IdleRPG.Scripts.Runtime.Controllers;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Managers;
using IdleRPG.Scripts.Runtime.Models;
using IdleRPG.Scripts.Runtime.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace IdleRPG.Scripts.Runtime.Views
{
    public class PlayerView : MonoBehaviour, IHittable, ITargetProvider
    {
        public event Action<float> OnHitReceived;
        public event Action OnHealthReset;
        
        [SerializeField] 
        private Slider healthBar;

        [SerializeField] 
        private EnemySpawner enemySpawner;

        [SerializeField] 
        private PlayerDataBundle playerDataBundle;

        [SerializeField] 
        private ProjectileSpawner projectileSpawner;

        [SerializeField] 
        private LevelManager levelManager;

        private PlayerAttackController _playerAttackController;
        private HealthController _healthController;

        private CharacterParametersModel _parametersModel;

        private IIterator _damageIterator;

        private List<IDisposable> _disposables;
        
        
        public Transform Target => transform;

        private void Awake()
        {
            ManageDependencies();

        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
        
        public void ReceiveHit(float hitPower)
        {
            OnHitReceived?.Invoke(hitPower);
        }

        public IHittable GetTarget() => this;

        public void ResetState()
        {
            OnHealthReset?.Invoke();
        }
        
        private void ManageDependencies()
        {
            _parametersModel = new CharacterParametersModel(playerDataBundle.ParametersData);
            _damageIterator = new SingleCoroutineManager(this);
            _playerAttackController = new PlayerAttackController(_parametersModel, Target.position, _damageIterator, 
                enemySpawner, projectileSpawner);
            _healthController = new HealthController(healthBar, this, _parametersModel, 
                new List<IDeathReceiver>{levelManager});
            _disposables = new List<IDisposable>{_playerAttackController, _healthController};
        }
    }
}
