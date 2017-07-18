﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Condition_bt_WrapperAI_Organ_OrganTurret_node9 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ObjBehaviMode curBehavior = ((ObjAgent) pAgent).GetCurBehavior();
            ObjBehaviMode mode2 = ObjBehaviMode.State_Dead;
            return ((curBehavior != mode2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

