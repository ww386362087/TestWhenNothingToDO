﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Assignment_bt_WrapperAI_Soldier_BTSoldierPro_node106 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int sightArea = ((ObjAgent) pAgent).GetSightArea();
            pAgent.SetVariable<int>("p_srchRange", sightArea, 0x921d0d6a);
            return status;
        }
    }
}

