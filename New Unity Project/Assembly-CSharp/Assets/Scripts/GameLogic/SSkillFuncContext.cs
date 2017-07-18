﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Common;
    using ResData;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SSkillFuncContext
    {
        public PoolObjHandle<ActorRoot> inTargetObj;
        public PoolObjHandle<ActorRoot> inOriginator;
        public SkillUseContext inUseContext;
        public ResDT_SkillFunc inSkillFunc;
        public ESkillFuncStage inStage;
        public SSkillFuncIntParam[] LocalParams;
        public PoolObjHandle<Action> inAction;
        public PoolObjHandle<BuffSkill> inBuffSkill;
        public HurtAttackerInfo inCustomData;
        public int inDoCount;
        public int inOverlayCount;
        public bool inLastEffect;
        public int inEffectCount;
        public int inEffectCountInSingleTrigger;
        public int inMarkCount;
        public int iSkillLevel
        {
            get
            {
                if ((this.inBuffSkill != 0) && (this.inBuffSkill.handle.cfgData != null))
                {
                    byte num = (byte) (this.inBuffSkill.handle.cfgData.bGrowthType % 10);
                    PoolObjHandle<ActorRoot> inOriginator = this.inOriginator;
                    SkillSlotType slotType = this.inUseContext.SlotType;
                    if ((inOriginator != 0) && (inOriginator.handle.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster))
                    {
                        MonsterWrapper actorControl = inOriginator.handle.ActorControl as MonsterWrapper;
                        if (actorControl != null)
                        {
                            PoolObjHandle<ActorRoot> hostActor = actorControl.hostActor;
                            if (((hostActor != 0) && (hostActor.handle.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)) && (actorControl.spawnSkillSlot != SkillSlotType.SLOT_SKILL_VALID))
                            {
                                inOriginator = hostActor;
                                slotType = actorControl.spawnSkillSlot;
                            }
                        }
                    }
                    if (num == 0)
                    {
                        if ((inOriginator != 0) && (inOriginator.handle.SkillControl != null))
                        {
                            SkillSlot skillSlot = null;
                            skillSlot = inOriginator.handle.SkillControl.GetSkillSlot(slotType);
                            if (skillSlot != null)
                            {
                                return skillSlot.GetSkillLevel();
                            }
                            if (inOriginator.handle.ValueComponent != null)
                            {
                                return inOriginator.handle.ValueComponent.actorSoulLevel;
                            }
                        }
                    }
                    else if (num == 1)
                    {
                        if ((inOriginator != 0) && (inOriginator.handle.ValueComponent != null))
                        {
                            return inOriginator.handle.ValueComponent.actorSoulLevel;
                        }
                    }
                    else if (((num > 1) && (num <= 4)) && ((inOriginator != 0) && (inOriginator.handle.SkillControl != null)))
                    {
                        SkillSlot slot2 = null;
                        slot2 = inOriginator.handle.SkillControl.GetSkillSlot((SkillSlotType) (num - 1));
                        if (slot2 != null)
                        {
                            return slot2.GetSkillLevel();
                        }
                        if (inOriginator.handle.ValueComponent != null)
                        {
                            return inOriginator.handle.ValueComponent.actorSoulLevel;
                        }
                    }
                }
                return 1;
            }
        }
        public int iSkillFuncInterval
        {
            get
            {
                int num = 1;
                if ((this.inBuffSkill != 0) && (this.inBuffSkill.handle.cfgData != null))
                {
                    num = this.inBuffSkill.handle.cfgData.bGrowthType / 10;
                    if (num == 0)
                    {
                        num = 1;
                    }
                }
                return num;
            }
        }
        public void InitSkillFuncContext()
        {
        }

        public int GetSkillFuncParam(int _index, bool _bGrow)
        {
            if ((_index < 0) || ((_index + 1) > 8))
            {
                object[] inParameters = new object[] { _index };
                DebugHelper.Assert(false, "GetSkillFuncParam: index = {0}", inParameters);
            }
            if (!_bGrow)
            {
                return this.inSkillFunc.astSkillFuncParam[_index].iParam;
            }
            int iParam = this.inSkillFunc.astSkillFuncParam[_index].iParam;
            int num3 = this.inSkillFunc.astSkillFuncGroup[_index].iParam * ((this.iSkillLevel - 1) / this.iSkillFuncInterval);
            iParam += num3;
            if (this.inMarkCount != 0)
            {
                return (iParam * this.inMarkCount);
            }
            return (iParam * this.inOverlayCount);
        }
    }
}

