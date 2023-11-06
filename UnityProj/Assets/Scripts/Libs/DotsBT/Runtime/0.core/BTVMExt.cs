//#define USE_BURST_ACTION
using System;
using BehaviorDesigner.Runtime.Tasks;
using Unity.Entities;


namespace DotsBT
{
    public unsafe static class BTVMExt
    {
        #region Var 
        public static BTPtr Linkvar(ref this BTVM self, int name_id, EBTVarScope scope)
        {
            return self.BlackBoard.LinkVar(name_id, scope);
        }

        public static Entity GetOwnerEntity(ref this BTVM self)
        {
            return self.BlackBoard.GetOwnerEntity();
        }

        public static T GetValue<T>(ref this BTVM self, BTPtr ptr) where T : unmanaged
        {
            T* p = self.Memory.Get<T>(ptr, false);
            if (p == null)
                return default;
            return *p;
        }

        public static bool SetValue<T>(ref this BTVM self, BTPtr ptr, T value) where T : unmanaged
        {
            T* p = self.Memory.Get<T>(ptr, true);
            if (p == null)
                return false;
            *p = value;
            return true;
        }
        #endregion

        public static T* Get<T>(ref this BTVM self, BTPtr ptr, bool write) where T : unmanaged
        {
            return self.Memory.Get<T>(ptr, write);
        }

        public static void StopNode(ref this BTVM self, BTPtr ptr)
        {
            BTNodeBase* p = self.Memory.Get<BTNodeBase>(ptr, true);
            if (p == null)
                return;
            EBTNode node_meta = p->Meta;

#if USE_BURST_ACTION
            if (!self.NodeActionVT.GetActionStop(node_meta, out var action))
#else
            if(!BTNodeVT.GetActionStop(node_meta,out var action))
#endif
            {
                UnityEngine.Debug.LogError($"找不到 {node_meta} 对应的Stop方法 ");
                return;
            }

            action.Invoke(p, ref self);
        }


        public static EBTStatus UpdateNode(ref this BTVM self, BTPtr ptr)
        {
            BTNodeBase* p = self.Memory.Get<BTNodeBase>(ptr, true);
            if (p == null)
                return EBTStatus.Failure;
            EBTNode node_meta = p->Meta;
#if USE_BURST_ACTION
            if (!self.NodeActionVT.GetActionUpdate(node_meta, out var action))
#else
            if(!BTNodeVT.GetActionUpdate(node_meta,out var action))
#endif
            {
                UnityEngine.Debug.LogError($"找不到 {node_meta} 对应的 Update 方法 ");
                return EBTStatus.Failure;
            }

            var status = action.Invoke(p, ref self);
#if UNITY_EDITOR
            self.DebugStatusArray.SetState(p->NodeId, status);
#endif
            return status;
        }

        public static BTPtr NewNode(ref this BTVM self, Task task, BTBakeContextNode context)
        {
            //1. 检查
            if (task == null)
                throw new Exception("Task Is Null");

            //1. 获取类型和大小
            if (!BTNodeVT.GetInfo(task, out var type_info))
                throw new Exception($"未知类型 {task.GetType()} ");

            //2. 分配内存
            BTPtr ptr = context.Allocator.Alloc(type_info.SelfSize);
            if (ptr.IsNull)
                return ptr;

            //3. 写入Meta
            BTNodeBase* p_node = self.Memory.Get<BTNodeBase>(ptr, true);
            p_node->Meta = type_info.Meta;


            //4. 构造
            var action = BTNodeVT.GetActionCtor(type_info.Meta);
            if (action == null)
                UnityEngine.Debug.LogError($"找不到 {type_info.Meta} 对应的方法 ");

            action?.Invoke(p_node, ref self, task, context);

            //5. 
            return ptr;
        }
    }
}
