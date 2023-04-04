using System;
using System.Linq;
using IdleRPG.Scripts.Data;
using Random = UnityEngine.Random;

namespace IdleRPG.Scripts.Runtime.Data
{
    public static class SpawnEntryDataExtension
    {
        public static EnemyDataBundle GetRandomEnemyData(this SpawnEntryData[] spawnEntryData)
        {
            var randomWeightsSum = spawnEntryData.Sum(spawnEntry => spawnEntry.SpawnProbabilityWeight);
            var randomValue = Random.Range(0f, randomWeightsSum);
            var previousWeights = 0f;
            
            foreach (var spawnEntry in spawnEntryData)
            {
                if (randomValue < previousWeights + spawnEntry.SpawnProbabilityWeight)
                {
                    return spawnEntry.EnemyData;
                }

                previousWeights += spawnEntry.SpawnProbabilityWeight;
            }
            
            throw new Exception("Error while calculation random enemy");
        }
    }
}
