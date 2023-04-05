using System;
using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [Serializable]
    public struct SkillLightingParametersData
    {
        [SerializeField] 
        private float cooldown;

        [SerializeField] 
        private float range;

        [SerializeField] 
        private float baseDamage;

        [SerializeField] 
        private float stepDamageMultiplier;

        [SerializeField] 
        private int stepsAmount;

        public float Cooldown => cooldown;

        public float Range => range;
        
        public float StepsAmount => stepsAmount;

        public float DamageOnStep(int step) => Mathf.Pow(stepDamageMultiplier, step) * baseDamage;
    }
}
