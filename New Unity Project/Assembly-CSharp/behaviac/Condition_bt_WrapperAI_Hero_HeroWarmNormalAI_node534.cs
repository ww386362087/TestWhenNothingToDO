﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Hero_HeroWarmNormalAI_node534 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            uint variable = (uint) pAgent.GetVariable((uint) 0xb8c50879);
            int actorHPPercent = ((ObjAgent) pAgent).GetActorHPPercent(variable);
            int num3 = 0xbb8;
            return ((actorHPPercent > num3) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}
