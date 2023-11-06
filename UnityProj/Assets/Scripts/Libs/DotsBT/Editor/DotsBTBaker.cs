using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DotsBT.ED
{
    public static class DotsBTBaker
    {
        [MenuItem("Tools/Behaivor Designer(Dots)/ReBake All BtAssets",priority =300)]
        public static void BakeAll()
        {
            string[] all_guilds = AssetDatabase.FindAssets("t:BTAsset", new string[] { "Assets/" });
            foreach (var asset_guid in all_guilds)
            {
                string path = AssetDatabase.GUIDToAssetPath(asset_guid);
                BTAsset asset = AssetDatabase.LoadAssetAtPath<BTAsset>(path);

                try
                {
                    var orig = asset.Orig.EdLoad();

                    asset.Data = BTBaker.BakeAsset(orig);
                    asset.Orig.EdSet(orig);
                    EditorUtility.SetDirty(asset);

                    Debug.Log("ReBake " + path);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Bake 失败 {asset.name}", asset);
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }
}
