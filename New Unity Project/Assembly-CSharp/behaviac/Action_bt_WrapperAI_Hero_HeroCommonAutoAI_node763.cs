﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Hero_HeroCommonAutoAI_node763 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            int variable = (int) pAgent.GetVariable((uint) 0x921d0d6a);
            return ((ObjAgent) pAgent).MoveToCommonAttackTargetWithRange(variable);
        }
    }
}

