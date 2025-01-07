using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemySystem : ISystem
{
    private EntityManager entityManager;
    private Entity playerEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        entityManager = state.EntityManager;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTrans = entityManager.GetComponentData<LocalTransform>(playerEntity);

        foreach (var (enemyTrans, enemyComponent) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EnemyComponent>>())
        {
            float3 moveDirection = math.normalize(playerTrans.Position - enemyTrans.ValueRW.Position);
            enemyTrans.ValueRW.Position += enemyComponent.ValueRW.EnemySpeed * moveDirection * SystemAPI.Time.DeltaTime;

            float3 direction = math.normalize(playerTrans.Position - enemyTrans.ValueRW.Position);
            float angle = math.atan2(direction.y, direction.x);
            angle -= math.radians(90f);
            quaternion lookRot = quaternion.AxisAngle(new float3(0, 0, 1), angle);
            enemyTrans.ValueRW.Rotation = lookRot;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
