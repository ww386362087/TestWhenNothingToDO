﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Hero_HeroWarmSimpleAI_node350 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            int hPPercent = ((ObjAgent) pAgent).GetHPPercent();
            int num2 = 0x1388;
            return ((hPPercent >= num2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

