﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Hero_HeroWarmNormalAI_node372 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            uint variable = (uint) pAgent.GetVariable((uint) 0x3e931a5c);
            return ((ObjAgent) pAgent).IsBase(variable);
        }
    }
}

