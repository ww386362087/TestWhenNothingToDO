﻿namespace Assets.Scripts.GameLogic
{
    using AGE;
    using Assets.Scripts.Common;
    using Pathfinding;
    using System;
    using UnityEngine;

    public class TriggerActionBlockSwitcher : TriggerActionBase
    {
        public TriggerActionBlockSwitcher(TriggerActionWrapper inWrapper, int inTriggerId) : base(inWrapper, inTriggerId)
        {
        }

        public override RefParamOperator TriggerEnter(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
        {
            foreach (GameObject obj2 in base.RefObjList)
            {
                if (obj2 != null)
                {
                    NavmeshCut componentInChildren = obj2.GetComponentInChildren<NavmeshCut>();
                    if (componentInChildren != null)
                    {
                        componentInChildren.set_enabled(base.bEnable);
                    }
                }
            }
            return null;
        }

        public override void TriggerLeave(PoolObjHandle<ActorRoot> src, ITrigger inTrigger)
        {
            if (base.bStopWhenLeaving)
            {
                foreach (GameObject obj2 in base.RefObjList)
                {
                    if (obj2 != null)
                    {
                        NavmeshCut componentInChildren = obj2.GetComponentInChildren<NavmeshCut>();
                        if (componentInChildren != null)
                        {
                            componentInChildren.set_enabled(!base.bEnable);
                        }
                    }
                }
            }
        }
    }
}

