using System;
using System.Collections.Generic;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Models;
using UnityEngine.UI;

namespace IdleRPG.Scripts.Runtime.Controllers
{
    public class HealthController: IDisposable
    {
        private readonly Slider _healthView;
        private IHittable _hittable;
        private readonly CharacterParametersModel _parametersModel;
        private readonly List<IDeathReceiver> _deathReceivers;

        public HealthController(Slider healthView, IHittable hittable, 
            CharacterParametersModel parametersModel, List<IDeathReceiver> deathReceivers)
        {
            _healthView = healthView;
            _hittable = hittable;
            _parametersModel = parametersModel;
            _deathReceivers = deathReceivers;
            
            SubscribeEvents();
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _parametersModel.OnHealthChange += ResolveHealthChange;
            _hittable.OnHitReceived += _parametersModel.HitReceive;
            _hittable.OnHealthReset += _parametersModel.ResetParameters;
            foreach (var deathReceiver in _deathReceivers)
            {
                _parametersModel.OnFullDamage += deathReceiver.ResolveDeath;
            }
        }
        
        private void UnsubscribeEvents()
        {
            _parametersModel.OnHealthChange -= ResolveHealthChange;
            _hittable.OnHitReceived -= _parametersModel.HitReceive;
            _hittable.OnHealthReset -= _parametersModel.ResetParameters;
            foreach (var deathReceiver in _deathReceivers)
            {
                _parametersModel.OnFullDamage -= deathReceiver.ResolveDeath;
            }
        }

        private void ResolveHealthChange(float current, float max, float lastChange)
        {
            _healthView.value = current / max;
        }
        
    }
}
