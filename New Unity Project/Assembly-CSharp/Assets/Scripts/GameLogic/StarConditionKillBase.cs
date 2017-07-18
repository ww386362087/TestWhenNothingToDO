﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Common;
    using ResData;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public abstract class StarConditionKillBase : StarCondition
    {
        [CompilerGenerated]
        private int <killCnt>k__BackingField;
        protected bool bCachedResult;
        protected PoolObjHandle<ActorRoot> CachedAttacker;
        protected PoolObjHandle<ActorRoot> CachedSource;

        protected StarConditionKillBase()
        {
        }

        protected virtual bool CheckResult()
        {
            bool flag = SmartCompare.Compare<int>(this.killCnt, this.targetCount, this.operation);
            if (flag != this.bCachedResult)
            {
                this.bCachedResult = flag;
                this.OnResultStateChanged();
                return true;
            }
            return false;
        }

        protected void DetachEventListener()
        {
            Singleton<GameEventSys>.instance.RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_BeginFightOver, new RefAction<DefaultGameEventParam>(this.onFightOver));
        }

        public override void Dispose()
        {
            this.DetachEventListener();
            this.killCnt = 0;
            base.Dispose();
        }

        public override bool GetActorRef(out PoolObjHandle<ActorRoot> OutSource, out PoolObjHandle<ActorRoot> OutAttacker)
        {
            OutSource = this.CachedSource;
            OutAttacker = this.CachedAttacker;
            return true;
        }

        public override void Initialize(ResDT_ConditionInfo InConditionInfo)
        {
            base.Initialize(InConditionInfo);
            Singleton<GameEventSys>.instance.AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_BeginFightOver, new RefAction<DefaultGameEventParam>(this.onFightOver));
            this.killCnt = 0;
            this.bCachedResult = SmartCompare.Compare<int>(this.killCnt, this.targetCount, this.operation);
        }

        public override void OnActorDeath(ref GameDeadEventParam prm)
        {
            this.CachedSource = prm.src;
            this.CachedAttacker = prm.orignalAtker;
            if ((((prm.src != 0) && (prm.orignalAtker != 0)) && (this.ShouldCare(prm.src.handle) && (prm.src.handle.TheActorMeta.ActorType == this.targetType))) && ((this.targetID == 0) || (prm.src.handle.TheActorMeta.ConfigId == this.targetID)))
            {
                this.killCnt++;
                this.CheckResult();
                this.OnStatChanged();
            }
        }

        protected virtual void onFightOver(ref DefaultGameEventParam prm)
        {
            this.CheckResult();
        }

        protected virtual void OnResultStateChanged()
        {
        }

        protected virtual void OnStatChanged()
        {
        }

        protected virtual bool ShouldCare(ActorRoot src)
        {
            if (this.isSelfCamp)
            {
                return src.IsHostCamp();
            }
            return !src.IsHostCamp();
        }

        public override string description
        {
            get
            {
                return string.Format("[{0}/{1}]", (this.killCnt <= this.targetCount) ? this.killCnt : this.targetCount, this.targetCount);
            }
        }

        protected abstract bool isSelfCamp { get; }

        public int killCnt
        {
            [CompilerGenerated]
            get
            {
                return this.<killCnt>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<killCnt>k__BackingField = value;
            }
        }

        public override StarEvaluationStatus status
        {
            get
            {
                return (!this.bCachedResult ? StarEvaluationStatus.Failure : StarEvaluationStatus.Success);
            }
        }

        protected abstract int targetCount { get; }

        protected abstract int targetID { get; }

        protected abstract ActorTypeDef targetType { get; }

        public override int[] values
        {
            get
            {
                return new int[] { this.killCnt };
            }
        }
    }
}

