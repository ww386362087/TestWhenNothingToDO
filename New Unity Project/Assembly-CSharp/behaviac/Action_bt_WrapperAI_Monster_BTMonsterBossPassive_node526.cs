﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Action_bt_WrapperAI_Monster_BTMonsterBossPassive_node526 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            return ((ObjAgent) pAgent).MoveToSkillTarget();
        }
    }
}

