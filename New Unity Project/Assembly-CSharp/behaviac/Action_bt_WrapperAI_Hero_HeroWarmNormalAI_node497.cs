﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Hero_HeroWarmNormalAI_node497 : Action
    {
        private bool method_p0 = false;

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ((ObjAgent) pAgent).SetIgnoreSrchrange(this.method_p0);
            return EBTStatus.BT_SUCCESS;
        }
    }
}

