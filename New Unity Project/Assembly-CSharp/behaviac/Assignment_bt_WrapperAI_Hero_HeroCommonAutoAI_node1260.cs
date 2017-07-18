﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Assignment_bt_WrapperAI_Hero_HeroCommonAutoAI_node1260 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            uint variable = (uint) pAgent.GetVariable((uint) 0x7e66728f);
            uint givenActorTarget = ((ObjAgent) pAgent).GetGivenActorTarget(variable);
            pAgent.SetVariable<uint>("p_captainTargetID", givenActorTarget, 0x98af4863);
            return status;
        }
    }
}

