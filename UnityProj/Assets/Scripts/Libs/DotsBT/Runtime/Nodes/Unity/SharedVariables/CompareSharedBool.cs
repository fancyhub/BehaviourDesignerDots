using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;

namespace DotsBT.SharedVariables
{
    [BTNodeMeta(EBTNode.CompareSharedBool, EBTNode.NodeBase)]
    public struct BTNodeCompareSharedBool : IBTNode<CompareSharedBool>
    {
        public BTNodeBase Base;
        public BTVarBool a;
        public BTVarBool b;
        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            var spcTask = task as CompareSharedBool;
            a.Ctor(spcTask.variable, context);
            b.Ctor(spcTask.compareTo, context);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return a.GetValue(ref vm).Equals(b.GetValue(ref vm)) ? EBTStatus.Success : EBTStatus.Failure;
        }

        public void Stop(ref BTVM vm)
        {

        }
    }
}