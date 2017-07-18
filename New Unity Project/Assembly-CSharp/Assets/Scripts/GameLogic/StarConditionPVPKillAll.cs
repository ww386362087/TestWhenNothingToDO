﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.GameLogic.GameKernal;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections.Generic;

    [StarConditionAttrContext(6)]
    internal class StarConditionPVPKillAll : StarCondition
    {
        private bool bHasComplete;
        private COM_PLAYERCAMP CachedSelfCamp;

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Initialize(ResDT_ConditionInfo InConditionInfo)
        {
            base.Initialize(InConditionInfo);
        }

        public override void OnActorDeath(ref GameDeadEventParam prm)
        {
            if ((!this.bHasComplete && (prm.src != 0)) && (prm.src.handle.TheActorMeta.ActorCamp != this.CachedSelfCamp))
            {
                List<Player>.Enumerator enumerator = Singleton<GamePlayerCenter>.instance.GetAllPlayers().GetEnumerator();
                bool flag = true;
                while (enumerator.MoveNext())
                {
                    Player current = enumerator.Current;
                    if ((current.PlayerCamp != this.CachedSelfCamp) && !current.IsAllHeroesDead())
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag && !this.bHasComplete)
                {
                    this.bHasComplete = true;
                    this.TriggerChangedEvent();
                }
            }
        }

        public override void Start()
        {
            base.Start();
            this.CachedSelfCamp = Singleton<GamePlayerCenter>.instance.GetHostPlayer().PlayerCamp;
        }

        public override StarEvaluationStatus status
        {
            get
            {
                return (!this.bHasComplete ? StarEvaluationStatus.InProgressing : StarEvaluationStatus.Success);
            }
        }

        public override int[] values
        {
            get
            {
                return new int[] { (!this.bHasComplete ? 0 : 1) };
            }
        }
    }
}

