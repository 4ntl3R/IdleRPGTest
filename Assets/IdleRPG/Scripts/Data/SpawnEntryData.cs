using System;
using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [Serializable]
    public struct SpawnEntryData
    {
        [SerializeField] 
        private EnemyDataBundle _enemyData;

        [SerializeField, Range(0, 1)] 
        private float _spawnProbabilityWeight;

        public EnemyDataBundle EnemyData => _enemyData;

        public float SpawnProbabilityWeight => _spawnProbabilityWeight;
    }
}
