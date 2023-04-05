using System;
using System.Collections.Generic;
using IdleRPG.Scripts.Data;
using IdleRPG.Scripts.Runtime.Interfaces;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Commands
{
    public class LightingSkillCommand : ISkillCommand
    {
        public event Action OnSuccess;

        private IMultipleTargetProvider _provider;
        private ITargetProvider _startProvider;
        private SkillLightingParametersData _data;

        public float CooldownTime => _data.Cooldown;
    
        public LightingSkillCommand(IMultipleTargetProvider provider, ITargetProvider startProvider, SkillLightingParametersData data)
        {
            _provider = provider;
            _startProvider = startProvider;
            _data = data;
        }
    
        public void Execute()
        {
            var exceptions = new List<IHittable>();
            var currentSource = _startProvider.GetTarget();
            
            int currentStep;
            for (currentStep = 0; currentStep < _data.StepsAmount; currentStep++)
            {
                currentSource = _provider.GetClosestToPosition(currentSource.Target.position, _data.Range, exceptions);
                if (currentSource is null)
                {
                    break;
                }

                exceptions.Add(currentSource);
                currentSource.ReceiveHit(_data.DamageOnStep(currentStep));
            }

            if (currentStep > 0)
            {
                OnSuccess?.Invoke();
            }
        }
    }
}
