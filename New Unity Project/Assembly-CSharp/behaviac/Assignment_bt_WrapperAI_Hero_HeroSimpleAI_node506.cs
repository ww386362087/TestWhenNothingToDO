﻿namespace behaviac
{
    using System;

    internal class Assignment_bt_WrapperAI_Hero_HeroSimpleAI_node506 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int num = 15;
            pAgent.SetVariable<int>("p_useSkillWeight", num, 0x105720bd);
            return status;
        }
    }
}

