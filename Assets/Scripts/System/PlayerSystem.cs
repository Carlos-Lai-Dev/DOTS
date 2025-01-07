using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerSystem : ISystem
{
    private EntityManager entityManager;

    private Entity playerEntity;
    private Entity inputEntity;
    private PlayerComponent playerComponent;
    private InputComponent inputComponent;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        entityManager = state.EntityManager;
    }

    public void OnUpdate(ref SystemState state)
    {
        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        inputComponent = entityManager.GetComponentData<InputComponent>(inputEntity);

        Move(ref state);
        Fire(ref state);
    }

    private void Fire(ref SystemState state)
    {
        if (inputComponent.Fire)
        {
            for (int i = 0; i < playerComponent.NumOfBulletToSpawn; i++)
            {
                EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);

                Entity bulletEntity = entityManager.Instantiate(playerComponent.BulletPrefab);
                ECB.AddComponent(bulletEntity,new BulletComponent() 
                {
                    Speed = playerComponent.BulletSpeed,
                    Size = playerComponent.BulletSize,
                    Damage = playerComponent.BulletDamage,
                });

                ECB.AddComponent(bulletEntity, new BulletLifeTimeComponent() 
                { 
                    RemainingLifeTime = playerComponent.BulletLifeTime,
                });

                LocalTransform bulletTrans = entityManager.GetComponentData<LocalTransform>(bulletEntity);
                LocalTransform playerTrans = entityManager.GetComponentData<LocalTransform>(playerEntity);
                bulletTrans.Rotation = playerTrans.Rotation;
                float randomOffset = UnityEngine.Random.Range(-playerComponent.BulletSpread, playerComponent.BulletSpread);
                bulletTrans.Position = playerTrans.Position + (playerTrans.Up() * 1.65f + bulletTrans.Right() * randomOffset);

                ECB.SetComponent(bulletEntity, bulletTrans);
                ECB.Playback(entityManager);
                ECB.Dispose();
            }
        }
    }

    private void Move(ref SystemState state)
    {
        LocalTransform playerTrans = entityManager.GetComponentData<LocalTransform>(playerEntity);
        playerTrans.Position += new float3(inputComponent.Movement * playerComponent.MoveSpeed * SystemAPI.Time.DeltaTime, 0);

        var dir = (Vector2)inputComponent.Position - (Vector2)Camera.main.WorldToScreenPoint(playerTrans.Position);
        float angle = math.degrees(math.atan2(dir.x, dir.y));
        playerTrans.Rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        entityManager.SetComponentData(playerEntity, playerTrans);

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

}
