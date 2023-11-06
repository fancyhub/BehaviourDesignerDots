using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace DotsBT
{
    public enum EBTStatus
    {
        Inactive,
        Failure,
        Success,
        Running
    }

    public interface IBTNode
    {
        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context);
        public EBTStatus Update(ref BTVM vm);
        public void Stop(ref BTVM vm);
    }

    public interface IBTNode<T> : IBTNode where T : Task
    {
    }

    [BTNodeMeta(EBTNode.NodeBase)]
    public struct BTNodeBase : IBTNode<Task>
    {
        public ushort _meta;
        public short NodeId;

        public EBTNode Meta { get => (EBTNode)_meta; set => _meta = (ushort)value; }

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            context.NodeNames.Add(task.FriendlyName);
            NodeId = context.NodeIdGenIndex++;
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return EBTStatus.Success;
        }
        public void Stop(ref BTVM vm) { }
    }

    [BTNodeMeta(EBTNode.Composite, EBTNode.NodeBase)]
    public struct BTNodeComposite : IBTNode<Composite>
    {
        public BTNodeBase Base;
        public BTArray<BTPtr> Children;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
            ParentTask parent_task = task as ParentTask;

            var children = new List<Task>();
            foreach (var iTask in parent_task.Children)
            {
                children.Add(iTask);
            }
            if (children.Count == 0)
                return;
            children.TrimExcess();
            Children.Ctor(ref context.Allocator, children.Count);

            int count = children.Count;
            for (int i = 0; i < count; i++)
            {
                BTPtr c = vm.NewNode(children[i], context);
                Children.Set(ref vm, i, c);
            }
        }

        public int GetChildCount()
        {
            return Children.Length;
        }

        public BTPtr GetChildPtrAt(ref BTVM vm, int index)
        {
            return Children.Get(ref vm, index);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return EBTStatus.Success;
        }

        public void Stop(ref BTVM vm)
        {
            int count = Children.Length;
            for (int i = 0; i < count; i++)
            {
                BTPtr child = Children.Get(ref vm, i);
                vm.StopNode(child);
            }
        }
    }

    [BTNodeMeta(EBTNode.Decorator, EBTNode.NodeBase)]
    public struct BTNodeDecorator : IBTNode<Decorator>
    {
        public BTNodeBase Base;
        public BTPtr DecoratedNode;

        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);

            Decorator spec_task = task as Decorator;
            if (spec_task.Children == null || spec_task.Children.Count != 1)
            {
                throw new Exception($"{spec_task.FriendlyName} 必须有一个子节点");
            }
            DecoratedNode = vm.NewNode(spec_task.Children[0], context);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return vm.UpdateNode(DecoratedNode);
        }

        public EBTStatus UpdateDecoratedNode(ref BTVM vm)
        {
            return vm.UpdateNode(DecoratedNode);
        }

        public void Stop(ref BTVM vm)
        {
            vm.StopNode(DecoratedNode);
        }
    }

    [BTNodeMeta(EBTNode.Action, EBTNode.NodeBase)]
    public struct BTNodeAction : IBTNode<BehaviorDesigner.Runtime.Tasks.Action>
    {
        public BTNodeBase Base;
        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return EBTStatus.Success;
        }

        public void Stop(ref BTVM vm)
        {
        }
    }

    [BTNodeMeta(EBTNode.Conditional, EBTNode.NodeBase)]
    public struct BTNodeConditional : IBTNode<Conditional>
    {
        public BTNodeBase Base;
        public void Ctor(ref BTVM vm, Task task, BTBakeContextNode context)
        {
            Base.Ctor(ref vm, task, context);
        }

        public EBTStatus Update(ref BTVM vm)
        {
            return EBTStatus.Success;
        }

        public void Stop(ref BTVM vm)
        {
        }
    }
}
