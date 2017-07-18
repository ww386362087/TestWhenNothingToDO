﻿namespace Assets.Scripts.GameSystem
{
    using Apollo;
    using Assets.Scripts.Framework;
    using Assets.Scripts.UI;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    [MessageHandlerClass]
    public class IDIPSys : MonoSingleton<IDIPSys>
    {
        [CompilerGenerated]
        private static Comparison<IDIPData> <>f__am$cache18;
        private Sprite m_BackImageSprite;
        private uint[] m_BanTimeInfo = new uint[100];
        private bool m_bFirst = true;
        private bool m_bHaveUpdateList;
        private bool m_bShow;
        private GameObject m_BtnDoSth;
        public int m_ChannelID;
        private IDIPData m_CurSelectData;
        private uint m_DataVersion;
        public ListView<IDIPData> m_IDIPDataList = new ListView<IDIPData>();
        private CUIFormScript m_IDIPForm;
        private List<IDIPItem> m_IDIPItemList;
        private Image m_ImageContent;
        private string m_MatchBegin = "<button=";
        private string m_MatchChildEnd = "=$";
        private string m_MatchEnd = "$>";
        private string m_matchTitle = "title=";
        private string m_MyOpenID = string.Empty;
        private int m_nRedPoint;
        private RectTransform m_ScrollRect;
        private Text m_TextContent;
        private Text m_Title;
        private CUIListScript m_uiListMenu;
        public static string s_formPath = (CUIUtility.s_Form_Activity_Dir + "Form_Activity.prefab");

        private void BuildMenuList()
        {
            this.m_IDIPItemList = new List<IDIPItem>();
            int count = this.m_IDIPDataList.Count;
            this.m_uiListMenu.SetElementAmount(count);
            for (int i = 0; i < count; i++)
            {
                IDIPData data = this.m_IDIPDataList[i];
                CUIListElementScript elemenet = this.m_uiListMenu.GetElemenet(i);
                IDIPItem item = new IDIPItem(elemenet.get_gameObject());
                item.name.set_text(data.title);
                if (item.glow != null)
                {
                    item.glow.get_gameObject().CustomSetActive(false);
                }
                if (data.bNoticeLabelType == 1)
                {
                    item.flag.SetSprite(CUIUtility.GetSpritePrefeb("UGUI/Sprite/Dynamic/Activity/RES_WEAL_COLORBAR_TYPE_NOTICE", false, false), false);
                    item.TypeText.set_text("公告");
                }
                else if (data.bNoticeLabelType == 2)
                {
                    item.flag.SetSprite(CUIUtility.GetSpritePrefeb("UGUI/Sprite/Dynamic/Activity/RES_WEAL_COLORBAR_TYPE_LIMIT", false, false), false);
                    item.TypeText.set_text("活动");
                }
                else if (data.bNoticeLabelType == 3)
                {
                    item.flag.SetSprite(CUIUtility.GetSpritePrefeb("UGUI/Sprite/Dynamic/Activity/RES_WEAL_COLORBAR_TYPE_HOT", false, false), false);
                    item.TypeText.set_text("赛事");
                }
                this.m_IDIPItemList.Add(item);
            }
        }

        private void CheckIsBtnUrl(IDIPData data)
        {
            string content = data.content;
            int index = content.IndexOf(this.m_MatchBegin);
            if (index > 0)
            {
                string str2 = content.Substring(0, index);
                int num2 = content.IndexOf(this.m_MatchEnd);
                if (num2 < 0)
                {
                    data.content = str2;
                }
                else
                {
                    int num3 = content.IndexOf(this.m_MatchChildEnd);
                    if (num3 < 0)
                    {
                        data.content = str2;
                    }
                    else
                    {
                        if (content.Substring(index + this.m_MatchBegin.Length, (num3 - index) - this.m_MatchBegin.Length) == "0")
                        {
                            data.btnDoSth = BTN_DOSOMTHING.BTN_DOSOMTHING_URL;
                        }
                        else
                        {
                            data.btnDoSth = BTN_DOSOMTHING.BTN_DOSOMTHING_GAME;
                        }
                        data.btnTitle = "去完成";
                        string str4 = content.Substring(num3 + this.m_MatchChildEnd.Length, (num2 - num3) - this.m_MatchChildEnd.Length);
                        if (str4.Contains(this.m_MatchChildEnd))
                        {
                            string str5 = str4;
                            int length = str5.IndexOf(this.m_MatchChildEnd);
                            string str6 = str5.Substring(0, length);
                            data.btnUrl = str6;
                            string str7 = str5.Substring(length + this.m_MatchChildEnd.Length, (str5.Length - length) - this.m_MatchChildEnd.Length);
                            int num5 = str7.IndexOf(this.m_matchTitle);
                            if ((num5 >= 0) && (str7.Length > 0))
                            {
                                string str8 = str7.Substring(num5 + this.m_matchTitle.Length, (str7.Length - num5) - this.m_matchTitle.Length);
                                data.btnTitle = str8;
                            }
                        }
                        else
                        {
                            data.btnUrl = str4;
                        }
                        data.content = str2;
                    }
                }
            }
            else
            {
                data.btnUrl = string.Empty;
                data.btnDoSth = BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
            }
        }

        private void CheckValidNotice()
        {
            for (int i = this.m_IDIPDataList.Count - 1; i >= 0; i--)
            {
                IDIPData item = this.m_IDIPDataList[i];
                if ((item != null) && ((CRoleInfo.GetCurrentUTCTime() < item.startTime) || (CRoleInfo.GetCurrentUTCTime() >= item.endTime)))
                {
                    this.m_IDIPDataList.Remove(item);
                }
            }
        }

        private void ClearContent()
        {
            if (this.m_bShow)
            {
                if (this.m_TextContent != null)
                {
                    this.m_TextContent.get_gameObject().CustomSetActive(false);
                }
                if (this.m_Title != null)
                {
                    this.m_Title.get_gameObject().CustomSetActive(false);
                }
                if (this.m_ImageContent != null)
                {
                    this.m_ImageContent.get_gameObject().CustomSetActive(false);
                }
            }
        }

        public void ClearIDIPData()
        {
            this.m_IDIPDataList.Clear();
            this.m_DataVersion = 0;
        }

        [DebuggerHidden]
        public IEnumerator DownloadImage(string preUrl, LoadRCallBack callBack, int checkDays = 0)
        {
            <DownloadImage>c__Iterator2A iteratora = new <DownloadImage>c__Iterator2A();
            iteratora.preUrl = preUrl;
            iteratora.checkDays = checkDays;
            iteratora.callBack = callBack;
            iteratora.<$>preUrl = preUrl;
            iteratora.<$>checkDays = checkDays;
            iteratora.<$>callBack = callBack;
            iteratora.<>f__this = this;
            return iteratora;
        }

        [DebuggerHidden]
        public IEnumerator DownloadImageByTag(string preUrl, int ImageIDx, LoadRCallBack2 callBack, string tagPath, int checkDays = 0)
        {
            <DownloadImageByTag>c__Iterator2B iteratorb = new <DownloadImageByTag>c__Iterator2B();
            iteratorb.preUrl = preUrl;
            iteratorb.tagPath = tagPath;
            iteratorb.checkDays = checkDays;
            iteratorb.callBack = callBack;
            iteratorb.ImageIDx = ImageIDx;
            iteratorb.<$>preUrl = preUrl;
            iteratorb.<$>tagPath = tagPath;
            iteratorb.<$>checkDays = checkDays;
            iteratorb.<$>callBack = callBack;
            iteratorb.<$>ImageIDx = ImageIDx;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public DateTime GetBanTime(COM_ACNT_BANTIME_TYPE kType)
        {
            if (kType < COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_MAX)
            {
                return Utility.ToUtcTime2Local((long) this.m_BanTimeInfo[(int) kType]);
            }
            return new DateTime();
        }

        [DebuggerHidden]
        public IEnumerator GetChannelID()
        {
            <GetChannelID>c__Iterator29 iterator = new <GetChannelID>c__Iterator29();
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void Init()
        {
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IDIP_GOTO_COMPLETE, new CUIEventManager.OnUIEventHandler(this.OnBtnComplete));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.XinYue_Open, new CUIEventManager.OnUIEventHandler(this.OnClickOpenXinYue));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OPEN_HELPME, new CUIEventManager.OnUIEventHandler(this.OnClickOpenHelpMe));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OPEN_HELPMEMONEY, new CUIEventManager.OnUIEventHandler(this.OnClickOpenHelpMeMoney));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OPEN_TongCai, new CUIEventManager.OnUIEventHandler(this.OnClickOpenTongCai));
            Singleton<CTimerManager>.GetInstance().AddTimer(0x493e0, -1, new CTimer.OnTimeUpHandler(this.OnRequestNoticeNum));
            Singleton<PopupMenuListSys>.CreateInstance();
        }

        public bool IsUseCoinforbid()
        {
            DateTime banTime = this.GetBanTime(COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_COINFROZEN);
            DateTime time2 = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
            if (banTime > time2)
            {
                string text = Singleton<CTextManager>.GetInstance().GetText("UseGoldCoinForbid");
                Singleton<CUIManager>.GetInstance().OpenMessageBox(text, false);
                return true;
            }
            return false;
        }

        public bool IsUseDiamondforbid()
        {
            DateTime banTime = this.GetBanTime(COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_DIAMONDFROZEN);
            DateTime time2 = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
            if (banTime > time2)
            {
                string text = Singleton<CTextManager>.GetInstance().GetText("UseDiamondForbid");
                Singleton<CUIManager>.GetInstance().OpenMessageBox(text, false);
                return true;
            }
            return false;
        }

        public bool IsUseDianQuanForbid()
        {
            DateTime banTime = this.GetBanTime(COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_COUPONSFROZEN);
            DateTime time2 = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
            if (banTime > time2)
            {
                string text = Singleton<CTextManager>.GetInstance().GetText("UseDianQuanForbid");
                Singleton<CUIManager>.GetInstance().OpenMessageBox(text, false);
                return true;
            }
            return false;
        }

        private bool isVisited(ulong noticeTime)
        {
            return (PlayerPrefs.GetInt(string.Format("Notice|{0}|{1}", this.m_MyOpenID, noticeTime.ToString())) > 0);
        }

        private string MakeXinYueHttp()
        {
            string str = string.Empty;
            ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
            if ((accountInfo == null) || ((accountInfo.Platform != ApolloPlatform.QQ) && (accountInfo.Platform != ApolloPlatform.Wechat)))
            {
                return str;
            }
            int num = 0x34;
            string s = string.Empty;
            string str3 = string.Empty;
            string qQAppID = string.Empty;
            int num2 = 0;
            string str5 = string.Empty;
            int logicWorldID = MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID;
            int num4 = 0;
            if (accountInfo.Platform == ApolloPlatform.QQ)
            {
                qQAppID = ApolloConfig.QQAppID;
                num2 = 1;
            }
            else
            {
                qQAppID = ApolloConfig.WXAppID;
                num2 = 2;
            }
            for (int i = 0; i < accountInfo.TokenList.Count; i++)
            {
                if (accountInfo.TokenList[i].Type == ApolloTokenType.Access)
                {
                    str5 = accountInfo.TokenList[i].Value;
                    break;
                }
            }
            object[] args = new object[] { accountInfo.OpenId, str5, qQAppID, num2 };
            s = string.Format("{0},{1},{2},{3}", args);
            str3 = Convert.ToBase64String(Encoding.Default.GetBytes(s));
            object[] objArray2 = new object[] { num, str3, logicWorldID, num4 };
            return string.Format("http://apps.game.qq.com/php/tgclub/v2/mobile_open/redirect?game_id={0}&opencode={1}&partition_id={2}&role_id={3}", objArray2);
        }

        [MessageHandler(0x413)]
        public static void ModifyBantimeInfo(CSPkg msg)
        {
            for (int i = 0; i < msg.stPkgData.stBanTimeChg.bBanTypeNum; i++)
            {
                ushort num2 = msg.stPkgData.stBanTimeChg.BanType[i];
                MonoSingleton<IDIPSys>.GetInstance().SetBanTimeInfo((COM_ACNT_BANTIME_TYPE) num2, msg.stPkgData.stBanTimeChg.BanTime[i]);
            }
        }

        [MessageHandler(0x58d)]
        public static void On_GetNiticeNum(CSPkg msg)
        {
            if (msg.stPkgData.stNoticeNewRsp.bHaveNew > 0)
            {
                MonoSingleton<IDIPSys>.GetInstance().HaveUpdateList = true;
            }
            else
            {
                MonoSingleton<IDIPSys>.GetInstance().HaveUpdateList = false;
            }
            Singleton<EventRouter>.instance.BroadCastEvent("IDIPNOTICE_UNREAD_NUM_UPDATE");
        }

        [MessageHandler(0x591)]
        public static void On_GetNoticeContentInfo(CSPkg msg)
        {
            MonoSingleton<IDIPSys>.GetInstance().PrcessOnNoticeInfo(msg);
        }

        [MessageHandler(0x58f)]
        public static void On_GetNoticeList(CSPkg msg)
        {
            MonoSingleton<IDIPSys>.GetInstance().ProcessGetNoticeList(msg);
        }

        [MessageHandler(0x529)]
        public static void On_GetQQBoxInfo(CSPkg msg)
        {
            if (msg.stPkgData.stGainChestRsp.iResult == 0)
            {
                int iActId = msg.stPkgData.stGainChestRsp.iActId;
                string boxID = Utility.UTF8Convert(msg.stPkgData.stGainChestRsp.szGainChestId);
                string title = Utility.UTF8Convert(msg.stPkgData.stGainChestRsp.szChestTitle, msg.stPkgData.stGainChestRsp.wTitleLen);
                string desc = Utility.UTF8Convert(msg.stPkgData.stGainChestRsp.szChestContent, msg.stPkgData.stGainChestRsp.wContentLen);
                Debug.Log("QQBox  receive srv msg boxid " + boxID);
                Singleton<ApolloHelper>.GetInstance().ShareQQBox(iActId.ToString(), boxID, title, desc);
            }
            else
            {
                Debug.Log("QQBox getboxinfo error " + msg.stPkgData.stGainChestRsp.iResult);
            }
        }

        [MessageHandler(0x57d)]
        public static void On_IDIPSelfMsgInfo(CSPkg msg)
        {
            for (int i = 0; i < msg.stPkgData.stAcntSelfMsgInfo.bMsgCnt; i++)
            {
                CSDT_SELFMSG_INFO csdt_selfmsg_info = msg.stPkgData.stAcntSelfMsgInfo.astMsgList[i];
                string str = Utility.UTF8Convert(csdt_selfmsg_info.szContent, csdt_selfmsg_info.wContentLen);
                PopupMenuListSys.PopupMenuListItem item = new PopupMenuListSys.PopupMenuListItem();
                item.m_show = new PopupMenuListSys.PopupMenuListItem.Show(MonoSingleton<IDIPSys>.GetInstance().ShowSelfMsgInfo);
                item.content = str;
                Singleton<PopupMenuListSys>.GetInstance().AddItem(item);
            }
            Singleton<PopupMenuListSys>.GetInstance().PopupMenuListStart();
        }

        private void OnBtnComplete(CUIEvent ciEvent)
        {
            if ((this.m_CurSelectData != null) && this.m_bShow)
            {
                IDIPData curSelectData = this.m_CurSelectData;
                if (curSelectData.btnDoSth == BTN_DOSOMTHING.BTN_DOSOMTHING_URL)
                {
                    CUICommonSystem.OpenUrl(curSelectData.btnUrl, true, 0);
                }
                else if (curSelectData.btnDoSth == BTN_DOSOMTHING.BTN_DOSOMTHING_GAME)
                {
                    int result = 0;
                    int.TryParse(curSelectData.btnUrl, out result);
                    if (result > 0)
                    {
                        CUICommonSystem.JumpForm((RES_GAME_ENTRANCE_TYPE) result, 0, 0);
                    }
                }
            }
        }

        public void OnClickOpenHelpMe(CUIEvent uiEvent)
        {
            CUICommonSystem.OpenUrl("https://kf.qq.com/touch/scene_product.html?scene_id=kf1062", true, 0);
        }

        public void OnClickOpenHelpMeMoney(CUIEvent uiEvent)
        {
            string strUrl = null;
            strUrl = "https://kf.qq.com/touch/scene_faq.html?scene_id=kf1064";
            CUICommonSystem.OpenUrl(strUrl, true, 0);
        }

        public void OnClickOpenTongCai(CUIEvent uiEvent)
        {
            ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
            MonoSingleton<CTongCaiSys>.instance.OpenTongCaiH5(accountInfo);
        }

        public void OnClickOpenXinYue(CUIEvent uiEvent)
        {
            Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().ShowGameRedDot = false;
            CUICommonSystem.OpenUrl(this.MakeXinYueHttp(), true, 0);
        }

        public void OnCloseIDIPForm(CUIEvent uiEvent)
        {
            this.m_IDIPForm = null;
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IDIP_SelectItem, new CUIEventManager.OnUIEventHandler(this.OnSelectItem));
            this.m_bShow = false;
            this.m_bFirst = false;
            if (((this.m_ImageContent != null) && (this.m_ImageContent.get_sprite() != null)) && (this.m_BackImageSprite != this.m_ImageContent.get_sprite()))
            {
                Object.Destroy(this.m_ImageContent.get_sprite());
            }
            this.m_CurSelectData = null;
        }

        public void OnOpenIDIPForm(CUIFormScript IDIPForm)
        {
            if (!CSysDynamicBlock.bLobbyEntryBlocked)
            {
                this.m_IDIPForm = IDIPForm;
                this.m_bShow = true;
                this.m_bFirst = true;
                this.HaveUpdateList = false;
                this.RequestNoticeList();
                this.ShowForm();
            }
        }

        private void OnRequestNoticeNum(int timer)
        {
            if (Singleton<CBattleSystem>.instance.FormScript == null)
            {
                this.RequestNoticeNum();
            }
        }

        private void OnSelectItem(CUIEvent uiEvent)
        {
            this.SelectMenuItem(uiEvent.m_srcWidgetIndexInBelongedList);
        }

        private void PrcessOnNoticeInfo(CSPkg msg)
        {
            for (int i = 0; i < this.m_IDIPDataList.Count; i++)
            {
                IDIPData data = this.m_IDIPDataList[i];
                if ((data != null) && (data.dwNoticeID == msg.stPkgData.stNoticeInfoRsp.dwNoticeID))
                {
                    data.content = Utility.UTF8Convert(msg.stPkgData.stNoticeInfoRsp.szContent, msg.stPkgData.stNoticeInfoRsp.wContentLen);
                    this.CheckIsBtnUrl(data);
                    data.ullNoticeTime = msg.stPkgData.stNoticeInfoRsp.ullNoticeTime;
                    data.bLoad = true;
                    MonoSingleton<IDIPSys>.GetInstance().UpdateContent(data);
                    break;
                }
            }
        }

        private void ProcessGetNoticeList(CSPkg msg)
        {
            this.m_MyOpenID = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false).OpenId;
            this.m_DataVersion = msg.stPkgData.stNoticeListRsp.dwDataVersion;
            ListView<IDIPData> view = new ListView<IDIPData>();
            for (int i = 0; i < msg.stPkgData.stNoticeListRsp.wNoticeCnt; i++)
            {
                CSDT_NOTICE_INFO csdt_notice_info = msg.stPkgData.stNoticeListRsp.astNoticeList[i];
                IDIPData data = null;
                for (int j = 0; j < this.m_IDIPDataList.Count; j++)
                {
                    IDIPData data2 = this.m_IDIPDataList[j];
                    if ((data2.dwLogicWorldID == csdt_notice_info.dwLogicWorldID) && (data2.ullNoticeTime == csdt_notice_info.ullNoticeTime))
                    {
                        data = data2;
                        break;
                    }
                }
                if (data == null)
                {
                    data = new IDIPData();
                    data.dwNoticeID = csdt_notice_info.dwNoticeID;
                    data.startTime = csdt_notice_info.dwStartTime;
                    data.endTime = csdt_notice_info.dwEndTime;
                    data.bNoticeType = csdt_notice_info.bNoticeType;
                    data.bNoticeLabelType = csdt_notice_info.bNoticeLabelType;
                    data.bPriority = csdt_notice_info.bPriority;
                    data.dwLogicWorldID = csdt_notice_info.dwLogicWorldID;
                    data.ullNoticeTime = csdt_notice_info.ullNoticeTime;
                    data.bLoad = false;
                    if (!this.isVisited(data.ullNoticeTime))
                    {
                        data.bVisited = false;
                    }
                    else
                    {
                        data.bVisited = true;
                    }
                    data.title = Utility.UTF8Convert(csdt_notice_info.szSubject, csdt_notice_info.bSubjectLen);
                    data.content = Utility.UTF8Convert(csdt_notice_info.szContent, csdt_notice_info.wContentLen);
                    this.CheckIsBtnUrl(data);
                    view.Add(data);
                }
                else
                {
                    view.Add(data);
                }
            }
            this.m_IDIPDataList.Clear();
            this.m_IDIPDataList = view;
            if ((this.m_IDIPDataList.Count > 0) && Singleton<CLobbySystem>.GetInstance().IsInLobbyForm())
            {
                this.ShowForm();
            }
        }

        private void RequestNoticeContentInfo(int idx)
        {
            if (idx < this.m_IDIPDataList.Count)
            {
                IDIPData data = this.m_IDIPDataList[idx];
                CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x590);
                msg.stPkgData.stNoticeInfoReq.dwLogicWorldID = data.dwLogicWorldID;
                msg.stPkgData.stNoticeInfoReq.dwNoticeID = data.dwNoticeID;
                msg.stPkgData.stNoticeInfoReq.ullNoticeTime = data.ullNoticeTime;
                Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
            }
        }

        private void RequestNoticeList()
        {
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x58e);
            msg.stPkgData.stNoticeListReq.dwDataVersion = this.m_DataVersion;
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }

        public void RequestNoticeNum()
        {
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x58c);
            Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
        }

        public void RequestQQBox()
        {
            ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
            CSPkg msg = NetworkModule.CreateDefaultCSPKG(0x528);
            if (accountInfo != null)
            {
                string pf = accountInfo.Pf;
                if (accountInfo.Pf == string.Empty)
                {
                    pf = "desktop_m_qq-73213123-android-73213123-qq-1104466820-BC569F700D770A26CD422F24FD675F10";
                }
                Utility.StringToByteArray(pf, ref msg.stPkgData.stGainChestReq.szPf);
                Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref msg, false);
            }
        }

        private void SelectMenuItem(int index)
        {
            this.ClearContent();
            for (int i = 0; i < this.m_IDIPItemList.Count; i++)
            {
                if (i == index)
                {
                    IDIPItem item = this.m_IDIPItemList[i];
                    if (item.glow != null)
                    {
                        IDIPItem item2 = this.m_IDIPItemList[i];
                        item2.glow.get_gameObject().CustomSetActive(true);
                    }
                }
                else
                {
                    IDIPItem item3 = this.m_IDIPItemList[i];
                    if (item3.glow != null)
                    {
                        IDIPItem item4 = this.m_IDIPItemList[i];
                        item4.glow.get_gameObject().CustomSetActive(false);
                    }
                }
            }
            if ((index >= 0) && (index < this.m_IDIPDataList.Count))
            {
                IDIPData curData = this.m_IDIPDataList[index];
                if (curData != null)
                {
                    curData.bVisited = true;
                    this.SetVisited(curData.ullNoticeTime);
                    IDIPItem item5 = this.m_IDIPItemList[index];
                    if (item5.RedSpot != null)
                    {
                        IDIPItem item6 = this.m_IDIPItemList[index];
                        item6.RedSpot.SetActive(false);
                    }
                    if (curData != null)
                    {
                        if (curData.bLoad)
                        {
                            this.UpdateContent(curData);
                        }
                        else
                        {
                            this.UpdateContent(curData);
                            this.RequestNoticeContentInfo(index);
                        }
                    }
                }
            }
            this.UpdateRedPoint();
            if (this.m_ScrollRect != null)
            {
                this.m_ScrollRect.set_anchoredPosition(new Vector2(this.m_ScrollRect.get_anchoredPosition().x, 0f));
            }
        }

        public void SetBanTimeInfo(COM_ACNT_BANTIME_TYPE kType, uint kBanTime)
        {
            if (kType < COM_ACNT_BANTIME_TYPE.COM_ACNT_BANTIME_MAX)
            {
                this.m_BanTimeInfo[(int) kType] = kBanTime;
            }
        }

        private void SetVisited(ulong noticeTime)
        {
            PlayerPrefs.SetInt(string.Format("Notice|{0}|{1}", this.m_MyOpenID, noticeTime), 1);
        }

        public void ShareActivityTask(string cdnUrl)
        {
            <ShareActivityTask>c__AnonStorey5C storeyc = new <ShareActivityTask>c__AnonStorey5C();
            storeyc.cdnUrl = cdnUrl;
            storeyc.<>f__this = this;
            base.StartCoroutine(this.DownloadImage(storeyc.cdnUrl, new LoadRCallBack(storeyc.<>m__46), 0));
        }

        private void ShowBtnDoSth(IDIPData curData)
        {
            this.m_CurSelectData = null;
            if (curData.btnDoSth == BTN_DOSOMTHING.BTN_DOSOMTHING_NONE)
            {
                this.m_BtnDoSth.CustomSetActive(false);
            }
            else
            {
                this.m_CurSelectData = curData;
                this.m_BtnDoSth.CustomSetActive(true);
                if ((this.m_BtnDoSth != null) && (this.m_CurSelectData != null))
                {
                    Transform transform = this.m_BtnDoSth.get_transform().Find("Text");
                    if (transform != null)
                    {
                        transform.GetComponent<Text>().set_text(this.m_CurSelectData.btnTitle);
                    }
                }
            }
        }

        private void ShowForm()
        {
            if (this.m_bShow)
            {
                Transform transform = this.m_IDIPForm.get_transform().Find("Panel/Panle_Activity");
                if (transform != null)
                {
                    transform.get_gameObject().CustomSetActive(false);
                }
                Transform transform2 = this.m_IDIPForm.get_transform().Find("Panel/Panle_IDIP");
                if (transform2 != null)
                {
                    transform2.get_gameObject().CustomSetActive(true);
                }
                this.m_uiListMenu = Utility.GetComponetInChild<CUIListScript>(this.m_IDIPForm.get_gameObject(), "Panel/Panle_IDIP/Menu/List");
                this.m_TextContent = Utility.GetComponetInChild<Text>(this.m_IDIPForm.get_gameObject(), "Panel/Panle_IDIP/ScrollRect/Content/Text");
                this.m_ImageContent = Utility.GetComponetInChild<Image>(this.m_IDIPForm.get_gameObject(), "Panel/Panle_IDIP/Image");
                this.m_Title = Utility.GetComponetInChild<Text>(this.m_IDIPForm.get_gameObject(), "Panel/Panle_IDIP/GameObject/ContentTitle");
                if ((this.m_ImageContent != null) && this.m_bFirst)
                {
                    this.m_bFirst = false;
                    this.m_BackImageSprite = this.m_ImageContent.get_sprite();
                }
                this.m_ScrollRect = this.m_IDIPForm.get_gameObject().get_transform().FindChild("Panel/Panle_IDIP/ScrollRect/Content").GetComponent<RectTransform>();
                this.m_BtnDoSth = this.m_IDIPForm.get_gameObject().get_transform().FindChild("Panel/Panle_IDIP/Button_DoComplete").get_gameObject();
                this.m_BtnDoSth.CustomSetActive(false);
                this.CheckValidNotice();
                this.SortbyPriority();
                this.BuildMenuList();
                this.SelectMenuItem(0);
                this.m_uiListMenu.SelectElement(0, true);
                this.UpdateRedPoint();
                Singleton<EventRouter>.instance.BroadCastEvent("IDIPNOTICE_UNREAD_NUM_UPDATE");
                Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IDIP_SelectItem, new CUIEventManager.OnUIEventHandler(this.OnSelectItem));
            }
        }

        private void ShowSelfMsgInfo(string content)
        {
            Singleton<CUIManager>.GetInstance().OpenMessageBox(content, enUIEventID.MENU_PopupMenuFinish, false);
        }

        private void SortbyPriority()
        {
            if (this.m_IDIPDataList.Count > 0)
            {
                if (<>f__am$cache18 == null)
                {
                    <>f__am$cache18 = delegate (IDIPData a, IDIPData b) {
                        if (a.bPriority < b.bPriority)
                        {
                            return -1;
                        }
                        if (a.bPriority > b.bPriority)
                        {
                            return 1;
                        }
                        if (a.ullNoticeTime > b.ullNoticeTime)
                        {
                            return -1;
                        }
                        if (a.ullNoticeTime < b.ullNoticeTime)
                        {
                            return 1;
                        }
                        return 0;
                    };
                }
                this.m_IDIPDataList.Sort(<>f__am$cache18);
            }
        }

        public string ToMD5(string str)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("X2"));
            }
            return builder.ToString();
        }

        private void UpdateContent(IDIPData curData)
        {
            if (this.m_bShow)
            {
                if (curData.bNoticeType == 1)
                {
                    if (this.m_ImageContent != null)
                    {
                        this.m_ImageContent.get_gameObject().CustomSetActive(true);
                    }
                    DebugHelper.Assert((this.m_TextContent != null) && (this.m_Title != null));
                    if (this.m_TextContent != null)
                    {
                        this.m_TextContent.get_gameObject().CustomSetActive(false);
                    }
                    if (this.m_Title != null)
                    {
                        this.m_Title.get_gameObject().CustomSetActive(false);
                    }
                    base.StartCoroutine(this.DownloadImage(curData.content, delegate (Texture2D text2) {
                        if (this.m_bShow && (this.m_ImageContent != null))
                        {
                            this.m_ImageContent.SetSprite(Sprite.Create(text2, new Rect(0f, 0f, (float) text2.get_width(), (float) text2.get_height()), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
                        }
                    }, 0));
                }
                else if (curData.bNoticeType == 0)
                {
                    DebugHelper.Assert(((this.m_ImageContent != null) && (this.m_TextContent != null)) && (this.m_Title != null));
                    if (this.m_ImageContent != null)
                    {
                        this.m_ImageContent.get_gameObject().CustomSetActive(false);
                    }
                    if (this.m_TextContent != null)
                    {
                        this.m_TextContent.get_gameObject().CustomSetActive(true);
                        this.m_TextContent.set_text(curData.content);
                        RectTransform component = this.m_TextContent.get_transform().get_parent().get_gameObject().GetComponent<RectTransform>();
                        if (component != null)
                        {
                            component.set_sizeDelta(new Vector2(component.get_sizeDelta().x, this.m_TextContent.get_preferredHeight() + 50f));
                        }
                    }
                    if (this.m_Title != null)
                    {
                        this.m_Title.set_text(curData.title);
                        this.m_Title.get_gameObject().CustomSetActive(true);
                    }
                }
                this.ShowBtnDoSth(curData);
            }
        }

        public void UpdateGlobalPoint()
        {
            int num = 0;
            int count = this.m_IDIPDataList.Count;
            for (int i = 0; i < count; i++)
            {
                IDIPData data = this.m_IDIPDataList[i];
                if ((data != null) && !data.bVisited)
                {
                    num++;
                }
            }
            this.m_nRedPoint = num;
            Singleton<EventRouter>.instance.BroadCastEvent("IDIPNOTICE_UNREAD_NUM_UPDATE");
        }

        private void UpdateRedPoint()
        {
            int num = 0;
            int count = this.m_IDIPDataList.Count;
            int num3 = this.m_IDIPItemList.Count;
            for (int i = 0; i < count; i++)
            {
                IDIPData data = this.m_IDIPDataList[i];
                if ((data != null) && (i < num3))
                {
                    if (!data.bVisited)
                    {
                        num++;
                        IDIPItem item = this.m_IDIPItemList[i];
                        if (item.RedSpot != null)
                        {
                            IDIPItem item2 = this.m_IDIPItemList[i];
                            item2.RedSpot.CustomSetActive(true);
                        }
                    }
                    else
                    {
                        IDIPItem item3 = this.m_IDIPItemList[i];
                        if (item3.RedSpot != null)
                        {
                            IDIPItem item4 = this.m_IDIPItemList[i];
                            item4.RedSpot.CustomSetActive(false);
                        }
                    }
                }
            }
            this.m_nRedPoint = num;
            Singleton<EventRouter>.instance.BroadCastEvent("IDIPNOTICE_UNREAD_NUM_UPDATE");
        }

        public bool HaveUpdateList
        {
            get
            {
                if (this.m_bHaveUpdateList)
                {
                    return this.m_bHaveUpdateList;
                }
                return (this.m_nRedPoint > 0);
            }
            set
            {
                this.m_bHaveUpdateList = value;
            }
        }

        public bool RedPotState
        {
            get
            {
                return (this.m_IDIPDataList.Count > 0);
            }
        }

        [CompilerGenerated]
        private sealed class <DownloadImage>c__Iterator2A : IDisposable, IEnumerator, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IDIPSys.LoadRCallBack <$>callBack;
            internal int <$>checkDays;
            internal string <$>preUrl;
            internal IDIPSys <>f__this;
            internal bool <bExist>__2;
            internal byte[] <data>__6;
            internal TimeSpan <deltaTime>__4;
            internal Exception <e>__8;
            internal Exception <ex>__5;
            internal string <imageType>__10;
            internal string <key>__0;
            internal bool <localCacheExpires>__3;
            internal string <localCachePath>__1;
            internal Texture2D <tex>__11;
            internal Texture2D <tex>__7;
            internal WWW <www>__9;
            internal IDIPSys.LoadRCallBack callBack;
            internal int checkDays;
            internal string preUrl;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<key>__0 = this.<>f__this.ToMD5(this.preUrl);
                        this.<localCachePath>__1 = CFileManager.GetCachePath(this.<key>__0);
                        this.<bExist>__2 = false;
                        this.<localCacheExpires>__3 = false;
                        this.<bExist>__2 = File.Exists(this.<localCachePath>__1);
                        if (this.<bExist>__2 && (this.checkDays > 0))
                        {
                            try
                            {
                                this.<deltaTime>__4 = (TimeSpan) (DateTime.Now - File.GetLastWriteTime(this.<localCachePath>__1));
                                if ((Application.get_internetReachability() == 2) && (this.<deltaTime>__4.TotalDays >= this.checkDays))
                                {
                                    this.<localCacheExpires>__3 = true;
                                }
                            }
                            catch (Exception exception)
                            {
                                this.<ex>__5 = exception;
                                Debug.Log("downloadImage error " + this.<ex>__5);
                                this.<localCacheExpires>__3 = true;
                            }
                        }
                        if (this.<bExist>__2 && !this.<localCacheExpires>__3)
                        {
                            try
                            {
                                this.<data>__6 = File.ReadAllBytes(this.<localCachePath>__1);
                                this.<tex>__7 = new Texture2D(4, 4, 5, false);
                                if (this.<tex>__7.LoadImage(this.<data>__6) && (this.callBack != null))
                                {
                                    this.callBack(this.<tex>__7);
                                }
                            }
                            catch (Exception exception2)
                            {
                                this.<e>__8 = exception2;
                                object[] inParameters = new object[] { this.<e>__8.Message, this.<e>__8.StackTrace };
                                DebugHelper.Assert(false, "Exception in IDIPSys.DownloadImage, {0}, {1}", inParameters);
                            }
                            goto Label_02D5;
                        }
                        this.<www>__9 = null;
                        this.<www>__9 = new WWW(this.preUrl);
                        this.$current = this.<www>__9;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (!string.IsNullOrEmpty(this.<www>__9.get_error()))
                        {
                            Debug.Log("loadimageerror " + this.<www>__9.get_error());
                            break;
                        }
                        this.<imageType>__10 = null;
                        this.<www>__9.get_responseHeaders().TryGetValue("CONTENT-TYPE", out this.<imageType>__10);
                        if (this.<imageType>__10 != null)
                        {
                            this.<imageType>__10 = this.<imageType>__10.ToLower();
                        }
                        if (string.IsNullOrEmpty(this.<imageType>__10) || !this.<imageType>__10.Contains("image/"))
                        {
                            goto Label_02DC;
                        }
                        this.<tex>__11 = this.<www>__9.get_texture();
                        if ((this.<tex>__11 != null) && (this.<localCachePath>__1 != null))
                        {
                            CFileManager.WriteFile(this.<localCachePath>__1, this.<www>__9.get_bytes());
                        }
                        if (this.callBack != null)
                        {
                            this.callBack(this.<tex>__11);
                        }
                        break;

                    default:
                        goto Label_02DC;
                }
                this.<www>__9.Dispose();
            Label_02D5:
                this.$PC = -1;
            Label_02DC:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <DownloadImageByTag>c__Iterator2B : IDisposable, IEnumerator, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IDIPSys.LoadRCallBack2 <$>callBack;
            internal int <$>checkDays;
            internal int <$>ImageIDx;
            internal string <$>preUrl;
            internal string <$>tagPath;
            internal IDIPSys <>f__this;
            internal bool <bExist>__2;
            internal byte[] <data>__6;
            internal TimeSpan <deltaTime>__4;
            internal Exception <ex>__5;
            internal string <imageType>__9;
            internal string <key>__0;
            internal bool <localCacheExpires>__3;
            internal string <localCachePath>__1;
            internal Texture2D <tex>__10;
            internal Texture2D <tex>__7;
            internal WWW <www>__8;
            internal IDIPSys.LoadRCallBack2 callBack;
            internal int checkDays;
            internal int ImageIDx;
            internal string preUrl;
            internal string tagPath;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<key>__0 = this.<>f__this.ToMD5(this.preUrl);
                        this.<localCachePath>__1 = CFileManager.CombinePath(this.tagPath, this.<key>__0);
                        this.<bExist>__2 = false;
                        this.<localCacheExpires>__3 = false;
                        this.<bExist>__2 = File.Exists(this.<localCachePath>__1);
                        if (this.<bExist>__2 && (this.checkDays > 0))
                        {
                            try
                            {
                                this.<deltaTime>__4 = (TimeSpan) (DateTime.Now - File.GetLastWriteTime(this.<localCachePath>__1));
                                if ((Application.get_internetReachability() == 2) && (this.<deltaTime>__4.TotalDays >= this.checkDays))
                                {
                                    this.<localCacheExpires>__3 = true;
                                }
                            }
                            catch (Exception exception)
                            {
                                this.<ex>__5 = exception;
                                Debug.Log("downloadImage error " + this.<ex>__5);
                                this.<localCacheExpires>__3 = true;
                            }
                        }
                        if (this.<bExist>__2 && !this.<localCacheExpires>__3)
                        {
                            this.<data>__6 = File.ReadAllBytes(this.<localCachePath>__1);
                            this.<tex>__7 = new Texture2D(4, 4, 5, false);
                            if (this.<tex>__7.LoadImage(this.<data>__6) && (this.callBack != null))
                            {
                                this.callBack(this.<tex>__7, this.ImageIDx);
                            }
                            goto Label_02B3;
                        }
                        this.<www>__8 = null;
                        this.<www>__8 = new WWW(this.preUrl);
                        this.$current = this.<www>__8;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (!string.IsNullOrEmpty(this.<www>__8.get_error()))
                        {
                            Debug.Log("DownloadImageByTag " + this.<www>__8.get_error() + " " + this.preUrl);
                            break;
                        }
                        this.<imageType>__9 = null;
                        this.<www>__8.get_responseHeaders().TryGetValue("CONTENT-TYPE", out this.<imageType>__9);
                        if (this.<imageType>__9 != null)
                        {
                            this.<imageType>__9 = this.<imageType>__9.ToLower();
                        }
                        if (string.IsNullOrEmpty(this.<imageType>__9) || !this.<imageType>__9.Contains("image/"))
                        {
                            goto Label_02BA;
                        }
                        this.<tex>__10 = this.<www>__8.get_texture();
                        if ((this.<tex>__10 != null) && (this.<localCachePath>__1 != null))
                        {
                            CFileManager.WriteFile(this.<localCachePath>__1, this.<www>__8.get_bytes());
                        }
                        if (this.callBack != null)
                        {
                            this.callBack(this.<tex>__10, this.ImageIDx);
                        }
                        break;

                    default:
                        goto Label_02BA;
                }
                this.<www>__8.Dispose();
            Label_02B3:
                this.$PC = -1;
            Label_02BA:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetChannelID>c__Iterator29 : IDisposable, IEnumerator, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IDIPSys <>f__this;
            internal FormatException <e>__3;
            internal string <localCachePath>__0;
            internal string <text>__2;
            internal WWW <www>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<localCachePath>__0 = CFileManager.GetStreamingAssetsPathWithHeader("channel.ini");
                        this.<www>__1 = new WWW(this.<localCachePath>__0);
                        this.$current = this.<www>__1;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (!string.IsNullOrEmpty(this.<www>__1.get_error()))
                        {
                            Debug.Log("GetChannelID3 = " + this.<www>__1.get_error());
                            break;
                        }
                        this.<text>__2 = this.<www>__1.get_text();
                        this.<text>__2 = this.<text>__2.Replace("CHANNEL=", string.Empty);
                        try
                        {
                            this.<>f__this.m_ChannelID = Convert.ToInt32(this.<text>__2);
                        }
                        catch (FormatException exception)
                        {
                            this.<e>__3 = exception;
                            this.<>f__this.m_ChannelID = 0;
                            Debug.Log("getchanelid " + this.<e>__3.ToString());
                        }
                        this.<www>__1.Dispose();
                        break;

                    default:
                        goto Label_011A;
                }
                this.$PC = -1;
            Label_011A:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShareActivityTask>c__AnonStorey5C
        {
            internal IDIPSys <>f__this;
            internal string cdnUrl;

            internal void <>m__46(Texture2D text2)
            {
                string cachePath = CFileManager.GetCachePath(this.<>f__this.ToMD5(this.cdnUrl));
                MonoSingleton<ShareSys>.GetInstance().GShare("TimeLine/Qzone", cachePath);
            }
        }

        public enum BTN_DOSOMTHING
        {
            BTN_DOSOMTHING_NONE,
            BTN_DOSOMTHING_URL,
            BTN_DOSOMTHING_GAME
        }

        public class IDIPData
        {
            public bool bLoad;
            public byte bNoticeLabelType;
            public byte bNoticeType;
            public byte bPriority;
            public IDIPSys.BTN_DOSOMTHING btnDoSth;
            public string btnTitle;
            public string btnUrl;
            public bool bVisited;
            public string content;
            public uint dwLogicWorldID;
            public uint dwNoticeID;
            public ulong endTime;
            public ulong startTime;
            public string title;
            public ulong ullNoticeTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IDIPItem
        {
            public GameObject root;
            public Text name;
            public Image icon;
            public Text TypeText;
            public Image flag;
            public Image glow;
            public GameObject RedSpot;
            public IDIPItem(GameObject node)
            {
                this.root = node;
                this.name = Utility.GetComponetInChild<Text>(node, "Name");
                this.icon = Utility.GetComponetInChild<Image>(node, "Icon");
                this.TypeText = Utility.GetComponetInChild<Text>(node, "Flag/Text");
                this.glow = Utility.GetComponetInChild<Image>(node, "Glow");
                this.flag = Utility.GetComponetInChild<Image>(node, "Flag");
                this.RedSpot = node.get_transform().Find("Hotspot").get_gameObject();
            }
        }

        public delegate void LoadRCallBack(Texture2D image);

        public delegate void LoadRCallBack2(Texture2D image, int imageIdx);
    }
}

