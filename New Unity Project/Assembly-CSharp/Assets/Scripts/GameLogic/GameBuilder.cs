﻿namespace Assets.Scripts.GameLogic
{
    using AGE;
    using Apollo;
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic.GameKernal;
    using Assets.Scripts.GameSystem;
    using CSProtocol;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class GameBuilder : Singleton<GameBuilder>
    {
        [CompilerGenerated]
        private GameInfoBase <gameInfo>k__BackingField;
        private List<KeyValuePair<string, string>> m_eventsLoadingTime = new List<KeyValuePair<string, string>>();
        private float m_fLoadingTime;
        private float m_fLoadProgress;
        public int m_iMapId;
        public COM_GAME_TYPE m_kGameType = COM_GAME_TYPE.COM_GAME_TYPE_MAX;

        public void EndGame()
        {
            if (Singleton<BattleLogic>.instance.isRuning)
            {
                try
                {
                    DebugHelper.CustomLog("Prepare GameBuilder EndGame");
                }
                catch (Exception)
                {
                }
                MonoSingleton<GSDKsys>.GetInstance().EndSpeed();
                Singleton<GameLogic>.GetInstance().HashCheckFreq = 500;
                Singleton<GameLogic>.GetInstance().SnakeTraceMasks = 0;
                Singleton<GameLogic>.GetInstance().SnakeTraceSize = 0xfa000;
                Singleton<LobbyLogic>.GetInstance().StopGameEndTimer();
                Singleton<LobbyLogic>.GetInstance().StopSettleMsgTimer();
                Singleton<LobbyLogic>.GetInstance().StopSettlePanelTimer();
                MonoSingleton<GameLoader>.instance.AdvanceStopLoad();
                Singleton<WatchController>.GetInstance().Stop();
                Singleton<FrameWindow>.GetInstance().ResetSendCmdSeq();
                Singleton<CBattleGuideManager>.GetInstance().resetPause();
                MonoSingleton<ShareSys>.instance.SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType.end, COM_ROOM_TYPE.COM_ROOM_TYPE_NULL, 0, 0, string.Empty);
                Singleton<StarSystem>.GetInstance().EndGame();
                Singleton<WinLoseByStarSys>.GetInstance().EndGame();
                Singleton<CMatchingSystem>.GetInstance().EndGame();
                string openID = Singleton<ApolloHelper>.GetInstance().GetOpenID();
                List<KeyValuePair<string, string>> events = new List<KeyValuePair<string, string>>();
                events.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
                events.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
                events.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
                events.Add(new KeyValuePair<string, string>("openid", openID));
                events.Add(new KeyValuePair<string, string>("GameType", this.m_kGameType.ToString()));
                events.Add(new KeyValuePair<string, string>("MapID", this.m_iMapId.ToString()));
                events.Add(new KeyValuePair<string, string>("LoadingTime", this.m_fLoadingTime.ToString()));
                Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_LoadingBattle", events, true);
                List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
                list2.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
                list2.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
                list2.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
                list2.Add(new KeyValuePair<string, string>("openid", openID));
                list2.Add(new KeyValuePair<string, string>("totaltime", Singleton<CHeroSelectBaseSystem>.instance.m_fOpenHeroSelectForm.ToString()));
                list2.Add(new KeyValuePair<string, string>("gameType", this.m_kGameType.ToString()));
                list2.Add(new KeyValuePair<string, string>("role_list", string.Empty));
                list2.Add(new KeyValuePair<string, string>("errorCode", string.Empty));
                list2.Add(new KeyValuePair<string, string>("error_msg", string.Empty));
                Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_Login_EnterGame", list2, true);
                float num = Singleton<FrameSynchr>.GetInstance().LogicFrameTick * 0.001f;
                Singleton<FrameSynchr>.GetInstance().PingVariance();
                List<KeyValuePair<string, string>> list3 = new List<KeyValuePair<string, string>>();
                list3.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
                list3.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
                list3.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
                list3.Add(new KeyValuePair<string, string>("openid", openID));
                list3.Add(new KeyValuePair<string, string>("GameType", this.m_kGameType.ToString()));
                list3.Add(new KeyValuePair<string, string>("MapID", this.m_iMapId.ToString()));
                list3.Add(new KeyValuePair<string, string>("Max_FPS", Singleton<CBattleSystem>.GetInstance().m_MaxBattleFPS.ToString()));
                list3.Add(new KeyValuePair<string, string>("Min_FPS", Singleton<CBattleSystem>.GetInstance().m_MinBattleFPS.ToString()));
                float num2 = -1f;
                if (Singleton<CBattleSystem>.GetInstance().m_BattleFPSCount > 0f)
                {
                    num2 = Singleton<CBattleSystem>.GetInstance().m_AveBattleFPS / Singleton<CBattleSystem>.GetInstance().m_BattleFPSCount;
                }
                list3.Add(new KeyValuePair<string, string>("Avg_FPS", num2.ToString()));
                list3.Add(new KeyValuePair<string, string>("Ab_FPS_time", Singleton<BattleLogic>.GetInstance().m_Ab_FPS_time.ToString()));
                list3.Add(new KeyValuePair<string, string>("Abnormal_FPS", Singleton<BattleLogic>.GetInstance().m_Abnormal_FPS_Count.ToString()));
                list3.Add(new KeyValuePair<string, string>("Less10FPSCount", Singleton<BattleLogic>.GetInstance().m_fpsCunt10.ToString()));
                list3.Add(new KeyValuePair<string, string>("Less18FPSCount", Singleton<BattleLogic>.GetInstance().m_fpsCunt18.ToString()));
                list3.Add(new KeyValuePair<string, string>("Ab_4FPS_time", Singleton<BattleLogic>.GetInstance().m_Ab_4FPS_time.ToString()));
                list3.Add(new KeyValuePair<string, string>("Abnormal_4FPS", Singleton<BattleLogic>.GetInstance().m_Abnormal_4FPS_Count.ToString()));
                list3.Add(new KeyValuePair<string, string>("Min_Ping", Singleton<FrameSynchr>.instance.m_MinPing.ToString()));
                list3.Add(new KeyValuePair<string, string>("Max_Ping", Singleton<FrameSynchr>.instance.m_MaxPing.ToString()));
                list3.Add(new KeyValuePair<string, string>("Avg_Ping", Singleton<FrameSynchr>.instance.m_AvePing.ToString()));
                list3.Add(new KeyValuePair<string, string>("Abnormal_Ping", Singleton<FrameSynchr>.instance.m_Abnormal_PingCount.ToString()));
                list3.Add(new KeyValuePair<string, string>("Ping300", Singleton<FrameSynchr>.instance.m_ping300Count.ToString()));
                list3.Add(new KeyValuePair<string, string>("Ping150to300", Singleton<FrameSynchr>.instance.m_ping150to300.ToString()));
                list3.Add(new KeyValuePair<string, string>("Ping150", Singleton<FrameSynchr>.instance.m_ping150.ToString()));
                list3.Add(new KeyValuePair<string, string>("LostpingCount", Singleton<FrameSynchr>.instance.m_pingLost.ToString()));
                list3.Add(new KeyValuePair<string, string>("PingSeqCount", Singleton<FrameSynchr>.instance.m_LastReceiveHeartSeq.ToString()));
                list3.Add(new KeyValuePair<string, string>("PingVariance", Singleton<FrameSynchr>.instance.m_PingVariance.ToString()));
                list3.Add(new KeyValuePair<string, string>("Battle_Time", num.ToString()));
                list3.Add(new KeyValuePair<string, string>("BattleSvr_Reconnect", Singleton<NetworkModule>.GetInstance().m_GameReconnetCount.ToString()));
                list3.Add(new KeyValuePair<string, string>("GameSvr_Reconnect", Singleton<NetworkModule>.GetInstance().m_lobbyReconnetCount.ToString()));
                list3.Add(new KeyValuePair<string, string>("music", GameSettings.EnableMusic.ToString()));
                list3.Add(new KeyValuePair<string, string>("quality", GameSettings.RenderQuality.ToString()));
                list3.Add(new KeyValuePair<string, string>("status", "1"));
                list3.Add(new KeyValuePair<string, string>("Quality_Mode", GameSettings.ModelLOD.ToString()));
                list3.Add(new KeyValuePair<string, string>("Quality_Particle", GameSettings.ParticleLOD.ToString()));
                list3.Add(new KeyValuePair<string, string>("receiveMoveCmdAverage", Singleton<FrameSynchr>.instance.m_receiveMoveCmdAverage.ToString()));
                list3.Add(new KeyValuePair<string, string>("receiveMoveCmdMax", Singleton<FrameSynchr>.instance.m_receiveMoveCmdMax.ToString()));
                list3.Add(new KeyValuePair<string, string>("execMoveCmdAverage", Singleton<FrameSynchr>.instance.m_execMoveCmdAverage.ToString()));
                list3.Add(new KeyValuePair<string, string>("execMoveCmdMax", Singleton<FrameSynchr>.instance.m_execMoveCmdMax.ToString()));
                list3.Add(new KeyValuePair<string, string>("LOD_Down", Singleton<BattleLogic>.GetInstance().m_iAutoLODState.ToString()));
                if (NetworkAccelerator.started)
                {
                    if (NetworkAccelerator.isAccerating())
                    {
                        list3.Add(new KeyValuePair<string, string>("AccState", "Acc"));
                    }
                    else
                    {
                        list3.Add(new KeyValuePair<string, string>("AccState", "Direct"));
                    }
                }
                else
                {
                    list3.Add(new KeyValuePair<string, string>("AccState", "Off"));
                }
                int num3 = 0;
                if (MonoSingleton<VoiceSys>.GetInstance().UseSpeak && MonoSingleton<VoiceSys>.GetInstance().UseMic)
                {
                    num3 = 2;
                }
                else if (MonoSingleton<VoiceSys>.GetInstance().UseSpeak)
                {
                    num3 = 1;
                }
                list3.Add(new KeyValuePair<string, string>("Mic", num3.ToString()));
                Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_PVPBattle_Summary", list3, true);
                this.m_eventsLoadingTime.Clear();
                try
                {
                    float num4 = ((float) Singleton<BattleLogic>.GetInstance().m_fpsCunt10) / ((float) Singleton<BattleLogic>.GetInstance().m_fpsCount);
                    int num5 = Mathf.CeilToInt((num4 * 100f) / 10f) * 10;
                    float num6 = ((float) (Singleton<BattleLogic>.GetInstance().m_fpsCunt18 + Singleton<BattleLogic>.GetInstance().m_fpsCunt10)) / ((float) Singleton<BattleLogic>.GetInstance().m_fpsCount);
                    int num7 = Mathf.CeilToInt((num6 * 100f) / 10f) * 10;
                    CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x1388);
                    msg.stPkgData.stCltPerformance.iMapID = this.m_iMapId;
                    msg.stPkgData.stCltPerformance.iPlayerCnt = Singleton<GamePlayerCenter>.instance.GetAllPlayers().Count;
                    msg.stPkgData.stCltPerformance.chModelLOD = (sbyte) GameSettings.ModelLOD;
                    msg.stPkgData.stCltPerformance.chParticleLOD = (sbyte) GameSettings.ParticleLOD;
                    msg.stPkgData.stCltPerformance.chCameraHeight = (sbyte) GameSettings.CameraHeight;
                    msg.stPkgData.stCltPerformance.chEnableOutline = !GameSettings.EnableOutline ? ((sbyte) 0) : ((sbyte) 1);
                    msg.stPkgData.stCltPerformance.iFps10PercentNum = num5;
                    msg.stPkgData.stCltPerformance.iFps18PercentNum = num7;
                    msg.stPkgData.stCltPerformance.iAveFps = (int) Singleton<CBattleSystem>.GetInstance().m_AveBattleFPS;
                    msg.stPkgData.stCltPerformance.iPingAverage = Singleton<FrameSynchr>.instance.m_PingAverage;
                    msg.stPkgData.stCltPerformance.iPingVariance = Singleton<FrameSynchr>.instance.m_PingVariance;
                    Utility.StringToByteArray(SystemInfo.get_deviceModel(), ref msg.stPkgData.stCltPerformance.szDeviceModel);
                    Utility.StringToByteArray(SystemInfo.get_graphicsDeviceName(), ref msg.stPkgData.stCltPerformance.szGPUName);
                    msg.stPkgData.stCltPerformance.iCpuCoreNum = SystemInfo.get_processorCount();
                    msg.stPkgData.stCltPerformance.iSysMemorySize = SystemInfo.get_systemMemorySize();
                    msg.stPkgData.stCltPerformance.iAvailMemory = DeviceCheckSys.GetAvailMemory();
                    msg.stPkgData.stCltPerformance.iIsTongCai = !MonoSingleton<CTongCaiSys>.GetInstance().IsCanUseTongCai() ? 0 : 1;
                    int num8 = 0;
                    if (NetworkAccelerator.started)
                    {
                        if (NetworkAccelerator.isAccerating())
                        {
                            num8 = 1;
                        }
                        else
                        {
                            num8 = 2;
                        }
                    }
                    else
                    {
                        num8 = 0;
                    }
                    if (MonoSingleton<GSDKsys>.GetInstance().get_enabled())
                    {
                        num8 = 4;
                    }
                    msg.stPkgData.stCltPerformance.iIsSpeedUp = num8;
                    Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);
                }
                MonoSingleton<DialogueProcessor>.GetInstance().Uninit();
                Singleton<TipProcessor>.GetInstance().Uninit();
                Singleton<LobbyLogic>.instance.inMultiRoom = false;
                Singleton<LobbyLogic>.instance.inMultiGame = false;
                Singleton<LobbyLogic>.GetInstance().reconnGameInfo = null;
                Singleton<BattleLogic>.GetInstance().isRuning = false;
                Singleton<BattleLogic>.GetInstance().isFighting = false;
                Singleton<BattleLogic>.GetInstance().isGameOver = false;
                Singleton<BattleLogic>.GetInstance().isWaitMultiStart = false;
                Singleton<NetworkModule>.GetInstance().CloseGameServerConnect(true);
                Singleton<ShenFuSystem>.instance.ClearAll();
                MonoSingleton<ActionManager>.GetInstance().ForceStop();
                Singleton<GameObjMgr>.GetInstance().ClearActor();
                Singleton<SceneManagement>.GetInstance().Clear();
                MonoSingleton<SceneMgr>.GetInstance().ClearAll();
                Singleton<GamePlayerCenter>.GetInstance().ClearAllPlayers();
                Singleton<ActorDataCenter>.instance.ClearHeroServerData();
                Singleton<FrameSynchr>.GetInstance().ResetSynchr();
                Singleton<GameReplayModule>.GetInstance().OnGameEnd();
                Singleton<BattleLogic>.GetInstance().ResetBattleSystem();
                ActionManager.Instance.frameMode = false;
                MonoSingleton<VoiceInteractionSys>.instance.OnEndGame();
                if (!Singleton<GameStateCtrl>.instance.isLobbyState)
                {
                    DebugHelper.CustomLog("GotoLobbyState by EndGame");
                    Singleton<GameStateCtrl>.GetInstance().GotoState("LobbyState");
                }
                Singleton<BattleSkillHudControl>.DestroyInstance();
                this.m_kGameType = COM_GAME_TYPE.COM_GAME_TYPE_MAX;
                this.m_iMapId = 0;
                try
                {
                    FogOfWar.EndLevel();
                }
                catch (DllNotFoundException exception2)
                {
                    object[] inParameters = new object[] { exception2.Message, exception2.StackTrace };
                    DebugHelper.Assert(false, "FOW Exception {0} {1}", inParameters);
                }
                Singleton<BattleStatistic>.instance.PostEndGame();
                try
                {
                    DebugHelper.CustomLog("Finish GameBuilder EndGame");
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnGameLoadComplete()
        {
            if (!Singleton<BattleLogic>.instance.isRuning)
            {
                DebugHelper.Assert(false, "都没有在游戏局内，何来的游戏加载完成");
            }
            else
            {
                if (Singleton<WatchController>.instance.workMode != WatchController.WorkMode.None)
                {
                    object[] inParameters = new object[] { Singleton<WatchController>.instance.workMode.ToString() };
                    DebugHelper.CustomLog("观战模式:{0}", inParameters);
                }
                try
                {
                    this.gameInfo.PostBeginPlay();
                }
                catch (Exception exception)
                {
                    object[] objArray2 = new object[] { exception.Message, exception.StackTrace };
                    DebugHelper.Assert(false, "Exception In PostBeginPlay {0} {1}", objArray2);
                    throw exception;
                }
                this.m_fLoadingTime = Time.get_time() - this.m_fLoadingTime;
                if (MonoSingleton<Reconnection>.GetInstance().g_fBeginReconnectTime > 0f)
                {
                    List<KeyValuePair<string, string>> events = new List<KeyValuePair<string, string>>();
                    float num = Time.get_time() - MonoSingleton<Reconnection>.GetInstance().g_fBeginReconnectTime;
                    events.Add(new KeyValuePair<string, string>("ReconnectTime", num.ToString()));
                    MonoSingleton<Reconnection>.GetInstance().g_fBeginReconnectTime = -1f;
                    Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_Reconnet_IntoGame", events, true);
                }
            }
        }

        private void onGameLoadProgress(float progress)
        {
            if ((this.gameInfo != null) && (progress >= (this.m_fLoadProgress + 0.01f)))
            {
                this.m_fLoadProgress = progress;
                this.gameInfo.OnLoadingProgress(progress);
            }
        }

        public void RestoreGame()
        {
        }

        public GameInfoBase StartGame(GameContextBase InGameContext)
        {
            DebugHelper.Assert(InGameContext != null);
            if (InGameContext == null)
            {
                return null;
            }
            if (Singleton<BattleLogic>.instance.isRuning)
            {
                return null;
            }
            SynchrReport.Reset();
            GameSettings.DecideDynamicParticleLOD();
            Singleton<CHeroSelectBaseSystem>.instance.m_fOpenHeroSelectForm = Time.get_time() - Singleton<CHeroSelectBaseSystem>.instance.m_fOpenHeroSelectForm;
            this.m_fLoadingTime = Time.get_time();
            this.m_eventsLoadingTime.Clear();
            ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
            DebugHelper.Assert(accountInfo != null, "account info is null");
            this.m_iMapId = InGameContext.levelContext.m_mapID;
            this.m_kGameType = InGameContext.levelContext.GetGameType();
            this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("OpenID", (accountInfo == null) ? "0" : accountInfo.OpenId));
            this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("LevelID", InGameContext.levelContext.m_mapID.ToString()));
            this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("isPVPLevel", InGameContext.levelContext.IsMobaMode().ToString()));
            this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("isPVPMode", InGameContext.levelContext.IsMobaMode().ToString()));
            this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("bLevelNo", InGameContext.levelContext.m_levelNo.ToString()));
            Singleton<BattleLogic>.GetInstance().isRuning = true;
            Singleton<BattleLogic>.GetInstance().isFighting = false;
            Singleton<BattleLogic>.GetInstance().isGameOver = false;
            Singleton<BattleLogic>.GetInstance().isWaitMultiStart = false;
            ActionManager.Instance.frameMode = true;
            MonoSingleton<ActionManager>.GetInstance().ForceStop();
            Singleton<GameObjMgr>.GetInstance().ClearActor();
            Singleton<SceneManagement>.GetInstance().Clear();
            MonoSingleton<SceneMgr>.GetInstance().ClearAll();
            MonoSingleton<GameLoader>.GetInstance().ResetLoader();
            InGameContext.PrepareStartup();
            if (!MonoSingleton<GameFramework>.instance.EditorPreviewMode)
            {
                DebugHelper.Assert(InGameContext.levelContext != null);
                DebugHelper.Assert(!string.IsNullOrEmpty(InGameContext.levelContext.m_levelDesignFileName));
                if (string.IsNullOrEmpty(InGameContext.levelContext.m_levelArtistFileName))
                {
                    MonoSingleton<GameLoader>.instance.AddLevel(InGameContext.levelContext.m_levelDesignFileName);
                }
                else
                {
                    MonoSingleton<GameLoader>.instance.AddDesignSerializedLevel(InGameContext.levelContext.m_levelDesignFileName);
                    MonoSingleton<GameLoader>.instance.AddArtistSerializedLevel(InGameContext.levelContext.m_levelArtistFileName);
                }
                MonoSingleton<GameLoader>.instance.AddSoundBank("Effect_Common");
                MonoSingleton<GameLoader>.instance.AddSoundBank("System_Voice");
            }
            GameInfoBase base2 = InGameContext.CreateGameInfo();
            DebugHelper.Assert(base2 != null, "can't create game logic object!");
            this.gameInfo = base2;
            base2.PreBeginPlay();
            Singleton<BattleLogic>.instance.m_LevelContext = this.gameInfo.gameContext.levelContext;
            try
            {
                object[] inParameters = new object[] { InGameContext.levelContext.IsMobaMode(), InGameContext.levelContext.m_mapID, InGameContext.levelContext.GetGameType(), InGameContext.levelContext.m_levelName, InGameContext.levelContext.IsMobaMode(), InGameContext.levelContext.GetSelectHeroType(), InGameContext.levelContext.m_pveLevelType };
                DebugHelper.CustomLog("GameBuilder Start Game: ispvplevel={0} ispvpmode={4} levelid={1} leveltype={6} levelname={3} Gametype={2} pick={5}", inParameters);
                Player hostPlayer = Singleton<GamePlayerCenter>.instance.GetHostPlayer();
                if (hostPlayer != null)
                {
                    object[] objArray2 = new object[] { hostPlayer.PlayerId, hostPlayer.Name };
                    DebugHelper.CustomLog("HostPlayer player id={1} name={2} ", objArray2);
                }
            }
            catch (Exception)
            {
            }
            if (!MonoSingleton<GameFramework>.instance.EditorPreviewMode)
            {
                this.m_fLoadProgress = 0f;
                MonoSingleton<GameLoader>.GetInstance().Load(new GameLoader.LoadProgressDelegate(this.onGameLoadProgress), new GameLoader.LoadCompleteDelegate(this.OnGameLoadComplete));
                MonoSingleton<VoiceSys>.GetInstance().HeroSelectTobattle();
                Singleton<GameStateCtrl>.GetInstance().GotoState("LoadingState");
            }
            return base2;
        }

        public void StoreGame()
        {
        }

        public GameInfoBase gameInfo
        {
            [CompilerGenerated]
            get
            {
                return this.<gameInfo>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<gameInfo>k__BackingField = value;
            }
        }
    }
}

