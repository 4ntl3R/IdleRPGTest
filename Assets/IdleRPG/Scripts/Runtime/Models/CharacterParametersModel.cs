using System;
using IdleRPG.Scripts.Data;

namespace IdleRPG.Scripts.Runtime.Models
{
    public class CharacterParametersModel
    {
        public const float FloatPrecision = 0.0001f;
        
        public event Action<float, float, float> OnHealthChange;
        public event Action OnFullDamage;

        private CharacterParametersData _parametersData;
        private float _currentDamage;

        public CharacterParametersModel(CharacterParametersData parametersData)
        {
            _parametersData = parametersData;
            ResetParameters();
        }

        public void ResetParameters()
        {
            _currentDamage = 0;
            ChangeHealth();
        }

        public void HitReceive(float value)
        {
            ChangeHealth(-value);
            CheckFullDamage();
        }

        private void ChangeHealth(float changeValue = 0)
        {
            _currentDamage += changeValue;
            OnHealthChange?.Invoke(_parametersData.MaxHealth - _currentDamage, _parametersData.MaxHealth, changeValue);
        }

        private void CheckFullDamage()
        {
            if (_currentDamage >= _parametersData.MaxHealth - FloatPrecision)
            {
                OnFullDamage?.Invoke();
            }
        }
    }
}
