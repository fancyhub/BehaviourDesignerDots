using System;
using BehaviorDesigner.Runtime.Tasks;
using System.Diagnostics;
using Unity.Collections;

namespace DotsBT
{
    [TaskCategory("Test")]
    public class TestDecorator : BehaviorDesigner.Runtime.Tasks.Decorator
    {
        [Tooltip("子节点应该返回的结果")]
        public bool ChildRetSucc;

        // The status of the child after it has finished running.
        private TaskStatus child_status = TaskStatus.Inactive;

        public override bool CanExecute()
        {
            return child_status == TaskStatus.Running || child_status == TaskStatus.Inactive;
        }
        public override void OnChildExecuted(TaskStatus childStatus)
        {
            child_status = childStatus;
        }
        public override void OnEnd()
        {
            child_status = TaskStatus.Inactive;
        }

        public override TaskStatus Decorate(TaskStatus status)
        {
            if ((ChildRetSucc && child_status == TaskStatus.Success) ||
                (!ChildRetSucc && child_status == TaskStatus.Failure))
            {
                UnityEngine.Debug.Log($"√ 测试通过 {FriendlyName} ");
                return TaskStatus.Success;
            }

            UnityEngine.Debug.LogError($"X 测试失败 {this.FriendlyName} {status} ");
            return TaskStatus.Failure;
        }
    }


    [BTNodeMeta(EBTNode.TestDecorator, EBTNode.Decorator)]
    public struct BTNodeTestDecorator : IBTNode<TestDecorator>
    {
        public BTNodeDecorator Base;
        public FixedString128Bytes TestName;
        public bool ChildRetSucc;

        public void Ctor(ref BTVM vm, Task task,BTBakeContextNode nodeExParam)
        {
            Base.Ctor(ref vm, task,nodeExParam);

            TestDecorator spec_task = task as TestDecorator;
            TestName.CopyFrom(spec_task.FriendlyName);
            ChildRetSucc = spec_task.ChildRetSucc;
        }

        public void Stop(ref BTVM vm)
        {
            Base.Stop(ref vm);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            var child_status = Base.UpdateDecoratedNode(ref vm);

            if (child_status == EBTStatus.Running)
                return EBTStatus.Running;

            if ((ChildRetSucc && child_status == EBTStatus.Success) ||
                (!ChildRetSucc && child_status == EBTStatus.Failure))
            {
                UnityEngine.Debug.Log($"√ 测试通过 {TestName} ");
                return EBTStatus.Success;
            }

            UnityEngine.Debug.LogError($"X 测试失败 {TestName} ");
            return EBTStatus.Failure;
        }
    }
}
