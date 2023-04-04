using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [CreateAssetMenu(order = 0, fileName = "New EnemyDataBundle", menuName = "Idle RPG/Character Data/Enemy Data Bundle")]
    public class EnemyDataBundle : CharacterDataBundle
    {
        [SerializeField] 
        private GameObject enemyPrefab;

        public GameObject EnemyPrefab => enemyPrefab;
    }
}
