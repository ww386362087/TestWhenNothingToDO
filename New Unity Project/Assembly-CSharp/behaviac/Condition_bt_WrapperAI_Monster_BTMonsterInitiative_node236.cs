﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Monster_BTMonsterInitiative_node236 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            bool flag = ((ObjAgent) pAgent).IsNeedToHelpOther();
            bool flag2 = true;
            return ((flag != flag2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

