﻿namespace behaviac
{
    using System;

    internal class Compute_bt_WrapperAI_Hero_HeroGuideFollowNew_node1045 : Compute
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int variable = (int) pAgent.GetVariable((uint) 0x13cef34);
            int num2 = 10;
            int num3 = variable + num2;
            pAgent.SetVariable<int>("p_waitToPlayBornAge", num3, 0x13cef34);
            return status;
        }
    }
}

