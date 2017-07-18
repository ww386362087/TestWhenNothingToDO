﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic;
    using Assets.Scripts.UI;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CChatController : Singleton<CChatController>
    {
        public bool bSendChat = true;
        public static string ChatEntryPath = "UGUI/Form/System/Chat/ChatEntry.prefab";
        public static string ChatFormPath = "UGUI/Form/System/Chat/ChatForm.prefab";
        public static string ChatPlayerInfo = "UGUI/Form/System/Chat/Form_ChatPlayerInfo.prefab";
        public static string ChatSelectHeroPath_BanPick = "UGUI/Form/System/Chat/Form_SelectChat_BanPick.prefab";
        public static string ChatSelectHeroPath_Normal = "UGUI/Form/System/Chat/Form_SelectChat_Normal.prefab";
        private int chatTimer = -1;
        public AllColorParser ColorParser = new AllColorParser();
        public int cur_chatTimer_totalTime = 4;
        public static string fmt = "{0}：{1}";
        public static string fmt_blue_name = "<color=#224c87>{0}：</color>{1}";
        public static string fmt_gold_name = "<color=#c9a634>{0}：</color>{1}";
        private static int GuildRecruitRequestTimeInterval = 10;
        public CHeroSelectChatView HeroSelectChatView = new CHeroSelectChatView();
        public static int init_chatTime = 4;
        private List<string> m_cachedLeaveRoomPlayerNames;
        public static int max_ChatTime = 30;
        public static int MaxCount = 50;
        public CChatModel model = new CChatModel();
        private int recruitTimer = -1;
        public static int step_time = 2;
        public CChatView view = new CChatView();

        public CChatController()
        {
            this.cur_chatTimer_totalTime = init_chatTime;
            this.chatTimer = Singleton<CTimerManager>.GetInstance().AddTimer(init_chatTime * 0x3e8, -1, new CTimer.OnTimeUpHandler(this.On_Send_GetChat_Req));
            UT.ResetTimer(this.chatTimer, true);
            this.recruitTimer = Singleton<CTimerManager>.GetInstance().AddTimer(GuildRecruitRequestTimeInterval * 0x3e8, -1, new CTimer.OnTimeUpHandler(this.On_Send_GetGuildRecruit_Req));
            UT.ResetTimer(this.recruitTimer, true);
        }

        public void BuildCachedPlayerLeaveRoomSystemMsg()
        {
            if ((this.m_cachedLeaveRoomPlayerNames != null) && (this.m_cachedLeaveRoomPlayerNames.Count > 0))
            {
                for (int i = 0; i < this.m_cachedLeaveRoomPlayerNames.Count; i++)
                {
                    this.BuildPlayerLeaveRoomSystemMsg(this.m_cachedLeaveRoomPlayerNames[i]);
                }
            }
        }

        private void BuildPlayerLeaveRoomSystemMsg(string playerName)
        {
            CChatChannel channel = this.model.channelMgr.GetChannel(EChatChannel.Settle);
            if (channel != null)
            {
                string[] args = new string[] { playerName };
                string text = Singleton<CTextManager>.GetInstance().GetText("Chat_Somebody_Leave_Room", args);
                channel.Add(CChatUT.Build_4_LeaveRoom(text));
                Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_RoomChatData_Change");
                this.model.sysData.Add_NewContent_Entry_ColorFlag(text, EChatChannel.Settle);
                Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
            }
        }

        public void BuildWarmBattlePlayerLeaveRoomSystemMsg()
        {
            SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
            if ((curLvelContext != null) && curLvelContext.m_isWarmBattle)
            {
                CPlayerKDAStat playerKDAStat = Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat;
                if (playerKDAStat != null)
                {
                    DictionaryView<uint, PlayerKDA>.Enumerator enumerator = playerKDAStat.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<uint, PlayerKDA> current = enumerator.Current;
                        if (current.Value.IsComputer)
                        {
                            KeyValuePair<uint, PlayerKDA> pair2 = enumerator.Current;
                            this.BuildPlayerLeaveRoomSystemMsg(pair2.Value.PlayerName);
                        }
                    }
                }
            }
        }

        public void CancleRefreshEvent()
        {
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_FriendChatData_Change", new Action(this, (IntPtr) this.On_Chat_FriendChatData_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_LobbyChatData_Change", new Action(this, (IntPtr) this.On_Chat_LobbyChatData_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_GuildChatData_Change", new Action(this, (IntPtr) this.On_Chat_GuildChatData_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_GuildMatchTeamChatData_Change", new Action(this, (IntPtr) this.On_Chat_GuildMatchTeamChatData_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_ChatEntry_Change", new Action(this, (IntPtr) this.On_Chat_ChatEntry_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_RoomChatData_Change", new Action(this, (IntPtr) this.On_Chat_RoomChatData_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Chat_TeamChat_Change", new Action(this, (IntPtr) this.On_Chat_TeamChatData_Change));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Guild_Enter_Guild", new Action(this, (IntPtr) this.On_Guild_EnterGuild));
            Singleton<EventRouter>.GetInstance().RemoveEventHandler("Guild_Leave_Guild", new Action(this, (IntPtr) this.On_Guild_LeaveGuild));
        }

        public enCheckChatResult CheckSend(EChatChannel channel, string content = "", bool bContentCheck = false)
        {
            if (bContentCheck && string.IsNullOrEmpty(content))
            {
                return enCheckChatResult.EmptyLimit;
            }
            if ((channel == EChatChannel.Lobby) && !Singleton<CFunctionUnlockSys>.instance.FucIsUnlock(RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_CHAT))
            {
                return enCheckChatResult.FunUnlockLimit;
            }
            DateTime banTime = MonoSingleton<IDIPSys>.GetInstance().GetBanTime(COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_DENYCHAT);
            DateTime time2 = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
            if (banTime > time2)
            {
                return enCheckChatResult.BanLimit;
            }
            if (!this.model.channelMgr.GetChannel(this.view.CurTab).IsInputValid())
            {
                return enCheckChatResult.CdLimit;
            }
            if (((channel == EChatChannel.Lobby) && !CSysDynamicBlock.bChatPayBlock) && (Singleton<CChatController>.GetInstance().model.sysData.restChatFreeCnt <= 0))
            {
                return enCheckChatResult.FreeCntLimit;
            }
            return enCheckChatResult.Success;
        }

        public void ClearAll()
        {
            if (this.model != null)
            {
                this.model.ClearAll();
            }
        }

        public void ClearAllPanel()
        {
            if (this.view != null)
            {
                this.view.ClearChatForm();
                Singleton<CUIManager>.GetInstance().CloseForm(ChatFormPath);
            }
        }

        public void ClearCachedPlayerLeaveRoomSystemMsg()
        {
            if (this.m_cachedLeaveRoomPlayerNames != null)
            {
                this.m_cachedLeaveRoomPlayerNames.Clear();
            }
        }

        public void CreateForm()
        {
            if (this.view != null)
            {
                this.view.CreateDetailChatForm();
            }
        }

        public void DoChatClosingAnim()
        {
            if ((this.view != null) && (this.view.Anim != null))
            {
                this.view.Anim.SetBool("ChatForm_Fade", false);
            }
        }

        public void DoChatOpenningAnim()
        {
            if ((this.view != null) && (this.view.Anim != null))
            {
                this.view.Anim.SetBool("ChatForm_Fade", true);
            }
        }

        private uint GetLastRecruitInfoTime()
        {
            uint sendTime = 0;
            if (((this.model != null) && (this.model.sysData != null)) && (this.model.sysData.m_guildRecruitInfos.Count > 0))
            {
                for (int i = 0; i < this.model.sysData.m_guildRecruitInfos.Count; i++)
                {
                    GuildRecruitInfo info = this.model.sysData.m_guildRecruitInfos[i];
                    if (sendTime < info.sendTime)
                    {
                        GuildRecruitInfo info2 = this.model.sysData.m_guildRecruitInfos[i];
                        sendTime = info2.sendTime;
                    }
                }
            }
            return sendTime;
        }

        public void Hide_SelectChat_MidNode()
        {
            if (this.HeroSelectChatView != null)
            {
                this.HeroSelectChatView.Show_SelectChat_MidNode(false);
            }
        }

        public override void Init()
        {
            base.Init();
            this.InitEvent();
        }

        private void InitEvent()
        {
            Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("On_Chat_GetMsg_NTF", new Action<CSPkg>(this.On_Chat_GetMsg_NTF));
            Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Chat_Offline_GetMsg_NTF", new Action<CSPkg>(this.On_Chat_Offline_GetMsg_NTF));
            Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Chat_PlayerLeaveSettle_Ntf", new Action<CSPkg>(this.On_Chat_PlayerLeaveSettle_Ntf));
            Singleton<EventRouter>.GetInstance().AddEventHandler<CFriendModel.FriendType, ulong, uint, bool>("Chat_Friend_Online_Change", new Action<CFriendModel.FriendType, ulong, uint, bool>(this, (IntPtr) this.On_Chat_Friend_Online_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_PlayerLevel_Set", new Action(this, (IntPtr) this.On_Chat_PlayerLevel_Set));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_HeorSelectChatData_Change", new Action(this, (IntPtr) this.On_Chat_HeorSelectChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler<string>(EventID.ROLLING_SYSTEM_CHAT_INFO_RECEIVED, new Action<string>(this.On_Rolling_SystemChatInfoReceived));
            Singleton<EventRouter>.GetInstance().AddEventHandler<int>(EventID.ERRCODE_NTF, new Action<int>(this.OnErrorCodeNtf));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_OpenForm, new CUIEventManager.OnUIEventHandler(this.On_Chat_OpenForm));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_CloseForm, new CUIEventManager.OnUIEventHandler(this.On_Chat_CloseForm));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Tab_Change, new CUIEventManager.OnUIEventHandler(this.On_Chat_Tab_Change));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ToolBar_Voice, new CUIEventManager.OnUIEventHandler(this.On_Chat_ToolBar_Voice));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ToolBar_Input, new CUIEventManager.OnUIEventHandler(this.On_Chat_ToolBar_Input));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ToolBar_Face, new CUIEventManager.OnUIEventHandler(this.On_Chat_ToolBar_Face));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ToolBar_Add, new CUIEventManager.OnUIEventHandler(this.On_Chat_ToolBar_Add));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Conent_Head_Icon, new CUIEventManager.OnUIEventHandler(this.On_Chat_Conent_Head_Icon));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_FriendTab_Item, new CUIEventManager.OnUIEventHandler(this.On_Chat_FriendTab_Item));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_List_FriendItem_Change, new CUIEventManager.OnUIEventHandler(this.On_Friend_TabList_Selected));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_EntryPanel_Click, new CUIEventManager.OnUIEventHandler(this.On_Chat_EntryPanel_Click));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_FaceList_Selected, new CUIEventManager.OnUIEventHandler(this.On_Chat_FaceList_Selected));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ScreenButton_Click, new CUIEventManager.OnUIEventHandler(this.On_Chat_ScreenButton_Click));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_SendButton_Click, new CUIEventManager.OnUIEventHandler(this.On_Chat_Text_Send));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Cost_Send, new CUIEventManager.OnUIEventHandler(this.On_Chat_Cost_Send));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ClearText_Click, new CUIEventManager.OnUIEventHandler(this.On_Chat_ClearText_Click));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_LobbyChat_Elem_Enable, new CUIEventManager.OnUIEventHandler(this.On_List_ElementEnable));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_FriendChat_Elem_Enable, new CUIEventManager.OnUIEventHandler(this.On_Chat_FriendChat_Elem_Enable));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Timer_Changed, new CUIEventManager.OnUIEventHandler(this.OnChatTimerChanged));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_Chat_Bubble_Close, new CUIEventManager.OnUIEventHandler(this.OnChatBubbleClose));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_Settle_Change", new Action(this, (IntPtr) this.On_Chat_Settle_Change));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Form_Open_Mini_Player_Info_Form, new CUIEventManager.OnUIEventHandler(this.OpenMiniPlayerInfoForm));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_ClickBubble, new CUIEventManager.OnUIEventHandler(this.OnChat_ClickBubble));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_OpenForm, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_OpenForm));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_CloseForm, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_CloseForm));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_Bottom_BtnClick, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_Bottom_BtnClick));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_TabChange, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_TabChange));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_TemplateList_Click, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_TemplateList_Click));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_List_ElementEnable, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_List_ElementEnable));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_Tab_Input, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Tab_Input));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_Tab_Voice, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Tab_Voice));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_Send_Text, new CUIEventManager.OnUIEventHandler(this.On_Chat_Hero_Select_Send));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Speaker_EntryNode_Open, new CUIEventManager.OnUIEventHandler(this.On_Speaker_EntryNode_Open));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Speaker_EntryNode_TimeUp, new CUIEventManager.OnUIEventHandler(this.On_Speaker_EntryNode_TimeUp));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_OpenSpeaker, new CUIEventManager.OnUIEventHandler(this.OnChatHeroSelectOpenSpeaker));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Hero_Select_OpenMic, new CUIEventManager.OnUIEventHandler(this.OnChatHeroSelectOpenMic));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Chat_Guild_Recruit_List_Element_Enabled, new CUIEventManager.OnUIEventHandler(this.On_Chat_Guild_Recruit_List_Element_Enabled));
        }

        public void Jump2FriendChat(COMDT_FRIEND_INFO info)
        {
            if ((info != null) && (this.view != null))
            {
                this.view.Jump2FriendChat(info);
            }
        }

        private void On_Chat_ChatEntry_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_EntryForm();
            }
        }

        private void On_Chat_ClearText_Click(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.ClearInputText();
            }
        }

        private void On_Chat_CloseForm(CUIEvent uievent)
        {
            this.DoChatClosingAnim();
        }

        private void On_Chat_Conent_Head_Icon(CUIEvent uievent)
        {
        }

        private void On_Chat_Cost_Send(CUIEvent cuiEvent)
        {
            string tagStr = cuiEvent.m_eventParams.tagStr;
            Singleton<CChatController>.GetInstance().On_InputFiled_EndEdit(tagStr);
            if ((this.view != null) && this.view.bShow)
            {
                this.view.ClearInputText();
            }
        }

        private void On_Chat_EntryPanel_Click(CUIEvent uievent)
        {
            this.ShowPanel(true, true);
            if (((this.view != null) && (this.view.chatForm != null)) && (this.view.chatForm.get_gameObject() != null))
            {
                this.DoChatOpenningAnim();
                CUITimerScript componetInChild = Utility.GetComponetInChild<CUITimerScript>(this.view.chatForm.get_gameObject(), "Timer");
                if (componetInChild != null)
                {
                    componetInChild.ReStartTimer();
                }
            }
        }

        private void On_Chat_FaceList_Selected(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.On_Chat_FaceList_Selected(uievent);
            }
        }

        private void On_Chat_Friend_Online_Change(CFriendModel.FriendType type, ulong ullUid, uint dwLogicWorldId, bool bOffline)
        {
            if ((this.view != null) && this.view.bShow)
            {
                if ((Singleton<CChatController>.GetInstance().model.sysData.ullUid == ullUid) && (Singleton<CChatController>.GetInstance().model.sysData.dwLogicWorldId == dwLogicWorldId))
                {
                    CChatChannel channel = Singleton<CChatController>.GetInstance().model.channelMgr._getChannel(EChatChannel.Friend, ullUid, dwLogicWorldId);
                    if (channel == null)
                    {
                        channel = Singleton<CChatController>.GetInstance().model.channelMgr.CreateChannel(EChatChannel.Friend, ullUid, dwLogicWorldId);
                    }
                    if (channel != null)
                    {
                        CChatEntity ent = CChatUT.Build_4_OfflineOrOnline(bOffline);
                        if (ent != null)
                        {
                            channel.bOffline = bOffline;
                            channel.Add(ent);
                        }
                    }
                }
                if (this.view.CurTab == EChatChannel.Friend)
                {
                    this.view.Process_Friend_Tip();
                }
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_FriendChat_Elem_Enable(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.On_FriendsList_ElementEnable(uievent);
            }
        }

        private void On_Chat_FriendChatData_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_FriendTab_Item(CUIEvent uievent)
        {
        }

        private void On_Chat_GetMsg_NTF(CSPkg msg)
        {
            SCPKG_CMD_CHAT_NTF stChatNtf = msg.stPkgData.stChatNtf;
            if (stChatNtf.dwTimeStamp != 0)
            {
                Singleton<CChatController>.GetInstance().model.SetTimeStamp(EChatChannel.Lobby, stChatNtf.dwTimeStamp);
            }
            Singleton<CChatController>.GetInstance().model.SetRestFreeCnt(EChatChannel.Lobby, stChatNtf.dwRestChatFreeCnt);
            int num = Mathf.Min(stChatNtf.astChatMsg.Length, stChatNtf.bMsgCnt);
            if (num == 0)
            {
                this.cur_chatTimer_totalTime += step_time;
                if (this.cur_chatTimer_totalTime >= max_ChatTime)
                {
                    this.cur_chatTimer_totalTime = max_ChatTime;
                }
                Singleton<CTimerManager>.instance.ResetTimerTotalTime(this.chatTimer, this.cur_chatTimer_totalTime * 0x3e8);
            }
            else
            {
                this.cur_chatTimer_totalTime = init_chatTime;
                Singleton<CTimerManager>.instance.ResetTimerTotalTime(this.chatTimer, this.cur_chatTimer_totalTime * 0x3e8);
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                bool flag5 = false;
                bool flag6 = false;
                bool flag7 = false;
                for (int i = num - 1; i >= 0; i--)
                {
                    COMDT_CHAT_MSG comdt_chat_msg = stChatNtf.astChatMsg[i];
                    if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Friend)
                    {
                        COMDT_CHAT_PLAYER_INFO stFrom = comdt_chat_msg.stContent.stPrivate.stFrom;
                        CChatEntity chatEnt = null;
                        if (stFrom.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
                        {
                            string content = UT.Bytes2String(comdt_chat_msg.stContent.stPrivate.szContent);
                            chatEnt = CChatUT.Build_4_Self(content);
                            this.model.channelMgr.Add_CurChatFriend(chatEnt);
                            string a = string.Format(fmt, Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().Name, content);
                            this.model.sysData.Add_NewContent_Entry(a, EChatChannel.Friend);
                            Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_FriendChatData_Change");
                            Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                            this.model.sysData.LastChannel = EChatChannel.Friend;
                            flag = true;
                        }
                        else
                        {
                            if (Singleton<CFriendContoller>.instance.model.IsBlack(stFrom.ullUid, (uint) stFrom.iLogicWorldID))
                            {
                                return;
                            }
                            string rawText = UT.Bytes2String(comdt_chat_msg.stContent.stPrivate.szContent);
                            chatEnt = CChatUT.Build_4_Friend(comdt_chat_msg.stContent.stPrivate);
                            this.model.channelMgr.Add_ChatEntity(chatEnt, EChatChannel.Friend, stFrom.ullUid, (uint) stFrom.iLogicWorldID);
                            string str4 = CChatUT.Build_4_EntryString(EChatChannel.Friend, stFrom.ullUid, (uint) stFrom.iLogicWorldID, rawText);
                            this.model.sysData.Add_NewContent_Entry(str4, EChatChannel.Friend);
                            this.model.sysData.LastChannel = EChatChannel.Friend;
                            flag = true;
                        }
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Lobby)
                    {
                        COMDT_CHAT_PLAYER_INFO comdt_chat_player_info2 = comdt_chat_msg.stContent.stLogicWord.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(comdt_chat_player_info2.ullUid, (uint) comdt_chat_player_info2.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag8 = comdt_chat_player_info2.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag8;
                        }
                        this.model.Add_Palyer_Info(comdt_chat_msg.stContent.stLogicWord.stFrom);
                        CChatEntity entity2 = CChatUT.Build_4_Lobby(comdt_chat_msg.stContent.stLogicWord);
                        if (flag8)
                        {
                            entity2.type = EChaterType.Self;
                        }
                        this.model.channelMgr.Add_ChatEntity(entity2, EChatChannel.Lobby, 0L, 0);
                        string str5 = UT.Bytes2String(comdt_chat_msg.stContent.stLogicWord.szContent);
                        string str6 = CChatUT.Build_4_EntryString(EChatChannel.Lobby, comdt_chat_player_info2.ullUid, (uint) comdt_chat_player_info2.iLogicWorldID, str5);
                        this.model.sysData.Add_NewContent_Entry(str6, EChatChannel.Lobby);
                        this.model.sysData.LastChannel = EChatChannel.Lobby;
                        flag2 = true;
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Guild)
                    {
                        COMDT_CHAT_PLAYER_INFO comdt_chat_player_info3 = comdt_chat_msg.stContent.stGuild.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(comdt_chat_player_info3.ullUid, (uint) comdt_chat_player_info3.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag9 = comdt_chat_player_info3.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag9;
                        }
                        this.model.Add_Palyer_Info(comdt_chat_msg.stContent.stGuild.stFrom);
                        CChatEntity entity3 = CChatUT.Build_4_Guild(comdt_chat_msg.stContent.stGuild);
                        if (flag9)
                        {
                            entity3.type = EChaterType.Self;
                        }
                        this.model.channelMgr.Add_ChatEntity(entity3, EChatChannel.Guild, 0L, 0);
                        string str7 = UT.Bytes2String(comdt_chat_msg.stContent.stGuild.szContent);
                        string str8 = CChatUT.Build_4_EntryString(EChatChannel.Guild, comdt_chat_player_info3.ullUid, (uint) comdt_chat_player_info3.iLogicWorldID, str7);
                        this.model.sysData.Add_NewContent_Entry(str8, EChatChannel.Guild);
                        this.model.sysData.LastChannel = EChatChannel.Guild;
                        flag4 = true;
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.GuildMatchTeam)
                    {
                        COMDT_CHAT_PLAYER_INFO info = comdt_chat_msg.stContent.stGuildTeam.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(info.ullUid, (uint) info.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag10 = info.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag10;
                        }
                        this.model.Add_Palyer_Info(info);
                        CChatEntity entity4 = CChatUT.Build_4_GuildMatchTeam(comdt_chat_msg.stContent.stGuildTeam);
                        if (flag10)
                        {
                            entity4.type = EChaterType.Self;
                        }
                        this.model.channelMgr.Add_ChatEntity(entity4, EChatChannel.GuildMatchTeam, 0L, 0);
                        string str9 = UT.Bytes2String(comdt_chat_msg.stContent.stGuildTeam.szContent);
                        string str10 = CChatUT.Build_4_EntryString(EChatChannel.GuildMatchTeam, info.ullUid, (uint) info.iLogicWorldID, str9);
                        this.model.sysData.Add_NewContent_Entry(str10, EChatChannel.GuildMatchTeam);
                        this.model.sysData.LastChannel = EChatChannel.GuildMatchTeam;
                        flag5 = true;
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Room)
                    {
                        COMDT_CHAT_PLAYER_INFO comdt_chat_player_info5 = comdt_chat_msg.stContent.stRoom.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(comdt_chat_player_info5.ullUid, (uint) comdt_chat_player_info5.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag11 = comdt_chat_player_info5.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag11;
                        }
                        this.model.Add_Palyer_Info(comdt_chat_player_info5);
                        CChatEntity entity5 = CChatUT.Build_4_Room(comdt_chat_msg.stContent.stRoom);
                        if (flag11)
                        {
                            entity5.type = EChaterType.Self;
                        }
                        this.model.channelMgr.Add_ChatEntity(entity5, EChatChannel.Room, 0L, 0);
                        this.model.sysData.LastChannel = EChatChannel.Room;
                        string str11 = UT.Bytes2String(comdt_chat_msg.stContent.stRoom.szContent);
                        string str12 = CChatUT.Build_4_EntryString(EChatChannel.Room, comdt_chat_player_info5.ullUid, (uint) comdt_chat_player_info5.iLogicWorldID, str11);
                        this.model.sysData.Add_NewContent_Entry(str12, EChatChannel.Room);
                        flag3 = true;
                        Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_RoomChatData_Change");
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Select_Hero)
                    {
                        COMDT_CHAT_PLAYER_INFO comdt_chat_player_info6 = comdt_chat_msg.stContent.stBattle.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(comdt_chat_player_info6.ullUid, (uint) comdt_chat_player_info6.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag12 = comdt_chat_player_info6.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag12;
                        }
                        this.model.Add_Palyer_Info(comdt_chat_msg.stContent.stBattle.stFrom);
                        CChatEntity entity6 = CChatUT.Build_4_SelectHero(comdt_chat_msg.stContent.stBattle);
                        this.model.channelMgr.Add_ChatEntity(entity6, EChatChannel.Select_Hero, 0L, 0);
                        Singleton<EventRouter>.instance.BroadCastEvent("Chat_HeorSelectChatData_Change");
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Team)
                    {
                        COMDT_CHAT_PLAYER_INFO comdt_chat_player_info7 = comdt_chat_msg.stContent.stTeam.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(comdt_chat_player_info7.ullUid, (uint) comdt_chat_player_info7.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag13 = comdt_chat_player_info7.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag13;
                        }
                        this.model.Add_Palyer_Info(comdt_chat_msg.stContent.stTeam.stFrom);
                        CChatEntity entity7 = CChatUT.Build_4_Team(comdt_chat_msg.stContent.stTeam);
                        if (flag13)
                        {
                            entity7.type = EChaterType.Self;
                        }
                        this.model.channelMgr.Add_ChatEntity(entity7, EChatChannel.Team, 0L, 0);
                        string str13 = UT.Bytes2String(comdt_chat_msg.stContent.stTeam.szContent);
                        string str14 = CChatUT.Build_4_EntryString(EChatChannel.Team, comdt_chat_player_info7.ullUid, (uint) comdt_chat_player_info7.iLogicWorldID, str13);
                        this.model.sysData.Add_NewContent_Entry(str14, EChatChannel.Team);
                        this.model.sysData.LastChannel = EChatChannel.Team;
                        flag6 = true;
                        Singleton<EventRouter>.instance.BroadCastEvent("Chat_TeamChat_Change");
                    }
                    else if (CChatUT.Convert_ChatMsgType_Channel(comdt_chat_msg.bType) == EChatChannel.Settle)
                    {
                        COMDT_CHAT_PLAYER_INFO comdt_chat_player_info8 = comdt_chat_msg.stContent.stSettle.stFrom;
                        if (Singleton<CFriendContoller>.instance.model.IsBlack(comdt_chat_player_info8.ullUid, (uint) comdt_chat_player_info8.iLogicWorldID))
                        {
                            return;
                        }
                        bool flag14 = comdt_chat_player_info8.ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
                        if (this.view != null)
                        {
                            this.view.bRefreshNew = !this.view.IsCheckHistory() || flag14;
                        }
                        this.model.Add_Palyer_Info(comdt_chat_msg.stContent.stSettle.stFrom);
                        CChatEntity entity8 = CChatUT.Build_4_Settle(comdt_chat_msg.stContent.stSettle);
                        if (flag14)
                        {
                            entity8.type = EChaterType.Self;
                        }
                        this.model.channelMgr.Add_ChatEntity(entity8, EChatChannel.Settle, 0L, 0);
                        string str15 = UT.Bytes2String(comdt_chat_msg.stContent.stSettle.szContent);
                        string str16 = CChatUT.Build_4_EntryString(EChatChannel.Settle, comdt_chat_player_info8.ullUid, (uint) comdt_chat_player_info8.iLogicWorldID, str15);
                        this.model.sysData.Add_NewContent_Entry(str16, EChatChannel.Settle);
                        this.model.sysData.LastChannel = EChatChannel.Settle;
                        flag7 = true;
                        Singleton<EventRouter>.instance.BroadCastEvent("Chat_Settle_Change");
                    }
                    else if (comdt_chat_msg.bType == 7)
                    {
                        Singleton<InBattleMsgMgr>.instance.Handle_InBattleMsg_Ntf(comdt_chat_msg.stContent.stInBattle);
                    }
                }
                if (flag2)
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_LobbyChatData_Change");
                }
                if (flag)
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_FriendChatData_Change");
                }
                if (flag4)
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_GuildChatData_Change");
                }
                if (flag5)
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_GuildMatchTeamChatData_Change");
                }
                if (((flag2 || flag) || flag4) && (this.model.channelMgr.ChatTab == CChatChannelMgr.EChatTab.Normal))
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                }
                if ((flag || flag3) && (this.model.channelMgr.ChatTab == CChatChannelMgr.EChatTab.Room))
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                }
                if ((flag || flag6) && (this.model.channelMgr.ChatTab == CChatChannelMgr.EChatTab.Team))
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                }
                if (((flag5 || flag4) || flag) && (this.model.channelMgr.ChatTab == CChatChannelMgr.EChatTab.GuildMatch))
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                }
                if (flag7 && (this.model.channelMgr.ChatTab == CChatChannelMgr.EChatTab.Settle))
                {
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                }
            }
        }

        private void On_Chat_Guild_Recruit_List_Element_Enabled(CUIEvent uiEvent)
        {
            this.view.SetGuildRecruitListElement(uiEvent);
        }

        private void On_Chat_GuildChatData_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_GuildMatchTeamChatData_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_HeorSelectChatData_Change()
        {
            this.HeroSelectChatView.On_Chat_HeorSelectChatData_Change();
        }

        private void On_Chat_Hero_Select_Bottom_BtnClick(CUIEvent uievent)
        {
            this.HeroSelectChatView.On_Bottom_Btn_Click();
        }

        private void On_Chat_Hero_Select_CloseForm(CUIEvent uievent)
        {
            this.HeroSelectChatView.CloseForm();
        }

        private void On_Chat_Hero_Select_List_ElementEnable(CUIEvent uievent)
        {
            this.HeroSelectChatView.On_List_ElementEnable(uievent);
        }

        private void On_Chat_Hero_Select_OpenForm(CUIEvent uievent)
        {
            this.HeroSelectChatView.OpenForm();
        }

        private void On_Chat_Hero_Select_Send(CUIEvent uievent)
        {
            this.HeroSelectChatView.On_Send_Text();
        }

        private void On_Chat_Hero_Select_TabChange(CUIEvent uievent)
        {
            int selectedIndex = uievent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
            this.HeroSelectChatView.On_Tab_Change(selectedIndex);
        }

        private void On_Chat_Hero_Select_TemplateList_Click(CUIEvent uievent)
        {
            int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
            this.HeroSelectChatView.On_List_Item_Click(srcWidgetIndexInBelongedList);
        }

        private void On_Chat_Hero_Tab_Input(CUIEvent uievent)
        {
            this.HeroSelectChatView.ChatType = CHeroSelectChatView.enChatType.Text;
        }

        private void On_Chat_Hero_Tab_Voice(CUIEvent uievent)
        {
            this.HeroSelectChatView.ChatType = CHeroSelectChatView.enChatType.Voice;
        }

        private void On_Chat_LobbyChatData_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(false, EChatChannel.Lobby);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_Offline_GetMsg_NTF(CSPkg msg)
        {
            SCPKG_OFFLINE_CHAT_NTF stOfflineChatNtf = msg.stPkgData.stOfflineChatNtf;
            for (int i = 0; i < stOfflineChatNtf.bChatCnt; i++)
            {
                CSDT_OFFLINE_CHAT_INFO csdt_offline_chat_info = stOfflineChatNtf.astChatInfo[i];
                if (csdt_offline_chat_info != null)
                {
                    COMDT_CHAT_PLAYER_INFO stFrom = csdt_offline_chat_info.stChatMsg.stChatMsg.stFrom;
                    COMDT_FRIEND_INFO gameOrSnsFriend = Singleton<CFriendContoller>.instance.model.GetGameOrSnsFriend(stFrom.ullUid, (uint) stFrom.iLogicWorldID);
                    if (gameOrSnsFriend != null)
                    {
                        CChatChannel channel = this.model.channelMgr._getChannel(EChatChannel.Friend, stFrom.ullUid, (uint) stFrom.iLogicWorldID);
                        if (channel == null)
                        {
                            channel = this.model.channelMgr.CreateChannel(EChatChannel.Friend, stFrom.ullUid, (uint) stFrom.iLogicWorldID);
                            channel.bOffline = true;
                            if (gameOrSnsFriend.bIsOnline == 0)
                            {
                                channel.list.Add(CChatUT.Build_4_OfflineInfo(Singleton<CTextManager>.instance.GetText("FriendChat_Offline_Info")));
                            }
                            channel.list.Add(CChatUT.Build_4_Time((int) csdt_offline_chat_info.stChatMsg.dwChatTime));
                        }
                        CChatEntity entNode = CChatUT.Build_4_Offline_Friend(csdt_offline_chat_info.stChatMsg);
                        if (entNode != null)
                        {
                            Singleton<CChatController>.instance.view.ChatParser.bProc_ChatEntry = false;
                            Singleton<CChatController>.instance.view.ChatParser.maxWidth = CChatParser.chat_list_max_width;
                            Singleton<CChatController>.instance.view.ChatParser.Parse(entNode.text, CChatParser.start_x, entNode);
                            channel.Add(entNode);
                            this.model.AddOfflineChatIndex(stFrom.ullUid, (uint) stFrom.iLogicWorldID, csdt_offline_chat_info.iIndex);
                            if (i == (stOfflineChatNtf.bChatCnt - 1))
                            {
                                string rawText = UT.Bytes2String(csdt_offline_chat_info.stChatMsg.stChatMsg.szContent);
                                string a = CChatUT.Build_4_EntryString(EChatChannel.Friend, stFrom.ullUid, (uint) stFrom.iLogicWorldID, rawText);
                                this.model.sysData.Add_NewContent_Entry(a, EChatChannel.Friend);
                                Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                                this.model.sysData.LastChannel = EChatChannel.Friend;
                            }
                        }
                    }
                }
            }
        }

        private void On_Chat_OpenForm(CUIEvent uievent)
        {
            this.view.ShowDetailChatForm();
        }

        private void On_Chat_PlayerLeaveSettle_Ntf(CSPkg msg)
        {
            string playerName = Singleton<BattleStatistic>.GetInstance().GetPlayerName(msg.stPkgData.stLeaveSettleUiNtf.ullUid, msg.stPkgData.stLeaveSettleUiNtf.dwLogicWorldID);
            if (Singleton<SettlementSystem>.GetInstance().IsInSettlementState())
            {
                this.BuildPlayerLeaveRoomSystemMsg(playerName);
            }
            else if (Singleton<BattleLogic>.GetInstance().isRuning)
            {
                if (this.m_cachedLeaveRoomPlayerNames == null)
                {
                    this.m_cachedLeaveRoomPlayerNames = new List<string>();
                }
                this.m_cachedLeaveRoomPlayerNames.Add(playerName);
            }
        }

        private void On_Chat_PlayerLevel_Set()
        {
            ResAcntExpInfo dataByKey = GameDataMgr.acntExpDatabin.GetDataByKey(Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo().PvpLevel);
            CChatChannelMgr channelMgr = Singleton<CChatController>.GetInstance().model.channelMgr;
            channelMgr.GetChannel(EChatChannel.Lobby).InitChat_InputTimer((int) dataByKey.dwChatCD);
            if ((Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().PvpLevel == CGuildHelper.GetGuildMemberMinPvpLevel()) && (channelMgr.ChatTab == CChatChannelMgr.EChatTab.Normal))
            {
                channelMgr.SetChatTab(CChatChannelMgr.EChatTab.Normal);
            }
        }

        private void On_Chat_RoomChatData_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_ScreenButton_Click(CUIEvent uievent)
        {
            if (this.view != null)
            {
                this.view.On_Chat_ScreenButton_Click();
            }
        }

        private void On_Chat_Settle_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_Tab_Change(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                CUIListScript component = uievent.m_srcWidget.GetComponent<CUIListScript>();
                int selectedIndex = component.GetSelectedIndex();
                selectedIndex = component.GetElemenet(selectedIndex).GetComponent<CUIEventScript>().m_onClickEventParams.tag;
                this.view.On_Tab_Change(selectedIndex);
                this.view.ReBuildTabText();
            }
        }

        private void On_Chat_TeamChatData_Change()
        {
            if (this.view != null)
            {
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
                this.view.Refresh_All_RedPoint();
            }
        }

        private void On_Chat_Text_Send(CUIEvent uiEvent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                string inputText = this.view.GetInputText();
                switch (this.CheckSend(this.view.CurTab, inputText, true))
                {
                    case enCheckChatResult.Success:
                        Singleton<CChatController>.GetInstance().On_InputFiled_EndEdit(inputText);
                        this.view.ClearInputText();
                        break;

                    case enCheckChatResult.EmptyLimit:
                        Singleton<CUIManager>.instance.OpenTips("Chat_Common_Tips_10", true, 1.5f, null, new object[0]);
                        break;

                    case enCheckChatResult.FunUnlockLimit:
                        Singleton<CUIManager>.instance.OpenTips("Chat_Common_Tips_6", true, 1.5f, null, new object[0]);
                        break;

                    case enCheckChatResult.BanLimit:
                    {
                        DateTime banTime = MonoSingleton<IDIPSys>.GetInstance().GetBanTime(COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_DENYCHAT);
                        object[] args = new object[] { banTime.Year, banTime.Month, banTime.Day, banTime.Hour, banTime.Minute };
                        string strContent = string.Format("您已被禁言！禁言截止时间为{0}年{1}月{2}日{3}时{4}分", args);
                        Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent, false);
                        this.view.ClearInputText();
                        break;
                    }
                    case enCheckChatResult.CdLimit:
                    {
                        int num = this.model.channelMgr.GetChannel(this.view.CurTab).Get_Left_CDTime();
                        Singleton<CUIManager>.instance.OpenTips("chat_input_cd", true, 1.5f, null, new object[0]);
                        this.view.ClearInputText();
                        break;
                    }
                    case enCheckChatResult.FreeCntLimit:
                        uiEvent.m_eventParams.tagStr = inputText;
                        CMallSystem.TryToPay(enPayPurpose.Chat, string.Empty, enPayType.DiamondAndDianQuan, (uint) Singleton<CChatController>.instance.model.sysData.chatCostNum, enUIEventID.Chat_Cost_Send, ref uiEvent.m_eventParams, enUIEventID.None, false, true, false);
                        break;
                }
            }
        }

        private void On_Chat_ToolBar_Add(CUIEvent uievent)
        {
        }

        private void On_Chat_ToolBar_Face(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.SetChatFaceShow(true);
            }
        }

        private void On_Chat_ToolBar_Input(CUIEvent uievent)
        {
        }

        private void On_Chat_ToolBar_Voice(CUIEvent uievent)
        {
        }

        private void On_Friend_TabList_Selected(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.On_Friend_TabList_Selected(uievent);
            }
        }

        private void On_Guild_EnterGuild()
        {
            this.model.channelMgr.SetChatTab(CChatChannelMgr.EChatTab.Normal);
        }

        private void On_Guild_LeaveGuild()
        {
            this.model.channelMgr.SetChatTab(CChatChannelMgr.EChatTab.Normal);
        }

        public bool On_InputFiled_EndEdit(string content)
        {
            if (this.view.CurTab == EChatChannel.Friend_Chat)
            {
                CChatNetUT.Send_Private_Chat(this.model.sysData.ullUid, this.model.sysData.dwLogicWorldId, content);
                CChatNetUT.Send_GetChat_Req(EChatChannel.Friend);
            }
            if (this.view.CurTab == EChatChannel.Lobby)
            {
                CChatNetUT.Send_Lobby_Chat(content);
                CChatNetUT.Send_GetChat_Req(EChatChannel.Lobby);
            }
            if (this.view.CurTab == EChatChannel.Guild)
            {
                CChatNetUT.Send_Guild_Chat(content);
                CChatNetUT.Send_GetChat_Req(EChatChannel.Guild);
            }
            if (this.view.CurTab == EChatChannel.GuildMatchTeam)
            {
                CChatNetUT.Send_GuildMatchTeam_Chat(content);
                CChatNetUT.Send_GetChat_Req(EChatChannel.GuildMatchTeam);
            }
            if (this.view.CurTab == EChatChannel.Room)
            {
                CChatNetUT.Send_Room_Chat(content);
                CChatNetUT.Send_GetChat_Req(EChatChannel.Room);
            }
            if (this.view.CurTab == EChatChannel.Team)
            {
                CChatNetUT.Send_Team_Chat(content);
                CChatNetUT.Send_GetChat_Req(EChatChannel.Team);
            }
            if (this.view.CurTab == EChatChannel.Settle)
            {
                SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
                if ((curLvelContext != null) && curLvelContext.m_isWarmBattle)
                {
                    string str = content;
                    CChatEntity chatEnt = CChatUT.Build_4_Self(str);
                    this.model.channelMgr.Add_ChatEntity(chatEnt, EChatChannel.Settle, 0L, 0);
                    string a = string.Format(fmt, Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().Name, str);
                    this.model.sysData.Add_NewContent_Entry(a, EChatChannel.Settle);
                    this.model.sysData.LastChannel = EChatChannel.Settle;
                    Singleton<EventRouter>.instance.BroadCastEvent("Chat_Settle_Change");
                    Singleton<EventRouter>.GetInstance().BroadCastEvent("Chat_ChatEntry_Change");
                }
                else
                {
                    CChatNetUT.Send_Settle_Chat(content);
                    CChatNetUT.Send_GetChat_Req(EChatChannel.Settle);
                }
            }
            return true;
        }

        private void On_List_ElementEnable(CUIEvent uievent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.On_List_ElementEnable(uievent);
            }
        }

        private void On_Rolling_SystemChatInfoReceived(string content)
        {
            CChatEntity chatEnt = CChatUT.Build_4_System(content);
            this.model.channelMgr.Add_ChatEntity(chatEnt, EChatChannel.Lobby, 0L, 0);
        }

        private void On_Send_GetChat_Req(int timerSequence)
        {
            if (this.bSendChat)
            {
                CChatNetUT.Send_GetChat_Req(EChatChannel.Lobby);
            }
        }

        private void On_Send_GetGuildRecruit_Req(int timerSequence)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (((masterRoleInfo != null) && (masterRoleInfo.PvpLevel >= CGuildHelper.GetGuildMemberMinPvpLevel())) && ((Singleton<CGuildSystem>.GetInstance() != null) && !Singleton<CGuildSystem>.GetInstance().IsInNormalGuild()))
            {
                CChatNetUT.RequestGetGuildRecruitReq(this.GetLastRecruitInfoTime());
            }
        }

        private void On_Speaker_EntryNode_Open(CUIEvent uievent)
        {
            if (this.view != null)
            {
                this.view.OpenSpeakerEntryNode(uievent.m_eventParams.tagStr);
            }
        }

        private void On_Speaker_EntryNode_TimeUp(CUIEvent uievent)
        {
            if (this.view != null)
            {
                this.view.CloseSpeakerEntryNode();
            }
        }

        private void OnChat_ClickBubble(CUIEvent uiEvent)
        {
            if (this.view != null)
            {
                this.view.bRefreshNew = true;
                this.view.Refresh_ChatEntity_List(true, EChatChannel.None);
            }
        }

        private void OnChatBubbleClose(CUIEvent cuiEvent)
        {
            this.HeroSelectChatView.OnChatBubbleClose(cuiEvent.m_srcWidget);
        }

        private void OnChatHeroSelectOpenMic(CUIEvent uievent)
        {
            this.HeroSelectChatView.OnChatHeroSelectOpenMic(true);
        }

        private void OnChatHeroSelectOpenSpeaker(CUIEvent uievent)
        {
            this.HeroSelectChatView.OnChatHeroSelectOpenSpeaker(false);
        }

        private void OnChatTimerChanged(CUIEvent cuiEvent)
        {
            if ((this.view != null) && this.view.bShow)
            {
                this.view.Refresh_ChatInputView();
            }
        }

        public void OnClosingAnimEnd()
        {
            if (Singleton<CUIManager>.instance.GetForm(CFriendContoller.FriendFormPath) != null)
            {
                this.view.HideDetailChatForm();
            }
            else
            {
                this.ShowPanel(true, false);
            }
        }

        private void OnErrorCodeNtf(int errorCode)
        {
            if (errorCode == 0x8a)
            {
                CChatChannel channel = this.model.channelMgr.GetChannel(EChatChannel.Lobby);
                if (channel != null)
                {
                    channel.ClearCd();
                    if (this.view != null)
                    {
                        this.view.Refresh_ChatInputView();
                    }
                }
            }
            else if (errorCode == 0x89)
            {
                CChatChannel channel2 = this.model.channelMgr.GetChannel(EChatChannel.Lobby);
                if (channel2 != null)
                {
                    channel2.Start_InputCD();
                    if (this.view != null)
                    {
                        this.view.Refresh_ChatInputView();
                    }
                }
            }
        }

        private void OpenMiniPlayerInfoForm(CUIEvent uiEvent)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                int num = uiEvent.m_eventParams.tag2;
                ulong num2 = uiEvent.m_eventParams.commonUInt64Param1;
                if (masterRoleInfo.playerUllUID != num2)
                {
                    CUIEvent event2 = new CUIEvent();
                    event2.m_eventID = enUIEventID.Mini_Player_Info_Open_Form;
                    event2.m_srcFormScript = uiEvent.m_srcFormScript;
                    event2.m_eventParams.tag = 2;
                    event2.m_eventParams.commonUInt64Param1 = num2;
                    event2.m_eventParams.tag2 = num;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(event2);
                }
            }
        }

        public void RefreshGuildRecruitInfo(SCPKG_GET_GUILD_RECRUIT_RSP rsp)
        {
            if ((this.model != null) && (this.model.sysData != null))
            {
                for (int i = 0; i < rsp.stInfo.iNum; i++)
                {
                    GuildRecruitInfo item = new GuildRecruitInfo();
                    COMDT_GUILD_RECRUIT_INFO comdt_guild_recruit_info = rsp.stInfo.astInfo[i];
                    item.senderUid = comdt_guild_recruit_info.ullSenderUid;
                    item.senderLevel = comdt_guild_recruit_info.bSenderLevel;
                    item.guildId = comdt_guild_recruit_info.ullGuildID;
                    item.sendTime = comdt_guild_recruit_info.dwSendTime;
                    item.guildName = StringHelper.UTF8BytesToString(ref comdt_guild_recruit_info.szGuildName);
                    item.senderName = StringHelper.UTF8BytesToString(ref comdt_guild_recruit_info.szSendName);
                    item.senderHeadUrl = StringHelper.UTF8BytesToString(ref comdt_guild_recruit_info.szSenderHeadUrl);
                    item.limitGrade = CGuildHelper.GetFixedGuildGradeLimit(comdt_guild_recruit_info.bLimitGrade);
                    item.limitLevel = CGuildHelper.GetFixedGuildLevelLimit(comdt_guild_recruit_info.bLimitLevel);
                    this.model.sysData.m_guildRecruitInfos.Add(item);
                }
                int count = this.model.sysData.m_guildRecruitInfos.Count - 100;
                if (count > 0)
                {
                    this.model.sysData.m_guildRecruitInfos.RemoveRange(0, count);
                }
            }
        }

        public void Set_Show_Bottom(bool bShow)
        {
            if (this.HeroSelectChatView != null)
            {
                this.HeroSelectChatView.Set_Show_Bottom(bShow);
            }
        }

        public void SetChatTimerEnable(bool b)
        {
            UT.ResetTimer(this.chatTimer, !b);
        }

        public void SetEntryNodeVoiceBtnShowable(bool bShow)
        {
            if (this.HeroSelectChatView != null)
            {
                this.HeroSelectChatView.SetEntryNodeVoiceBtnShowable(bShow);
            }
        }

        public void SetEntryVisible(bool bShow)
        {
            if (this.view != null)
            {
                this.view.SetEntryVisible(bShow);
            }
        }

        public void SetGuildRecruitTimerEnable(bool b)
        {
            UT.ResetTimer(this.recruitTimer, !b);
        }

        public void ShowPanel(bool bShow, bool bDetail)
        {
            if (this.view != null)
            {
                this.view.HideDetailChatForm();
                if (bShow)
                {
                    if (bDetail)
                    {
                        this.view.ShowDetailChatForm();
                    }
                    else
                    {
                        this.view.ShowEntryForm();
                    }
                }
            }
        }

        public void SubmitRefreshEvent()
        {
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_FriendChatData_Change", new Action(this, (IntPtr) this.On_Chat_FriendChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_LobbyChatData_Change", new Action(this, (IntPtr) this.On_Chat_LobbyChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_GuildChatData_Change", new Action(this, (IntPtr) this.On_Chat_GuildChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_GuildMatchTeamChatData_Change", new Action(this, (IntPtr) this.On_Chat_GuildMatchTeamChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_ChatEntry_Change", new Action(this, (IntPtr) this.On_Chat_ChatEntry_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_RoomChatData_Change", new Action(this, (IntPtr) this.On_Chat_RoomChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Chat_TeamChat_Change", new Action(this, (IntPtr) this.On_Chat_TeamChatData_Change));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Guild_Enter_Guild", new Action(this, (IntPtr) this.On_Guild_EnterGuild));
            Singleton<EventRouter>.GetInstance().AddEventHandler("Guild_Leave_Guild", new Action(this, (IntPtr) this.On_Guild_LeaveGuild));
            if (this.view != null)
            {
                this.view.InitCheckTimer();
            }
        }

        public void Update()
        {
            this.view.Update();
        }

        public class Chat_Model_EventID
        {
            public const string Chat_ChatEntry_Change = "Chat_ChatEntry_Change";
            public const string Chat_Friend_Online_Change = "Chat_Friend_Online_Change";
            public const string Chat_FriendChatData_Change = "Chat_FriendChatData_Change";
            public const string Chat_GetMsg_NTF = "On_Chat_GetMsg_NTF";
            public const string Chat_GuildChatData_Change = "Chat_GuildChatData_Change";
            public const string Chat_GuildMatchTeamChatData_Change = "Chat_GuildMatchTeamChatData_Change";
            public const string Chat_HeorSelectChatData_Change = "Chat_HeorSelectChatData_Change";
            public const string Chat_LobbyChatData_Change = "Chat_LobbyChatData_Change";
            public const string Chat_Offline_GetMsg_NTF = "Chat_Offline_GetMsg_NTF";
            public const string Chat_PlayerLeaveSettle_Ntf = "Chat_PlayerLeaveSettle_Ntf";
            public const string Chat_PlayerLevel_Set = "Chat_PlayerLevel_Set";
            public const string Chat_RoomChatData_Change = "Chat_RoomChatData_Change";
            public const string Chat_Settle_Change = "Chat_Settle_Change";
            public const string Chat_TeamChat_Change = "Chat_TeamChat_Change";
        }

        public enum enCheckChatResult
        {
            Success,
            CdLimit,
            BanLimit,
            FreeCntLimit,
            FunUnlockLimit,
            EmptyLimit
        }
    }
}

