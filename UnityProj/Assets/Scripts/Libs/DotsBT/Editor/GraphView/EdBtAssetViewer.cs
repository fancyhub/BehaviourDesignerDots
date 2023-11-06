using System;
using UnityEditor;
using UnityEngine;

namespace DotsBT.GraphView.ED
{
    public class EdBtAssetViewer : EditorWindow
    {
        public EdBtNodeGroup _node_group = new EdBtNodeGroup();
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

            EdBtAssetViewer window = GetWindow<EdBtAssetViewer>();
            window.ShowWithAsset(asset);
            return true;
        }


        private void ShowWithAsset(BTAsset asset)
        {
            _node_group.SetAsset(asset);

            minSize = new Vector2(100, 100);
            maxSize = new Vector2(10000, 10000);

            string title_name = "BtAsset Viewer";
            if (asset != null)
                title_name = title_name  + " : "+ asset.name;


            titleContent = new GUIContent($"{title_name} ");

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
