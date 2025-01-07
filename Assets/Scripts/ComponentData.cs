using Unity.Entities;
using Unity.Mathematics;

public struct InputComponent : IComponentData
{
    public float2 Movement;
    public float2 Position;
    public bool Fire;
}

public struct PlayerComponent : IComponentData
{
    public float MoveSpeed;
    public Entity BulletPrefab;
    public int NumOfBulletToSpawn;
    public float BulletSpread;
    public float BulletSpeed;
    public float BulletLifeTime;
    public float BulletSize;
    public float BulletDamage;
}

public struct BulletComponent : IComponentData
{
    public float Speed;
    public float Size;
    public float Damage;
}

public struct BulletLifeTimeComponent : IComponentData
{
    public float RemainingLifeTime;
}

public struct EnemySpawnerComponent : IComponentData
{
    public Entity EnemyPrefab;
    public int NumOfEnemiesToSpawnPerSecond;
    public int NumOfEnemiesToSpawnIncrementAmount;
    public int MaxNumOfEnemiesToSpawnPerSecond;
    public float EnemySpawnRadius;
    public float MinDistanceFromPlayer;
    public float TimeBeforeNextSpawn;
    public float CurrentTimeBeforeNextSpawn;
}

public struct EnemyComponent : IComponentData
{
    public float CurrentHealth;
    public float EnemySpeed;
}

