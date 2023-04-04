using System;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Models;

namespace IdleRPG.Scripts.Runtime.Controllers
{
    public class EnemyAttackController : IDisposable, IDeathReceiver
    {
        private readonly MovingModel _movingModel;
        private readonly CharacterParametersModel _parametersModel;
        private readonly IIterator _iterator;
        private readonly ITargetProvider _targetProvider;

        public EnemyAttackController(MovingModel movingModel, CharacterParametersModel parametersModel, 
            IIterator iterator, ITargetProvider targetProvider)
        {
            _movingModel = movingModel;
            _parametersModel = parametersModel;
            _iterator = iterator;
            _targetProvider = targetProvider;
            
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
            _movingModel.OnTargetReached += StartAttacking;
            _iterator.OnIterate += MakeDamage;
            _parametersModel.OnFullDamage += _iterator.StopIterating;
        }

        private void UnsubscribeEvents()
        {
            _movingModel.OnTargetReached -= StartAttacking;
            _iterator.OnIterate -= MakeDamage;
            _parametersModel.OnFullDamage -= _iterator.StopIterating;
        }

        private void StartAttacking()
        {
            _iterator.StartIterating(_parametersModel.AttackSpeed);
        }

        private void MakeDamage(float delay)
        {
            _targetProvider.GetTarget().ReceiveHit(_parametersModel.AttackDamage);
        }
    }
}
