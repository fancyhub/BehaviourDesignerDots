using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DotsBT.ED
{
    public static class EditorBtDotsCreator
    {
        [MenuItem("Tools/Behaivor Designer(Dots)/Create Global Var Prefab")]
        public static void CreateGlobalVarPrefab()
        {
            var asset = Resources.Load<DotsGlobalVar>(DotsGlobalVar.CPath);
            if (asset != null)
                return;


            var globalVar = BehaviorDesigner.Runtime.GlobalVariables.Instance;
            if (globalVar == null)
            {
                Debug.LogError("Can't find global  Behaivor Designer Global Vars");
                return;
            }

            
            string path = "Assets/Resources/" + DotsGlobalVar.CPath + ".prefab";

            if (!_CreateFolder(System.IO.Path.GetDirectoryName(path)))
                return;

            var gameobject = new GameObject();
            asset = gameobject.AddComponent<DotsGlobalVar>();
            asset.variables = globalVar;
            PrefabUtility.SaveAsPrefabAsset(gameobject, path);
            GameObject.DestroyImmediate(gameobject);

            Debug.Log("Create Succ " + path);
        }

        private static bool _CreateFolder(string path)
        {
            if (System.IO.Directory.Exists(path))
                return true;

            if (string.IsNullOrEmpty(path))
                return false;

            if (!_CreateFolder(System.IO.Path.GetDirectoryName(path)))
                return false;

            System.IO.Directory.CreateDirectory(path);
            return true;
        }
    }
}
