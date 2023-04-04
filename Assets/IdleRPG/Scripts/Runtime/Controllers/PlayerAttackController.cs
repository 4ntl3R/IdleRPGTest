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
        private readonly Vector3 _position;

        public PlayerAttackController(CharacterParametersModel parametersModel, 
            IIterator iterator, ITargetProvider targetProvider, Vector3 position)
        {
            _parametersModel = parametersModel;
            _iterator = iterator;
            _targetProvider = targetProvider;
            
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
                target.ReceiveHit(_parametersModel.AttackDamage);
            }
        }
    }
}