using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

/*partial struct MoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        *//* foreach (var (transform, moveData) in SystemAPI.Query <RefRW<LocalTransform>, RefRO<MoveData>>())
         {
             transform.ValueRW.Position += moveData.ValueRO.Direction * moveData.ValueRO.Speed * SystemAPI.Time.DeltaTime;
         }*//*

        //var moveJob = new MoveJob() { deltaTime = SystemAPI.Time.DeltaTime };
        //moveJob.ScheduleParallel();
        //state.Enabled = false;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }


}*/

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    public float deltaTime;

    [BurstCompile]
    public void Execute(ref MoveData moveData, ref LocalTransform transform)
    {
        transform.Position += moveData.Direction * moveData.Speed * deltaTime;
    }
}

public partial class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Entities.ForEach(() => { }).Run();

        Entities.ForEach((Entity entity, ref MoveData moveData, ref LocalTransform transform) => 
        {
            transform.Position += moveData.Direction * moveData.Speed * SystemAPI.Time.DeltaTime;
        }).Run();

    }
}