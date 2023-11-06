using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT.SubTree
{
    [BTNodeMeta(EBTNode.BehaviorTreeReference, EBTNode.Decorator)]
    public struct BTNodeBehaviorTreeReference : IBTNode<BehaviorTreeReference>
    {
        public BTNodeDecorator Base;
        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode nodeExParam)
        {
            Base.Base.Ctor(ref vm, task, nodeExParam);
            var spc = task as BehaviorTreeReference;
            var srcs = spc.GetExternalBehaviors();
            if (srcs.Length != 1)
            {
                throw new Exception("behaviortree only support one subtree now");
            }

            BehaviorSource src = srcs[0].GetBehaviorSource();
            src.ExDeserialization();
            Base.DecoratedNode = vm.NewNode(src.RootTask, nodeExParam);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return Base.UpdateDecoratedNode(ref vm);
        }

        public void Stop(ref BTVM vm)
        {
        }
    }
}