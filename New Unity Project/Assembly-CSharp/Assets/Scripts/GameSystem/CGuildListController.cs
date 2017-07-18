﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.UI;
    using CSProtocol;
    using System;

    [MessageHandlerClass]
    internal class CGuildListController : Singleton<CGuildListController>
    {
        private CGuildModel m_Model;
        private CGuildListView m_View;

        public void CloseForm()
        {
            this.m_View.CloseForm();
        }

        public override void Init()
        {
            base.Init();
            this.m_Model = Singleton<CGuildModel>.GetInstance();
            this.m_View = Singleton<CGuildListView>.GetInstance();
            Singleton<EventRouter>.GetInstance().AddEventHandler<int, int>("Request_Guild_List", new Action<int, int>(this, (IntPtr) this.OnRequestGuildList));
            Singleton<EventRouter>.GetInstance().AddEventHandler<ListView<GuildInfo>, bool>("Receive_Guild_List_Success", new Action<ListView<GuildInfo>, bool>(this, (IntPtr) this.OnReceiveGuildListSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler<int>("Request_PrepareGuild_List", new Action<int>(this.OnRequestPrepareGuildList));
            Singleton<EventRouter>.GetInstance().AddEventHandler<ListView<PrepareGuildInfo>, uint, byte, byte>("Receive_PrepareGuild_List_Success", new Action<ListView<PrepareGuildInfo>, uint, byte, byte>(this, (IntPtr) this.OnReceivePrepareGuildList));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Request_PrepareGuild_Info", new Action(this, (IntPtr) this.OnRequestPrepareGuldInfo));
            Singleton<EventRouter>.GetInstance().AddEventHandler<PrepareGuildInfo>("Receive_PrepareGuild_Info_Success", new Action<PrepareGuildInfo>(this.OnRequestPrepareGuldInfoSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Receive_PrepareGuild_Info_Failed", new Action(this, (IntPtr) this.OnRequestPrepareGuldInfoFailed));
            Singleton<EventRouter>.GetInstance().AddEventHandler<stPrepareGuildCreateInfo>("PrepareGuild_Create", new Action<stPrepareGuildCreateInfo>(this.OnRequestCreatePrepareGuild));
            Singleton<EventRouter>.GetInstance().AddEventHandler<PrepareGuildInfo>("Receive_PrepareGuild_Create_Success", new Action<PrepareGuildInfo>(this.OnReceivePrepareGuildCreateSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Receive_PrepareGuild_Create_Failed", new Action(this, (IntPtr) this.OnReceivePrepareGuildCreateFailed));
            Singleton<EventRouter>.GetInstance().AddEventHandler<GuildInfo>("Request_Apply_Guild_Join", new Action<GuildInfo>(this.OnRequestApplyJoinGuild));
            Singleton<EventRouter>.GetInstance().AddEventHandler<stAppliedGuildInfo>("Receive_Apply_Guild_Join_Success", new Action<stAppliedGuildInfo>(this.OnReceiveApplyJoinGuildSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler<stAppliedGuildInfo>("Receive_Apply_Guild_Join_Failed", new Action<stAppliedGuildInfo>(this.OnReceiveApplyJoinGuildFailed));
            Singleton<EventRouter>.GetInstance().AddEventHandler<PrepareGuildInfo>("PrepareGuild_Join", new Action<PrepareGuildInfo>(this.OnRequestJoinPrepareGuild));
            Singleton<EventRouter>.GetInstance().AddEventHandler<PrepareGuildInfo>("Receive_PrepareGuild_Join_Success", new Action<PrepareGuildInfo>(this.OnReceivePrepareGuildJoinSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler<PrepareGuildInfo>("Receive_PrepareGuild_Join_Rsp", new Action<PrepareGuildInfo>(this.OnReceivePrepareGuildJoinFailed));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Guild_Quit_Success", new Action(this, (IntPtr) this.OnGuildQuitSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler<GuildInfo, int>("Receive_Guild_Search_Success", new Action<GuildInfo, int>(this, (IntPtr) this.OnReceiveGuildSearchSuccess));
            Singleton<EventRouter>.GetInstance().AddEventHandler<PrepareGuildInfo, int>("Receive_Search_Prepare_Guild_Success", new Action<PrepareGuildInfo, int>(this, (IntPtr) this.OnReceiveSearchPrepareGuildSuccess));
        }

        public void OnGuild_Join()
        {
        }

        public void OnGuild_Join_Failed()
        {
        }

        public void OnGuildQuitSuccess()
        {
            this.m_View.OpenForm(CGuildListView.Tab.None, true);
        }

        public void OnReceiveApplyJoinGuildFailed(stAppliedGuildInfo info)
        {
            this.m_View.RefreshGuildListPanel(false);
        }

        public void OnReceiveApplyJoinGuildSuccess(stAppliedGuildInfo info)
        {
            Singleton<CUIManager>.GetInstance().OpenTips("Guild_Send_Apply_Success_Tip", true, 1.5f, null, new object[0]);
            this.m_View.RefreshGuildListPanel(false);
        }

        private void OnReceiveGuildListSuccess(ListView<GuildInfo> guildList, bool firstPage)
        {
            if (!Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                if (firstPage)
                {
                    this.m_Model.ClearGuildInfoList();
                }
                this.m_Model.AddGuildInfoList(guildList);
                this.m_View.RefreshGuildListPanel(false);
            }
        }

        private void OnReceiveGuildSearchSuccess(GuildInfo guildInfo, int searchType)
        {
            if (!Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                this.m_Model.ClearGuildInfoList();
                this.m_Model.AddGuildInfo(guildInfo);
                if (searchType == 0)
                {
                    this.m_View.RefreshGuildListPanel(true);
                }
                else if (searchType == 1)
                {
                    Singleton<CGuildInfoView>.GetInstance().OpenGuildPreviewForm(true);
                }
            }
        }

        private void OnReceivePrepareGuildCreateFailed()
        {
        }

        private void OnReceivePrepareGuildCreateSuccess(PrepareGuildInfo info)
        {
            Singleton<CUIManager>.GetInstance().OpenTips("Guild_Prepare_Guild_Created_Tip", true, 1.5f, null, new object[0]);
            this.m_View.SelectTabElement(CGuildListView.Tab.PrepareGuild, true);
        }

        private void OnReceivePrepareGuildJoinFailed(PrepareGuildInfo info)
        {
            this.m_View.RefreshPrepareGuildPanel(false, 0, false);
        }

        private void OnReceivePrepareGuildJoinSuccess(PrepareGuildInfo info)
        {
            this.m_View.RefreshPrepareGuildPanel(false, 0, false);
        }

        private void OnReceivePrepareGuildList(ListView<PrepareGuildInfo> guildList, uint totalCnt, byte pageId, byte thisCnt)
        {
            if (pageId == 0)
            {
                this.m_Model.ClearPrepareGuildInfoList();
            }
            this.m_Model.AddPrepareGuildInfoList(guildList);
            if (CGuildHelper.IsLastPage(pageId, totalCnt, 10))
            {
                this.m_View.RefreshPrepareGuildPanel(false, pageId, true);
            }
            else
            {
                this.m_View.RefreshPrepareGuildPanel(false, pageId, false);
            }
        }

        private void OnReceiveSearchPrepareGuildSuccess(PrepareGuildInfo prepareGuildInfo, int searchType)
        {
            if (!Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                this.m_Model.ClearPrepareGuildInfoList();
                this.m_Model.AddPrepareGuildInfo(prepareGuildInfo);
                if (searchType == 0)
                {
                    this.m_View.RefreshPrepareGuildPanel(true, 0, true);
                }
                else if (searchType == 1)
                {
                    this.m_View.OpenForm(CGuildListView.Tab.PrepareGuild, false);
                    this.m_View.RefreshPrepareGuildPanel(true, 0, true);
                }
            }
        }

        public void OnRequestApplyJoinGuild(GuildInfo info)
        {
            this.m_Model.SetPlayerGuildStateToTemp();
            this.RequestApplyJoinGuild(info.briefInfo.uulUid, false);
        }

        public void OnRequestCreatePrepareGuild(stPrepareGuildCreateInfo info)
        {
            this.m_Model.SetPlayerGuildStateToTemp();
            this.RequestCreatePrepareGuild(info);
        }

        private void OnRequestGuildList(int start, int limit)
        {
            Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0xa2a);
            msg.stPkgData.stGetRankingListReq.bNumberType = 3;
            msg.stPkgData.stGetRankingListReq.iImageFlag = 0;
            msg.stPkgData.stGetRankingListReq.iStart = start;
            msg.stPkgData.stGetRankingListReq.iLimit = limit;
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }

        public void OnRequestJoinPrepareGuild(PrepareGuildInfo info)
        {
            this.m_Model.SetPlayerGuildStateToTemp();
            this.RequestJoinPrepareGuild(info);
        }

        public void OnRequestPrepareGuildList(int pageId)
        {
            this.RequestPrepareGuildList(pageId);
        }

        public void OnRequestPrepareGuldInfo()
        {
            this.RequestPrepareGuildInfo();
        }

        private void OnRequestPrepareGuldInfoFailed()
        {
            this.m_View.RefreshPrepareGuildPanel(false, 0, false);
        }

        private void OnRequestPrepareGuldInfoSuccess(PrepareGuildInfo info)
        {
            this.m_View.RefreshPrepareGuildPanel(false, 0, false);
        }

        public void OpenForm()
        {
            this.m_View.OpenForm(CGuildListView.Tab.None, true);
        }

        public void RequestApplyJoinGuild(ulong guildId, bool isRecruit)
        {
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x8ad);
            msg.stPkgData.stApplyJoinGuildReq.ullGuildID = guildId;
            msg.stPkgData.stApplyJoinGuildReq.bIsZhaomu = Convert.ToByte(isRecruit);
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }

        private void RequestCreatePrepareGuild(stPrepareGuildCreateInfo info)
        {
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x8a1);
            StringHelper.StringToUTF8Bytes(info.sName, ref msg.stPkgData.stCreateGuildReq.szName);
            StringHelper.StringToUTF8Bytes(info.sBulletin, ref msg.stPkgData.stCreateGuildReq.szNotice);
            msg.stPkgData.stCreateGuildReq.bIsOnlyFriend = Convert.ToByte(info.isOnlyFriend);
            msg.stPkgData.stCreateGuildReq.dwHeadID = info.dwHeadId;
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }

        private void RequestJoinPrepareGuild(PrepareGuildInfo info)
        {
            if (info == null)
            {
                DebugHelper.Assert(false, "CGuildListController.RequestJoinPrepareGuild(): info is null!!!");
            }
            else
            {
                CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x8a3);
                msg.stPkgData.stJoinPrepareGuildReq.ullGuildID = info.stBriefInfo.uulUid;
                Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
            }
        }

        private void RequestPrepareGuildInfo()
        {
            Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x89f);
            msg.stPkgData.stGetPrepareGuildInfoReq.ullGuildID = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.uulUid;
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }

        private void RequestPrepareGuildList(int page)
        {
            Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x89b);
            msg.stPkgData.stGetPrepareGuildListReq.bPageID = (byte) page;
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }
    }
}

