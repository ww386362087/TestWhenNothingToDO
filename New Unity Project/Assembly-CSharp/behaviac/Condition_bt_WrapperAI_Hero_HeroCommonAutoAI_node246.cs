﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Hero_HeroCommonAutoAI_node246 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            bool flag = ((ObjAgent) pAgent).HasMoveCMD();
            bool flag2 = true;
            return ((flag != flag2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

