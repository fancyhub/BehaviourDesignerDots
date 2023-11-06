using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using UnityEngine;

namespace DotsBT.UnityTransform
{
    [BTNodeMeta(EBTNode.SetPosition, EBTNode.NodeBase)]
    public unsafe struct BTNodeSetPosition : IBTNode<SetPosition>
    {
        public BTNodeBase Base;
        public BTVarVector3 position;
        public BTVarGameObject targetGameObject;

        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            var setPos = task as SetPosition;

            position.Ctor(setPos.position, context);
            targetGameObject.Ctor(setPos.targetGameObject, context);
        }

        public EBTStatus Update(ref BTVM vm)
        {

            var e = targetGameObject.GetValue(ref vm);
            var pos = position.GetValue(ref vm);
            vm.EcsLookup.LocalTransform.TryGetComponent(e, out var tran);
            tran.Position = pos;

            vm.Ecb.SetComponent(e, tran);
            return EBTStatus.Success;
        }

        public void Stop(ref BTVM vm)
        {

        }

    }
}