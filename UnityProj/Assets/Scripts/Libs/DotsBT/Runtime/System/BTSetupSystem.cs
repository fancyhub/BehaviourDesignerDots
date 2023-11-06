using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;

namespace DotsBT
{
    public partial struct BTSetupSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<BTAssetCompData>().WithNone<BTComponentRunTimeData>().Build());
        }

        public unsafe void OnUpdate(ref SystemState state)
        {
            BeginSimulationEntityCommandBufferSystem.Singleton singleton = SystemAPI.GetSingletonRW<BeginSimulationEntityCommandBufferSystem.Singleton>().ValueRW;
            EntityCommandBuffer cmb = singleton.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (bt_comp, e) in SystemAPI.Query<BTAssetCompData>().WithNone<BTComponentRunTimeData>().WithEntityAccess())
            {
                if (!bt_comp.Image.IsReferenceValid)
                    continue;

                var status = bt_comp.Image.LoadingStatus;
                switch (status)
                {
                    case Unity.Entities.Content.ObjectLoadingStatus.None:
                        bt_comp.Image.LoadAsync();
                        break;

                    case Unity.Entities.Content.ObjectLoadingStatus.Queued:
                    case Unity.Entities.Content.ObjectLoadingStatus.Loading:
                        break;

                    case Unity.Entities.Content.ObjectLoadingStatus.Error:
                        {
                            Unity.Logging.Log.Error("Loading Error");
                            cmb.RemoveComponent<BTAssetCompData>(e);
                        }
                        break;

                    case Unity.Entities.Content.ObjectLoadingStatus.Completed:
                        {
                            BTAsset asset = bt_comp.Image.Result;
                            if (asset == null)
                            {
                                cmb.RemoveComponent<BTAssetCompData>(e);
                                break;
                            }

                            if (asset.Data.RootTask.IsNull)
                            { 
                                Unity.Logging.Log.Error("Root Task is Null " + asset.Data.Name);
                                cmb.RemoveComponent<BTAssetCompData>(e);
                                break;
                            }

                            if (bt_comp.GlobalVarEntity == Entity.Null)
                            {
                                Unity.Logging.Log.Error("Global Var Is Null " + asset.Data.Name);
                                cmb.RemoveComponent<BTAssetCompData>(e);
                                break;
                            }

                            BTGlobalCompData global_vars = state.EntityManager.GetComponentData<BTGlobalCompData>(bt_comp.GlobalVarEntity);

                            if (!global_vars.Vars.CheckVars(asset.Data, EBTVarScope.Global)
                                || !bt_comp.Vars.CheckVars(asset.Data, EBTVarScope.Share))
                            {
                                Unity.Logging.Log.Error("Link Error " + asset.Data.Name);
                                cmb.RemoveComponent<BTAssetCompData>(e);
                                break;
                            }


                            //构建 运行数据
                            BTCleanUpComp clean_up_data = new BTCleanUpComp();
                            clean_up_data.ImageData = new NativeArray<byte>(asset.Data.Data, Allocator.Persistent);
                            clean_up_data.SharedVarData = bt_comp.Vars.CreateRuntime(Allocator.Persistent);

                            BTMemory memory = new BTMemory();
                            memory.Exe = BTSegMemory.Create(EBTMemSeg.Exe, true, clean_up_data.ImageData);
                            memory.SharedVar = BTSegMemory.Create(EBTMemSeg.SharedVar, true, clean_up_data.SharedVarData);
                            memory.GlobalVar = BTSegMemory.Create(EBTMemSeg.GlobalVar, false, global_vars.Vars.Data);

                            BTComponentRunTimeData runtime = new BTComponentRunTimeData();
                            runtime.Valid = true;
                            runtime.RootTask = asset.Data.RootTask;
                            runtime.Memory = memory;
                            runtime.BlackBoard = new BTBlackBoard(e, bt_comp.Vars.NameList, global_vars.Vars.NameList, memory);
                            runtime.DeltaTime = 0;

                            //Link
                            foreach (var var_item in asset.Data.UsedVarList)
                            {
                                BTPtr value_ptr = runtime.BlackBoard.LinkVar(var_item.NameId, var_item.Scope);

                                BTPtr ptr_ptr = var_item.LinkPtr;
                                for (; ; )
                                {
                                    if (ptr_ptr.IsNull)
                                        break;
                                    BTPtr next_ptr = memory.GetValue<BTPtr>(ptr_ptr);
                                    UnityEngine.Debug.Log($"替换 位置 {ptr_ptr} -> {value_ptr}");
                                    memory.SetValue(ptr_ptr, value_ptr);
                                    ptr_ptr = next_ptr;
                                }
                            }
                            cmb.AddComponent(e, runtime);
                            cmb.AddComponent(e, clean_up_data);
                        }
                        break;
                }
            }
        }
    }

}
