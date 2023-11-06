using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [BTNodeMeta(EBTNode.RandomProbability)]
    public struct BTNodeRandomProbability : IBTNode<RandomProbability>
    {
        public BTNodeConditional Base;
        public BTVarFloat _successProbability;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            RandomProbability spec_task = task as RandomProbability;
            _successProbability.Ctor(spec_task.successProbability, context);
        }

        public void Stop(ref BTVM vm)
        {
        }

        public EBTStatus Update(ref BTVM vm)
        {
            float randomValue = MyMath.RandomFloat();
            if (randomValue < _successProbability.GetValue(ref vm))
            {
                return EBTStatus.Success;
            }
            return EBTStatus.Failure;
        }
    }
}
