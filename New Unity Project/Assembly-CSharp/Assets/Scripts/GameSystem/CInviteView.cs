﻿namespace Assets.Scripts.GameSystem
{
    using Apollo;
    using Assets.Scripts.UI;
    using CSProtocol;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    internal class CInviteView
    {
        public static string[] TabName = new string[] { Singleton<CTextManager>.GetInstance().GetText("Invite_Tab_Title_Friend"), Singleton<CTextManager>.GetInstance().GetText("Invite_Tab_Title_Guild"), Singleton<CTextManager>.GetInstance().GetText("Invite_Tab_Title_LBS") };

        public static string ConnectPlayerNameAndNickName(byte[] szUserName, string nickName)
        {
            if (szUserName == null)
            {
                return string.Empty;
            }
            if (!string.IsNullOrEmpty(nickName))
            {
                return string.Format("{0}({1})", Utility.UTF8Convert(szUserName), nickName);
            }
            return Utility.UTF8Convert(szUserName);
        }

        private static void GetFriendRankGradeAndStar(COMDT_FRIEND_INFO friendInfo, out int rankGrade, out int rankStar)
        {
            if ((friendInfo != null) && (friendInfo.RankVal != null))
            {
                uint elo = friendInfo.RankVal[7];
                rankGrade = CLadderSystem.ConvertEloToRank(elo);
                rankStar = CLadderSystem.GetCurXingByEloAndRankLv(elo, (uint) rankGrade);
            }
            else
            {
                rankGrade = 0;
                rankStar = 0;
            }
        }

        private static void GetGuildMemberGradeAndStar(GuildMemInfo guildMemInfo, out int rankGrade, out int rankStar)
        {
            if (guildMemInfo != null)
            {
                rankGrade = CLadderSystem.ConvertEloToRank(guildMemInfo.stBriefInfo.dwScoreOfRank);
                rankStar = CLadderSystem.GetCurXingByEloAndRankLv(guildMemInfo.stBriefInfo.dwScoreOfRank, (uint) rankGrade);
            }
            else
            {
                rankGrade = 0;
                rankStar = 0;
            }
        }

        public static enInviteListTab GetInviteListTab(int index)
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return (enInviteListTab) index;
            }
            return ((index <= 0) ? enInviteListTab.Friend : ((enInviteListTab) (index + 1)));
        }

        private static int GetStartMinute(uint startTime)
        {
            DateTime time = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
            DateTime time2 = Utility.ToUtcTime2Local((long) startTime);
            if (time < time2)
            {
                return 1;
            }
            TimeSpan span = (TimeSpan) (time - time2);
            int totalMinutes = (int) span.TotalMinutes;
            return Mathf.Clamp(totalMinutes, 1, 0x63);
        }

        public static int GetTabCount()
        {
            return (!Singleton<CGuildSystem>.GetInstance().IsInNormalGuild() ? 2 : 3);
        }

        public static string GetTabName(int index)
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return TabName[index];
            }
            return ((index <= 0) ? TabName[0] : TabName[index + 1]);
        }

        public static void InitListTab(CUIFormScript form)
        {
            CUIListScript component = form.GetWidget(7).GetComponent<CUIListScript>();
            int tabCount = GetTabCount();
            component.SetElementAmount(tabCount);
            for (int i = 0; i < component.GetElementAmount(); i++)
            {
                component.GetElemenet(i).get_transform().Find("txtName").GetComponent<Text>().set_text(GetTabName(i));
            }
            component.SelectElement(0, true);
        }

        private static bool IsLadderInvite()
        {
            if (Singleton<CInviteSystem>.GetInstance().InviteType == COM_INVITE_JOIN_TYPE.COM_INVITE_JOIN_TEAM)
            {
                CMatchingSystem instance = Singleton<CMatchingSystem>.GetInstance();
                if ((instance != null) && (instance.teamInfo.stTeamInfo.bMapType == 3))
                {
                    return true;
                }
            }
            return false;
        }

        public static void RefreshInviteGuildMemberList(CUIFormScript form, int allGuildMemberLen)
        {
            form.GetWidget(5).GetComponent<CUIListScript>().SetElementAmount(allGuildMemberLen);
        }

        private static void SetFriendState(GameObject element, ref COMDT_FRIEND_INFO friend)
        {
            GameObject go = element.get_transform().FindChild("HeadBg").get_gameObject();
            Text component = element.get_transform().FindChild("Online").GetComponent<Text>();
            GameObject obj3 = element.get_transform().FindChild("InviteButton").get_gameObject();
            Text text2 = element.get_transform().FindChild("PlayerName").GetComponent<Text>();
            Text text3 = element.get_transform().FindChild("Time").GetComponent<Text>();
            if (text3 != null)
            {
                text3.get_gameObject().CustomSetActive(false);
            }
            if (component != null)
            {
                component.get_gameObject().CustomSetActive(true);
            }
            SetListElementLadderInfo(element, friend);
            COM_ACNT_GAME_STATE state = COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE;
            if (friend.bIsOnline == 1)
            {
                CFriendModel.FriendInGame friendInGaming = Singleton<CFriendContoller>.instance.model.GetFriendInGaming(friend.stUin.ullUid, friend.stUin.dwLogicWorldId);
                if (friendInGaming == null)
                {
                    state = COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE;
                }
                else
                {
                    state = friendInGaming.State;
                }
                if (state == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE)
                {
                    component.set_text(Singleton<CInviteSystem>.instance.GetInviteStateStr(friend.stUin.ullUid, false));
                    CUIEventScript script = obj3.GetComponent<CUIEventScript>();
                    script.m_onClickEventParams.tag = (int) Singleton<CInviteSystem>.instance.InviteType;
                    script.m_onClickEventParams.tag2 = (int) friend.stUin.dwLogicWorldId;
                    script.m_onClickEventParams.commonUInt64Param1 = friend.stUin.ullUid;
                }
                else if (((state == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_SINGLEGAME) || (state == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_MULTIGAME)) || (state == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_WAITMULTIGAME))
                {
                    if (friendInGaming == null)
                    {
                        component.get_gameObject().CustomSetActive(true);
                        component.set_text("friendInGame is null");
                        return;
                    }
                    if (friendInGaming.startTime > 0)
                    {
                        component.get_gameObject().CustomSetActive(false);
                        text3.get_gameObject().CustomSetActive(true);
                        text3.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Common_Gaming_Time"), GetStartMinute(friendInGaming.startTime)));
                        Singleton<CInviteSystem>.instance.CheckInviteListGameTimer();
                    }
                    else
                    {
                        component.get_gameObject().CustomSetActive(true);
                        component.set_text(string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.instance.GetText("Common_Gaming_NoTime")));
                    }
                }
                else if (state == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_TEAM)
                {
                    component.set_text(string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.instance.GetText("Common_Teaming")));
                }
                text2.set_color(CUIUtility.s_Color_White);
                CUIUtility.GetComponentInChildren<Image>(go).set_color(CUIUtility.s_Color_White);
            }
            else
            {
                component.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Common_Offline"), new object[0]));
                text2.set_color(CUIUtility.s_Color_Grey);
                CUIUtility.GetComponentInChildren<Image>(go).set_color(CUIUtility.s_Color_GrayShader);
            }
            obj3.CustomSetActive((friend.bIsOnline == 1) && (state == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE));
        }

        public static void SetInvitedRelatedWidgets(GameObject inviteButtonGo, Text gameStateText)
        {
            inviteButtonGo.CustomSetActive(false);
            gameStateText.set_text(Singleton<CTextManager>.GetInstance().GetText("Guild_Has_Invited"));
        }

        public static void SetInviteFriendData(CUIFormScript form, COM_INVITE_JOIN_TYPE joinType)
        {
            ListView<COMDT_FRIEND_INFO> allFriendList = Singleton<CInviteSystem>.instance.GetAllFriendList();
            int count = allFriendList.Count;
            int num2 = 0;
            form.GetWidget(2).GetComponent<CUIListScript>().SetElementAmount(count);
            form.GetWidget(3).get_gameObject().CustomSetActive(allFriendList.Count == 0);
            for (int i = 0; i < count; i++)
            {
                if (allFriendList[i].bIsOnline == 1)
                {
                    num2++;
                }
            }
            string[] args = new string[] { num2.ToString(), count.ToString() };
            form.GetWidget(4).GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", args));
            GameObject widget = form.GetWidget(9);
            if (CSysDynamicBlock.bLobbyEntryBlocked || (ApolloConfig.IsUseCEPackage() >= 1))
            {
                widget.CustomSetActive(false);
            }
            else
            {
                bool flag = false;
                Text component = Utility.FindChild(widget, "ShareInviteButton/Text").GetComponent<Text>();
                GameObject obj3 = widget.get_transform().FindChild("ShareInviteButton/IconQQ").get_gameObject();
                GameObject obj4 = widget.get_transform().FindChild("ShareInviteButton/IconWeixin").get_gameObject();
                if (Singleton<CRoomSystem>.GetInstance().IsInRoom)
                {
                    flag = true;
                }
                else if (Singleton<CMatchingSystem>.GetInstance().IsInMatchingTeam)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                if (flag)
                {
                    widget.CustomSetActive(true);
                    if (Singleton<ApolloHelper>.GetInstance().CurPlatform == ApolloPlatform.QQ)
                    {
                        component.set_text(Singleton<CTextManager>.GetInstance().GetText("Share_Room_Info_QQ"));
                        obj3.CustomSetActive(true);
                        obj4.CustomSetActive(false);
                    }
                    else if (Singleton<ApolloHelper>.GetInstance().CurPlatform == ApolloPlatform.Wechat)
                    {
                        component.set_text(Singleton<CTextManager>.GetInstance().GetText("Share_Room_Info_WX"));
                        obj3.CustomSetActive(false);
                        obj4.CustomSetActive(true);
                    }
                }
                else
                {
                    widget.CustomSetActive(false);
                }
            }
        }

        public static void SetInviteGuildMemberData(CUIFormScript form)
        {
            ListView<GuildMemInfo> allGuildMemberList = Singleton<CInviteSystem>.GetInstance().GetAllGuildMemberList();
            int count = allGuildMemberList.Count;
            int num2 = 0;
            RefreshInviteGuildMemberList(form, count);
            for (int i = 0; i < count; i++)
            {
                if (CGuildHelper.IsMemberOnline(allGuildMemberList[i]))
                {
                    num2++;
                }
            }
            string[] args = new string[] { num2.ToString(), count.ToString() };
            form.GetWidget(6).GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", args));
        }

        public static void SetLBSData(CUIFormScript form, COM_INVITE_JOIN_TYPE joinType)
        {
            CUIListScript component = form.GetWidget(11).GetComponent<CUIListScript>();
            Text text = form.GetWidget(12).GetComponent<Text>();
            int amount = 0;
            if (Singleton<CFriendContoller>.instance.model.EnableShareLocation)
            {
                ListView<CSDT_LBS_USER_INFO> lBSList = Singleton<CFriendContoller>.instance.model.GetLBSList(CFriendModel.LBSGenderType.Both);
                amount = (lBSList == null) ? 0 : lBSList.Count;
                component.SetElementAmount(amount);
                Utility.FindChild(form.GetWidget(10), "Empty/Normal").CustomSetActive(true);
                Utility.FindChild(form.GetWidget(10), "Empty/GotoBtn").CustomSetActive(false);
            }
            else
            {
                component.SetElementAmount(0);
                Utility.FindChild(form.GetWidget(10), "Empty/Normal").CustomSetActive(false);
                Utility.FindChild(form.GetWidget(10), "Empty/GotoBtn").CustomSetActive(true);
            }
            string[] args = new string[] { amount.ToString(), amount.ToString() };
            text.set_text(Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", args));
        }

        private static void SetLBSState(GameObject element, ref CSDT_LBS_USER_INFO LBSInfo)
        {
            COMDT_FRIEND_INFO stLbsUserInfo = LBSInfo.stLbsUserInfo;
            GameObject go = element.get_transform().FindChild("HeadBg").get_gameObject();
            Text component = element.get_transform().FindChild("Online").GetComponent<Text>();
            GameObject obj3 = element.get_transform().FindChild("InviteButton").get_gameObject();
            Text text2 = element.get_transform().FindChild("PlayerName").GetComponent<Text>();
            SetListElementLadderInfo(element, stLbsUserInfo);
            COM_ACNT_GAME_STATE friendInGamingState = COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE;
            if (stLbsUserInfo.bIsOnline != 1)
            {
                component.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Common_Offline"), new object[0]));
                text2.set_color(CUIUtility.s_Color_Grey);
                CUIUtility.GetComponentInChildren<Image>(go).set_color(CUIUtility.s_Color_GrayShader);
            }
            else
            {
                friendInGamingState = Singleton<CFriendContoller>.instance.model.GetFriendInGamingState(stLbsUserInfo.stUin.ullUid, stLbsUserInfo.stUin.dwLogicWorldId);
                switch (friendInGamingState)
                {
                    case COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE:
                    {
                        component.set_text(Singleton<CInviteSystem>.instance.GetInviteStateStr(stLbsUserInfo.stUin.ullUid, false));
                        CUIEventScript script = obj3.GetComponent<CUIEventScript>();
                        script.m_onClickEventParams.tag = (int) Singleton<CInviteSystem>.instance.InviteType;
                        script.m_onClickEventParams.tag2 = (int) stLbsUserInfo.stUin.dwLogicWorldId;
                        script.m_onClickEventParams.tag3 = (int) LBSInfo.dwGameSvrEntity;
                        script.m_onClickEventParams.commonUInt64Param1 = stLbsUserInfo.stUin.ullUid;
                        break;
                    }
                    case COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_SINGLEGAME:
                    case COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_MULTIGAME:
                    case COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_WAITMULTIGAME:
                        component.set_text(string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.instance.GetText("Common_Gaming")));
                        break;

                    case COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_TEAM:
                        component.set_text(string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.instance.GetText("Common_Teaming")));
                        break;
                }
                text2.set_color(CUIUtility.s_Color_White);
                CUIUtility.GetComponentInChildren<Image>(go).set_color(CUIUtility.s_Color_White);
            }
            obj3.CustomSetActive((stLbsUserInfo.bIsOnline == 1) && (friendInGamingState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE));
        }

        private static void SetListElementLadderInfo(GameObject element, GuildMemInfo guildMemInfo)
        {
            if (guildMemInfo != null)
            {
                int num;
                int num2;
                GameObject obj2 = element.get_transform().Find("RankCon").get_gameObject();
                if (obj2 != null)
                {
                    obj2.CustomSetActive(false);
                }
                GetGuildMemberGradeAndStar(guildMemInfo, out num, out num2);
                if (Singleton<CLadderSystem>.GetInstance().IsHaveFightRecord(false, num, num2))
                {
                    obj2.CustomSetActive(true);
                    CLadderView.ShowRankDetail(obj2, (byte) num, (byte) guildMemInfo.stBriefInfo.dwClassOfRank, !CGuildHelper.IsMemberOnline(guildMemInfo), true);
                }
            }
        }

        private static void SetListElementLadderInfo(GameObject element, COMDT_FRIEND_INFO friendInfo)
        {
            int num;
            int num2;
            GameObject obj2 = element.get_transform().Find("RankCon").get_gameObject();
            if (obj2 != null)
            {
                obj2.CustomSetActive(false);
            }
            GetFriendRankGradeAndStar(friendInfo, out num, out num2);
            if (Singleton<CLadderSystem>.GetInstance().IsHaveFightRecord(false, num, num2))
            {
                obj2.CustomSetActive(true);
                CLadderView.ShowRankDetail(obj2, (byte) num, (byte) friendInfo.dwRankClass, friendInfo.bIsOnline != 1, true);
            }
        }

        public static void UpdateFriendListElement(GameObject element, COMDT_FRIEND_INFO friend)
        {
            UpdateFriendListElementBase(element, ref friend);
            SetFriendState(element, ref friend);
        }

        public static void UpdateFriendListElementBase(GameObject element, ref COMDT_FRIEND_INFO friend)
        {
            GameObject go = element.get_transform().FindChild("HeadBg").get_gameObject();
            Text component = element.get_transform().FindChild("PlayerName").GetComponent<Text>();
            Image image = element.get_transform().FindChild("NobeIcon").GetComponent<Image>();
            Image image2 = element.get_transform().FindChild("HeadBg/NobeImag").GetComponent<Image>();
            if (image != null)
            {
                MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(image, (int) friend.stGameVip.dwCurLevel, false);
            }
            if (image2 != null)
            {
                MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(image2, (int) friend.stGameVip.dwHeadIconId);
            }
            CFriendModel.FriendInGame friendInGaming = Singleton<CFriendContoller>.instance.model.GetFriendInGaming(friend.stUin.ullUid, friend.stUin.dwLogicWorldId);
            if (friendInGaming == null)
            {
                component.set_text(ConnectPlayerNameAndNickName(friend.szUserName, string.Empty));
            }
            else
            {
                component.set_text(ConnectPlayerNameAndNickName(friend.szUserName, friendInGaming.NickName));
            }
            string url = Utility.UTF8Convert(friend.szHeadUrl);
            if (!CSysDynamicBlock.bFriendBlocked)
            {
                CUIUtility.GetComponentInChildren<CUIHttpImageScript>(go).SetImageUrl(Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(url));
            }
        }

        public static void UpdateGuildMemberListElement(GameObject element, GuildMemInfo guildMember, bool isGuildMatchInvite)
        {
            GameObject go = element.get_transform().FindChild("HeadBg").get_gameObject();
            GameObject obj3 = element.get_transform().FindChild("InviteButton").get_gameObject();
            Text component = element.get_transform().FindChild("PlayerName").GetComponent<Text>();
            Text gameStateText = element.get_transform().FindChild("Online").GetComponent<Text>();
            Image image = element.get_transform().FindChild("NobeIcon").GetComponent<Image>();
            Image image2 = element.get_transform().FindChild("HeadBg/NobeImag").GetComponent<Image>();
            Text text3 = element.get_transform().FindChild("Time").GetComponent<Text>();
            if (text3 != null)
            {
                text3.get_gameObject().CustomSetActive(false);
            }
            if (gameStateText != null)
            {
                gameStateText.get_gameObject().CustomSetActive(true);
            }
            Transform transform = element.get_transform().FindChild("RemindButton");
            GameObject obj4 = null;
            if (transform != null)
            {
                obj4 = transform.get_gameObject();
                obj4.CustomSetActive(false);
            }
            SetListElementLadderInfo(element, guildMember);
            if (image != null)
            {
                MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(image, (int) guildMember.stBriefInfo.stVip.level, false);
            }
            if (image2 != null)
            {
                MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(image2, (int) guildMember.stBriefInfo.stVip.headIconId);
            }
            component.set_text(Utility.UTF8Convert(guildMember.stBriefInfo.sName));
            if (CGuildHelper.IsMemberOnline(guildMember))
            {
                if (guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE)
                {
                    gameStateText.set_text(Singleton<CInviteSystem>.instance.GetInviteStateStr(guildMember.stBriefInfo.uulUid, isGuildMatchInvite));
                    CUIEventScript script = obj3.GetComponent<CUIEventScript>();
                    script.m_onClickEventParams.tag = (int) Singleton<CInviteSystem>.instance.InviteType;
                    script.m_onClickEventParams.tag2 = guildMember.stBriefInfo.dwLogicWorldId;
                    script.m_onClickEventParams.commonUInt64Param1 = guildMember.stBriefInfo.uulUid;
                }
                else if (((guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_SINGLEGAME) || (guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_MULTIGAME)) || (guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_WAITMULTIGAME))
                {
                    if (guildMember.dwGameStartTime > 0)
                    {
                        if (gameStateText != null)
                        {
                            gameStateText.get_gameObject().CustomSetActive(false);
                        }
                        if (text3 != null)
                        {
                            text3.get_gameObject().CustomSetActive(true);
                        }
                        if (text3 != null)
                        {
                            text3.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Common_Gaming_Time"), GetStartMinute(guildMember.dwGameStartTime)));
                        }
                        Singleton<CInviteSystem>.instance.CheckInviteListGameTimer();
                    }
                    else
                    {
                        if (gameStateText != null)
                        {
                            gameStateText.get_gameObject().CustomSetActive(true);
                        }
                        if (gameStateText != null)
                        {
                            gameStateText.set_text(string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.instance.GetText("Common_Gaming_NoTime")));
                        }
                    }
                }
                else if (guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_TEAM)
                {
                    gameStateText.set_text(string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.instance.GetText("Common_Teaming")));
                }
                component.set_color(CUIUtility.s_Color_White);
                CUIUtility.GetComponentInChildren<Image>(go).set_color(CUIUtility.s_Color_White);
            }
            else
            {
                gameStateText.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Common_Offline"), new object[0]));
                component.set_color(CUIUtility.s_Color_Grey);
                CUIUtility.GetComponentInChildren<Image>(go).set_color(CUIUtility.s_Color_GrayShader);
            }
            obj3.CustomSetActive(CGuildHelper.IsMemberOnline(guildMember) && (guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE));
            string szHeadUrl = guildMember.stBriefInfo.szHeadUrl;
            CUIUtility.GetComponentInChildren<CUIHttpImageScript>(go).SetImageUrl(Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(szHeadUrl));
            if ((isGuildMatchInvite && Singleton<CGuildMatchSystem>.GetInstance().IsInTeam(guildMember.GuildMatchInfo.ullTeamLeaderUid, Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)) && CGuildHelper.IsMemberOnline(guildMember))
            {
                SetInvitedRelatedWidgets(obj3, gameStateText);
                if (((guildMember.GameState == COM_ACNT_GAME_STATE.COM_ACNT_GAME_STATE_IDLE) && !Convert.ToBoolean(guildMember.GuildMatchInfo.bIsReady)) && (obj4 != null))
                {
                    obj4.CustomSetActive(true);
                    obj4.GetComponent<CUIEventScript>().m_onClickEventParams.commonUInt64Param1 = guildMember.stBriefInfo.uulUid;
                }
            }
        }

        public static void UpdateLBSListElement(GameObject element, CSDT_LBS_USER_INFO LBSInfo)
        {
            UpdateFriendListElementBase(element, ref LBSInfo.stLbsUserInfo);
            SetLBSState(element, ref LBSInfo);
        }

        public enum enInviteFormWidget
        {
            Friend_Panel,
            GuildMember_Panel,
            Friend_List,
            FriendEmpty_Panel,
            FriendTotalNum_Text,
            GuildMember_List,
            GuildMemberTotalNum_Text,
            InviteTab_List,
            RefreshGuildMemberGameState_Timer,
            Bottom_Widget,
            LBS_Panel,
            LBS_List,
            LBSTotalNum_Text
        }

        public enum enInviteListTab
        {
            Friend,
            GuildMember,
            LBS,
            Count
        }
    }
}

