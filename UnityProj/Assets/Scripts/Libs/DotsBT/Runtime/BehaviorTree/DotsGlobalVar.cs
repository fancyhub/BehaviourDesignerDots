using BehaviorDesigner.Runtime;
using Unity.Entities;
using UnityEngine;

namespace DotsBT
{
    public class DotsGlobalVar : MonoBehaviour
    {
        public const string CPath = "BtDots/BehaviorDesignerGlobalVariables";

        public GlobalVariables variables;


        public static DotsGlobalVar Load()
        {
            var ret= Resources.Load<DotsGlobalVar>(CPath);
            if(ret  ==null)
            {
                Debug.LogError("Cant find global var prefab: "+ CPath+".prefab");
            }
            return ret;            
        }
    }

    public class DotsGlobalVarBaker : Baker<DotsGlobalVar>
    {
        public override void Bake(DotsGlobalVar authoring)
        {
            try
            {
                var sharedComp = new BTGlobalCompData();
                sharedComp.Vars = BTBaker.BakeGlobalVars(this, authoring.variables.GetAllVariables());
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), sharedComp);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Bake 行为树的全局变量的时候出错", authoring);
                UnityEngine.Debug.LogException(e);
                return;
            }
        }
    }
}