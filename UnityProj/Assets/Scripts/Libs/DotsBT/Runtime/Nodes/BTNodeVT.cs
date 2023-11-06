using Unity.Collections;
using Unity.Entities;
using Unity.Burst;

namespace DotsBT
{
    public unsafe static partial class BTNodeVT
    {
        [BurstCompile]
        public struct NodeActionDictRT
        {
            [Unity.Collections.ReadOnly] public readonly NativeHashMap<int, FunctionPointer<NodeAction_Update>>.ReadOnly Update;
            [Unity.Collections.ReadOnly] public readonly NativeHashMap<int, FunctionPointer<NodeAction_Stop>>.ReadOnly Stop;

            public NodeActionDictRT(NodeActionDict dict)
            {
                Update = dict.Update.AsReadOnly();
                Stop = dict.Stop.AsReadOnly();
            }

            public bool GetActionStop(EBTNode meta, out FunctionPointer<NodeAction_Stop> action)
            {
                int key = (int)meta;
                if (!Stop.TryGetValue(key, out action))
                    return false;
                return action.IsCreated;
            }

            public bool GetActionUpdate(EBTNode meta, out FunctionPointer<NodeAction_Update> action)
            {
                int key = (int)meta;
                if (!Update.TryGetValue(key, out action))
                    return false;
                return action.IsCreated;
            }
        }

        public struct NodeActionDict
        {
            public NativeHashMap<int, FunctionPointer<NodeAction_Update>> Update;
            public NativeHashMap<int, FunctionPointer<NodeAction_Stop>> Stop;

            public void Dispose()
            {
                Update.Dispose();
                Stop.Dispose();
            }
        }

        public static NodeActionDict CreateEmptyActionDict(Allocator allocator)
        {
            NodeActionDict ret = new NodeActionDict();
            ret.Update = new NativeHashMap<int, FunctionPointer<NodeAction_Update>>(_NodeAction_UpdateDict.Count, allocator);
            ret.Stop = new NativeHashMap<int, FunctionPointer<NodeAction_Stop>>(_NodeAction_StopDict.Count, allocator);          
            return ret;
        }
        public static NodeActionDict CreateActionDict(Allocator allocator)
        {
            NodeActionDict ret = new NodeActionDict();

            ret.Update = new NativeHashMap<int, FunctionPointer<NodeAction_Update>>(_NodeAction_UpdateDict.Count, allocator);
            foreach (var p in _NodeAction_UpdateDict)
            {
                try
                {
                    FunctionPointer<NodeAction_Update> func_pointer = BurstCompiler.CompileFunctionPointer(p.Value);
                    ret.Update.Add((int)p.Key, func_pointer);
                }
                catch (System.Exception e)
                {
                    Unity.Logging.Log.Error($"{p.Key} Update {ret.Update.Count}/{_NodeAction_UpdateDict.Count}");
                }
            }

            ret.Stop = new NativeHashMap<int, FunctionPointer<NodeAction_Stop>>(_NodeAction_StopDict.Count, allocator);
            foreach (var p in _NodeAction_StopDict)
            {
                try
                {
                    FunctionPointer<NodeAction_Stop> func_pointer = BurstCompiler.CompileFunctionPointer(p.Value);
                    ret.Stop.Add((int)p.Key, func_pointer);
                }
                catch (System.Exception e)
                {
                    Unity.Logging.Log.Error($"{p.Key} Stop {ret.Stop.Count}/{_NodeAction_StopDict.Count}");
                }
            }
            return ret;
        }

        public static bool GetInfo(BehaviorDesigner.Runtime.Tasks.Task task, out NodeInfo node_info)
        {
            if (task == null)
            {
                node_info = default(NodeInfo);
                return false;
            }
            return _BDNodeInfoDict.TryGetValue(task.GetType(), out node_info);
        }

        public static NodeAction_Ctor GetActionCtor(EBTNode meta)
        {
            _NodeAction_CtorDict.TryGetValue(meta, out var action);
            return action;
        }

        public static bool GetActionUpdate(EBTNode meta, out NodeAction_Update action)
        {
            _NodeAction_UpdateDict.TryGetValue(meta, out action);
            return action != null;
        }

        public static bool GetActionStop(EBTNode meta, out NodeAction_Stop action)
        {
            _NodeAction_StopDict.TryGetValue(meta, out action);
            return action != null;
        }

        public static TTar* Cast<TTar, TSrc>(TSrc* p)
            where TSrc : unmanaged, IBTNode
            where TTar : unmanaged, IBTNode
        {
            if (p == null)
                return null;
            BTNodeBase* pBase = (BTNodeBase*)p;
            EBTNode meta = pBase->Meta;

            for (; ; )
            {
                if (meta == EBTNode.None)
                    return null;

                if (!_BTNodeInfoDict.TryGetValue(meta, out var info))
                    return null;

                if (info.SelfType == typeof(TTar))
                    return (TTar*)p;
                meta = info.ParentMeta;
            }
        }
    }
}
