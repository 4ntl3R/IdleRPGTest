using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Views;
using UnityEngine;
using UnityEngine.Pool;

namespace IdleRPG.Scripts.Runtime.Spawners
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] 
        private GameObject projectilePrefab;

        private IObjectPool<ProjectileView> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<ProjectileView>(Create, Activate, Deactivate, Destroy);
        }

        public void Spawn(Vector3 senderPosition, ITargetProvider target, float damage)
        {
            _pool
                .Get()
                .Launch(senderPosition, target, damage);
        }

        private ProjectileView Create()
        {
            var instantiated = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
            var projectileView = instantiated.GetComponent<ProjectileView>();
            projectileView.OnHit += ReturnToPool;
            return projectileView;
        }

        private void Activate(ProjectileView projectileView)
        {
            projectileView.gameObject.SetActive(true);
        }

        private void Deactivate(ProjectileView projectileView)
        {
            projectileView.gameObject.SetActive(false);
        }

        private void Destroy(ProjectileView projectileView)
        {
            projectileView.OnHit -= ReturnToPool;
        }

        private void ReturnToPool(ProjectileView sender)
        {
            _pool.Release(sender);
        }
    }
}
