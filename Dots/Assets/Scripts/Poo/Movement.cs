using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class Movement : MonoBehaviour
{
    public float MoveSpeed;
    public float RotationSpeed;

    public class Baker : Baker<Movement>
    {
        public override void Bake(Movement authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveUnitComponent
            {
                MoveSpeed = authoring.MoveSpeed,
                RotationSpeed = authoring.RotationSpeed
            });
        }
    }
}
public struct MoveUnitComponent : IComponentData
{
    public float MoveSpeed;
    public float RotationSpeed;
    public float3 TargetPosition;
}
