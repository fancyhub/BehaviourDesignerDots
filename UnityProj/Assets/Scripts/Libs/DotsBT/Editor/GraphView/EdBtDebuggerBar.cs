using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT.GraphView.ED
{
    public class EdBtDebuggerBar
    {
        private const int C_COUNT = 200;
        private const float C_VIEW_HEIGHT = 100;

        public Action<EdBtDebuggerFrameData> OnFrameChange;
        private MyFixedQueue<EdBtDebuggerFrameData> _data_queue = new MyFixedQueue<EdBtDebuggerFrameData>(C_COUNT);
        private int _node_count = 0;
        private int _current_select_index = -1;

        public void Reset(int node_count)
        {
            _node_count = node_count;
            _data_queue.Clear();
            _current_select_index = -1;
        }

        public bool AddFrameData(BTDebugStatusArray data)
        {
            if (data.FrameIndex <= _LastFrameIndex(_data_queue))
                return false;

            EdBtDebuggerFrameData frame = _CreateFrameData(data, _node_count);

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

        private static EdBtDebuggerFrameData _CreateFrameData(BTDebugStatusArray data,int nodeCount)
        {
            EdBtDebuggerFrameData frame = new EdBtDebuggerFrameData()
            {
                FrameIndex = data.FrameIndex,
                Status = new EBTStatus[nodeCount],
            };

            Debug.Assert(nodeCount <= data.Count, $"行为树的Debug数组 太小了, {data.Count} < {nodeCount}");
            nodeCount = Math.Min(data.Count, nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                frame.Status[i] = data.GetState(i);
            }
            
            return frame;
        }

        //获取最后一个FrameData的FrameIndex
        private static int _LastFrameIndex(MyFixedQueue<EdBtDebuggerFrameData> queue)
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

}
