using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace DotsBT
{
    public partial struct BTExeSystem : ISystem
    {
        public BTEcsLookup _lookup;
        public BTNodeVT.NodeActionDict _node_action_dict;

        public void OnCreate(ref SystemState state)
        {
            _lookup = new BTEcsLookup();
            _lookup.Init(ref state);
            _node_action_dict = BTNodeVT.CreateEmptyActionDict(Allocator.Persistent);

        }

        public void OnDestroy(ref SystemState state)
        {
            _node_action_dict.Dispose();
        }

        //[BurstCompile]
        public unsafe void OnUpdate(ref SystemState state)
        {
            _lookup.Update(ref state);

            EntityCommandBuffer ecb = SystemAPI.GetSingletonRW<BeginSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged);
            var job = new BTExeJob()
            {
                _lookup = _lookup,
                _ecb = ecb.AsParallelWriter(),
                _time = SystemAPI.Time,
                _node_action_vt = new BTNodeVT.NodeActionDictRT(_node_action_dict),
            };

#if UNITY_EDITOR
            if (SystemAPI.TryGetSingleton(out job._debug_status_array))
            {
                job._debug_status_array.FrameIndex++;
                SystemAPI.SetSingleton(job._debug_status_array);
            }
#endif

            state.Dependency = job.ScheduleParallel(state.Dependency);
        }


        //[BurstCompile]
        public partial struct BTExeJob : IJobEntity
        {
            public const float C_FIXED_DELTA_TIME = 0;// 1.0f / 10; //一秒10帧

            [ReadOnly] public BTEcsLookup _lookup;
            [ReadOnly] public Unity.Core.TimeData _time;
            [ReadOnly] public BTNodeVT.NodeActionDictRT _node_action_vt;
            public EntityCommandBuffer.ParallelWriter _ecb;

            public DotsBTRuntimeArrayComp _debug_status_array;

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

                    EcsLookup = _lookup,
                    Time = time,
                    DebugStatusArray = _debug_status_array.CreateArray(e),
                    NodeActionVT = _node_action_vt,
                    Ecb = new BTEntityCommandBuffer(_ecb, sortKey),
                };
                bt_comp.Status = vm.UpdateNode(bt_comp.RootTask);
            }
        }
    }
}
