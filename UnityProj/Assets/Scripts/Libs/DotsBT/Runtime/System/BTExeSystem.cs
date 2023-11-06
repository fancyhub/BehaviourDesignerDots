using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace DotsBT
{
    public partial struct BTExeSystem : ISystem
    {
        public BTEcsLookup _lookup;
        public NativeArray<byte> _debugStatus;

        public void OnCreate(ref SystemState state)
        {
            _lookup = new BTEcsLookup();
            _lookup.Init(ref state);

#if UNITY_EDITOR
            _debugStatus = new NativeArray<byte>(1024, Allocator.Persistent);

            var debugComp = new BtDebugComp();
            debugComp.Status = _debugStatus;
            state.EntityManager.CreateSingleton(debugComp);
#endif            
        }

        public void OnDestroy(ref SystemState state)
        {
            _debugStatus.Dispose();
        }

        //[BurstCompile]
        public unsafe void OnUpdate(ref SystemState state)
        {
            BtDebugComp debugComp = new BtDebugComp();
#if UNITY_EDITOR
            if(SystemAPI.TryGetSingleton<BtDebugComp>(out debugComp))
            {
                debugComp.FrameIndex++;
                SystemAPI.SetSingleton(debugComp);
            }            
#endif

            _lookup.Update(ref state);

            EntityCommandBuffer ecb = SystemAPI.GetSingletonRW<BeginSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged);
            var job = new BTExeJob()
            {
                _lookup = _lookup,
                _ecb = ecb.AsParallelWriter(),
                _time = SystemAPI.Time,
                _debugComp = debugComp,
            };

            state.Dependency = job.ScheduleParallel(state.Dependency);
        }


        //[BurstCompile]
        public partial struct BTExeJob : IJobEntity
        {
            public const float C_FIXED_DELTA_TIME = 0;// 1.0f / 10; //一秒10帧

            [ReadOnly] public BTEcsLookup _lookup;
            [ReadOnly] public Unity.Core.TimeData _time;
            public EntityCommandBuffer.ParallelWriter _ecb;
            public BtDebugComp _debugComp;

            //[BurstCompile]
            public void Execute(ref BTComponentRunTimeData bt_comp, [ChunkIndexInQuery] int sortKey, in Entity e)
            {
                //1. 先判断状态
                if (!bt_comp.Valid || bt_comp.Status == EBTStatus.Failure || bt_comp.Status == EBTStatus.Success)
                    return;

                //2. 更新时间,如果时间不够, 直接返回
                bt_comp.DeltaTime += _time.DeltaTime;
                if (bt_comp.DeltaTime < C_FIXED_DELTA_TIME)
                    return;

                //3. 设置当前时间
                Unity.Core.TimeData time = new Unity.Core.TimeData(_time.ElapsedTime, bt_comp.DeltaTime);
                bt_comp.DeltaTime = 0;

                //4. 更新行为树
                // EBTStatus.Inactive || EBTStatus.Running 
                BTVM vm = new BTVM()
                {
                    Memory = bt_comp.Memory,
                    BlackBoard = bt_comp.BlackBoard,

                    Ecb = new BTEntityCommandBuffer(_ecb, sortKey),
                    EcsLookup = _lookup,
                    Time = time,

                    DebugStatusArray = _debugComp.CreateArray(e),                    
                };
                bt_comp.Status = vm.UpdateNode(bt_comp.RootTask);
            }
        }
    }
}
