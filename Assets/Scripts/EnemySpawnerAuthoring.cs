using Unity.Entities;
using UnityEngine;

class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public int NumOfEnemiesToSpawnPerSecond;
    public int NumOfEnemiesToSpawnIncrementAmount;
    public int MaxNumOfEnemiesToSpawnPerSecond;
    public float EnemySpawnRadius;
    public float MinDistanceFromPlayer;

    public float TimeBeforeNextSpawn;
}

class EnemySpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        Entity enemySpawnEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(enemySpawnEntity,new EnemySpawnerComponent 
        {
            EnemyPrefab = GetEntity(authoring.EnemyPrefab,TransformUsageFlags.Dynamic),
            NumOfEnemiesToSpawnPerSecond = authoring.NumOfEnemiesToSpawnPerSecond,
            NumOfEnemiesToSpawnIncrementAmount = authoring.NumOfEnemiesToSpawnIncrementAmount,
            MaxNumOfEnemiesToSpawnPerSecond = authoring.MaxNumOfEnemiesToSpawnPerSecond,
            EnemySpawnRadius = authoring.EnemySpawnRadius,
            MinDistanceFromPlayer = authoring.MinDistanceFromPlayer,
            TimeBeforeNextSpawn = authoring.TimeBeforeNextSpawn,
        });
    }
}
