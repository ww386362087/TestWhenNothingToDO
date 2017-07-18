﻿namespace Assets.Scripts.GameSystem
{
    using Apollo;
    using Assets.Scripts.Framework;
    using Assets.Scripts.UI;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CGuildHelper
    {
        public const int CrownStarConversionRatio = 0xd8;
        public const string DynamicPrefabCoinIconName = "90001";
        public const string DynamicPrefabDiamondIconName = "90005";
        public static readonly string DynamicPrefabPathCrown = (CUIUtility.s_Sprite_Dynamic_Guild_Dir + "Guild_Icon_crown");
        public static readonly string DynamicPrefabPathMoon = (CUIUtility.s_Sprite_Dynamic_Guild_Dir + "Guild_Icon_moon");
        public static readonly string DynamicPrefabPathStar = (CUIUtility.s_Sprite_Dynamic_Guild_Dir + "Guild_Icon_star");
        public static readonly string DynamicPrefabPathSun = (CUIUtility.s_Sprite_Dynamic_Guild_Dir + "Guild_Icon_sun");
        public const int MoonStarConversionRatio = 6;
        public const int SunStarConversionRatio = 0x24;

        public static RankpointRankInfo CreatePlayerGuildRankpointRankInfo(enGuildRankpointRankListType rankListType)
        {
            RankpointRankInfo info = new RankpointRankInfo();
            info.guildId = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.uulUid;
            info.rankScore = !IsWeekRankListType(rankListType) ? Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.RankInfo.totalRankPoint : Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.RankInfo.weekRankPoint;
            info.guildHeadId = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.dwHeadId;
            info.guildName = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.sName;
            info.guildLevel = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.bLevel;
            info.memberNum = (byte) GetGuildMemberCount();
            info.star = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.star;
            info.rankNo = 0;
            return info;
        }

        public static string GetBindQQGroupSignature()
        {
            ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
            if (accountInfo != null)
            {
                object[] objArray1 = new object[] { accountInfo.OpenId, "_", Singleton<ApolloHelper>.GetInstance().GetAppId(), "_", Singleton<ApolloHelper>.GetInstance().GetAppKey(), "_", GetGroupGuildId(), "_", GetGuildLogicWorldId() };
                string input = string.Concat(objArray1);
                Debug.Log("signature=" + input);
                return Utility.CreateMD5Hash(input);
            }
            return string.Empty;
        }

        public static string GetBuildingName(int buildingType)
        {
            switch (((RES_GUILD_BUILDING_TYPE) buildingType))
            {
                case RES_GUILD_BUILDING_TYPE.RES_GUILD_BUILDING_TYPE_HALL:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Type_Hall");

                case RES_GUILD_BUILDING_TYPE.RES_GUILD_BUILDING_TYPE_BARRACK:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Type_Barrack");

                case RES_GUILD_BUILDING_TYPE.RES_GUILD_BUILDING_TYPE_FACTORY:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Type_Factory");

                case RES_GUILD_BUILDING_TYPE.RES_GUILD_BUILDING_TYPE_STATUE:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Type_Statue");

                case RES_GUILD_BUILDING_TYPE.RES_GUILD_BUILDING_TYPE_SHOP:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Type_Shop");
            }
            return Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Type_Unknown");
        }

        public static ulong GetChairmanUid()
        {
            ListView<GuildMemInfo> listMemInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listMemInfo;
            for (int i = 0; i < listMemInfo.Count; i++)
            {
                if (listMemInfo[i].enPosition == COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_CHAIRMAN)
                {
                    return listMemInfo[i].stBriefInfo.uulUid;
                }
            }
            return 0L;
        }

        public static uint GetCoinProfitPercentage(int guildLevel)
        {
            int index = guildLevel - 1;
            if (((CGuildSystem.s_coinProfitPercentage != null) && (0 <= index)) && (index < CGuildSystem.s_coinProfitPercentage.Length))
            {
                return CGuildSystem.s_coinProfitPercentage[index];
            }
            return 0;
        }

        public static uint GetDonateCostCoin(RES_GUILD_DONATE_TYPE donateType)
        {
            return GameDataMgr.guildDonateDatabin.GetDataByKey((uint) ((byte) donateType)).dwCostGold;
        }

        public static uint GetDonateCostDianQuan(RES_GUILD_DONATE_TYPE donateType)
        {
            return GameDataMgr.guildDonateDatabin.GetDataByKey((uint) ((byte) donateType)).dwCostCoupons;
        }

        public static string GetDonateDescription(RES_GUILD_DONATE_TYPE donateType)
        {
            ResGuildDonate dataByKey = GameDataMgr.guildDonateDatabin.GetDataByKey((uint) ((byte) donateType));
            uint dwCostGold = dataByKey.dwCostGold;
            uint dwCostCoupons = dataByKey.dwCostCoupons;
            uint dwGetConstruct = dataByKey.dwGetConstruct;
            uint dwGetGuildMoney = dataByKey.dwGetGuildMoney;
            uint dwGetCoinPool = dataByKey.dwGetCoinPool;
            string str = (dwCostGold != 0) ? dwCostGold.ToString() : dwCostCoupons.ToString();
            string text = Singleton<CTextManager>.GetInstance().GetText((dwCostGold != 0) ? "Money_Type_GoldCoin" : "Money_Type_DianQuan");
            string[] args = new string[] { str, text, dwGetConstruct.ToString(), dwGetGuildMoney.ToString(), dwGetCoinPool.ToString() };
            return Singleton<CTextManager>.GetInstance().GetText("Guild_Donate_Description", args);
        }

        public static string GetDonateSuccessTip(RES_GUILD_DONATE_TYPE donateType)
        {
            ResGuildDonate dataByKey = GameDataMgr.guildDonateDatabin.GetDataByKey((uint) ((byte) donateType));
            uint dwGetConstruct = dataByKey.dwGetConstruct;
            uint dwGetGuildMoney = dataByKey.dwGetGuildMoney;
            uint dwGetCoinPool = dataByKey.dwGetCoinPool;
            string[] args = new string[] { dwGetConstruct.ToString(), dwGetGuildMoney.ToString(), dwGetCoinPool.ToString() };
            return Singleton<CTextManager>.GetInstance().GetText("Guild_Donate_Success", args);
        }

        public static byte GetFixedGuildGradeLimit(byte originalGradeLimit)
        {
            if (originalGradeLimit < 1)
            {
                return 1;
            }
            return originalGradeLimit;
        }

        public static byte GetFixedGuildLevelLimit(byte originalLevelLimit)
        {
            uint guildMemberMinPvpLevel = GetGuildMemberMinPvpLevel();
            if (originalLevelLimit < guildMemberMinPvpLevel)
            {
                return (byte) guildMemberMinPvpLevel;
            }
            return originalLevelLimit;
        }

        public static int GetFixedPlayerRankGrade(int playerRankGrade)
        {
            return ((playerRankGrade != 0) ? playerRankGrade : 1);
        }

        public static uint GetGradeByRankpointScore(uint rankpointScore)
        {
            ResGuildGradeConf gradeResByRankpointScore = GetGradeResByRankpointScore(rankpointScore);
            return ((gradeResByRankpointScore == null) ? 0 : ((uint) gradeResByRankpointScore.bIndex));
        }

        public static string GetGradeIconPathByGrade(int grade)
        {
            ResGuildGradeConf dataByKey = GameDataMgr.guildGradeDatabin.GetDataByKey((uint) ((byte) grade));
            if (dataByKey != null)
            {
                return (CUIUtility.s_Sprite_Dynamic_Guild_Dir + dataByKey.szIcon);
            }
            return string.Empty;
        }

        public static string GetGradeIconPathByRankpointScore(uint rankpointScore)
        {
            ResGuildGradeConf gradeResByRankpointScore = GetGradeResByRankpointScore(rankpointScore);
            return ((gradeResByRankpointScore == null) ? string.Empty : (CUIUtility.s_Sprite_Dynamic_Guild_Dir + gradeResByRankpointScore.szIcon));
        }

        public static string GetGradeName(uint rankpointScore)
        {
            ResGuildGradeConf gradeResByRankpointScore = GetGradeResByRankpointScore(rankpointScore);
            return ((gradeResByRankpointScore == null) ? string.Empty : StringHelper.BytesToString(gradeResByRankpointScore.szGradeDesc));
        }

        public static string GetGradeNameForOpenGuildHeadImageShopSlot(int slotOffset)
        {
            uint dwGuildHeadImageShopOpenSlotCnt = 0;
            int count = GameDataMgr.guildGradeDatabin.count;
            for (int i = 0; i < count; i++)
            {
                ResGuildGradeConf dataByIndex = GameDataMgr.guildGradeDatabin.GetDataByIndex(i);
                if ((dwGuildHeadImageShopOpenSlotCnt <= slotOffset) && (slotOffset < dataByIndex.dwGuildHeadImageShopOpenSlotCnt))
                {
                    return StringHelper.UTF8BytesToString(ref dataByIndex.szGradeDesc);
                }
                dwGuildHeadImageShopOpenSlotCnt = dataByIndex.dwGuildHeadImageShopOpenSlotCnt;
            }
            object[] inParameters = new object[] { slotOffset };
            DebugHelper.Assert(false, "error slotOffset{0}: check shop and guildGrade res!!!", inParameters);
            return string.Empty;
        }

        private static ResGuildGradeConf GetGradeResByRankpointScore(uint rankpointScore)
        {
            for (int i = 0; i < GameDataMgr.guildGradeDatabin.count; i++)
            {
                ResGuildGradeConf dataByIndex = GameDataMgr.guildGradeDatabin.GetDataByIndex(i);
                if (rankpointScore <= dataByIndex.iScore)
                {
                    return dataByIndex;
                }
            }
            return GameDataMgr.guildGradeDatabin.GetDataByIndex(GameDataMgr.guildGradeDatabin.count - 1);
        }

        public static uint GetGroupGuildId()
        {
            return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.groupGuildId;
        }

        public static uint GetGuildGrade()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return GetGradeByRankpointScore(Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.RankInfo.totalRankPoint);
            }
            return 0;
        }

        public static uint GetGuildHeadImageShopOpenSlotCount()
        {
            uint guildGrade = GetGuildGrade();
            if (guildGrade > 0)
            {
                return GameDataMgr.guildGradeDatabin.GetDataByKey(guildGrade).dwGuildHeadImageShopOpenSlotCnt;
            }
            object[] inParameters = new object[] { guildGrade };
            DebugHelper.Assert(false, "error guild grade: {0}!!!", inParameters);
            return 0;
        }

        public static string GetGuildHeadPath()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return (CUIUtility.s_Sprite_Dynamic_GuildHead_Dir + Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.dwHeadId);
            }
            return string.Empty;
        }

        public static uint GetGuildItemShopOpenSlotCount()
        {
            <GetGuildItemShopOpenSlotCount>c__AnonStorey50 storey = new <GetGuildItemShopOpenSlotCount>c__AnonStorey50();
            storey.guildStarLevel = GetGuildStarLevel();
            if (storey.guildStarLevel == 0)
            {
                object[] inParameters = new object[] { storey.guildStarLevel };
                DebugHelper.Assert(false, "error guildStarLevel: {0}!!!", inParameters);
                return 0;
            }
            ResGuildShopStarIndexConf conf = GameDataMgr.guildStarLevel.FindIf(new Func<ResGuildShopStarIndexConf, bool>(storey, (IntPtr) this.<>m__3A));
            if (conf != null)
            {
                return conf.dwGuildItemShopOpenSlotCnt;
            }
            return 0;
        }

        public static string GetGuildJoinLimitText(int levelLimit, int gradeLimit, uint settingMask)
        {
            string[] args = new string[] { levelLimit.ToString() };
            string text = Singleton<CTextManager>.GetInstance().GetText("Guild_List_Colunm_Limit_Level", args);
            string[] textArray2 = new string[] { GetLadderGradeLimitText(gradeLimit) };
            string str2 = Singleton<CTextManager>.GetInstance().GetText("Guild_List_Colunm_Limit_Ladder_Grade", textArray2);
            string str3 = Singleton<CTextManager>.GetInstance().GetText(!IsGuildNeedApproval(settingMask) ? "Guild_List_Colunm_Limit_No_Need_Apply" : "Guild_List_Colunm_Limit_Need_Apply");
            string[] textArray3 = new string[] { text, "、", str2, "\n", str3 };
            return string.Concat(textArray3);
        }

        public static int GetGuildLevel()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.bLevel;
            }
            return -1;
        }

        public static int GetGuildLogicWorldId()
        {
            return MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID;
        }

        public static uint GetGuildMatchLastWeekRankNo()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchInfo.dwLastRankNo;
            }
            return 0;
        }

        public static int GetGuildMatchLeftCntInCurRound(int curMatchCnt)
        {
            return (((int) GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 0x30).dwConfValue) - curMatchCnt);
        }

        public static uint GetGuildMatchSeasonScore()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchInfo.dwScore;
            }
            return 0;
        }

        public static uint GetGuildMatchWeekScore()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchInfo.dwWeekScore;
            }
            return 0;
        }

        public static int GetGuildMemberCount()
        {
            return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listMemInfo.Count;
        }

        public static GuildMemInfo GetGuildMemberInfoByName(string name)
        {
            return Singleton<CGuildModel>.GetInstance().GetGuildMemberInfoByName(name);
        }

        public static GuildMemInfo GetGuildMemberInfoByUid(ulong uid)
        {
            return Singleton<CGuildModel>.GetInstance().GetGuildMemberInfoByUid(uid);
        }

        public static ListView<GuildMemInfo> GetGuildMemberInfos()
        {
            return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listMemInfo;
        }

        public static uint GetGuildMemberMinPvpLevel()
        {
            return GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 10).dwConfValue;
        }

        public static string GetGuildName()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.sName;
            }
            return string.Empty;
        }

        public static uint GetGuildStarLevel()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.star;
            }
            return 0;
        }

        public static ulong GetGuildUid()
        {
            if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
            {
                return Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.uulUid;
            }
            return 0L;
        }

        public static string GetHeadUrl(string serverUrl)
        {
            return Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(serverUrl);
        }

        public static string GetLadderGradeLimitText(int gradeLimit)
        {
            string text = string.Empty;
            if (gradeLimit > 1)
            {
                return CLadderView.GetRankName((byte) gradeLimit, 0);
            }
            if (gradeLimit == 1)
            {
                text = Singleton<CTextManager>.GetInstance().GetText("Guild_No_Grade_Limit_Tip");
            }
            return text;
        }

        public static int GetMaxGuildMemberCountByLevel(int guildLevel)
        {
            ResGuildLevel dataByKey = GameDataMgr.guildLevelDatabin.GetDataByKey((uint) ((byte) guildLevel));
            if (dataByKey != null)
            {
                return dataByKey.bMaxMemberCnt;
            }
            object[] inParameters = new object[] { guildLevel };
            DebugHelper.Assert(false, "CGuildHelper.GetMaxGuildMemberCountByLevel(): resGuildLevel is null, guildLevel={0}", inParameters);
            return -1;
        }

        public static int GetNobeHeadIconId(ulong playerUid, uint nobeHeadIconIdFromGuild)
        {
            return (!IsSelf(playerUid) ? ((int) nobeHeadIconIdFromGuild) : MonoSingleton<NobeSys>.GetInstance().GetSelfNobeHeadIdx());
        }

        public static int GetNobeLevel(ulong playerUid, uint nobeLevelFromGuild)
        {
            return (!IsSelf(playerUid) ? ((int) nobeLevelFromGuild) : MonoSingleton<NobeSys>.GetInstance().GetSelfNobeLevel());
        }

        public static uint GetPlayerGuildConstruct()
        {
            GuildMemInfo playerGuildMemberInfo = Singleton<CGuildModel>.GetInstance().GetPlayerGuildMemberInfo();
            if (playerGuildMemberInfo != null)
            {
                return playerGuildMemberInfo.dwConstruct;
            }
            DebugHelper.Assert(false, "CGuildHelper.GetPlayerGuildConstruct() playerMemInfo == null!!! Maybe server not send GuildInfo at login time!!!");
            return 0;
        }

        public static GuildMemInfo GetPlayerGuildMemberInfo()
        {
            return Singleton<CGuildModel>.GetInstance().GetPlayerGuildMemberInfo();
        }

        public static RankpointRankInfo GetPlayerGuildRankpointRankInfo(enGuildRankpointRankListType rankListType)
        {
            ListView<RankpointRankInfo> view = Singleton<CGuildModel>.GetInstance().RankpointRankInfoLists[(int) rankListType];
            for (int i = 0; i < view.Count; i++)
            {
                if (Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.uulUid == view[i].guildId)
                {
                    return view[i];
                }
            }
            return CreatePlayerGuildRankpointRankInfo(rankListType);
        }

        public static string GetPositionName(COM_PLAYER_GUILD_STATE position)
        {
            switch (position)
            {
                case COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_CHAIRMAN:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_ChairMan");

                case COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_VICE_CHAIRMAN:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Vice_Chairman_Short");

                case COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_ELDER:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Elder");

                case COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_MEMBER:
                    return Singleton<CTextManager>.GetInstance().GetText("Guild_Normal_Member");
            }
            return string.Empty;
        }

        public static string GetRankpointClearTimeFormatString()
        {
            uint dwConfValue = GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 0x26).dwConfValue;
            uint num2 = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.RankInfo.seasonStartTime + dwConfValue;
            return Utility.ToUtcTime2Local((long) num2).ToString(Singleton<CTextManager>.GetInstance().GetText("Guild_Rankpoint_Clear_Time_Format"));
        }

        public static int GetRankpointMemberListPlayerIndex()
        {
            List<KeyValuePair<ulong, MemberRankInfo>> rankpointMemberInfoList = Singleton<CGuildModel>.GetInstance().RankpointMemberInfoList;
            for (int i = 0; i < rankpointMemberInfoList.Count; i++)
            {
                KeyValuePair<ulong, MemberRankInfo> pair = rankpointMemberInfoList[i];
                if (pair.Key == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
                {
                    return i;
                }
            }
            return -1;
        }

        public static uint GetRankpointSeasonAwardCoin(uint grade)
        {
            ResGuildGradeConf dataByKey = GameDataMgr.guildGradeDatabin.GetDataByKey(grade);
            if (dataByKey != null)
            {
                return dataByKey.dwGold;
            }
            return 0;
        }

        public static uint GetRankpointSeasonAwardDiamond(uint grade)
        {
            ResGuildGradeConf dataByKey = GameDataMgr.guildGradeDatabin.GetDataByKey(grade);
            if (dataByKey != null)
            {
                return dataByKey.dwDiamond;
            }
            return 0;
        }

        public static uint GetRankpointWeekAwardCoin(uint rank)
        {
            <GetRankpointWeekAwardCoin>c__AnonStorey4E storeye = new <GetRankpointWeekAwardCoin>c__AnonStorey4E();
            storeye.rank = rank;
            ResGuildRankReward reward = GameDataMgr.guildRankRewardDatabin.FindIf(new Func<ResGuildRankReward, bool>(storeye, (IntPtr) this.<>m__38));
            return ((reward == null) ? GameDataMgr.guildRankRewardDatabin.GetDataByKey((long) (-1L)).dwGold : reward.dwGold);
        }

        public static uint GetRankpointWeekAwardDiamond(uint rank)
        {
            <GetRankpointWeekAwardDiamond>c__AnonStorey4F storeyf = new <GetRankpointWeekAwardDiamond>c__AnonStorey4F();
            storeyf.rank = rank;
            ResGuildRankReward reward = GameDataMgr.guildRankRewardDatabin.FindIf(new Func<ResGuildRankReward, bool>(storeyf, (IntPtr) this.<>m__39));
            return ((reward == null) ? GameDataMgr.guildRankRewardDatabin.GetDataByKey((long) (-1L)).dwDiamond : reward.dwDiamond);
        }

        public static double GetSelfRecommendTimeout()
        {
            uint dwConfValue = GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 0x11).dwConfValue;
            TimeSpan span = new TimeSpan(0, 0, 0, (int) dwConfValue);
            return span.TotalHours;
        }

        public static byte GetSendGuildMailCnt()
        {
            GuildExtInfo extGuildInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_extGuildInfo;
            if (extGuildInfo != null)
            {
                return extGuildInfo.bSendGuildMailCnt;
            }
            return 0;
        }

        public static uint GetSendGuildMailLimit()
        {
            ResGuildMisc dataByKey = GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 0x2e);
            if (dataByKey != null)
            {
                return dataByKey.dwConfValue;
            }
            return 0;
        }

        public static uint GetStarLevelForOpenGuildItemShopSlot(int slotOffset)
        {
            uint dwGuildItemShopOpenSlotCnt = 0;
            int count = GameDataMgr.guildStarLevel.count;
            for (int i = 0; i < count; i++)
            {
                ResGuildShopStarIndexConf dataByIndex = GameDataMgr.guildStarLevel.GetDataByIndex(i);
                if ((dwGuildItemShopOpenSlotCnt <= slotOffset) && (slotOffset < dataByIndex.dwGuildItemShopOpenSlotCnt))
                {
                    return dataByIndex.dwBeginStar;
                }
                dwGuildItemShopOpenSlotCnt = dataByIndex.dwGuildItemShopOpenSlotCnt;
            }
            object[] inParameters = new object[] { slotOffset };
            DebugHelper.Assert(false, "error slotOffset{0}: check shop and guildStarLevel res!!!", inParameters);
            return 0;
        }

        private static string GetStarLevelTipString(uint starLevel)
        {
            string[] args = new string[] { starLevel.ToString() };
            return Singleton<CTextManager>.GetInstance().GetText("Guild_StarLevel_Current", args);
        }

        public static int GetUpgradeCostDianQuanByLevel(int guildLevel)
        {
            ResGuildLevel dataByKey = GameDataMgr.guildLevelDatabin.GetDataByKey((uint) ((byte) guildLevel));
            if (dataByKey != null)
            {
                return dataByKey.iUpgradeCostCoupons;
            }
            object[] inParameters = new object[] { guildLevel };
            DebugHelper.Assert(false, "CGuildHelper.GetUpgradeCostDianQuanByLevel(): resGuildLevel is null, guildLevel={0}", inParameters);
            return -1;
        }

        public static int GetViceChairmanMaxCount()
        {
            int guildLevel = GetGuildLevel();
            if (guildLevel > 0)
            {
                return GameDataMgr.guildLevelDatabin.GetDataByKey((uint) ((byte) guildLevel)).bViceChairManCnt;
            }
            return -1;
        }

        public static void GetViceChairmanUidAndName(out List<ulong> uids, out List<string> names)
        {
            uids = new List<ulong>();
            names = new List<string>();
            ListView<GuildMemInfo> listMemInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listMemInfo;
            for (int i = 0; i < listMemInfo.Count; i++)
            {
                if (listMemInfo[i].enPosition == COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_VICE_CHAIRMAN)
                {
                    uids.Add(listMemInfo[i].stBriefInfo.uulUid);
                    names.Add(listMemInfo[i].stBriefInfo.sName);
                }
            }
        }

        public static int GuildMemberComparisonForInvite(GuildMemInfo a, GuildMemInfo b)
        {
            if (IsMemberOnline(a) && !IsMemberOnline(b))
            {
                return -1;
            }
            if (!IsMemberOnline(a) && IsMemberOnline(b))
            {
                return 1;
            }
            return ((a.stBriefInfo.uulUid >= b.stBriefInfo.uulUid) ? 1 : -1);
        }

        public static bool IsDonateUseCoin(RES_GUILD_DONATE_TYPE donateType)
        {
            return (GameDataMgr.guildDonateDatabin.GetDataByKey((uint) ((byte) donateType)).dwCostGold != 0);
        }

        public static bool IsFirstGuildListPage(SCPKG_GET_RANKING_LIST_RSP rsp)
        {
            return (rsp.stRankingListDetail.stOfSucc.iStart == 1);
        }

        public static bool IsGuildHighestMatchScore()
        {
            ResGuildMisc dataByKey = GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 50);
            if (dataByKey != null)
            {
                uint guildMatchLastWeekRankNo = GetGuildMatchLastWeekRankNo();
                if (guildMatchLastWeekRankNo != 0)
                {
                    return (guildMatchLastWeekRankNo <= dataByKey.dwConfValue);
                }
            }
            return false;
        }

        public static bool IsGuildMatchLeaderPosition(GuildMemInfo guildMemInfo)
        {
            return ((guildMemInfo != null) && Convert.ToBoolean(guildMemInfo.GuildMatchInfo.bIsLeader));
        }

        public static bool IsGuildMatchLeaderPosition(ulong memberUid)
        {
            return IsGuildMatchLeaderPosition(GetGuildMemberInfoByUid(memberUid));
        }

        public static bool IsGuildMatchReachMatchCntLimit(int curMatchCnt)
        {
            return (GetGuildMatchLeftCntInCurRound(curMatchCnt) <= 0);
        }

        public static bool IsGuildMaxGrade()
        {
            ResGuildGradeConf dataByIndex = GameDataMgr.guildGradeDatabin.GetDataByIndex(GameDataMgr.guildGradeDatabin.count - 1);
            return ((dataByIndex != null) && (GetGuildGrade() == dataByIndex.bIndex));
        }

        public static bool IsGuildMaxLevel(int curLevel)
        {
            return (GetUpgradeCostDianQuanByLevel(curLevel) == -1);
        }

        public static bool IsGuildMemberFull()
        {
            return (GetGuildMemberCount() >= GetMaxGuildMemberCountByLevel(GetGuildLevel()));
        }

        public static bool IsGuildNeedApproval(uint guildSettingMask)
        {
            return Convert.ToBoolean((uint) (guildSettingMask & 1));
        }

        public static bool IsInGuildMatchJoinLimitTime(GuildMemInfo guildMemInfo)
        {
            if (guildMemInfo != null)
            {
                uint dwConfValue = GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 0x31).dwConfValue;
                return ((CRoleInfo.GetCurrentUTCTime() - guildMemInfo.JoinTime) < dwConfValue);
            }
            return false;
        }

        public static bool IsInLastQuitGuildCd()
        {
            if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_extGuildInfo.dwLastQuitGuildTime != 0)
            {
                int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
                uint dwConfValue = GameDataMgr.guildMiscDatabin.GetDataByKey((uint) 7).dwConfValue;
                int seconds = ((int) (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_extGuildInfo.dwLastQuitGuildTime + dwConfValue)) - currentUTCTime;
                TimeSpan span = new TimeSpan(0, 0, 0, seconds);
                if (seconds > 0)
                {
                    string[] args = new string[] { ((int) span.TotalMinutes).ToString(), span.Seconds.ToString() };
                    Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Guild_Cannot_Apply_Tip", args), false, 1.5f, null, new object[0]);
                    return true;
                }
            }
            return false;
        }

        public static bool IsInSameGuild(ulong playerUid)
        {
            return (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild() && (GetGuildMemberInfoByUid(playerUid) != null));
        }

        public static bool IsLastPage(int curPageId, uint totalCnt, int maxCntPerPage)
        {
            int num = ((int) Math.Ceiling((double) (((double) totalCnt) / ((double) maxCntPerPage)))) - 1;
            return (curPageId >= num);
        }

        public static bool IsMemberOnline(GuildMemInfo guildMemInfo)
        {
            return (guildMemInfo.stBriefInfo.dwGameEntity != 0);
        }

        public static bool IsNeedRequestNewRankpoinRank(enGuildRankpointRankListType rankListType)
        {
            return ((CRoleInfo.GetCurrentUTCTime() - Singleton<CGuildModel>.GetInstance().RankpointRankLastGottenTimes[(int) rankListType]) > 300);
        }

        public static bool IsPlayerSigned()
        {
            GuildMemInfo playerGuildMemberInfo = GetPlayerGuildMemberInfo();
            return ((playerGuildMemberInfo != null) && playerGuildMemberInfo.RankInfo.isSigned);
        }

        public static bool IsReachGuildJoinLimit(int playerPvpLevel, int playerRankGrade)
        {
            GuildInfo currentGuildInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo;
            return ((playerPvpLevel >= currentGuildInfo.briefInfo.LevelLimit) && (GetFixedPlayerRankGrade(playerRankGrade) >= currentGuildInfo.briefInfo.GradeLimit));
        }

        public static bool IsSelf(ulong playerUid)
        {
            return (playerUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID);
        }

        public static bool IsSelfInGuildMemberList()
        {
            ulong playerUllUID = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
            ListView<GuildMemInfo> listMemInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listMemInfo;
            if (listMemInfo != null)
            {
                for (int i = 0; i < listMemInfo.Count; i++)
                {
                    if (listMemInfo[i].stBriefInfo.uulUid == playerUllUID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsViceChairmanFull()
        {
            ListView<GuildMemInfo> listMemInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listMemInfo;
            int num = 0;
            for (int i = 0; i < listMemInfo.Count; i++)
            {
                if (listMemInfo[i].enPosition == COM_PLAYER_GUILD_STATE.COM_PLAYER_GUILD_STATE_VICE_CHAIRMAN)
                {
                    num++;
                }
            }
            int viceChairmanMaxCount = GetViceChairmanMaxCount();
            return ((viceChairmanMaxCount > 0) && (num >= viceChairmanMaxCount));
        }

        private static bool IsWeekRankListType(enGuildRankpointRankListType rankListType)
        {
            return ((rankListType == enGuildRankpointRankListType.CurrentWeek) || (rankListType == enGuildRankpointRankListType.LastWeek));
        }

        public static bool IsWeekRankpointRank(enGuildRankpointRankListType rankListType)
        {
            return ((rankListType == enGuildRankpointRankListType.CurrentWeek) || (rankListType == enGuildRankpointRankListType.LastWeek));
        }

        public static void SetHallBuildingLevel(byte hallLevel)
        {
            List<GuildBuildingInfo> listBuildingInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.listBuildingInfo;
            for (int i = 0; i < listBuildingInfo.Count; i++)
            {
                if (listBuildingInfo[i].type == RES_GUILD_BUILDING_TYPE.RES_GUILD_BUILDING_TYPE_HALL)
                {
                    listBuildingInfo[i].level = hallLevel;
                    return;
                }
            }
        }

        public static void SetPlayerSigned(bool isSigned)
        {
            GuildMemInfo playerGuildMemberInfo = GetPlayerGuildMemberInfo();
            if (playerGuildMemberInfo != null)
            {
                playerGuildMemberInfo.RankInfo.isSigned = isSigned;
            }
        }

        public static void SetSendGuildMailCnt(byte sendGuildMailCnt)
        {
            GuildExtInfo extGuildInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_extGuildInfo;
            if (extGuildInfo != null)
            {
                extGuildInfo.bSendGuildMailCnt = sendGuildMailCnt;
            }
        }

        public static void SetStarLevelPanel(uint starLevel, Transform panelTransform, CUIFormScript form)
        {
            if (panelTransform != null)
            {
                int num = (int) (starLevel / 0xd8);
                int num2 = (int) ((starLevel % 0xd8) / 0x24);
                int num3 = (int) (((starLevel % 0xd8) % 0x24) / 6);
                int num4 = (int) (((starLevel % 0xd8) % 0x24) % 6);
                int num5 = panelTransform.get_childCount();
                for (int i = 0; i < num5; i++)
                {
                    Transform child = panelTransform.GetChild(i);
                    if (child == null)
                    {
                        return;
                    }
                    Image component = child.GetComponent<Image>();
                    if (component == null)
                    {
                        return;
                    }
                    child.get_gameObject().CustomSetActive(true);
                    if (i < num)
                    {
                        component.SetSprite(DynamicPrefabPathCrown, form, true, false, false, false);
                    }
                    else if (i < (num + num2))
                    {
                        component.SetSprite(DynamicPrefabPathSun, form, true, false, false, false);
                    }
                    else if (i < ((num + num2) + num3))
                    {
                        component.SetSprite(DynamicPrefabPathMoon, form, true, false, false, false);
                    }
                    else if (i < (((num + num2) + num3) + num4))
                    {
                        component.SetSprite(DynamicPrefabPathStar, form, true, false, false, false);
                    }
                    else
                    {
                        child.get_gameObject().CustomSetActive(false);
                    }
                }
                CUICommonSystem.SetCommonTipsEvent(form, panelTransform.get_gameObject(), GetStarLevelTipString(starLevel), enUseableTipsPos.enTop);
            }
        }

        [CompilerGenerated]
        private sealed class <GetGuildItemShopOpenSlotCount>c__AnonStorey50
        {
            internal uint guildStarLevel;

            internal bool <>m__3A(ResGuildShopStarIndexConf x)
            {
                return ((x.dwBeginStar <= this.guildStarLevel) && (this.guildStarLevel <= x.dwEndStar));
            }
        }

        [CompilerGenerated]
        private sealed class <GetRankpointWeekAwardCoin>c__AnonStorey4E
        {
            internal uint rank;

            internal bool <>m__38(ResGuildRankReward x)
            {
                return ((x.iStartRankNo <= this.rank) && (this.rank <= x.iEndRankNo));
            }
        }

        [CompilerGenerated]
        private sealed class <GetRankpointWeekAwardDiamond>c__AnonStorey4F
        {
            internal uint rank;

            internal bool <>m__39(ResGuildRankReward x)
            {
                return ((x.iStartRankNo <= this.rank) && (this.rank <= x.iEndRankNo));
            }
        }
    }
}

