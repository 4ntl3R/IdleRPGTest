using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Spawners;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleRPG.Scripts.Runtime.Managers
{
    public class LevelManager : MonoBehaviour, IDeathReceiver
    {
        [SerializeField] 
        private GameObject endPopUp;

        [SerializeField] 
        private GameObject buttonsPanel;

        [SerializeField]
        private EnemySpawner spawner;
        
        public void ResolveDeath()
        {
            ActivateDeathMenu();
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }

        private void ActivateDeathMenu()
        {
            endPopUp.SetActive(true);
            buttonsPanel.SetActive(false);
            Destroy(spawner);
        }
    }
}
