﻿namespace behaviac
{
    using System;

    internal class Assignment_bt_WrapperAI_Hero_HeroWarmNormalAI_node396 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int num = 0x32c8;
            pAgent.SetVariable<int>("p_escapeRange", num, 0x2cb8695e);
            return status;
        }
    }
}

