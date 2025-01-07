using Unity.Entities;
using Unity.Mathematics;

public struct MoveData : IComponentData
{
    public float Speed;
    public float3 Direction;
}
