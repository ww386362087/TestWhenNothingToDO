﻿namespace behaviac
{
    using System;

    internal class Assignment_bt_WrapperAI_Hero_HeroWarmNormalAI_node242 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            bool flag = false;
            pAgent.SetVariable<bool>("p_needFarm", flag, 0x57de6687);
            return status;
        }
    }
}

