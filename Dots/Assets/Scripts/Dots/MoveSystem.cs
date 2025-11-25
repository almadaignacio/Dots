using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct MoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        MoveUnitJobs moveUnitJobs = new MoveUnitJobs
        {
            deltatime = SystemAPI.Time.DeltaTime,
        };
        moveUnitJobs.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct MoveUnitJobs : IJobEntity
{

    public float deltatime;
    private void Execute(ref LocalTransform localTransform, in MoveUnitComponent moveUnit, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirection = moveUnit.TargetPosition - localTransform.Position;

        float reachedTargetDistanceSq = 2f;
        if (math.lengthsq(moveDirection) < reachedTargetDistanceSq)
        {
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }
        moveDirection = math.normalize(moveDirection);

        localTransform.Rotation = math.slerp(localTransform.Rotation,
            quaternion.LookRotation(moveDirection, math.up()), deltatime * moveUnit.RotationSpeed);

        physicsVelocity.Linear = moveDirection * moveUnit.MoveSpeed;
        physicsVelocity.Angular = float3.zero;
    }
}
