using DotsBT;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    [UpdateInGroup(typeof(BTSystemGroup))]
    public partial struct DotsBTDebugSystem : ISystem
    {
        public DotsBTRuntimeArrayComp _comp;
        public void OnCreate(ref SystemState state)
        {
            _comp = new DotsBTRuntimeArrayComp();
            _comp.Status = new NativeArray<EBTStatus>(200, Allocator.Persistent);
            state.EntityManager.CreateSingleton(_comp);
        } 

        public void OnDestroy(ref SystemState state)
        {
            _comp.Status.Dispose();
        }
    }
}