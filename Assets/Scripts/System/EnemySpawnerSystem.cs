using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemySpawnerSystem : ISystem
{
    private EntityManager entityManager;

    private Entity playerEntity;
    private Entity enemySpawnerEntity;

    private EnemySpawnerComponent enemySpawnerComponent;

    private Unity.Mathematics.Random random;


    public void OnCreate(ref SystemState state)
    {
        entityManager = state.EntityManager;
        random = Unity.Mathematics.Random.CreateFromIndex((uint)enemySpawnerComponent.GetHashCode());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        enemySpawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
        enemySpawnerComponent = entityManager.GetComponentData<EnemySpawnerComponent>(enemySpawnerEntity);

        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        SpawnEnemies(ref state);
    }

    private void SpawnEnemies(ref SystemState state)
    {
        enemySpawnerComponent.CurrentTimeBeforeNextSpawn -= SystemAPI.Time.DeltaTime;
        if (enemySpawnerComponent.CurrentTimeBeforeNextSpawn <= 0f)
        {
            for (int i = 0; i < enemySpawnerComponent.NumOfEnemiesToSpawnPerSecond; i++)
            {
                EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
                Entity enemyEntity = entityManager.Instantiate(enemySpawnerComponent.EnemyPrefab);

                LocalTransform enemyTrans = entityManager.GetComponentData<LocalTransform>(enemyEntity);
                LocalTransform playerTrans = entityManager.GetComponentData<LocalTransform>(playerEntity);

                float minDistanceSquared = enemySpawnerComponent.MinDistanceFromPlayer * enemySpawnerComponent.MinDistanceFromPlayer;
                float2 randomOffset = random.NextFloat2Direction() * random.NextFloat(enemySpawnerComponent.MinDistanceFromPlayer, enemySpawnerComponent.EnemySpawnRadius);
                float2 playerPos = new float2(playerTrans.Position.x, playerTrans.Position.y);
                float2 spawnPos = playerPos + randomOffset;
                float distanceSquared = math.lengthsq(spawnPos - playerPos);

                if (distanceSquared < minDistanceSquared)
                {
                    spawnPos = playerPos + math.normalize(randomOffset) * math.sqrt(minDistanceSquared);
                }
                enemyTrans.Position = new float3(spawnPos.x, spawnPos.y, 0f);

                float3 direction = math.normalize(playerTrans.Position - enemyTrans.Position);
                float angle = math.atan2(direction.y, direction.x);
                angle -= math.radians(90f);
                quaternion lookRot = quaternion.AxisAngle(new float3(0, 0, 1), angle);
                enemyTrans.Rotation = lookRot;

                ECB.SetComponent(enemyEntity, enemyTrans);
                ECB.AddComponent(enemyEntity, new EnemyComponent
                {
                    CurrentHealth = 100f,
                    EnemySpeed = 1.25f,
                });

                ECB.Playback(entityManager);
                ECB.Dispose();
            }

            int desiredEnemiesPerWave = enemySpawnerComponent.NumOfEnemiesToSpawnPerSecond + enemySpawnerComponent.NumOfEnemiesToSpawnIncrementAmount;
            int enemiesPerWave = math.min(desiredEnemiesPerWave, enemySpawnerComponent.MaxNumOfEnemiesToSpawnPerSecond);
            enemySpawnerComponent.NumOfEnemiesToSpawnPerSecond = enemiesPerWave;

            enemySpawnerComponent.CurrentTimeBeforeNextSpawn = enemySpawnerComponent.TimeBeforeNextSpawn;
        }
        entityManager.SetComponentData(enemySpawnerEntity, enemySpawnerComponent);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
