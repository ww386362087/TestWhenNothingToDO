﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;
    using UnityEngine;

    internal class Action_bt_WrapperAI_Hero_HeroWarmSimpleAI_node276 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            Vector3 variable = (Vector3) pAgent.GetVariable((uint) 0x53c255c5);
            ((ObjAgent) pAgent).RealMoveDirection(variable);
            return EBTStatus.BT_SUCCESS;
        }
    }
}

