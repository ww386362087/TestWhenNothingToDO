﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;
    using UnityEngine;

    internal class Action_bt_WrapperAI_Hero_HeroGuideFollow_node460 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            Vector3 variable = (Vector3) pAgent.GetVariable((uint) 0x9bc0c9a2);
            ((ObjAgent) pAgent).RealMovePosition(variable);
            return EBTStatus.BT_SUCCESS;
        }
    }
}

