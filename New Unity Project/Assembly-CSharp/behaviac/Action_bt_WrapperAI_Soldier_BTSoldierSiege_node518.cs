﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Action_bt_WrapperAI_Soldier_BTSoldierSiege_node518 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ((ObjAgent) pAgent).SetTauntMeActorAsMyTarget();
            return EBTStatus.BT_SUCCESS;
        }
    }
}

