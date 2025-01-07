using Unity.Entities;
using UnityEngine;


public partial class InputSystem : SystemBase
{
    private ControllerECS controller;
    private Vector2 MoveVector => controller.Player.Move.ReadValue<Vector2>();
    private Vector2 MousePos => controller.Player.MousePos.ReadValue<Vector2>();
    private bool IsFire => controller.Player.Fire.IsPressed();
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton(out InputComponent _))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        controller = new ControllerECS();
        controller.Enable();
    }
    protected override void OnUpdate()
    {
        SystemAPI.SetSingleton(new InputComponent
        {
            Movement = MoveVector,
            Position = MousePos,
            Fire = IsFire
        });
    }
    protected override void OnDestroy()
    {

    }
}
