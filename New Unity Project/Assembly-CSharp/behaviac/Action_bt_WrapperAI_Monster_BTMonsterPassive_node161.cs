﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Action_bt_WrapperAI_Monster_BTMonsterPassive_node161 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ((ObjAgent) pAgent).ResetEndurance();
            return EBTStatus.BT_SUCCESS;
        }
    }
}

