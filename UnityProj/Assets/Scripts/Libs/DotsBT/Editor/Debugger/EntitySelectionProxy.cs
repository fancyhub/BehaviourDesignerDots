using System;
using UnityEditor;
using UnityEngine;
using Unity.Entities;

namespace DotsBT.ED
{
    //Ref Unity.Entities.Editor.EntitySelectionProxy
    public class EntitySelectionProxy
    {

        public static Type _T;

        private static Type _GetT()
        {
            if (_T != null)
                return _T;
            
            foreach(var p in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t= p.GetType("Unity.Entities.Editor.EntitySelectionProxy");
                if (t == null)
                    continue;
                _T = t;
                break;
            }
            return _T;
        }


        private static System.Reflection.MethodInfo _MethodCreateInstance;

        public static UnityEngine.ScriptableObject CreateInstance(World world, Entity e)
        {
            if(_MethodCreateInstance==null)
            {
                Type t = _GetT();
                if (t == null)
                    return null;
                _MethodCreateInstance = t.GetMethod("CreateInstance");
            }

            if (_MethodCreateInstance == null)
                return null;

            var result = _MethodCreateInstance.Invoke(null, new object[] { world, e });
            return result as UnityEngine.ScriptableObject;
        }
    }
}