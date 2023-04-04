using System;
using IdleRPG.Scripts.Data.Enums;
using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [Serializable]
    public struct CharacterParametersData
    {
        [SerializeField] 
        private float maxHealth;

        [SerializeField] 
        private float attackDamage;

        [SerializeField] 
        private float attackPerSecond;
        
        [SerializeField] 
        private AttackType attackType;

        [SerializeField] 
        private float attackRange;

        [SerializeField] 
        private float movementSpeed;

        public float MaxHealth => maxHealth;
        
        public float AttackDamage => attackDamage;
        
        public float AttackPerSecond => attackPerSecond;

        public AttackType AttackType => attackType;

        public float AttackRange => attackRange;

        public float MovementSpeed => movementSpeed;
    }
}
