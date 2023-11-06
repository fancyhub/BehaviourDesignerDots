using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotsBT.GraphView.ED
{
    public class EdBtNode
    {
        public int Id;
        public string Name;
        public EBTNode NodeType;

        public List<EdBtNode> Children;

        public Rect Rect;
        public Vector2 FullSize; //包括子节点的所有大小
        private bool Folder = false;

        public EBTStatus Status;


        public Vector2 InPortPos => new Vector2(Rect.center.x, Rect.yMin);
        public Vector2 OutPortPos => new Vector2(Rect.center.x, Rect.yMax);

        public void Draw()
        {
            string msg = $"{Name}\n{Status}";

            GUI.Box(Rect, msg, DotsBTGraphViewConfig.Inst[Status]);

            if (Children != null && Children.Count > 0)
            {
                Rect btn_rect = new Rect(DotsBTGraphViewConfig.Inst.BtnRect.position + Rect.position, DotsBTGraphViewConfig.Inst.BtnRect.size);
                if (GUI.Button(btn_rect, "", Folder ? DotsBTGraphViewConfig.Inst.BtnPlus : DotsBTGraphViewConfig.Inst.BtnMinus))
                {
                    Folder = !Folder;
                }
            }

            //画子节点
            if (Children == null || Folder)
                return;

            foreach (var p in Children)
            {
                p.Draw();

                //画线
                Vector2 start_pos = OutPortPos;
                Vector2 end_pos = p.InPortPos;
                Vector2 start_tan = start_pos - Vector2.down * DotsBTGraphViewConfig.Inst.BorderHeight * 0.3f;
                Vector2 end_tan = end_pos + Vector2.down * DotsBTGraphViewConfig.Inst.BorderHeight * 0.3f;
                Handles.DrawBezier(
                    start_pos,
                    end_pos,
                    start_tan,
                    end_tan,
                    Color.white,
                    null,
                    3f
                );
            }
        }

        public void AdjustPos(Vector2 pos)
        {
            Rect = new Rect(pos, DotsBTGraphViewConfig.Inst.NodeSize);

            if (Children == null || Children.Count == 0 || Folder)
                return;

            Vector2 child_pos = pos;
            child_pos.y += DotsBTGraphViewConfig.Inst.NodeFullSize.y;
            child_pos.x -= FullSize.x * 0.5f;
            foreach (var p in Children)
            {
                child_pos.x += p.FullSize.x * 0.5f;
                p.AdjustPos(child_pos);
                child_pos.x += p.FullSize.x * 0.5f;
            }
        }

        public Vector2 CalcFullSize()
        {
            FullSize = Vector2.zero;

            if (Children != null && Children.Count > 0 && !Folder)
            {
                foreach (var p in Children)
                {
                    var child_size = p.CalcFullSize();
                    FullSize.x += child_size.x;
                    FullSize.y = Mathf.Max(FullSize.y, child_size.y);
                }

                FullSize.x = Mathf.Max(FullSize.x, DotsBTGraphViewConfig.Inst.NodeFullSize.x);
                FullSize.y += DotsBTGraphViewConfig.Inst.NodeFullSize.y;
            }
            else
            {
                FullSize.x = DotsBTGraphViewConfig.Inst.NodeFullSize.x;
                FullSize.y = DotsBTGraphViewConfig.Inst.NodeHeight;
            }
            return FullSize;
        }

        //true: 发生变化
        //false: 没有变化
        public bool UpdateStatus(EBTStatus[] status_array)
        {

            EBTStatus new_status = EBTStatus.Inactive;
            if (status_array != null && Id >= 0 && Id < status_array.Length)
                new_status = status_array[Id];
            bool ret = new_status != Status;
            Status = new_status;

            if (Children != null && Children.Count > 0)
            {
                foreach (var p in Children)
                {
                    ret = p.UpdateStatus(status_array) | ret;
                }
            }
            return ret;
        }
    }

    public class EdBtNodeGroup
    {
        public BTAsset Asset;
        public EdBtNode Root;

        public EdBtNodeGroup()
        {
        }

        //true: 发生变化了
        //false: 没有变化
        public bool UpdateStatus(EBTStatus[] status)
        {
            if (Root == null)
                return false;
            return Root.UpdateStatus(status);

        }

        public void Draw(Vector2 offset)
        {
            if (Root == null)
                return;
            Root.CalcFullSize();
            Root.AdjustPos(offset);
            Root.Draw();
        }

        public void SetAsset(BTAsset asset)
        {
            if (Asset == asset)
                return;

            Asset = asset;
            if (Asset == null || asset.Data == null || asset.Data.Data == null || asset.Data.Data.Length == 0)
                return;

            BTMemory memory = new BTMemory();
            memory.Exe = BTSegMemory.Create(EBTMemSeg.Exe, false, asset.Data.Data);
            var vm = new BTVM() { Memory = memory };
            Root = CreateNode(vm, asset.Data.NodeName, asset.Data.RootTask);
            Root.CalcFullSize();
        }

        private static unsafe EdBtNode CreateNode(BTVM vm, string[] node_names, BTPtr task)
        {
            BTNodeBase* p = vm.Memory.Get<BTNodeBase>(task, false);
            EdBtNode ret = new EdBtNode()
            {
                Id = p->NodeId,
                Name = node_names[p->NodeId],
                NodeType = p->Meta,
            };

            var deco = BTNodeVT.Cast<BTNodeDecorator, BTNodeBase>(p);
            if (deco != null)
            {
                ret.Children = new List<EdBtNode>(1);
                var child_node = CreateNode(vm, node_names, deco->DecoratedNode);
                ret.Children.Add(child_node);
                return ret;
            }
            var comp = BTNodeVT.Cast<BTNodeComposite, BTNodeBase>(p);
            if (comp != null)
            {
                int child_count = (comp->Children).Length;
                ret.Children = new List<EdBtNode>(child_count);
                for (int i = 0; i < child_count; i++)
                {
                    BTPtr child_ptr = (comp->Children).Get(ref vm, i);
                    var child_node = CreateNode(vm, node_names, child_ptr);
                    ret.Children.Add(child_node);
                }
            }
            return ret;
        }
    }

}
