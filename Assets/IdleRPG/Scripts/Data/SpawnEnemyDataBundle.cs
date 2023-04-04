using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [CreateAssetMenu(order = 0, fileName = "New Enemy Spawn Schedule", menuName = "Idle RPG/Character Data/Enemy Spawn Schedule")]
    public class SpawnEnemyDataBundle : ScriptableObject
    {
        [SerializeField] 
        private SpawnEntryData[][] spawnEntries;

        [SerializeField]
        private float defaultSpawnDelay;

        [SerializeField] 
        private float minimalSpawnDelay;

        [SerializeField] 
        private float spawnDelayDecrease;

        public SpawnEntryData[][] SpawnEntries => spawnEntries;
        
        public float DefaultSpawnDelay => defaultSpawnDelay;

        public float MinimalSpawnDelay => minimalSpawnDelay;

        public float SpawnDelayDecrease => spawnDelayDecrease;
    }
}
