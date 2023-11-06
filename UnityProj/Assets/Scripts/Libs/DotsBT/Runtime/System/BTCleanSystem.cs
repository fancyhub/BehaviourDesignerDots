using Unity.Entities;

namespace DotsBT
{   
    public partial struct BTCleanSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<BTCleanUpComp>().WithNone<BTComponentRunTimeData>().Build());
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer cmb = SystemAPI.GetSingletonRW<BeginSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (clean_comp, e) in SystemAPI.Query<BTCleanUpComp>().WithNone<BTComponentRunTimeData>().WithEntityAccess())
            {
                clean_comp.Dispose();
                cmb.RemoveComponent(e, ComponentType.ReadWrite<BTCleanUpComp>());
                cmb.DestroyEntity(e);
            }
        }
    }
     
}
