﻿using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[MessageHandlerClass]
public class NewbieGuideManager : MonoSingleton<NewbieGuideManager>
{
    public static NewbieGuideScriptControl.AddScriptDelegate addScriptDelegate;
    public bool bTimeOutSkip;
    public static string FORM_3v3GUIDE_CONFIRM = "UGUI/Form/System/Newbie/Form_3v3Guide_Confirm.prefab";
    public static string FORM_5v5GUIDE_CONFIRM = "UGUI/Form/System/Newbie/Form_5v5Guide_Confirm.prefab";
    private int hostDeadNum;
    public bool IsComplteNewbieGuide;
    private int lastChangeTime;
    private int lastRate = 100;
    private bool m_IsCheckSkip;
    private Dictionary<uint, bool> mCompleteCacheDic;
    private uint mCurrentNewbieGuideId;
    private NewbieGuideScriptControl mCurrentScriptControl;
    private bool mIsInit;
    private List<uint> mSingleBattleCompleteCacheDic;
    private Dictionary<uint, bool> mWeakCompleteCacheDic;
    public bool newbieGuideEnable;
    public static short WEAKGUIDE_BIT_OFFSET = 110;

    private void AddSingleBattleCompleteID(uint id)
    {
        this.mSingleBattleCompleteCacheDic.Add(id);
    }

    protected override void Awake()
    {
        base.Awake();
        addScriptDelegate = new NewbieGuideScriptControl.AddScriptDelegate(NewbieGuideSctiptFactory.AddScript);
        DebugHelper.Assert(addScriptDelegate != null);
        this.mCompleteCacheDic = new Dictionary<uint, bool>();
        this.mWeakCompleteCacheDic = new Dictionary<uint, bool>();
        this.mSingleBattleCompleteCacheDic = new List<uint>();
        List<uint> mainLineIDList = Singleton<NewbieGuideDataManager>.GetInstance().GetMainLineIDList();
        int count = mainLineIDList.Count;
        for (int i = 0; i < count; i++)
        {
            this.mCompleteCacheDic.Add(mainLineIDList[i], false);
        }
        List<uint> weakMianLineIDList = Singleton<NewbieGuideDataManager>.GetInstance().GetWeakMianLineIDList();
        count = weakMianLineIDList.Count;
        for (int j = 0; j < count; j++)
        {
            this.mWeakCompleteCacheDic.Add(weakMianLineIDList[j], false);
        }
        this.newbieGuideEnable = true;
        this.bTimeOutSkip = true;
        this.Initialize();
    }

    private void CheckForceSkipCondition(NewbieGuideSkipConditionType type, params uint[] param)
    {
        ListView<NewbieGuideMainLineConf> newbieGuideMainLineConfListBySkipType = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieGuideMainLineConfListBySkipType(type);
        int count = newbieGuideMainLineConfListBySkipType.Count;
        for (int i = 0; i < count; i++)
        {
            NewbieGuideMainLineConf conf = newbieGuideMainLineConfListBySkipType[i];
            if (!this.IsMianLineComplete(conf.dwID))
            {
                for (int j = 0; j < conf.astSkipCondition.Length; j++)
                {
                    if ((((NewbieGuideSkipConditionType) conf.astSkipCondition[j].wType) == type) && NewbieGuideCheckSkipConditionUtil.CheckSkipCondition(conf.astSkipCondition[j], param))
                    {
                        if (null != this.mCurrentScriptControl)
                        {
                            if (this.mCurrentScriptControl.currentMainLineId == conf.dwID)
                            {
                                goto Label_00B7;
                            }
                            this.SetNewbieGuideComplete(conf.dwID, false, false, true);
                        }
                        else
                        {
                            this.SetNewbieGuideComplete(conf.dwID, false, false, true);
                        }
                        break;
                    Label_00B7:;
                    }
                }
            }
        }
    }

    private bool CheckLevelLimit(NewbieWeakGuideMainLineConf conf)
    {
        if (conf.wTriggerLevelUpperLimit != 0)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                return (masterRoleInfo.PvpLevel <= conf.wTriggerLevelUpperLimit);
            }
        }
        return true;
    }

    private bool CheckLevelLimitLower(NewbieWeakGuideMainLineConf conf)
    {
        if (conf.wTriggerLevelLowerLimit != 0)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                return (masterRoleInfo.PvpLevel >= conf.wTriggerLevelLowerLimit);
            }
        }
        return true;
    }

    public void CheckSkipCondition(NewbieGuideSkipConditionType type, params uint[] param)
    {
        this.CheckForceSkipCondition(type, param);
        this.CheckWeakSkipCondition(type, param);
    }

    public void CheckSkipIntoLobby()
    {
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasComplete33Guide, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasRewardTaskPvp, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasRewardTaskPve, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteHeroAdv, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteHeroStar, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasBoughtHero, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasGotChapterReward, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasAdvancedEquip, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasMopup, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredPvP, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredTrial, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredZhuangzi, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredBurning, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredElitePvE, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredGuild, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasUsedSymbol, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredMysteryShop, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteNewbieArchive, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteBaseGuide, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteHumanAi33Match, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteHuman33Match, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteHumanAi33, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasManufacuredSymbol, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnoughSymbolOf, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteLottery, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasIncreaseEquip, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteHeroUp, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasEnteredTournament, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasOverThreeHeroes, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteWeakNewbieArchive, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteDungeon, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteNewbieGuide, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteTrainLevel55, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasComplete11Match, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteTrainLevel33, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasDiamondDraw, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteFirst55Warm, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteCoronaGuide, new uint[0]);
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCoinDrawFive, new uint[0]);
        this.m_IsCheckSkip = true;
    }

    private bool CheckTrigger(NewbieGuideTriggerTimeType type, NewbieGuideMainLineConf conf)
    {
        if (this.CheckTriggerTime(conf) && this.CheckTriggerCondition(conf.dwID, conf.astTriggerCondition))
        {
            int startIndexByTriggerTime = this.GetStartIndexByTriggerTime(type, conf);
            this.TriggerNewbieGuide(conf, startIndexByTriggerTime);
            return true;
        }
        return false;
    }

    private bool CheckTriggerCondition(uint id, NewbieGuideTriggerConditionItem[] conditions)
    {
        int length = conditions.Length;
        for (int i = 0; i < length; i++)
        {
            NewbieGuideTriggerConditionItem condition = conditions[i];
            if ((condition.wType != 0) && !NewbieGuideCheckTriggerConditionUtil.CheckTriggerCondition(id, condition))
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckTriggerTime(NewbieGuideMainLineConf conf)
    {
        if ((this.IsMianLineComplete(conf.dwID) && (conf.iSavePoint != -1)) || Singleton<WatchController>.instance.IsWatching)
        {
            return false;
        }
        bool flag = true;
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (masterRoleInfo != null)
        {
            if (conf.wTriggerLevelUpperLimit > 0)
            {
                flag &= masterRoleInfo.PvpLevel <= conf.wTriggerLevelUpperLimit;
            }
            if (conf.wTriggerLevelLowerLimit > 0)
            {
                flag &= masterRoleInfo.PvpLevel >= conf.wTriggerLevelLowerLimit;
            }
        }
        return flag;
    }

    public bool CheckTriggerTime(NewbieGuideTriggerTimeType type, params uint[] param)
    {
        if (this.newbieGuideEnable)
        {
            if (!this.m_IsCheckSkip)
            {
                return false;
            }
            if (this.currentNewbieGuideId == 0)
            {
                ListView<NewbieGuideMainLineConf> newbieGuideMainLineConfListByTriggerTimeType = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieGuideMainLineConfListByTriggerTimeType(type, param);
                int count = newbieGuideMainLineConfListByTriggerTimeType.Count;
                for (int i = 0; i < count; i++)
                {
                    NewbieGuideMainLineConf conf = newbieGuideMainLineConfListByTriggerTimeType[i];
                    if (this.CheckTrigger(type, conf))
                    {
                        if (Singleton<NewbieWeakGuideControl>.instance.isGuiding)
                        {
                            Singleton<NewbieWeakGuideControl>.instance.RemoveAllEffect();
                        }
                        return true;
                    }
                }
                ListView<NewbieWeakGuideMainLineConf> newBieGuideWeakMainLineConfListByTiggerTimeType = Singleton<NewbieGuideDataManager>.GetInstance().GetNewBieGuideWeakMainLineConfListByTiggerTimeType(type, param);
                count = newBieGuideWeakMainLineConfListByTiggerTimeType.Count;
                for (int j = 0; j < count; j++)
                {
                    NewbieWeakGuideMainLineConf conf2 = newBieGuideWeakMainLineConfListByTiggerTimeType[j];
                    if (this.TriggerWeakNewbieGuide(conf2, type, true))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void CheckWeakGuideTrigger(NewbieGuideWeakGuideType type, params uint[] param)
    {
    }

    private void CheckWeakSkipCondition(NewbieGuideSkipConditionType type, params uint[] param)
    {
        ListView<NewbieWeakGuideMainLineConf> newbieGuideWeakMianLineConfListBySkipType = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieGuideWeakMianLineConfListBySkipType(type);
        int count = newbieGuideWeakMianLineConfListBySkipType.Count;
        for (int i = 0; i < count; i++)
        {
            NewbieWeakGuideMainLineConf conf = newbieGuideWeakMianLineConfListBySkipType[i];
            if (!this.IsWeakLineComplete(conf.dwID))
            {
                for (int j = 0; j < conf.astSkipCondition.Length; j++)
                {
                    NewbieGuideSkipConditionItem item = conf.astSkipCondition[j];
                    if ((((NewbieGuideSkipConditionType) item.wType) == type) && NewbieGuideCheckSkipConditionUtil.CheckSkipCondition(item, param))
                    {
                        this.SetWeakGuideComplete(conf.dwID, true, true);
                        break;
                    }
                }
            }
        }
    }

    public void ClearSingleBattleCache()
    {
        this.mSingleBattleCompleteCacheDic.Clear();
    }

    public static void CloseGuideForm()
    {
        Singleton<CUIManager>.GetInstance().CloseForm(NewbieGuideScriptControl.FormGuideMaskPath);
    }

    private void CommitIsCompleteNewbieGuide(bool IsComplete)
    {
        CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x147a);
        msg.stPkgData.stAcntSetOldType.bYes = !IsComplete ? ((byte) 0) : ((byte) 1);
        Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, IsComplete);
    }

    public static void CompleteAllNewbieGuide()
    {
        MonoSingleton<NewbieGuideManager>.GetInstance().ForceCompleteNewbieGuideAll(true, true, false);
        MonoSingleton<NewbieGuideManager>.GetInstance().ForceSetWeakGuideCompleteAll(true, true, false);
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (masterRoleInfo != null)
        {
            masterRoleInfo.SetGuidedStateSet(0, true);
            masterRoleInfo.GameDifficult = COM_ACNT_NEWBIE_TYPE.COM_ACNT_NEWBIE_TYPE_MASTER;
        }
        masterRoleInfo.SetNewbieAchieve(0x11, true, false);
        masterRoleInfo.SyncClientBitsToSvr();
        masterRoleInfo.SyncNewbieAchieveToSvr(false);
    }

    public static int ConvertNewbieBitToClientBit(uint newGuideBit)
    {
        if ((newGuideBit < 300) && (newGuideBit > 200))
        {
            return (((int) newGuideBit) - 100);
        }
        return 0;
    }

    private void DestroyCurrentScriptControl()
    {
        this.mCurrentScriptControl.CompleteEvent -= new NewbieGuideScriptControl.NewbieGuideScriptControlDelegate(this.NewbieGuideCompleteHandler);
        this.mCurrentScriptControl.SaveEvent -= new NewbieGuideScriptControl.NewbieGuideScriptControlDelegate(this.NewbieGuideSaveHandler);
        this.mCurrentScriptControl.addScriptDelegate = null;
        Object.Destroy(this.mCurrentScriptControl);
        this.mCurrentScriptControl = null;
        this.mCurrentNewbieGuideId = 0;
    }

    public void ForceCompleteNewbieGuide()
    {
        if (this.mCompleteCacheDic.ContainsKey(this.mCurrentNewbieGuideId) && !this.mCompleteCacheDic[this.mCurrentNewbieGuideId])
        {
            NewbieGuideMainLineConf newbieGuideMainLineConf = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieGuideMainLineConf(this.mCurrentNewbieGuideId);
            if (newbieGuideMainLineConf != null)
            {
                int length = newbieGuideMainLineConf.astSetCompleteId.Length;
                for (int i = 0; i < length; i++)
                {
                    if (newbieGuideMainLineConf.astSetCompleteId[i].dwID != 0)
                    {
                        uint dwID = newbieGuideMainLineConf.astSetCompleteId[i].dwID;
                        if (dwID <= WEAKGUIDE_BIT_OFFSET)
                        {
                            this.SetNewbieGuideComplete(dwID, false, true, true);
                        }
                        else
                        {
                            this.SetWeakGuideComplete(dwID - ((uint) WEAKGUIDE_BIT_OFFSET), true, true);
                        }
                    }
                }
            }
            if (newbieGuideMainLineConf != null)
            {
                this.SetNewbieGuideComplete(this.mCurrentNewbieGuideId, true, false, true);
            }
        }
        if (null != this.mCurrentScriptControl)
        {
            this.mCurrentScriptControl.Stop();
            this.DestroyCurrentScriptControl();
        }
    }

    public void ForceCompleteNewbieGuideAll(bool bReset, bool setOldPlayerBit = false, bool sync = true)
    {
        if (null != this.mCurrentScriptControl)
        {
            this.mCurrentScriptControl.Stop();
            this.DestroyCurrentScriptControl();
        }
        List<uint> list = new List<uint>();
        Dictionary<uint, bool>.KeyCollection.Enumerator enumerator = this.mCompleteCacheDic.Keys.GetEnumerator();
        while (enumerator.MoveNext())
        {
            uint current = enumerator.Current;
            if (!this.mCompleteCacheDic[current])
            {
                NewbieGuideMainLineConf newbieGuideMainLineConf = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieGuideMainLineConf(current);
                if ((newbieGuideMainLineConf != null) && (newbieGuideMainLineConf.bOldPlayerGuide != 1))
                {
                    list.Add(newbieGuideMainLineConf.dwID);
                }
                if ((setOldPlayerBit && (newbieGuideMainLineConf != null)) && (newbieGuideMainLineConf.bOldPlayerGuide == 1))
                {
                    list.Add(newbieGuideMainLineConf.dwID);
                }
            }
        }
        List<uint>.Enumerator enumerator2 = list.GetEnumerator();
        while (enumerator2.MoveNext())
        {
            this.SetNewbieGuideComplete(enumerator2.Current, bReset, false, sync);
        }
    }

    public void ForceCompleteWeakGuide()
    {
        if (this.newbieGuideEnable)
        {
            Singleton<NewbieWeakGuideControl>.GetInstance().ForceCompleteWeakGuide();
        }
    }

    public void ForceSetWeakGuideCompleteAll(bool bReset, bool setOldPlayerBit = false, bool sync = true)
    {
        List<uint> list = new List<uint>();
        Dictionary<uint, bool>.KeyCollection.Enumerator enumerator = this.mWeakCompleteCacheDic.Keys.GetEnumerator();
        while (enumerator.MoveNext())
        {
            uint current = enumerator.Current;
            if (!this.mWeakCompleteCacheDic[current])
            {
                NewbieWeakGuideMainLineConf newbieWeakGuideMainLineConf = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieWeakGuideMainLineConf(current);
                if (((newbieWeakGuideMainLineConf != null) && (newbieWeakGuideMainLineConf.bOnlyOnce == 1)) && (newbieWeakGuideMainLineConf.bOldPlayerGuide != 1))
                {
                    list.Add(newbieWeakGuideMainLineConf.dwID);
                }
                if ((setOldPlayerBit && (newbieWeakGuideMainLineConf != null)) && (newbieWeakGuideMainLineConf.bOldPlayerGuide == 1))
                {
                    list.Add(newbieWeakGuideMainLineConf.dwID);
                }
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            MonoSingleton<NewbieGuideManager>.GetInstance().SetWeakGuideComplete(list[i], bReset, sync);
        }
    }

    private int GetStartIndexByTriggerTime(NewbieGuideTriggerTimeType type, NewbieGuideMainLineConf conf)
    {
        int length = conf.astTriggerTime.Length;
        for (int i = 0; i < length; i++)
        {
            NewbieGuideTriggerTimeItem item = conf.astTriggerTime[i];
            if (type == ((NewbieGuideTriggerTimeType) item.wType))
            {
                return (int) item.dwStartIndex;
            }
        }
        return 0;
    }

    private uint GetWeakStartIndexByMianLineConf(NewbieGuideTriggerTimeType type, NewbieWeakGuideMainLineConf conf)
    {
        uint dwStartIndex = 1;
        int length = conf.astTriggerTime.Length;
        for (int i = 0; i < length; i++)
        {
            NewbieGuideTriggerTimeItem item = conf.astTriggerTime[i];
            if ((((NewbieGuideTriggerTimeType) item.wType) == type) && (item.dwStartIndex > 0))
            {
                dwStartIndex = item.dwStartIndex;
            }
        }
        return dwStartIndex;
    }

    public bool HasSingleBattleComplete(uint id)
    {
        return this.mSingleBattleCompleteCacheDic.Contains(id);
    }

    protected override void Init()
    {
        base.Init();
        Singleton<GameEventSys>.instance.AddEventHandler<TalentLevelChangeParam>(GameEventDef.Event_TalentLevelChange, new RefAction<TalentLevelChangeParam>(this.onTalentLevelChange));
        Singleton<GameEventSys>.instance.AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_FightStart, new RefAction<DefaultGameEventParam>(this.OnFightStart));
        if (!this.IsBigMapSignGuideCompelte())
        {
            Singleton<GameEventSys>.GetInstance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_FightPrepare, new RefAction<DefaultGameEventParam>(this.OnFightPrepare));
            Singleton<GameEventSys>.instance.AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
        }
        if (!this.IsCameraMoveGuideComplete())
        {
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_OnCameraAxisPushed, new CUIEventManager.OnUIEventHandler(this.onMoveCamera));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_OnPanelCameraStartDrag, new CUIEventManager.OnUIEventHandler(this.onMoveCamera));
        }
        if (!this.IsBattleEquipGuideComplete())
        {
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BattleEquip_Form_Close, new CUIEventManager.OnUIEventHandler(this.onBattleEuipFormClose));
        }
        if (!this.IsHurtByTowerGuideComplete())
        {
            Singleton<EventRouter>.GetInstance().AddEventHandler("NewbieHostPlayerInAndHitByTower", new Action(this, (IntPtr) this.OnHostPlayerHitByTower));
        }
        Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Newbie_ClickCompleteNewbieGuide, new CUIEventManager.OnUIEventHandler(this.OnChooseCompletenewbieGuide));
        Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Newbie_ClickNotCompleteNewbieGuide, new CUIEventManager.OnUIEventHandler(this.OnChooseNotCompleteNewbieGuide));
        Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Newbie_BubbleTimeout, new CUIEventManager.OnUIEventHandler(this.OnWeakGuideBubbleTimeOut));
        Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Newbie_CommomBubbleTimeout, new CUIEventManager.OnUIEventHandler(this.OnBattleBubbleTimeout));
    }

    public void Initialize()
    {
        Singleton<NewbieWeakGuideControl>.CreateInstance();
        this.mIsInit = true;
    }

    private bool IsBattleEquipGuideComplete()
    {
        bool flag = false;
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if ((masterRoleInfo != null) && masterRoleInfo.IsClientBitsSet(6))
        {
            flag = true;
        }
        return flag;
    }

    private bool IsBigMapSignGuideCompelte()
    {
        bool flag = false;
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (((masterRoleInfo != null) && masterRoleInfo.IsNewbieAchieveSet(0x4c)) && masterRoleInfo.IsNewbieAchieveSet(0x4d))
        {
            flag = true;
        }
        return flag;
    }

    private bool IsCameraMoveGuideComplete()
    {
        bool flag = false;
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (((masterRoleInfo != null) && masterRoleInfo.IsNewbieAchieveSet(0x40)) && (masterRoleInfo.IsNewbieAchieveSet(0x41) && masterRoleInfo.IsNewbieAchieveSet(0x42)))
        {
            flag = true;
        }
        return flag;
    }

    private bool IsHurtByTowerGuideComplete()
    {
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (((masterRoleInfo != null) && (masterRoleInfo.acntMobaInfo.iMobaLevel == 0)) && (masterRoleInfo.PvpLevel <= 5))
        {
            return false;
        }
        return true;
    }

    private bool IsLowHPGuideComplete()
    {
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (((masterRoleInfo != null) && (masterRoleInfo.acntMobaInfo.iMobaLevel == 0)) && (masterRoleInfo.PvpLevel <= 5))
        {
            return false;
        }
        return true;
    }

    private bool IsMianLineComplete(uint id)
    {
        bool flag;
        return (this.mCompleteCacheDic.TryGetValue(id, out flag) && flag);
    }

    public bool IsNewbieBitSet(int inIndex)
    {
        bool flag = false;
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (masterRoleInfo != null)
        {
            if ((inIndex >= 290) && (inIndex < 300))
            {
                return masterRoleInfo.IsClientBitsSet(ConvertNewbieBitToClientBit((uint) inIndex));
            }
            if ((inIndex >= 0x40) && (inIndex < 0x80))
            {
                flag = masterRoleInfo.IsNewbieAchieveSet(inIndex);
            }
        }
        return flag;
    }

    public bool IsNewbieGuideComplete(uint id)
    {
        bool flag;
        if (this.mCompleteCacheDic.TryGetValue(id, out flag))
        {
            return flag;
        }
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (masterRoleInfo == null)
        {
            return false;
        }
        if ((id >= 290) && (id < 300))
        {
            return masterRoleInfo.IsClientBitsSet(ConvertNewbieBitToClientBit(id));
        }
        return (((id >= 0x40) && (id < 0x80)) && masterRoleInfo.IsNewbieAchieveSet((int) id));
    }

    public bool IsWeakLineComplete(uint lineId)
    {
        bool flag;
        if (this.mWeakCompleteCacheDic.TryGetValue(lineId, out flag))
        {
            return flag;
        }
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        return ((masterRoleInfo != null) && masterRoleInfo.IsNewbieAchieveSet((int) (lineId + WEAKGUIDE_BIT_OFFSET)));
    }

    private void NewbieGuideCompleteHandler()
    {
        if (null != this.mCurrentScriptControl)
        {
            uint currentNewbieGuideId = this.currentNewbieGuideId;
            this.SetNewbieGuideComplete(this.currentNewbieGuideId, true, true, true);
            NewbieGuideMainLineConf newbieGuideMainLineConf = Singleton<NewbieGuideDataManager>.GetInstance().GetNewbieGuideMainLineConf(currentNewbieGuideId);
            if (newbieGuideMainLineConf != null)
            {
                int length = newbieGuideMainLineConf.astSetCompleteId.Length;
                for (int i = 0; i < length; i++)
                {
                    if (newbieGuideMainLineConf.astSetCompleteId[i].dwID != 0)
                    {
                        uint dwID = newbieGuideMainLineConf.astSetCompleteId[i].dwID;
                        if (dwID <= WEAKGUIDE_BIT_OFFSET)
                        {
                            this.SetNewbieGuideComplete(dwID, false, true, true);
                        }
                        else
                        {
                            this.SetWeakGuideComplete(dwID - ((uint) WEAKGUIDE_BIT_OFFSET), true, true);
                        }
                    }
                }
            }
            this.AddSingleBattleCompleteID(currentNewbieGuideId);
            this.DestroyCurrentScriptControl();
            uint[] param = new uint[] { currentNewbieGuideId };
            this.CheckTriggerTime(NewbieGuideTriggerTimeType.preNewbieGuideComplete, param);
        }
    }

    private void NewbieGuideSaveHandler()
    {
        this.Save();
    }

    private void onActorDead(ref GameDeadEventParam param)
    {
        if ((param.src != 0) && ActorHelper.IsHostCtrlActor(ref param.src))
        {
            this.hostDeadNum++;
            if ((this.hostDeadNum == 1) && (Singleton<BattleLogic>.GetInstance().GetCurLvelContext().m_mapID == 0x4e2b))
            {
                this.CheckTriggerTime(NewbieGuideTriggerTimeType.onHostFirstDead, new uint[0]);
            }
        }
    }

    private void OnBattleBubbleTimeout(CUIEvent uiEvent)
    {
        uiEvent.m_srcWidget.get_transform().get_parent().get_gameObject().CustomSetActive(false);
    }

    private void onBattleEuipFormClose(CUIEvent uiEvent)
    {
        if (!this.IsBattleEquipGuideComplete())
        {
            CSkillButtonManager manager = (Singleton<CBattleSystem>.GetInstance().FightForm != null) ? Singleton<CBattleSystem>.GetInstance().FightForm.m_skillButtonManager : null;
            if (manager != null)
            {
                SkillButton button = manager.GetButton(SkillSlotType.SLOT_SKILL_9);
                PlayerKDA hostKDA = Singleton<BattleStatistic>.GetInstance().m_playerKDAStat.GetHostKDA();
                if (hostKDA != null)
                {
                    bool flag = false;
                    ListView<HeroKDA>.Enumerator enumerator = hostKDA.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.Equips.Length > 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (((button != null) && button.m_button.get_activeSelf()) && flag)
                    {
                        CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(FightForm.s_skillBtnFormPath);
                        if (form != null)
                        {
                            Transform transform = form.GetWidget(0x1b).get_transform().FindChild("Panel_Guide");
                            if (transform != null)
                            {
                                transform.get_gameObject().CustomSetActive(true);
                                CUITimerScript component = transform.FindChild("Timer").GetComponent<CUITimerScript>();
                                component.ResetTime();
                                component.ReStartTimer();
                                CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                                if (masterRoleInfo != null)
                                {
                                    masterRoleInfo.SetClientBits(6, true, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnChooseCompletenewbieGuide(CUIEvent uiEvent)
    {
        this.IsComplteNewbieGuide = true;
        this.CommitIsCompleteNewbieGuide(true);
    }

    private void OnChooseNotCompleteNewbieGuide(CUIEvent uiEvent)
    {
        this.IsComplteNewbieGuide = false;
        this.CommitIsCompleteNewbieGuide(false);
    }

    protected override void OnDestroy()
    {
        Singleton<GameEventSys>.instance.RmvEventHandler<TalentLevelChangeParam>(GameEventDef.Event_TalentLevelChange, new RefAction<TalentLevelChangeParam>(this.onTalentLevelChange));
        Singleton<GameEventSys>.instance.RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_FightPrepare, new RefAction<DefaultGameEventParam>(this.OnFightPrepare));
        Singleton<GameEventSys>.instance.RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_FightStart, new RefAction<DefaultGameEventParam>(this.OnFightStart));
        Singleton<GameEventSys>.instance.RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_OnCameraAxisPushed, new CUIEventManager.OnUIEventHandler(this.onMoveCamera));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_OnPanelCameraStartDrag, new CUIEventManager.OnUIEventHandler(this.onMoveCamera));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Newbie_ClickCompleteNewbieGuide, new CUIEventManager.OnUIEventHandler(this.OnChooseCompletenewbieGuide));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Newbie_ClickNotCompleteNewbieGuide, new CUIEventManager.OnUIEventHandler(this.OnChooseNotCompleteNewbieGuide));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Newbie_BubbleTimeout, new CUIEventManager.OnUIEventHandler(this.OnWeakGuideBubbleTimeOut));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BattleEquip_Form_Close, new CUIEventManager.OnUIEventHandler(this.onBattleEuipFormClose));
        Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Newbie_CommomBubbleTimeout, new CUIEventManager.OnUIEventHandler(this.OnBattleBubbleTimeout));
        Singleton<EventRouter>.GetInstance().RemoveEventHandler("NewbieHostPlayerInAndHitByTower", new Action(this, (IntPtr) this.OnHostPlayerHitByTower));
        Singleton<NewbieWeakGuideControl>.DestroyInstance();
        base.OnDestroy();
    }

    private void OnFightPrepare(ref DefaultGameEventParam param)
    {
        this.hostDeadNum = 0;
    }

    private void OnFightStart(ref DefaultGameEventParam param)
    {
        if (!MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.onBattleStart, new uint[0]) && (Singleton<CBattleSystem>.GetInstance().FightForm != null))
        {
            Singleton<CBattleSystem>.GetInstance().FightForm.ShowVoiceTips();
        }
        SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
        if (((curLvelContext != null) && !this.IsLowHPGuideComplete()) && curLvelContext.IsMobaMode())
        {
            ReadonlyContext<PoolObjHandle<ActorRoot>> allHeroes = Singleton<GamePlayerCenter>.instance.GetHostPlayer().GetAllHeroes();
            DebugHelper.Assert(allHeroes.isValidReference, "newbie guide manager invalid all heros list.");
            if (allHeroes.isValidReference)
            {
                for (int i = 0; i < allHeroes.Count; i++)
                {
                    if (allHeroes[i] != 0)
                    {
                        PoolObjHandle<ActorRoot> handle = allHeroes[i];
                        if (handle.handle.ValueComponent != null)
                        {
                            PoolObjHandle<ActorRoot> handle2 = allHeroes[i];
                            handle2.handle.ValueComponent.HpChgEvent -= new ValueChangeDelegate(this.OnHostPlayerHPChange);
                        }
                    }
                }
            }
            Player hostPlayer = Singleton<GamePlayerCenter>.instance.GetHostPlayer();
            if (((hostPlayer != null) && (hostPlayer.Captain != 0)) && (hostPlayer.Captain.handle.ValueComponent != null))
            {
                hostPlayer.Captain.handle.ValueComponent.HpChgEvent += new ValueChangeDelegate(this.OnHostPlayerHPChange);
            }
        }
    }

    private void OnHostPlayerHitByTower()
    {
        SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
        if ((curLvelContext != null) && curLvelContext.IsMobaModeWithOutGuide())
        {
            this.CheckTriggerTime(NewbieGuideTriggerTimeType.onHostPlayerHitByTower, new uint[0]);
        }
    }

    private void OnHostPlayerHPChange()
    {
        int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
        if ((currentUTCTime - this.lastChangeTime) >= 2)
        {
            this.lastChangeTime = currentUTCTime;
            Player hostPlayer = Singleton<GamePlayerCenter>.instance.GetHostPlayer();
            if (((hostPlayer != null) && (hostPlayer.Captain != 0)) && ((hostPlayer.Captain.handle.ActorControl != null) && (hostPlayer.Captain.handle.ValueComponent != null)))
            {
                VFactor factor = (VFactor) (hostPlayer.Captain.handle.ValueComponent.GetHpRate() * 100L);
                int roundInt = factor.roundInt;
                if (this.lastRate < roundInt)
                {
                    this.lastRate = roundInt;
                }
                else
                {
                    this.lastRate = roundInt;
                    if (roundInt < 40)
                    {
                        SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
                        if ((curLvelContext != null) && curLvelContext.IsMobaModeWithOutGuide())
                        {
                            this.CheckTriggerTime(NewbieGuideTriggerTimeType.onHostPlayerLowHp, new uint[0]);
                        }
                    }
                }
            }
        }
    }

    private void onMoveCamera(CUIEvent uiEvt)
    {
        Singleton<CBattleGuideManager>.GetInstance().CloseFormShared(CBattleGuideManager.EBattleGuideFormType.FingerMovement);
    }

    [MessageHandler(0x4a8)]
    public static void OnNTFNewieAllBitSyn(CSPkg msg)
    {
        SCPKG_NTF_NEWIEALLBITSYN stNewieAllBitSyn = msg.stPkgData.stNewieAllBitSyn;
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (masterRoleInfo != null)
        {
            masterRoleInfo.InitGuidedStateBits(stNewieAllBitSyn.stNewbieBits);
        }
    }

    private void onTalentLevelChange(ref TalentLevelChangeParam inParam)
    {
        if (Singleton<GamePlayerCenter>.instance.GetHostPlayer() != null)
        {
            PoolObjHandle<ActorRoot> captain = Singleton<GamePlayerCenter>.instance.GetHostPlayer().Captain;
            if ((captain != 0) && ((inParam.src != 0) && (inParam.src == captain)))
            {
                uint[] param = new uint[] { inParam.SoulLevel, inParam.TalentLevel };
                MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.onTalentLevelChange, param);
            }
        }
    }

    private void OnWeakGuideBubbleTimeOut(CUIEvent uiEvent)
    {
        Singleton<NewbieWeakGuideControl>.GetInstance().RemoveEffectByHilight(uiEvent.m_srcWidget);
    }

    private void Save()
    {
        if ((this.mIsInit && (null != this.mCurrentScriptControl)) && this.mCurrentScriptControl.CheckSavePoint())
        {
            this.SetNewbieGuideComplete(this.mCurrentNewbieGuideId, false, false, true);
        }
    }

    private void SetCurrentNewbieGuideId(uint id)
    {
        this.mCurrentNewbieGuideId = id;
    }

    public void SetNewbieBit(int inIndex, bool bOpen, bool bSync = false)
    {
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if (masterRoleInfo != null)
        {
            if ((inIndex >= 290) && (inIndex < 300))
            {
                masterRoleInfo.SetClientBits(ConvertNewbieBitToClientBit((uint) inIndex), bOpen, bSync);
            }
            else if ((inIndex >= 0x40) && (inIndex < 0x80))
            {
                masterRoleInfo.SetNewbieAchieve(inIndex, bOpen, bSync);
            }
        }
    }

    public void SetNewbieGuideComplete(uint id, bool reset, bool bNormalComplete = false, bool bSync = true)
    {
        bool flag = this.mCompleteCacheDic[id];
        if (!bNormalComplete)
        {
            this.mCompleteCacheDic[id] = true;
        }
        else if ((this.mCurrentScriptControl == null) || (this.mCurrentScriptControl.savePoint >= 0))
        {
            this.mCompleteCacheDic[id] = true;
        }
        if (reset)
        {
            this.SetCurrentNewbieGuideId(0);
        }
        if (!flag && this.mCompleteCacheDic[id])
        {
            this.SetNewbieBit((int) id, true, bSync);
            uint[] param = new uint[] { id };
            this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteNewbieGuide, param);
        }
    }

    public void SetNewbieGuideState(uint id, bool bOpen)
    {
        if ((id > WEAKGUIDE_BIT_OFFSET) && (id <= 0x80))
        {
            this.mWeakCompleteCacheDic[id - ((uint) WEAKGUIDE_BIT_OFFSET)] = bOpen;
        }
        else
        {
            this.mCompleteCacheDic[id] = bOpen;
        }
    }

    public void SetWeakGuideComplete(uint lineId, bool reset = true, bool sync = true)
    {
        bool flag;
        if (this.mWeakCompleteCacheDic.TryGetValue(lineId, out flag))
        {
            this.mWeakCompleteCacheDic[lineId] = true;
        }
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        this.SetNewbieBit((int) (lineId + WEAKGUIDE_BIT_OFFSET), true, sync);
        uint[] param = new uint[] { lineId };
        this.CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteNewbieGuide, param);
    }

    public static void ShowCompleteNewbieGuidePanel()
    {
        Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Old_Acnt_Choose_Complete_Text"), enUIEventID.Newbie_ClickCompleteNewbieGuide, enUIEventID.Newbie_ClickNotCompleteNewbieGuide, false);
    }

    public void StopCurrentGuide()
    {
        if (null != this.mCurrentScriptControl)
        {
            this.mCurrentScriptControl.Stop();
            this.DestroyCurrentScriptControl();
        }
    }

    private void TriggerNewbieGuide(NewbieGuideMainLineConf conf, int startIndex)
    {
        if (Singleton<NetworkModule>.GetInstance().lobbySvr.connected || (conf.bIndependentNet == 1))
        {
            this.SetCurrentNewbieGuideId(conf.dwID);
            this.mCurrentScriptControl = base.get_gameObject().AddComponent<NewbieGuideScriptControl>();
            this.mCurrentScriptControl.CompleteEvent += new NewbieGuideScriptControl.NewbieGuideScriptControlDelegate(this.NewbieGuideCompleteHandler);
            this.mCurrentScriptControl.SaveEvent += new NewbieGuideScriptControl.NewbieGuideScriptControlDelegate(this.NewbieGuideSaveHandler);
            this.mCurrentScriptControl.SetData(conf.dwID, startIndex);
            this.mCurrentScriptControl.addScriptDelegate = addScriptDelegate;
        }
    }

    private bool TriggerWeakNewbieGuide(uint weakLineId, uint startIndex)
    {
        return Singleton<NewbieWeakGuideControl>.GetInstance().TriggerWeakGuide(weakLineId, startIndex);
    }

    public bool TriggerWeakNewbieGuide(NewbieWeakGuideMainLineConf conf, NewbieGuideTriggerTimeType type, bool checkCondition)
    {
        if ((((conf == null) || this.IsWeakLineComplete(conf.dwID)) || Singleton<WatchController>.instance.IsWatching) || (checkCondition && ((!this.CheckLevelLimit(conf) || !this.CheckLevelLimitLower(conf)) || !this.CheckTriggerCondition(0, conf.astTriggerCondition))))
        {
            return false;
        }
        uint weakStartIndexByMianLineConf = this.GetWeakStartIndexByMianLineConf(type, conf);
        return this.TriggerWeakNewbieGuide(conf.dwID, weakStartIndexByMianLineConf);
    }

    public uint currentNewbieGuideId
    {
        get
        {
            return this.mCurrentNewbieGuideId;
        }
    }

    public bool isNewbieGuiding
    {
        get
        {
            return (this.currentNewbieGuideId != 0);
        }
    }

    private string logTitle
    {
        get
        {
            return "[<color=cyan>NewbieGuide</color>][<color=green>test</color>]";
        }
    }
}

