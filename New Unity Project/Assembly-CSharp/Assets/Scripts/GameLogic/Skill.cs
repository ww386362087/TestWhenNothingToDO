﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Common;
    using Assets.Scripts.Framework;
    using ResData;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Skill : BaseSkill
    {
        [CompilerGenerated]
        private SkillSlotType <SlotType>k__BackingField;
        public SkillRangeAppointType AppointType;
        private ResBattleParam battleParam;
        public bool bDelayAbortSkill;
        public bool bProtectAbortSkill;
        public ResSkillCfgInfo cfgData;
        public string EffectPrefabName;
        public string EffectWarnPrefabName;
        public string FixedPrefabName;
        public string FixedWarnPrefabName;
        public string GuidePrefabName;
        public string GuideWarnPrefabName;
        public string IconName;
        public SkillAbort skillAbort;
        public float SkillCD;
        public int SkillCost;

        public Skill(int id)
        {
            base.SkillID = id;
            this.skillAbort = new SkillAbort();
            this.skillAbort.InitAbort(false);
            this.bDelayAbortSkill = false;
            this.bProtectAbortSkill = false;
            this.cfgData = GameDataMgr.skillDatabin.GetDataByKey((long) id);
            if (this.cfgData != null)
            {
                base.ActionName = StringHelper.UTF8BytesToString(ref this.cfgData.szPrefab);
                object[] inParameters = new object[] { id };
                DebugHelper.Assert(base.ActionName != null, "Action name is null in skill databin id = {0}", inParameters);
                this.GuidePrefabName = StringHelper.UTF8BytesToString(ref this.cfgData.szGuidePrefab);
                this.GuideWarnPrefabName = StringHelper.UTF8BytesToString(ref this.cfgData.szGuideWarnPrefab);
                this.EffectPrefabName = StringHelper.UTF8BytesToString(ref this.cfgData.szEffectPrefab);
                this.EffectWarnPrefabName = StringHelper.UTF8BytesToString(ref this.cfgData.szEffectWarnPrefab);
                this.FixedPrefabName = StringHelper.UTF8BytesToString(ref this.cfgData.szFixedPrefab);
                this.FixedWarnPrefabName = StringHelper.UTF8BytesToString(ref this.cfgData.szFixedWarnPrefab);
                this.IconName = StringHelper.UTF8BytesToString(ref this.cfgData.szIconPath);
                this.SkillCD = 5f;
                this.AppointType = (SkillRangeAppointType) this.cfgData.bRangeAppointType;
                base.bAgeImmeExcute = this.cfgData.bAgeImmeExcute == 1;
            }
            this.battleParam = GameDataMgr.battleParam.GetAnyData();
        }

        public bool canAbort(SkillAbortType _type)
        {
            return this.skillAbort.Abort(_type);
        }

        public int GetMaxSearchDistance(int level = 0)
        {
            if (this.cfgData.iMaxSearchDistanceGrowthValue == 0)
            {
                return this.cfgData.iMaxSearchDistance;
            }
            int num = level - 1;
            if (num < 0)
            {
                num = 0;
            }
            return (this.cfgData.iMaxSearchDistance + (this.cfgData.iMaxSearchDistanceGrowthValue * num));
        }

        private void SetSkillSpeed(PoolObjHandle<ActorRoot> user)
        {
            int totalValue = 0;
            int num2 = 0;
            int num3 = 0;
            ValueDataInfo info = null;
            if (base.curAction != 0)
            {
                info = user.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_PROPERTY_ATTACKSPEED];
                totalValue = info.totalValue;
                num3 = (int) ((totalValue + (user.handle.ValueComponent.mActorValue.actorLvl * this.battleParam.dwM_AttackSpeed)) + this.battleParam.dwN_AttackSpeed);
                if (num3 != 0)
                {
                    num2 = (totalValue * 0x2710) / num3;
                }
                num2 += user.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_FUNCEFT_ATKSPDADD].totalValue;
                if ((this.cfgData != null) && (this.cfgData.bNoInfluenceAnim == 1))
                {
                    num2 = 0;
                }
                VFactor factor = new VFactor((long) (0x2710 + num2), 0x2710L);
                this.curAction.handle.SetPlaySpeed(factor);
            }
        }

        public int SkillEnergyCost(PoolObjHandle<ActorRoot> Actor, int CurSkillLevel)
        {
            if ((this.cfgData != null) && (this.cfgData.bEnergyCostType != 1))
            {
                if (this.cfgData.bEnergyCostCalcType == 0)
                {
                    return (((int) this.cfgData.iEnergyCost) + ((CurSkillLevel - 1) * this.cfgData.iEnergyCostGrowth));
                }
                if (this.cfgData.bEnergyCostCalcType == 1)
                {
                    int actorEpTotal = Actor.handle.ValueComponent.actorEpTotal;
                    long num3 = (((long) (this.cfgData.iEnergyCost + ((CurSkillLevel - 1) * this.cfgData.iEnergyCostGrowth))) * actorEpTotal) / 0x2710L;
                    return (int) num3;
                }
                if (this.cfgData.bEnergyCostCalcType == 2)
                {
                    int actorEp = Actor.handle.ValueComponent.actorEp;
                    long num5 = (((long) (this.cfgData.iEnergyCost + ((CurSkillLevel - 1) * this.cfgData.iEnergyCostGrowth))) * actorEp) / 0x2710L;
                    return (int) num5;
                }
            }
            return 0;
        }

        public override bool Use(PoolObjHandle<ActorRoot> user, ref SkillUseParam param)
        {
            param.SetOriginator(user);
            param.Instigator = user;
            this.skillAbort.InitAbort(false);
            this.bDelayAbortSkill = false;
            this.bProtectAbortSkill = false;
            param.bExposing = this.bExposing;
            if (!base.Use(user, ref param))
            {
                return false;
            }
            if (param.SlotType == SkillSlotType.SLOT_SKILL_0)
            {
                this.SetSkillSpeed(user);
            }
            return true;
        }

        public bool bExposing
        {
            get
            {
                return (this.cfgData.bSkillNonExposing == 0);
            }
        }

        public SkillSlotType SlotType
        {
            [CompilerGenerated]
            get
            {
                return this.<SlotType>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SlotType>k__BackingField = value;
            }
        }
    }
}

