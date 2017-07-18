﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Assignment_bt_WrapperAI_Hero_HeroWarmSimpleAI_node363 : Assignment
    {
        private int opr_p0 = 0xbb8;

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int enemyCountInRange = ((ObjAgent) pAgent).GetEnemyCountInRange(this.opr_p0);
            pAgent.SetVariable<int>("p_enemyCount", enemyCountInRange, 0x343447);
            return status;
        }
    }
}

