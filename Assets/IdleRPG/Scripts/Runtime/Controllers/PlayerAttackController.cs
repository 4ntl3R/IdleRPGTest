using System;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Models;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Controllers
{
    public class PlayerAttackController : IDisposable, IDeathReceiver
    {
        private readonly CharacterParametersModel _parametersModel;
        private readonly IIterator _iterator;
        private readonly ITargetProvider _targetProvider;
        private readonly ProjectileSpawner _projectileSpawner;
        private readonly Vector3 _position;

        public PlayerAttackController(CharacterParametersModel parametersModel, Vector3 position,
            IIterator iterator, ITargetProvider targetProvider, ProjectileSpawner projectileSpawner)
        {
            _parametersModel = parametersModel;
            _iterator = iterator;
            _targetProvider = targetProvider;
            _position = position;
            _projectileSpawner = projectileSpawner;
            
            _iterator.StartIterating(parametersModel.AttackSpeed);
            
            SubscribeEvents();
        }
    
        public void Dispose()
        {
            UnsubscribeEvents();
        }
        
        public void ResolveDeath()
        {
            _iterator.StopIterating();
        }

        private void SubscribeEvents()
        {
            _iterator.OnIterate += MakeDamage;
            _parametersModel.OnFullDamage += _iterator.StopIterating;
        }

        private void UnsubscribeEvents()
        {
            _iterator.OnIterate -= MakeDamage;
            _parametersModel.OnFullDamage -= _iterator.StopIterating;
        }

        private void MakeDamage(float delay)
        {
            var target = _targetProvider.GetTarget();
            if (!(target is null) && Vector3.Distance(target.Target.position, _position) < _parametersModel.AttackRange)
            {
                _projectileSpawner.Spawn(_position, _targetProvider, _parametersModel.AttackDamage);
            }
        }
    }
}