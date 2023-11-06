using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace DotsBT
{
    [CreateAssetMenu(menuName="Behavior Designer/External Behavior Tree(Dots)", fileName= "NewExternalBehavior")]
    [System.Serializable]
    public class DotsExternalBehavior: ExternalBehavior
    {
        public Unity.Entities.Content.WeakObjectReference<BTAsset> RefDotsBTAsset;
    }
}
