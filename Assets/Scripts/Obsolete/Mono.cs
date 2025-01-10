using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Mono : MonoBehaviour
{
    private World world;
    void Start()
    {
        world = World.DefaultGameObjectInjectionWorld;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NativeArray<Entity> allEntities = world.EntityManager.CreateEntityQuery(typeof(MoveData)).ToEntityArray(Allocator.Temp);

            foreach (var entity in allEntities)
            {
                MoveData moveData = world.EntityManager.GetComponentData<MoveData>(entity);
                moveData.Direction = new float3(-1, 0, 0);
                world.EntityManager.SetComponentData(entity, moveData);
            }

           /* Entity entity = world.EntityManager.CreateEntityQuery(typeof(PlayerTag)).GetSingletonEntity();

            MoveData moveData = world.EntityManager.GetComponentData<MoveData>(entity);
            moveData.Direction = new float3 (-1, 0, 0);
            world.EntityManager.SetComponentData(entity, moveData);*/
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
           /* Entity entity = world.EntityManager.CreateEntityQuery(typeof(PlayerTag)).GetSingletonEntity();

            MoveData moveData = world.EntityManager.GetComponentData<MoveData>(entity);
            moveData.Direction = new float3(1, 0, 0);
            world.EntityManager.SetComponentData(entity, moveData);*/

            NativeArray<Entity> allEntities = world.EntityManager.CreateEntityQuery(typeof(MoveData)).ToEntityArray(Allocator.Temp);

            foreach (var entity in allEntities)
            {
                MoveData moveData = world.EntityManager.GetComponentData<MoveData>(entity);
                moveData.Direction = new float3(-1, 0, 0);
                world.EntityManager.SetComponentData(entity, moveData);
            }
        }
    }
}
