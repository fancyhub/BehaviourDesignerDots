using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace DotsBT
{
    [TaskCategory("Test")]
    public class TestReturnAction : BehaviorDesigner.Runtime.Tasks.Action
    {
        [Tooltip("返回值, True: Succ,False: Fail")]
        public bool RetSucc;

        [Tooltip("等待的帧数")]
        public int WaitFrame;

        private int _frame;

        public override void OnStart()
        {
            base.OnStart();
            _frame = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (_frame >= WaitFrame)
            {
                if (RetSucc)
                    return TaskStatus.Success;
                return TaskStatus.Failure;
            }

            _frame++;
            return TaskStatus.Running;
        }
    }

    [BTNodeMeta(EBTNode.TestReturnAction, EBTNode.Action)]
    public struct BTNodeTestReturnAction : IBTNode<TestReturnAction>
    {
        public BTNodeAction Base;
        public bool RetSucc;
        public int WaitFrame;
        private int _frame;
        private bool _run;

        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode nodeExParam)
        {
            Base.Ctor(ref vm, task,nodeExParam);
            TestReturnAction spec_task = task as TestReturnAction;
            RetSucc = spec_task.RetSucc;
            WaitFrame = spec_task.WaitFrame;
            _frame = 0;
            _run = false;
        }

        public void Stop(ref BTVM vm)
        {
            Base.Stop(ref vm);
            _frame = 0;
        }

        public EBTStatus Update(ref BTVM vm)
        {
            if (!_run) //Start
            {
                _run = true;
                _frame = 0;
            }

            if (_frame >= WaitFrame)
            {
                if (RetSucc)
                    return EBTStatus.Success;
                return EBTStatus.Failure;
            }

            _frame++;
            return EBTStatus.Running;
        }
    }
}
