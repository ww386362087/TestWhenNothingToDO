﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Hero_HeroWarmNormalAI_node501 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            bool flag = ((ObjAgent) pAgent).IsIgnoreSrchRange();
            bool flag2 = false;
            return ((flag != flag2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

