using UnityEngine;

namespace IdleRPG.Scripts.Data
{
    public class CharacterDataBundle : ScriptableObject
    {
        [SerializeField] 
        private CharacterParametersData parametersData;

        public CharacterParametersData ParametersData => parametersData;
    }
}
