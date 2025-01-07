using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class MoveAuthoring : MonoBehaviour
{
    public float Speed;
    public float3 Direction;
}

class MoveAuthoringBaker : Baker<MoveAuthoring>
{
    public override void Bake(MoveAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity,new MoveData() {Speed = authoring.Speed, Direction = authoring.Direction });
    }
}