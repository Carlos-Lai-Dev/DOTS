using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public GameObject BulletPrefab;
    public int NumOfBulletToSpawn;
    public float BulletSpread;
    public float bulletSpeed;
    public float bulletLifeTime;
    public float bulletSize;
    public float bulletDamage;
}

class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new PlayerComponent()
        {
            MoveSpeed = authoring.MoveSpeed,
            BulletSpread = authoring.BulletSpread,
            NumOfBulletToSpawn = authoring.NumOfBulletToSpawn,
            BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
            BulletLifeTime = authoring.bulletLifeTime,
            BulletSpeed = authoring.bulletSpeed, 
            BulletDamage = authoring.bulletDamage,
            BulletSize = authoring.bulletSize,
        });
    }
}
