﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Monster_BTMonsterBossPassive_node169 : Action
    {
        private bool method_p0 = true;

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ((ObjAgent) pAgent).SetMonsterEnduranceDown(this.method_p0);
            return EBTStatus.BT_FAILURE;
        }
    }
}

