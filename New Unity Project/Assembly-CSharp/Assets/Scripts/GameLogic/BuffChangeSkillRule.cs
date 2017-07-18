﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Common;
    using System;
    using System.Runtime.InteropServices;

    public class BuffChangeSkillRule
    {
        private BuffHolderComponent buffHolder;
        private Assets.Scripts.GameLogic.ChangeSkillSlot[] changeSkillSlot = new Assets.Scripts.GameLogic.ChangeSkillSlot[10];
        private PoolObjHandle<ActorRoot> sourceActor;

        public void ChangeSkillSlot(SkillSlotType _slotType, int _skillID, int _orgSkillID = 0)
        {
            int skillID = 0;
            int num2 = 0;
            SkillSlot slot = null;
            if (this.sourceActor.handle.SkillControl.TryGetSkillSlot(_slotType, out slot))
            {
                int ornamentFirstSwitchCd = 0;
                if (_slotType == SkillSlotType.SLOT_SKILL_7)
                {
                    SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
                    if (curLvelContext != null)
                    {
                        if (this.sourceActor.handle.SkillControl.ornamentFirstSwitchCdEftTime >= Singleton<FrameSynchr>.instance.LogicFrameTick)
                        {
                            ornamentFirstSwitchCd = curLvelContext.m_ornamentFirstSwitchCd;
                        }
                        else
                        {
                            ornamentFirstSwitchCd = curLvelContext.m_ornamentSwitchCD;
                        }
                        this.sourceActor.handle.SkillControl.ornamentFirstSwitchCdEftTime = 0;
                    }
                }
                else
                {
                    ornamentFirstSwitchCd = (int) slot.CurSkillCD;
                }
                int skillLevel = slot.GetSkillLevel();
                if (slot.SkillObj != null)
                {
                    skillID = slot.SkillObj.SkillID;
                }
                if (slot.PassiveSkillObj != null)
                {
                    num2 = slot.PassiveSkillObj.SkillID;
                }
                if ((_orgSkillID == 0) || (skillID == _orgSkillID))
                {
                    slot.DestoryIndicatePrefab();
                    this.sourceActor.handle.SkillControl.InitSkillSlot((int) _slotType, _skillID, num2);
                    if (this.sourceActor.handle.SkillControl.TryGetSkillSlot(_slotType, out slot))
                    {
                        slot.CurSkillCD = ornamentFirstSwitchCd;
                        slot.IsCDReady = ornamentFirstSwitchCd == 0;
                        slot.SetSkillLevel(skillLevel);
                        DefaultSkillEventParam param = new DefaultSkillEventParam(_slotType, 0, this.sourceActor);
                        Singleton<GameSkillEventSys>.GetInstance().SendEvent<DefaultSkillEventParam>(GameSkillEventDef.Event_UpdateSkillUI, this.sourceActor, ref param, GameSkillEventChannel.Channel_HostCtrlActor);
                        if (this.changeSkillSlot[(int) _slotType].changeCount == 0)
                        {
                            this.changeSkillSlot[(int) _slotType].initSkillID = skillID;
                            this.changeSkillSlot[(int) _slotType].initPassiveSkillID = num2;
                        }
                        this.changeSkillSlot[(int) _slotType].changeSkillID = _skillID;
                        this.changeSkillSlot[(int) _slotType].changeCount++;
                    }
                }
            }
        }

        public bool GetChangeSkillSlot(int _slotType, out int _changeSkillID)
        {
            if (this.changeSkillSlot[_slotType].changeCount > 0)
            {
                _changeSkillID = this.changeSkillSlot[_slotType].changeSkillID;
                if (_changeSkillID != 0)
                {
                    return true;
                }
            }
            _changeSkillID = 0;
            return false;
        }

        public void Init(BuffHolderComponent _buffHolder)
        {
            this.buffHolder = _buffHolder;
            this.sourceActor = _buffHolder.actorPtr;
            for (int i = 0; i < 10; i++)
            {
                this.changeSkillSlot[i].changeCount = 0;
                this.changeSkillSlot[i].initSkillID = 0;
                this.changeSkillSlot[i].changeSkillID = 0;
            }
        }

        public void RecoverSkillSlot(SkillSlotType _slotType)
        {
            SkillSlot slot = null;
            if (this.sourceActor.handle.SkillControl.TryGetSkillSlot(_slotType, out slot))
            {
                if (this.changeSkillSlot[(int) _slotType].changeCount == 1)
                {
                    int initSkillID = this.changeSkillSlot[(int) _slotType].initSkillID;
                    int initPassiveSkillID = this.changeSkillSlot[(int) _slotType].initPassiveSkillID;
                    int curSkillCD = (int) slot.CurSkillCD;
                    int skillLevel = slot.GetSkillLevel();
                    slot.DestoryIndicatePrefab();
                    this.sourceActor.handle.SkillControl.InitSkillSlot((int) _slotType, initSkillID, initPassiveSkillID);
                    if (this.sourceActor.handle.SkillControl.TryGetSkillSlot(_slotType, out slot))
                    {
                        slot.SetSkillLevel(skillLevel);
                        slot.CurSkillCD = curSkillCD;
                        slot.IsCDReady = curSkillCD == 0;
                        DefaultSkillEventParam param = new DefaultSkillEventParam(_slotType, 0, this.sourceActor);
                        Singleton<GameSkillEventSys>.GetInstance().SendEvent<DefaultSkillEventParam>(GameSkillEventDef.Event_UpdateSkillUI, this.sourceActor, ref param, GameSkillEventChannel.Channel_HostCtrlActor);
                    }
                    this.changeSkillSlot[(int) _slotType].initSkillID = 0;
                    this.changeSkillSlot[(int) _slotType].changeSkillID = 0;
                    this.changeSkillSlot[(int) _slotType].initPassiveSkillID = 0;
                }
                this.changeSkillSlot[(int) _slotType].changeCount--;
            }
        }
    }
}

