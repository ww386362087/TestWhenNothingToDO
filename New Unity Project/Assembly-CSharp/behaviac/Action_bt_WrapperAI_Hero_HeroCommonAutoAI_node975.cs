﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Hero_HeroCommonAutoAI_node975 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            int variable = (int) pAgent.GetVariable((uint) 0x56cb5020);
            ((ObjAgent) pAgent).SetInDanger(variable);
            return EBTStatus.BT_SUCCESS;
        }
    }
}

