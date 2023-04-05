using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    [CreateAssetMenu(order = 0, fileName = "New Skills Data Bundle", menuName = "Idle RPG/Skills Data/Skills Data Bundle")]
    public class SkillsDataBundle : ScriptableObject
    {
        [SerializeField] 
        private SkillLightingParametersData lightingData;

        public SkillLightingParametersData LightingData => lightingData;
    }
}
