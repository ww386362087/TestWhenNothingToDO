﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Monster_BTMonsterPassiveThief_node69 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            bool flag = ((ObjAgent) pAgent).IsAttackByEnemy();
            bool flag2 = true;
            return ((flag != flag2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

