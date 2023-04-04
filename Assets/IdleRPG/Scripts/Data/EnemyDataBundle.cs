using IdleRPG.Scripts.Data.Enums;
using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [CreateAssetMenu(order = 0, fileName = "New EnemyDataBundle", menuName = "Idle RPG/Character Data/Enemy Data Bundle")]
    public class EnemyDataBundle : CharacterDataBundle
    {
        [SerializeField] 
        private GameObject enemyPrefab;

        [SerializeField]
        private EnemyType enemyType;

        public GameObject EnemyPrefab => enemyPrefab;

        public EnemyType EnemyType => enemyType;
    }
}
