﻿namespace Assets.Scripts.GameLogic.Treasure
{
    using Assets.Scripts.Common;
    using Assets.Scripts.GameLogic;
    using ResData;
    using System;
    using System.Runtime.CompilerServices;

    internal abstract class BaseStrategy : ITreasureChestStrategy
    {
        protected int DropedCount;
        protected int MaxCount;

        public event OnDropTreasureChestDelegate OnDropTreasure;

        protected BaseStrategy()
        {
        }

        public ResMonsterCfgInfo FindMonsterConfig(int ConfigID)
        {
            return MonsterDataHelper.GetDataCfgInfoByCurLevelDiff(ConfigID);
        }

        protected virtual void FinishDrop()
        {
            while (this.DropedCount < this.MaxCount)
            {
                this.PlayDrop();
            }
        }

        public virtual void Initialize(int InMaxCount)
        {
            this.MaxCount = InMaxCount;
            this.DropedCount = 0;
            DebugHelper.Assert(this.MaxCount < 0x80, "你tm在逗我？");
        }

        public virtual void NotifyDropEvent(PoolObjHandle<ActorRoot> actor)
        {
        }

        public virtual void NotifyMatchEnd()
        {
            if (!Singleton<StarSystem>.instance.isFailure)
            {
                this.FinishDrop();
            }
            this.MaxCount = 0;
            this.DropedCount = 0;
        }

        public virtual void PlayDrop()
        {
            this.DropedCount++;
            DebugHelper.Assert(this.DropedCount <= this.maxCount, "尼玛你是认真的么？");
            if (this.OnDropTreasure != null)
            {
                this.OnDropTreasure();
            }
        }

        public virtual void Stop()
        {
        }

        public int droppedCount
        {
            get
            {
                return this.DropedCount;
            }
        }

        protected virtual bool hasRemain
        {
            get
            {
                return (this.DropedCount < this.maxCount);
            }
        }

        public virtual bool isSupportDrop
        {
            get
            {
                return false;
            }
        }

        public int maxCount
        {
            get
            {
                return this.MaxCount;
            }
        }
    }
}

