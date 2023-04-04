using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IdleRPG.Scripts.Data
{
    [Serializable]
    public struct SpawnPositionRangeData
    {
        [SerializeField] 
        private Transform firstPoint;

        [SerializeField] 
        private Transform secondPoint;

        public Vector3 GetRandomPosition => Vector3.Lerp(firstPoint.position, secondPoint.position, Random.value);
    }
}
