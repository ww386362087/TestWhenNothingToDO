﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Condition_bt_WrapperAI_Hero_HeroWarmSimpleAI_node61 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ObjBehaviMode curBehavior = ((ObjAgent) pAgent).GetCurBehavior();
            ObjBehaviMode mode2 = ObjBehaviMode.State_Idle;
            return ((curBehavior != mode2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

