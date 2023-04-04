using System;
using IdleRPG.Scripts.Data;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Models
{
    public class CharacterParametersModel
    {
        public const float FloatPrecision = 0.0001f;
        private const float Second = 1f;
        
        public event Action<float, float, float> OnHealthChange;
        public event Action OnFullDamage;

        private CharacterParametersData _parametersData;
        private float _currentDamage;

        public float AttackRange => _parametersData.AttackRange;
        
        public float AttackSpeed => Second / _parametersData.AttackPerSecond;

        public float AttackDamage => _parametersData.AttackDamage;

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
            _currentDamage -= changeValue;
            _currentDamage = Mathf.Max(0, _currentDamage);
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
