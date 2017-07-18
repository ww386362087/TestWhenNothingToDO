﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Organ_CrystalWithAttack_node4 : Action
    {
        private string method_p0 = "CrystalBrokenAg";

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            return ((ObjAgent) pAgent).PlayHelperAgeAction(this.method_p0);
        }
    }
}

