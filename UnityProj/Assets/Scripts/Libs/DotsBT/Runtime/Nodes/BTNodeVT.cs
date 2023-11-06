using Unity.Collections;
using Unity.Entities;
using Unity.Burst;

namespace DotsBT
{
    public unsafe static partial class BTNodeVT
    { 
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
