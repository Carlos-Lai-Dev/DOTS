using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*foreach (var (bulletTrans, bulletComponent, bulletLifeTimeComponent, entity)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<BulletComponent>, RefRW<BulletLifeTimeComponent>>().WithEntityAccess())
        {
            bulletTrans.ValueRW.Position += bulletComponent.ValueRO.Speed * bulletTrans.ValueRW.Up() * SystemAPI.Time.DeltaTime;
            bulletLifeTimeComponent.ValueRW.RemainingLifeTime -= SystemAPI.Time.DeltaTime;

          *//*  if (bulletLifeTimeComponent.ValueRW.RemainingLifeTime <= 0f)
            {
                state.EntityManager.DestroyEntity(entity);
                break;
            }*//*
        }*/

        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        foreach (Entity entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<BulletLifeTimeComponent>(entity))
            {
                LocalTransform bulletTrans = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                bulletTrans.Position += bulletComponent.Speed * bulletTrans.Up() * SystemAPI.Time.DeltaTime;
                entityManager.SetComponentData(entity, bulletTrans);

                BulletLifeTimeComponent bulletLifeTimeComponent = entityManager.GetComponentData<BulletLifeTimeComponent>(entity);
                bulletLifeTimeComponent.RemainingLifeTime -= SystemAPI.Time.DeltaTime;

                if (bulletLifeTimeComponent.RemainingLifeTime <= 0f)
                {
                    entityManager.DestroyEntity(entity);
                    continue;
                }
                entityManager.SetComponentData(entity, bulletLifeTimeComponent);

                NativeList<ColliderCastHit> hits = new(Allocator.Temp);
                float3 point1 = new(bulletTrans.Position - bulletTrans.Up() * 0.15f);
                float3 point2 = new(bulletTrans.Position + bulletTrans.Up() * 0.15f);

                uint layerMask = LayerMaskHelper.GetLayerMaskFromTwoLayers(CollisionLayer.Wall, CollisionLayer.Enemy);

                physicsWorld.CapsuleCastAll(point1, point2, bulletComponent.Size / 2, float3.zero, 1f, ref hits, new CollisionFilter
                {
                    BelongsTo = (uint)CollisionLayer.Default,
                    CollidesWith = layerMask,
                });

                if (hits.Length > 0) 
                {
                    for (int i = 0; i < hits.Length; i++) 
                    {
                        Entity hitEntity = hits[i].Entity;
                        if (entityManager.HasComponent<EnemyComponent>(hitEntity))
                        { 
                            EnemyComponent enemyComponent = entityManager.GetComponentData<EnemyComponent>(hitEntity);
                            enemyComponent.CurrentHealth -= bulletComponent.Damage;
                            entityManager.SetComponentData(hitEntity,enemyComponent);
                            if (enemyComponent.CurrentHealth <= 0)
                            {
                                entityManager.DestroyEntity(hitEntity);
                            }
                        }
                    }
                    entityManager.DestroyEntity(entity); 
                }
                hits.Dispose();
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
