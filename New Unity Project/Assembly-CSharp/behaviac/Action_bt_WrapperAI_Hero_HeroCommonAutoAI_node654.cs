﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Action_bt_WrapperAI_Hero_HeroCommonAutoAI_node654 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            return ((ObjAgent) pAgent).IsFollowOtherPlayer();
        }
    }
}

