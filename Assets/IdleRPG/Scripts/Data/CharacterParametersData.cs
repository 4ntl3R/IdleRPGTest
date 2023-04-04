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
    }
}
