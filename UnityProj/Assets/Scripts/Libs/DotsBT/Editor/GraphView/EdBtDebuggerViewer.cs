using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;

namespace DotsBT.GraphView.ED
{
    public class EdBtDebuggerViewer : EditorWindow
    {
        public EdBtNodeGroup _node_group = new EdBtNodeGroup();
        public EdBtDebuggerBar _bar = new EdBtDebuggerBar();
        private EditorZoomer _zoomer;
        public EdBtDebuggerTarget _target;

        protected void OnEnable()
        {
            _zoomer = new EditorZoomer();
            _target = new EdBtDebuggerTarget();
            _bar.OnFrameChange = _OnFrameDataChange;
        }

        private void _OnFrameDataChange(EdBtDebuggerFrameData data)
        {
            _node_group.UpdateStatus(data.Status);
        }

        public static void ShowGraph(World world, Entity e)
        {
            EdBtDebuggerViewer window = GetWindow<EdBtDebuggerViewer>();
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

            string asset_name = "BT Debugger Viewer";
            if (asset != null)
                asset_name = asset_name + " : " + asset.name;

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
                _bar.AddFrameData(data);
            }

            _bar.Draw();

            _zoomer.Begin();
            _node_group.Draw(-_zoomer.GetContentOffset());
            _zoomer.End();

            Repaint();
        }
    }

}
