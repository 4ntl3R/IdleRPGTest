using System;
using System.Collections.Generic;
using System.Linq;
using IdleRPG.Scripts.Data;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Views;

namespace IdleRPG.Scripts.Runtime.Commands
{
    public class LightingSkillCommand : ISkillCommand
    {
        public event Action OnSuccess;

        private IMultipleTargetProvider _provider;
        private ITargetProvider _startProvider;
        private SkillLightingParametersData _data;
        private LightingView _lighting;

        public float CooldownTime => _data.Cooldown;
    
        public LightingSkillCommand(IMultipleTargetProvider provider, ITargetProvider startProvider, 
            SkillLightingParametersData data, LightingView lightingView)
        {
            _provider = provider;
            _startProvider = startProvider;
            _data = data;
            _lighting = lightingView;
        }
    
        public void Execute()
        {
            var currentSource = _startProvider.GetTarget();
            var stepPoints = new List<IHittable>{currentSource};
            
            int currentStep;
            for (currentStep = 0; currentStep < _data.StepsAmount; currentStep++)
            {
                currentSource = _provider.GetClosestToPosition(currentSource.Target.position, _data.Range, stepPoints);
                if (currentSource is null)
                {
                    break;
                }

                stepPoints.Add(currentSource);
                currentSource.ReceiveHit(_data.DamageOnStep(currentStep));
            }

            if (currentStep <= 0)
            {
                return;
            }
            
            OnSuccess?.Invoke();
            _lighting.Show(stepPoints.Select(x => x.Target.position).ToArray());
        }
    }
}
