using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotsBT
{
    [Serializable]
    public struct GResGUID<T> : IEquatable<GResGUID<T>> where T : UnityEngine.Object
    {
        public Unity.Entities.Hash128 AssetGUID;

        public override string ToString()
        {
            return AssetGUID.ToString();
        }

        public bool Equals(GResGUID<T> other)
        {
            return AssetGUID.Equals(other.AssetGUID);
        }
#if UNITY_EDITOR

        public bool EdSet(T obj)
        {
            if (obj == null)
            {
                AssetGUID = default;
                return true;
            }

            string path = UnityEditor.AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
                return false;

            string str_guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
            AssetGUID = new Unity.Entities.Hash128(str_guid);            
            return true;
        }

        public bool EdIsMissing()
        {
            if (!AssetGUID.IsValid)
                return false;
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(AssetGUID.ToString());
            if (string.IsNullOrEmpty(path))
                return true;

            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path) == null;
        }

        public T EdLoad()
        {
            if (!AssetGUID.IsValid)
                return null;

            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(AssetGUID.ToString());
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }

#endif
    }

    [Serializable]
    public class BTAsset : ScriptableObject
    {
        public GResGUID<DotsExternalBehavior> Orig;
        public BTAssetData Data;

        public int NodeCount
        {
            get
            {
                if (Data == null)
                    return 0;
                if (Data.NodeName == null)
                    return 0;
                return Data.NodeName.Length;
            }
        }
    }

    [Serializable]
    public struct BTAssetVar
    {
        public string Name;
        public int NameId;
        public EBTVar VarMeta;
        public int ValueSize;
        public EBTVarScope Scope;
        public BTPtr LinkPtr;//这个是一个链表的头, 动态link的时候,需要把所有的Next LinkPtr 都指向 SharedVar/GlobalVar的值
    }

    [Serializable]
    public class BTAssetData
    {
        public string Name;

        [HideInInspector]
        public BTPtr RootTask;
        [HideInInspector]
        public byte[] Data;

        public string[] NodeName;

        public BTAssetVar[] UsedVarList;
    }
}
