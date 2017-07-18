﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Assignment_bt_WrapperAI_Monster_BTMonsterBossInitiative_node31 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int pursuitRange = ((ObjAgent) pAgent).GetPursuitRange();
            pAgent.SetVariable<int>("p_pursuitRange", pursuitRange, 0x332d755);
            return status;
        }
    }
}

