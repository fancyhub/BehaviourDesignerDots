using System;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;

namespace DotsBT.UnityTransform
{
    [BTNodeMeta(EBTNode.Rotate, EBTNode.NodeBase)]
    public unsafe struct BTNodeRotate : IBTNode<Rotate>
    {
        public BTNodeBase Base;
        public BTVarVector3 eulerAngles;
        public BTVarGameObject targetGameObject;
        public Space relativeTo;
        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            Rotate spec_task = task as Rotate;
            relativeTo = spec_task.relativeTo;

            eulerAngles.Ctor(spec_task.eulerAngles, context);
            targetGameObject.Ctor(spec_task.targetGameObject, context);
        }

        public EBTStatus Update(ref BTVM vm)
        {

            Entity e = targetGameObject.GetValue(ref vm);
            Vector3 eulers = eulerAngles.GetValue(ref vm);

            vm.EcsLookup.LocalTransform.TryGetComponent(e, out var tran);
            //LocalTransform tran = vm.EntityManager.GetComponentData<LocalTransform>(e);
            tran = tran.Rotate(Unity.Mathematics.quaternion.Euler(eulers));
            //vm.EcsWorld.EntityManager.SetComponentData(e, tran);

            vm.Ecb.SetComponent(e, tran);
            Debug.Log("rotate one time");
            return EBTStatus.Success;

        }

        public void Stop(ref BTVM vm)
        {
        }
    }
}
