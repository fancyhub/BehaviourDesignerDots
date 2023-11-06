//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.Entities;
//using Unity.Collections;
//using GGame;

//namespace DotsBT
//{
//    public unsafe struct BTVarDict
//    {
//        private MyBlobArray<BTBlobVarItemIndex> _name;

//        public static BTVarDict CreateShared(BTBlobVars blob_vars)
//        {
//            BTVarDict ret = new BTVarDict();
//            ret._name = blob_vars.NameList;            
//            return ret;
//        }

//        public static BTVarDict CreateGlobal(BTBlobVars blob_vars)
//        {
//            BTVarDict ret = new BTVarDict();
//            ret._name = blob_vars.NameList;            

//            return ret;
//        }

//        public BTPtr FindItem(int name_id)
//        {
//            for (int i = 0; i < _name.Length; i++)
//            {
//                if (_name[i].NameId == name_id)
//                    return _name[i].Ptr;
//            }
//            return BTPtr.Null;
//        }         
//    }
//}
