using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct SpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        /* var gameData = SystemAPI.GetSingleton<GameSpawnData>();
         state.EntityManager.Instantiate(gameData.entityPrefab);*/
        /* foreach (var gameData in SystemAPI.Query<RefRO<GameSpawnData>>())
         {
             state.EntityManager.Instantiate(gameData.ValueRO.entityPrefab);
         }
         state.Enabled = false ;*/
       /* float a = 0;
        float b = 0;
        float c = 0;
        var gameData = SystemAPI.GetSingleton<GameSpawnData>();
        //var moveData = SystemAPI.GetSingleton<MoveData>();
        var entityArr = state.EntityManager.Instantiate(gameData.entityPrefab, 1000, Allocator.Temp);
        for (int i = 0; i < 1000; i++)
        {
            LocalTransform transform = state.EntityManager.GetComponentData<LocalTransform>(entityArr[i]);
            transform.Position = new float3(a, b, c);
            state.EntityManager.SetComponentData(entityArr[i], transform);
            state.EntityManager.AddComponentData(entityArr[i], new MoveData() { Speed = 5, Direction = new float3(1, 0, 0)});
            c++;
            if (c > 9)
            {
                c = 0;
                b++;
                if (b > 9)
                {
                    b = 0;
                    a++;
                }
            }
        }
        state.Enabled = false;*/
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
