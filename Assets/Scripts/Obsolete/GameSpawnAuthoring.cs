using Unity.Entities;
using UnityEngine;

public struct GameSpawnData : IComponentData
{
    public Entity entityPrefab;
}

class GameSpawnAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

class GameSpawnAuthoringBaker : Baker<GameSpawnAuthoring>
{
    public override void Bake(GameSpawnAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity,new GameSpawnData() {entityPrefab = GetEntity(authoring.prefab,TransformUsageFlags.Dynamic) });
    }
}
