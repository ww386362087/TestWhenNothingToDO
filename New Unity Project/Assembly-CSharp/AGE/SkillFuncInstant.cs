﻿namespace AGE
{
    using Assets.Scripts.Common;
    using Assets.Scripts.GameLogic;
    using ResData;
    using System;

    [EventCategory("MMGame/SkillFunc")]
    public class SkillFuncInstant : TickCondition
    {
        private bool m_bSucceeded;
        public RES_SKILLFUNC_TYPE SkillFuncType;

        public override bool Check(Action _action, Track _track)
        {
            return this.m_bSucceeded;
        }

        public override BaseEvent Clone()
        {
            SkillFuncInstant instant = ClassObjPool<SkillFuncInstant>.Get();
            instant.CopyData(this);
            return instant;
        }

        protected override void CopyData(BaseEvent src)
        {
            base.CopyData(src);
            SkillFuncInstant instant = src as SkillFuncInstant;
            this.SkillFuncType = instant.SkillFuncType;
            this.m_bSucceeded = instant.m_bSucceeded;
        }

        public override void OnUse()
        {
            base.OnUse();
            this.SkillFuncType = RES_SKILLFUNC_TYPE.RES_SKILLFUNC_TYPE_PHYSHURT;
            this.m_bSucceeded = false;
        }

        public override void Process(Action _action, Track _track)
        {
            PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(1);
            PoolObjHandle<ActorRoot> handle2 = _action.GetActorHandle(0);
            SkillUseContext refParamObject = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
            BuffSkill skill = _action.refParams.GetRefParamObject<BuffSkill>("SkillObj");
            ResDT_SkillFunc outSkillFunc = null;
            if ((skill != null) && skill.FindSkillFunc((int) this.SkillFuncType, out outSkillFunc))
            {
                SSkillFuncContext inContext = new SSkillFuncContext();
                inContext.inTargetObj = actorHandle;
                inContext.inOriginator = handle2;
                inContext.inUseContext = refParamObject;
                inContext.inSkillFunc = outSkillFunc;
                inContext.inStage = ESkillFuncStage.Enter;
                inContext.inAction = new PoolObjHandle<Action>(_action);
                inContext.inBuffSkill = new PoolObjHandle<BuffSkill>(skill);
                inContext.inOverlayCount = skill.GetOverlayCount();
                inContext.inLastEffect = false;
                if (refParamObject != null)
                {
                    inContext.inEffectCount = refParamObject.EffectCount;
                    inContext.inEffectCountInSingleTrigger = refParamObject.EffectCountInSingleTrigger;
                    inContext.inMarkCount = refParamObject.MarkCount;
                }
                inContext.InitSkillFuncContext();
                skill.SetBuffLevel(inContext.iSkillLevel);
                inContext.LocalParams = new SSkillFuncIntParam[8];
                for (int i = 0; i < 8; i++)
                {
                    inContext.LocalParams[i] = new SSkillFuncIntParam();
                    inContext.LocalParams[i].iParam = 0;
                }
                if (!Singleton<BattleLogic>.GetInstance().isRuning || Singleton<BattleLogic>.GetInstance().isGameOver)
                {
                    return;
                }
                this.m_bSucceeded = Singleton<SkillFuncDelegator>.GetInstance().DoSkillFunc((int) this.SkillFuncType, ref inContext);
            }
            base.Process(_action, _track);
        }

        public override bool SupportEditMode()
        {
            return true;
        }
    }
}

