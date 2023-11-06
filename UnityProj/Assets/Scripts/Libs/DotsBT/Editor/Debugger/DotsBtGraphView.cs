using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT.ED
{
    public class EdBTNode
    {
        public int Id;
        public string Name;
        public EBTNode NodeType;

        public List<EdBTNode> Children;

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

    public class EdBTNodeGroup
    {
        public BTAsset Asset;
        public EdBTNode Root;

        public EdBTNodeGroup()
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

        private static unsafe EdBTNode CreateNode(BTVM vm, string[] node_names, BTPtr task)
        {
            BTNodeBase* p = vm.Memory.Get<BTNodeBase>(task, false);
            EdBTNode ret = new EdBTNode()
            {
                Id = p->NodeId,
                Name = node_names[p->NodeId],
                NodeType = p->Meta,
            };

            var deco = BTNodeVT.Cast<BTNodeDecorator, BTNodeBase>(p);
            if (deco != null)
            {
                ret.Children = new List<EdBTNode>(1);
                var child_node = CreateNode(vm, node_names, deco->DecoratedNode);
                ret.Children.Add(child_node);
                return ret;
            }
            var comp = BTNodeVT.Cast<BTNodeComposite, BTNodeBase>(p);
            if (comp != null)
            {
                int child_count = (comp->Children).Length;
                ret.Children = new List<EdBTNode>(child_count);
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

    public struct EdBTGraphViewTarget
    {
        public Entity Entity;
        public World World;

        //True: 发生变化
        //false: 没有变化
        public bool Set(World world, Entity entity)
        {
            if (this.World == world && this.Entity == entity)
                return false;
            this.World = world;
            this.Entity = entity;
            return true;
        }

        public BTAsset GetAsset()
        {
            if (World == null || !World.IsCreated || Entity == Entity.Null)
                return null;
            BTAssetCompData bt_asset_comp = World.EntityManager.GetComponentData<BTAssetCompData>(Entity);
            return BTBaker.EdGetWeakRefObj(bt_asset_comp.Image);
        }

        public bool GetDebugRTArray(out DotsBTRuntimeArrayComp result)
        {
            result = default;
            if (World == null || !World.IsCreated)
                return false;

            var query = World.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<DotsBTRuntimeArrayComp>());
            var single_entity = query.GetSingletonEntity();
            if (single_entity == Entity.Null)
                return false;

            result = World.EntityManager.GetComponentData<DotsBTRuntimeArrayComp>(single_entity);

            if (!result.Status.IsCreated)
                return false;
            if (result.Status.Length == 0)
                return false;
            if (result.Status[0] == EBTStatus.Inactive)
                return false;
            if (result.TargetEntity != Entity)
                return false;
            return true;
        }

        public void EnableDebug(bool enable)
        {
            if (World == null || !World.IsCreated)
                return;

            var query = World.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<DotsBTRuntimeArrayComp>());
            var single_entity = query.GetSingletonEntity();
            var comp = World.EntityManager.GetComponentData<DotsBTRuntimeArrayComp>(single_entity);

            Entity new_tar = Entity;
            if (!enable)
                new_tar = Entity.Null;
            if (comp.TargetEntity == new_tar)
                return;

            comp.TargetEntity = new_tar;
            if (new_tar != Entity.Null)
                comp.Reset();
            World.EntityManager.SetComponentData(single_entity, comp);
        }
    }

    public struct EdBTFrameData
    {
        public int FrameIndex;
        public EBTStatus[] Status;

        public bool IsStatusSame(EdBTFrameData other)
        {
            if (Status.Length != other.Status.Length)
                return false;

            for (int i = 0; i < other.Status.Length; i++)
            {
                if (other.Status[i] != Status[i])
                    return false;
            }
            return true;
        }
    }

    public class MyFixedQueue<T>
    {
        private T[] _array;
        private int _index = 0; //末尾位置, 用来插入的
        private int _count = 0;
        public MyFixedQueue(int cap)
        {
            _array = new T[cap];
        }

        public int Count { get => _count; }

        public void Enqueue(T data)
        {
            if (_array.Length == 0)
                return;

            _array[_index % _array.Length] = data;
            _index++;
            _count = Math.Min(_count + 1, _array.Length);
        }

        public T Dequeue()
        {
            int index = _CalcArrayIndex(0);
            if (index < 0)
                return default;
            var ret = _array[index];
            _count--;
            return ret;
        }

        public void Clear()
        {
            _count = 0;
        }

        public T this[int index]
        {
            get
            {
                int real_index = _CalcArrayIndex(index);
                if (real_index < 0)
                    return default;
                return _array[real_index];
            }
        }

        private int _CalcArrayIndex(int index)
        {
            if (index < 0 || index >= _count)
                return -1;
            return (_index - _count + index) % _array.Length;
        }
    }

    public class EdBTGraphBar
    {
        private const int C_COUNT = 200;
        private const float C_VIEW_HEIGHT = 100;

        public Action<EdBTFrameData> OnFrameChange;
        private MyFixedQueue<EdBTFrameData> _data_queue = new MyFixedQueue<EdBTFrameData>(C_COUNT);
        private int _node_count = 0;
        private int _current_select_index = -1;

        public void Reset(int node_count)
        {
            _node_count = node_count;
            _data_queue.Clear();
            _current_select_index = -1;
        }

        public bool AddFrameData(int frame_index, NativeArray<EBTStatus> status)
        {
            if (frame_index <= _LastFrameIndex(_data_queue))
                return false;

            EdBTFrameData frame = _CreateFrameData(frame_index, status, _node_count);

            //比较
            if (_data_queue.Count > 0 && _data_queue[_data_queue.Count - 1].IsStatusSame(frame))
                return false;

            _data_queue.Enqueue(frame);

            _current_select_index = _data_queue.Count - 1;
            OnFrameChange?.Invoke(frame);
            return true;
        }

        public bool Draw()
        {
            float width = EditorGUIUtility.currentViewWidth;
            Rect pos = new Rect(0, 0, width, C_VIEW_HEIGHT);
            GUILayout.BeginArea(pos);


            int new_index = _HandleEvent(_current_select_index, pos, _data_queue.Count);
            bool changed = new_index != _current_select_index;
            _current_select_index = new_index;

            Handles.BeginGUI();

            {
                Vector2 start = new Vector2(0, C_VIEW_HEIGHT * 0.5f);
                Vector2 end = new Vector2(width * _data_queue.Count / C_COUNT, start.y);
                Handles.DrawLine(start, end);
            }

            //Handles.color = Color.red;
            if (_current_select_index > 0 && _current_select_index < _data_queue.Count)
            {
                Vector2 start = new Vector2(width * _current_select_index / C_COUNT, 0);
                Vector2 end = new Vector2(start.x, pos.height);
                Handles.DrawLine(start, end);
            }

            Handles.EndGUI();
            GUILayout.EndArea();

            pos.height = 20;
            EditorGUI.LabelField(pos, $"{_current_select_index + 1} / {_data_queue.Count}   FrameIndex: {_data_queue[_current_select_index].FrameIndex}");

            if (changed)
                OnFrameChange?.Invoke(_data_queue[_current_select_index]);
            return changed;
        }

        private static EdBTFrameData _CreateFrameData(int frame_index, NativeArray<EBTStatus> status, int count)
        {
            EdBTFrameData frame = new EdBTFrameData()
            {
                FrameIndex = frame_index,
                Status = new EBTStatus[count],
            };

            if (status.IsCreated)
            {
                Debug.Assert(count <= status.Length, $"行为树的Debug数组 太小了, {status.Length} < {count}");
                count = Math.Min(status.Length, count);
                for (int i = 0; i < count; i++)
                {
                    frame.Status[i] = status[i];
                }
            }

            return frame;
        }

        //获取最后一个FrameData的FrameIndex
        private static int _LastFrameIndex(MyFixedQueue<EdBTFrameData> queue)
        {
            if (queue.Count == 0)
                return -1;
            return queue[queue.Count - 1].FrameIndex;
        }

        private static int _HandleEvent(int cur_index, Rect rect, int data_count)
        {
            if (data_count == 0)
                return cur_index;

            var point = Event.current.mousePosition;
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    {
                        if (Event.current.button == 0 && rect.Contains(point))
                        {
                            if (Application.isPlaying)
                                EditorApplication.isPaused = true;

                            float p = point.x / rect.width;
                            return Math.Clamp((int)(p * C_COUNT), 0, data_count - 1);
                        }
                    }
                    break;
                case EventType.KeyDown:
                    {
                        if (Application.isPlaying && !EditorApplication.isPaused)
                            return cur_index;

                        if (Event.current.keyCode == KeyCode.LeftArrow)
                        {
                            return Math.Max(cur_index - 1, 0);
                        }
                        else if (Event.current.keyCode == KeyCode.RightArrow)
                        {
                            return Math.Min(cur_index + 1, data_count - 1);
                        }
                    }
                    break;
            }
            return cur_index;
        }
    }

    public class EdBTGraphView : EditorWindow
    {
        public EdBTNodeGroup _node_group = new EdBTNodeGroup();
        public EdBTGraphBar _bar = new EdBTGraphBar();
        private EditorZoomer _zoomer;
        public EdBTGraphViewTarget _target;

        protected void OnEnable()
        {
            _zoomer = new EditorZoomer();
            _target = new EdBTGraphViewTarget();
            _bar.OnFrameChange = _OnFrameDataChange;
        }

        private void _OnFrameDataChange(EdBTFrameData data)
        {
            _node_group.UpdateStatus(data.Status);
        }

        public static void ShowGraph(World world, Entity e)
        {
            EdBTGraphView window = GetWindow<EdBTGraphView>();
            if (!window._target.Set(world, e))
                return;
            window._target.EnableDebug(true);

            BTAsset asset = window._target.GetAsset();
            window.ShowWithAsset(asset, e.Index.ToString());
        }

        private void ShowWithAsset(BTAsset asset, string extra_name)
        {
            _node_group.SetAsset(asset);
            _bar.Reset(asset.NodeCount);

            minSize = new Vector2(100, 100);
            maxSize = new Vector2(10000, 10000);

            string asset_name = "BTGraphView";
            if (asset != null)
                asset_name = asset.name;

            if (string.IsNullOrEmpty(extra_name))
                titleContent = new GUIContent($"{asset_name}");
            else
                titleContent = new GUIContent($"{asset_name} | {extra_name}");

            Show();
        }

        private void OnGUI()
        {
            if (Application.isPlaying && !EditorApplication.isPaused && _target.GetDebugRTArray(out var data))
            {
                _bar.AddFrameData(data.FrameIndex, data.Status);
            }

            _bar.Draw();

            _zoomer.Begin();
            _node_group.Draw(-_zoomer.GetContentOffset());
            _zoomer.End();

            Repaint();
        }
    }

    public class EdBTAssetViewer : EditorWindow
    {
        public EdBTNodeGroup _node_group = new EdBTNodeGroup();
        private EditorZoomer _zoomer;
        protected void OnEnable()
        {
            _zoomer = new EditorZoomer();
        }

        [UnityEditor.Callbacks.OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            if (!assetPath.EndsWith(".asset"))
                return false;
            BTAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<BTAsset>(assetPath);
            if (asset == null)
                return false;

            EdBTAssetViewer window = GetWindow<EdBTAssetViewer>();
            window.ShowWithAsset(asset);
            return true;
        }


        private void ShowWithAsset(BTAsset asset)
        {
            _node_group.SetAsset(asset);

            minSize = new Vector2(100, 100);
            maxSize = new Vector2(10000, 10000);

            string asset_name = "BTAsset Viewer";
            if (asset != null)
                asset_name = asset.name;


            titleContent = new GUIContent($"{asset_name} ");

            Show();
        }

        private void OnGUI()
        {
            _zoomer.Begin();
            _node_group.Draw(-_zoomer.GetContentOffset());
            _zoomer.End();
        }
    }
}
