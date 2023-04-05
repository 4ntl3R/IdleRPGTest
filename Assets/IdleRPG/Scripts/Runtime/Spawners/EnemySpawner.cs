using System.Collections.Generic;
using System.Linq;
using IdleRPG.Scripts.Data;
using IdleRPG.Scripts.Data.Enums;
using IdleRPG.Scripts.Runtime.Data;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Managers;
using IdleRPG.Scripts.Runtime.Views;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace IdleRPG.Scripts.Runtime.Spawners
{
    //todo: split enemy spawner to pooling handler and spawner
    public class EnemySpawner : MonoBehaviour, IMultipleTargetProvider
    {
        private static readonly List<IHittable> ZeroExceptions = new List<IHittable>();

        [SerializeField] 
        private List<SpawnPositionRangeData> spawnPositionRanges;

        [SerializeField] 
        private SpawnEnemyDataBundle spawnEnemyDataBundle;

        [SerializeField]
        private PlayerView player;

        private Dictionary<EnemyType, IObjectPool<EnemyView>> _enemyPools;
        private List<EnemyView> _activatedEnemies;

        private EnemyDataBundle _currentEnemyData;
        private int _spawnIndex = 0;
        private float _spawnDelay;
        private IIterator _spawnIterator;

        private void Awake()
        {
            _activatedEnemies = new List<EnemyView>();
            _enemyPools = new Dictionary<EnemyType, IObjectPool<EnemyView>>();
            
            _spawnIterator = new SingleCoroutineManager(this);
            _spawnIterator.OnIterate += Spawn;
            
            _spawnDelay = spawnEnemyDataBundle.DefaultSpawnDelay;
            LaunchSpawner();
        }

        private void OnDestroy()
        {
            _spawnIterator.StopIterating();
            _spawnIterator.OnIterate -= Spawn;
        }
        
        public IHittable GetTarget()
        {
            return GetClosestToPosition(player.Target.position);
        }
        
        public IHittable GetClosestToPosition(Vector3 position, float rangeLimit = float.MaxValue, List<IHittable> exceptions = null)
        {
            exceptions = exceptions ?? ZeroExceptions;
            IHittable result = null;
            var minDistance = rangeLimit;
            foreach (var enemy in _activatedEnemies.Except(exceptions))
            {
                var currentDistance = Vector3.Distance(position, enemy.Target.position);
                if (Vector3.Distance(position, enemy.Target.position) <= minDistance)
                {
                    minDistance = currentDistance;
                    result = enemy;
                }
            }

            return result;
        }

        private void Spawn(float interval)
        {
            _currentEnemyData = spawnEnemyDataBundle.SpawnEntries.GetRandomEnemyData();

            if (!_enemyPools.ContainsKey(_currentEnemyData.EnemyType))
            {
                _enemyPools.Add(_currentEnemyData.EnemyType, CreatePool());
            }

            var enemySpawned = _enemyPools[_currentEnemyData.EnemyType].Get();
            _activatedEnemies.Add(enemySpawned);

            _spawnIndex++;
            if (_spawnIndex >= spawnEnemyDataBundle.SpawnEntries.Length)
            {
                UpdateSpawnLoop();
            }
        }
        
        private void UpdateSpawnLoop()
        {
            _spawnIndex = 0;
            _spawnIterator.StopIterating();
            LaunchSpawner();
        }

        private void LaunchSpawner()
        {
            _spawnIterator.StartIterating(_spawnDelay);
            _spawnDelay = Mathf.Max(_spawnDelay - spawnEnemyDataBundle.SpawnDelayDecrease,
                spawnEnemyDataBundle.MinimalSpawnDelay);
        }
        
        private Vector3 GetSpawnPosition()
        {
            return spawnPositionRanges[Random.Range(0, spawnPositionRanges.Count)].GetRandomPosition;
        }

        private IObjectPool<EnemyView> CreatePool()
        {
            IObjectPool<EnemyView> pool = new ObjectPool<EnemyView>
                (CreateNewEnemy, ActivateEnemy, DeactivateEnemy, DestroyEnemy);

            return pool;
        }

        private EnemyView CreateNewEnemy()
        {
            var instantiatedObject = Instantiate(_currentEnemyData.EnemyPrefab, Vector3.zero, Quaternion.identity);
            var enemyView = instantiatedObject.GetComponent<EnemyView>();
            enemyView.Inject(player, _currentEnemyData);
            enemyView.OnDeath += EnemyDeathResolver;
            return enemyView;
        }

        private void ActivateEnemy(EnemyView enemyView)
        {
            enemyView.gameObject.SetActive(true);
            enemyView.ResetEnemy(GetSpawnPosition());
        }

        private void DeactivateEnemy(EnemyView enemyView)
        {
            enemyView.gameObject.SetActive(false);
            _activatedEnemies.Remove(enemyView);
        }

        private void DestroyEnemy(EnemyView enemyView)
        {
            enemyView.OnDeath -= EnemyDeathResolver;
        }

        private void EnemyDeathResolver(EnemyView sender)
        {
            _enemyPools[sender.EnemyType].Release(sender);
        }
    }
}
