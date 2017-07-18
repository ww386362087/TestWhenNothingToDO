﻿namespace Assets.Scripts.GameSystem
{
    using Apollo;
    using Assets.Scripts.Common;
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic;
    using Assets.Scripts.UI;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CUICommonSystem : Singleton<CUICommonSystem>
    {
        public static string FORM_COMMON_TIPS = "UGUI/Form/Common/Form_CommonInfo.prefab";
        public static string FORM_ITEM_TIPS = "UGUI/Form/Common/Form_ItemInfo.prefab";
        public static string FORM_MESSAGE_BOX = "UGUI/Form/Common/Form_MessageBox.prefab";
        public static string FORM_SENDING_ALERT = "UGUI/Form/Common/Form_SendMsgAlert.prefab";
        public static string FORM_TEXT_TIPS = "UGUI/Form/Common/Form_Text_Tips.prefab";
        public static string FPS_FORM_PATH = "UGUI/Form/System/FPS/FPSForm.prefab";
        public static string GIFT_BAG_DETAIL_PATH = "UGUI/Form/Common/Form_Gift_Bag_Detail.prefab";
        public static List<NewHeroOrSkinParams> newHeroOrSkinList = new List<NewHeroOrSkinParams>();
        public static string[] s_attNameList = new string[0x25];
        public static uint s_heroId = 0;
        public static ValueDataInfo[] s_heroValArr = new ValueDataInfo[0x25];
        private static bool s_isNewHeroOrSkinShowing = false;
        private static string s_last3DModelPath = null;
        public static string s_manPath = "Prefab_Characters/Prefab_Image/Man/Handsome_1";
        public static string s_newHeroOrSkinPath = "UGUI/Form/Common/Form_NewHeroOrSkin.prefab";
        public static string s_newSymbolFormPath = string.Format("{0}{1}", "UGUI/Form/System/", "Symbol/Form_NewSymbol.prefab");
        public static List<uint> s_pctFuncEftList = new List<uint>();
        public static string s_womanPath = "Prefab_Characters/Prefab_Image/Girl/BeautyGirl_1";

        public static void AddRedDot(GameObject target, enRedDotPos dotPos = 2, int alertNum = 0)
        {
            CUIRedDotSystem.AddRedDot(target, dotPos, alertNum);
        }

        public static void AppendMultipleText(Text target, int multiple)
        {
            if (multiple > 0)
            {
                target.set_color((multiple <= 1) ? Color.get_yellow() : Color.get_green());
            }
        }

        public static void CloseCommonTips()
        {
            Singleton<CUIManager>.GetInstance().CloseForm(FORM_COMMON_TIPS);
        }

        public static void CloseUseableTips()
        {
            Singleton<CUIManager>.GetInstance().CloseForm(FORM_ITEM_TIPS);
        }

        public static void DelRedDot(GameObject target)
        {
            CUIRedDotSystem.DelRedDot(target);
        }

        public static RES_SPECIALFUNCUNLOCK_TYPE EntryTypeToUnlockType(RES_GAME_ENTRANCE_TYPE entranceType)
        {
            RES_SPECIALFUNCUNLOCK_TYPE res_specialfuncunlock_type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_MAX;
            switch (entranceType)
            {
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_RECHARGE:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_COUPONSSHOP:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVE:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_NOTICE:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_NOBE:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_QQVIP:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_TASK_DAY:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVE_ADV:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_NEWPROD:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP_PLAYER:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP_COMPUTER:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP_TRAIN:
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SEVENCHECKIN:
                    return res_specialfuncunlock_type;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_PVPMODE;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_LADDER:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_PVPRANK;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_ANRENA:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_ARENA;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_BURNING:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_LIUGUOYUANZHENG;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_HERO:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_HERO;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SYMBOL:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SYMBOL;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PACKAGE:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_BAG;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_TASK:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_TASK;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_GUILD:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_UNION;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_FRIEND:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_FRIEND;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_HERO:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_SKIN:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_SYMBOL:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_DISCOUNT:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_TREASURE:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_MISTERY:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_BLACKSHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_DIAMOND_TREASURE:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SYMBOL_MAKE:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SYMBOL;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_UNION_BATTLE_ENTRY:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_REWARDMATCH;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_ADDSKILL:
                    return RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_ADDEDSKILL;
            }
            return res_specialfuncunlock_type;
        }

        private static ObjNameData Get3DObjPath(CActorInfo actorInfoRes, bool bLobbyShow)
        {
            if (actorInfoRes == null)
            {
                return new ObjNameData();
            }
            CActorInfo info = Object.Instantiate(actorInfoRes);
            string artPrefabNameLobby = null;
            if (bLobbyShow)
            {
                artPrefabNameLobby = info.GetArtPrefabNameLobby(0);
            }
            if (string.IsNullOrEmpty(artPrefabNameLobby))
            {
                artPrefabNameLobby = info.GetArtPrefabName(0, -1);
            }
            ObjNameData data2 = new ObjNameData();
            data2.ObjectName = artPrefabNameLobby;
            data2.ActorInfo = info;
            return data2;
        }

        public static GameObject GetAnimation3DOjb(string animationName)
        {
            GameObject content = Singleton<CResourceManager>.GetInstance().GetResource(CUIUtility.s_Animation3D_Dir + animationName, typeof(GameObject), enResourceType.UI3DImage, false, false).m_content;
            return (GameObject) Object.Instantiate(content);
        }

        public static string GetCountFormatString(int curCnt, int totalCnt, bool bShowTotalCnt = true)
        {
            if (curCnt < totalCnt)
            {
                if (bShowTotalCnt)
                {
                    return string.Format("<color=#c60e00ff>{0}</color><color=#cecfe1ff>/{1}</color>", curCnt, totalCnt);
                }
                return string.Format("<color=#c60e00ff>{0}</color>", curCnt);
            }
            if (bShowTotalCnt)
            {
                return string.Format("<color=#cecfe1ff>{0}/{1}</color>", curCnt, totalCnt);
            }
            return string.Format("<color=#cecfe1ff>{0}</color>", curCnt);
        }

        public static string GetFuncEftDesc(ref ResDT_FuncEft_Obj[] funcEftArr, bool bValueExpand = false)
        {
            string str = string.Empty;
            if ((funcEftArr == null) || (funcEftArr.Length == 0))
            {
                return string.Empty;
            }
            for (int i = 0; i < funcEftArr.Length; i++)
            {
                if ((funcEftArr[i].iValue != 0) && (funcEftArr[i].wType < s_attNameList.Length))
                {
                    if (funcEftArr[i].bValType == 0)
                    {
                        if (s_pctFuncEftList.IndexOf(funcEftArr[i].wType) != -1)
                        {
                            if (bValueExpand)
                            {
                                str = str + string.Format("{0}<color=#31d840ff>+{1}</color>\n", s_attNameList[funcEftArr[i].wType], GetValuePercent(funcEftArr[i].iValue / 100));
                            }
                            else
                            {
                                str = str + string.Format("{0}<color=#31d840ff>+{1}</color>\n", s_attNameList[funcEftArr[i].wType], GetValuePercent(funcEftArr[i].iValue));
                            }
                        }
                        else if (bValueExpand)
                        {
                            str = str + string.Format("{0}<color=#31d840ff>+{1}</color>\n", s_attNameList[funcEftArr[i].wType], ((float) funcEftArr[i].iValue) / 100f);
                        }
                        else
                        {
                            str = str + string.Format("{0}<color=#31d840ff>+{1}</color>\n", s_attNameList[funcEftArr[i].wType], funcEftArr[i].iValue);
                        }
                    }
                    else if (funcEftArr[i].bValType == 1)
                    {
                        str = str + string.Format("{0}<color=#31d840ff>+{1}</color>\n", s_attNameList[funcEftArr[i].wType], GetValuePercent(funcEftArr[i].iValue));
                    }
                }
            }
            return str;
        }

        public static ObjData GetHero3DObj(uint heroID, bool bLobbyShow)
        {
            ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey(heroID);
            if (dataByKey == null)
            {
                return new ObjData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataByKey.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            if (content == null)
            {
                return new ObjData();
            }
            ObjData data3 = new ObjData();
            data3.Object = InstantiateArtPrefabObj(content, bLobbyShow);
            data3.ActorInfo = content;
            return data3;
        }

        public static ObjNameData GetHero3DObjPath(uint heroID, bool bLobbyShow)
        {
            ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey(heroID);
            if (dataByKey == null)
            {
                return new ObjNameData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataByKey.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            return Get3DObjPath(content, bLobbyShow);
        }

        public static ObjNameData GetHeroPrefabPath(uint heroID, int skidId, bool bLobbyShow)
        {
            ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey(heroID);
            if (dataByKey == null)
            {
                return new ObjNameData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataByKey.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            if (content == null)
            {
                return new ObjNameData();
            }
            CActorInfo info3 = Object.Instantiate(content);
            if (info3 == null)
            {
                return new ObjNameData();
            }
            string artPrefabName = info3.GetArtPrefabName(skidId, -1);
            if (bLobbyShow)
            {
                artPrefabName = info3.GetArtPrefabNameLobby(skidId);
            }
            ObjNameData data4 = new ObjNameData();
            data4.ObjectName = artPrefabName;
            data4.ActorInfo = info3;
            return data4;
        }

        public static stMatchOpenInfo GetMatchOpenState(RES_BATTLE_MAP_TYPE mapType, uint mapId)
        {
            stMatchOpenInfo info;
            info.descStr = string.Empty;
            info.leftDay = 0;
            info.leftSec = 0;
            ResRewardMatchTimeInfo info2 = null;
            GameDataMgr.matchTimeInfoDict.TryGetValue(GameDataMgr.GetDoubleKey((uint) mapType, mapId), out info2);
            uint utilOpenSec = 0;
            int utilOpenDay = 0;
            if (info2 == null)
            {
                info.matchState = enMatchOpenState.enMatchClose;
                return info;
            }
            if (!IsMatchOpened(info2))
            {
                info.matchState = enMatchOpenState.enMatchClose;
            }
            else
            {
                GetTimeUtilOpen(info2, out utilOpenSec, out utilOpenDay);
                if (utilOpenSec == 0)
                {
                    info.matchState = enMatchOpenState.enMatchOpen_InActiveTime;
                }
                else
                {
                    info.matchState = enMatchOpenState.enMatchOpen_NotInActiveTime;
                }
            }
            info.descStr = info2.szTimeTips;
            info.leftSec = utilOpenSec;
            info.leftDay = utilOpenDay;
            return info;
        }

        public static ObjData GetMonster3DObj(int inMonId, bool bLobbyShow)
        {
            ResMonsterCfgInfo dataCfgInfoByCurLevelDiff = MonsterDataHelper.GetDataCfgInfoByCurLevelDiff(inMonId);
            if (dataCfgInfoByCurLevelDiff == null)
            {
                return new ObjData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            if (content == null)
            {
                return new ObjData();
            }
            ObjData data3 = new ObjData();
            data3.Object = InstantiateArtPrefabObj(content, bLobbyShow);
            data3.ActorInfo = content;
            return data3;
        }

        public static ObjNameData GetMonster3DObjPath(int inMonId, bool bLobbyShow)
        {
            ResMonsterCfgInfo dataCfgInfoByCurLevelDiff = MonsterDataHelper.GetDataCfgInfoByCurLevelDiff(inMonId);
            if (dataCfgInfoByCurLevelDiff == null)
            {
                dataCfgInfoByCurLevelDiff = MonsterDataHelper.GetDataCfgInfo(inMonId, 1);
            }
            if (dataCfgInfoByCurLevelDiff == null)
            {
                return new ObjNameData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            return Get3DObjPath(content, bLobbyShow);
        }

        public static ObjData GetOrgan3DObj(int inOrganId, bool bLobbyShow)
        {
            ResOrganCfgInfo dataCfgInfoByCurLevelDiff = OrganDataHelper.GetDataCfgInfoByCurLevelDiff(inOrganId);
            if (dataCfgInfoByCurLevelDiff == null)
            {
                return new ObjData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            if (content == null)
            {
                return new ObjData();
            }
            ObjData data3 = new ObjData();
            data3.Object = InstantiateArtPrefabObj(content, bLobbyShow);
            data3.ActorInfo = content;
            return data3;
        }

        public static ObjNameData GetOrgan3DObjPath(int inOrganId, bool bLobbyShow)
        {
            ResOrganCfgInfo dataCfgInfoByCurLevelDiff = OrganDataHelper.GetDataCfgInfoByCurLevelDiff(inOrganId);
            if (dataCfgInfoByCurLevelDiff == null)
            {
                return new ObjNameData();
            }
            CActorInfo content = Singleton<CResourceManager>.GetInstance().GetResource(StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szCharacterInfo), typeof(CActorInfo), enResourceType.UI3DImage, false, false).m_content as CActorInfo;
            return Get3DObjPath(content, bLobbyShow);
        }

        public static string GetPlatformArea()
        {
            string str = "1";
            if (ApolloConfig.platform == ApolloPlatform.QQ)
            {
                if (Application.get_platform() == 11)
                {
                    return "1";
                }
                if (Application.get_platform() != 8)
                {
                    return str;
                }
                return "2";
            }
            if (ApolloConfig.platform == ApolloPlatform.Wechat)
            {
                if (Application.get_platform() == 11)
                {
                    return "3";
                }
                if (Application.get_platform() == 8)
                {
                    str = "4";
                }
            }
            return str;
        }

        public static string GetRole3DObjPath(int roleID)
        {
            return ((roleID != 1) ? s_manPath : s_womanPath);
        }

        private static int GetSecUtilOpenOfDay(DateTime dateTimeNow, ResActTime[] ActTime)
        {
            ResActTime[] timeArray = ActTime;
            int num = -1;
            if (timeArray != null)
            {
                DateTime time = new DateTime(0x7b2, 1, 1, dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second);
                for (int i = 0; i < timeArray.Length; i++)
                {
                    if ((timeArray[i].dwStartTime == 0) && (timeArray[i].dwEndTime == 0))
                    {
                        return num;
                    }
                    ulong dtVal = (ulong) (timeArray[i].dwStartTime + 0x11eac985ab40L);
                    ulong num4 = (ulong) (timeArray[i].dwEndTime + 0x11eac985ab40L);
                    num4 = (num4 != 0x11eac98954c0L) ? num4 : ((ulong) 0x11eac98944f7L);
                    DateTime time2 = Utility.ULongToDateTime(dtVal);
                    DateTime time3 = Utility.ULongToDateTime(num4);
                    if (DateTime.Compare(time, time2) < 0)
                    {
                        TimeSpan span = (TimeSpan) (time2 - time);
                        return (int) span.TotalSeconds;
                    }
                    if ((DateTime.Compare(time, time2) >= 0) && (DateTime.Compare(time, time3) <= 0))
                    {
                        return 0;
                    }
                }
            }
            return num;
        }

        public static string GetSkillDesc(int skillId, ValueDataInfo[] valueData, int skillLevel, uint heroId)
        {
            ResSkillCfgInfo skillCfgInfo = CSkillData.GetSkillCfgInfo(skillId);
            if (skillCfgInfo == null)
            {
                return string.Empty;
            }
            string skillDesc = StringHelper.UTF8BytesToString(ref skillCfgInfo.szSkillDesc);
            string[] escapeString = CSkillData.GetEscapeString(skillDesc);
            if (escapeString != null)
            {
                for (int i = 0; i < escapeString.Length; i++)
                {
                    int num2 = CSkillData.CalcEscapeValue(escapeString[i], valueData, skillLevel, 1, heroId);
                    string newValue = string.Empty;
                    if (num2 != 0)
                    {
                        newValue = num2.ToString();
                    }
                    skillDesc = skillDesc.Replace("[" + escapeString[i] + "]", newValue);
                }
            }
            return skillDesc;
        }

        public static string GetSkillDesc(int skillId, CHeroInfo heroInfo, int skillLevel, uint heroId)
        {
            ResSkillCfgInfo skillCfgInfo = CSkillData.GetSkillCfgInfo(skillId);
            if (skillCfgInfo == null)
            {
                return string.Empty;
            }
            string skillDesc = StringHelper.UTF8BytesToString(ref skillCfgInfo.szSkillDesc);
            string[] escapeString = CSkillData.GetEscapeString(skillDesc);
            if (escapeString != null)
            {
                for (int i = 0; i < escapeString.Length; i++)
                {
                    int num2 = CSkillData.CalcEscapeValue(escapeString[i], heroInfo, skillLevel, 1, heroId);
                    string newValue = string.Empty;
                    if (num2 != 0)
                    {
                        newValue = num2.ToString();
                    }
                    skillDesc = skillDesc.Replace("[" + escapeString[i] + "]", newValue);
                }
            }
            return skillDesc;
        }

        public static string GetSkillDesc(string skillDesc, ValueDataInfo[] valueData, int skillLevel, int heroSoulLevel, uint heroId)
        {
            string str = skillDesc;
            string[] escapeString = CSkillData.GetEscapeString(str);
            skillLevel = (skillLevel >= 1) ? skillLevel : 1;
            if (escapeString != null)
            {
                for (int i = 0; i < escapeString.Length; i++)
                {
                    str = str.Replace("[" + escapeString[i] + "]", CSkillData.CalcEscapeValue(escapeString[i], valueData, skillLevel, heroSoulLevel, heroId).ToString());
                }
            }
            return str;
        }

        public static string GetSkillDescLobby(string skillDesc, uint heroId)
        {
            SetHeroValueArr(heroId, ref s_heroValArr);
            return GetSkillDesc(skillDesc, s_heroValArr, 1, 1, heroId);
        }

        public static int GetSkillPropertyValue(string skillDesc, uint heroId)
        {
            SetHeroValueArr(heroId, ref s_heroValArr);
            return CSkillData.CalcEscapeValue(skillDesc, s_heroValArr, 1, 1, heroId);
        }

        public static Color GetTextQualityColor(int quality)
        {
            quality = (quality >= 1) ? quality : 1;
            quality = (quality <= 5) ? quality : 5;
            return CUIUtility.s_Text_Color_Hero_Advance[quality - 1];
        }

        public static void GetTimeUtilOpen(ResRewardMatchTimeInfo inMatchTimeInfo, out uint utilOpenSec, out int utilOpenDay)
        {
            uint num = 0;
            int num2 = 0;
            ResRewardMatchTimeInfo info = inMatchTimeInfo;
            DateTime dateTimeNow = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
            if (info != null)
            {
                int secUtilOpenOfDay;
                switch (((RES_CYCLETIME_TYPE) info.bCycleType))
                {
                    case RES_CYCLETIME_TYPE.RES_CYCLETIME_DAY:
                        secUtilOpenOfDay = -1;
                        while (secUtilOpenOfDay < 0)
                        {
                            secUtilOpenOfDay = GetSecUtilOpenOfDay(dateTimeNow, info.astActTime);
                            if (secUtilOpenOfDay < 0)
                            {
                                int newDayDeltaSec = (int) Utility.GetNewDayDeltaSec((int) Utility.ToUtcSeconds(dateTimeNow));
                                dateTimeNow = dateTimeNow.AddSeconds((double) newDayDeltaSec);
                                num += (uint) newDayDeltaSec;
                                num2++;
                            }
                            else
                            {
                                num += (uint) secUtilOpenOfDay;
                            }
                        }
                        break;

                    case RES_CYCLETIME_TYPE.RES_CYCLETIME_WEEK:
                    {
                        int bCycleParmNum = info.bCycleParmNum;
                        secUtilOpenOfDay = -1;
                        while (secUtilOpenOfDay < 0)
                        {
                            for (int i = 0; i < bCycleParmNum; i++)
                            {
                                if (info.CycleParm[i] == ((long) dateTimeNow.DayOfWeek))
                                {
                                    secUtilOpenOfDay = GetSecUtilOpenOfDay(dateTimeNow, info.astActTime);
                                }
                            }
                            if (secUtilOpenOfDay < 0)
                            {
                                int num7 = (int) Utility.GetNewDayDeltaSec((int) Utility.ToUtcSeconds(dateTimeNow));
                                dateTimeNow = dateTimeNow.AddSeconds((double) num7);
                                num += (uint) num7;
                                num2++;
                            }
                            else
                            {
                                num += (uint) secUtilOpenOfDay;
                            }
                        }
                        goto Label_011C;
                    }
                }
            }
        Label_011C:
            utilOpenDay = num2;
            utilOpenSec = num;
        }

        public static uint GetTimeUtilOpen(RES_BATTLE_MAP_TYPE mapType, uint mapId, out uint utilOpenSec, out int utilOpenDay)
        {
            uint num = 0;
            int num2 = 0;
            ResRewardMatchTimeInfo info = null;
            GameDataMgr.matchTimeInfoDict.TryGetValue(GameDataMgr.GetDoubleKey((uint) mapType, mapId), out info);
            GetTimeUtilOpen(info, out num, out num2);
            utilOpenSec = num;
            utilOpenDay = num2;
            return num;
        }

        public static CUIEventScript GetUIEventScript(Transform trans)
        {
            if (trans != null)
            {
                return trans.GetComponent<CUIEventScript>();
            }
            return null;
        }

        public static CUIListScript GetUIListScript(Transform trans)
        {
            if (trans != null)
            {
                return trans.GetComponent<CUIListScript>();
            }
            return null;
        }

        public static GameObject GetUIPrefab(string prefabPath)
        {
            GameObject content = Singleton<CResourceManager>.GetInstance().GetResource(prefabPath, typeof(GameObject), enResourceType.UIPrefab, false, false).m_content;
            return (GameObject) Object.Instantiate(content);
        }

        public static string GetValuePercent(int value)
        {
            if ((value % 100) != 0)
            {
                value = (value / 10) * 10;
            }
            return string.Format("{0}%", ((float) value) / 100f);
        }

        public override void Init()
        {
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Tips_Close, new CUIEventManager.OnUIEventHandler(this.OnTips_Close));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Common_SendMsgAlertOpen, new CUIEventManager.OnUIEventHandler(this.OnSendMsgAlertOpen));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Common_SendMsgAlertClose, new CUIEventManager.OnUIEventHandler(this.OnSendMsgAlertClose));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Tips_ItemInfoOpen, new CUIEventManager.OnUIEventHandler(this.OnTips_ItemInfoOpen));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Tips_ItemInfoClose, new CUIEventManager.OnUIEventHandler(this.OnTips_ItemInfoClose));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Tips_CommonInfoOpen, new CUIEventManager.OnUIEventHandler(this.OnTips_CommonInfoOpen));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Tips_CommonInfoClose, new CUIEventManager.OnUIEventHandler(this.OnTips_CommonInfoClose));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSkin_LoadNewHeroOrSkin3DModel, new CUIEventManager.OnUIEventHandler(CUICommonSystem.OnLoadNewHeroOrSkin3DModel));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.GiftBag_OnShowDetail, new CUIEventManager.OnUIEventHandler(this.OnGiftBag_ShowDetail));
            for (int i = 0; i < s_heroValArr.Length; i++)
            {
                s_heroValArr[i] = new ValueDataInfo((RES_FUNCEFT_TYPE) i, 0, 0, null, 0, 0);
            }
            InitPctFuncEftList();
            InitAttNameList();
            CFileManager.s_delegateOnOperateFileFail = (CFileManager.DelegateOnOperateFileFail) Delegate.Combine(CFileManager.s_delegateOnOperateFileFail, new CFileManager.DelegateOnOperateFileFail(this.OnOperateFileFail));
        }

        private static void InitAttNameList()
        {
            CTextManager instance = Singleton<CTextManager>.GetInstance();
            for (int i = 0; i < 0x25; i++)
            {
                switch (((RES_FUNCEFT_TYPE) i))
                {
                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_PHYATKPT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_PhyAtkPt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_MGCATKPT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MgcAtkPt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_PHYDEFPT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_PhyDefPt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_MGCDEFPT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MgcDefPt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MaxHp");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_CRITRATE:
                        s_attNameList[i] = instance.GetText("Hero_Prop_CritRate");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_PHYARMORHURT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_PhyArmorHurt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_MGCARMORHURT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MgcArmorHurt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_PHYVAMP:
                        s_attNameList[i] = instance.GetText("Hero_Prop_PhyVamp");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_MGCVAMP:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MgcVamp");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_ANTICRIT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_AntiCrit");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_CRITEFT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_CritEft");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_REALHURT:
                        s_attNameList[i] = instance.GetText("Hero_Prop_RealHurt");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_REALHURTLESS:
                        s_attNameList[i] = instance.GetText("Hero_Prop_RealHurtLess");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_MOVESPD:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MoveSpd");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_HPRECOVER:
                        s_attNameList[i] = instance.GetText("Hero_Prop_HpRecover");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_CTRLREDUCE:
                        s_attNameList[i] = instance.GetText("Hero_Prop_CtrlReduce");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_ATKSPDADD:
                        s_attNameList[i] = instance.GetText("Hero_Prop_AtkSpd");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_BASEHURTADD:
                        s_attNameList[i] = instance.GetText("Hero_Prop_BaseHurtAdd");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_CDREDUCE:
                        s_attNameList[i] = instance.GetText("Hero_Prop_CdReduce");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_SightArea:
                        s_attNameList[i] = instance.GetText("Hero_Prop_SightArea");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_HitRate:
                        s_attNameList[i] = instance.GetText("Hero_Prop_HitRate");
                        break;

                    case RES_FUNCEFT_TYPE.RES_FUNCEFT_HitRateAvoid:
                        s_attNameList[i] = instance.GetText("Hero_Prop_HitRateAvoid");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_CRITICAL:
                        s_attNameList[i] = instance.GetText("Hero_Prop_CritLvl");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_REDUCECRITICAL:
                        s_attNameList[i] = instance.GetText("Hero_Prop_ReduceCritLvl");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_PHYSICSHEM:
                        s_attNameList[i] = instance.GetText("Hero_Prop_PhyVampLvl");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_MAGICHEM:
                        s_attNameList[i] = instance.GetText("Hero_Prop_MgcVampLvl");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_ATTACKSPEED:
                        s_attNameList[i] = instance.GetText("Hero_Prop_AtkSpdLvl");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_TENACITY:
                        s_attNameList[i] = instance.GetText("Hero_Prop_CtrlReduceLvl");
                        break;

                    case RES_FUNCEFT_TYPE.RES_PROPERTY_HURTREDUCERATE:
                        s_attNameList[i] = instance.GetText("Hero_Prop_HurtLessRate");
                        break;

                    default:
                        s_attNameList[i] = string.Empty;
                        break;
                }
            }
        }

        public static void InitMenuPanel(GameObject listObj, string[] titleList, int selectIndex, bool isDispatchEvent = true)
        {
            if (listObj != null)
            {
                CUIListScript component = listObj.GetComponent<CUIListScript>();
                component.SetElementAmount(titleList.Length);
                for (int i = 0; i < component.m_elementAmount; i++)
                {
                    component.GetElemenet(i).get_gameObject().get_transform().Find("Text").GetComponent<Text>().set_text(titleList[i]);
                }
                component.SelectElement(-1, false);
                component.SelectElement(selectIndex, isDispatchEvent);
            }
        }

        private static void InitPctFuncEftList()
        {
            s_pctFuncEftList.Clear();
            s_pctFuncEftList.Add(0x12);
            s_pctFuncEftList.Add(6);
            s_pctFuncEftList.Add(12);
            s_pctFuncEftList.Add(9);
            s_pctFuncEftList.Add(10);
            s_pctFuncEftList.Add(15);
            s_pctFuncEftList.Add(20);
        }

        private static GameObject InstantiateArtPrefabObj(CActorInfo actorInfoRes, bool bLobbyShow)
        {
            CActorInfo info = Object.Instantiate(actorInfoRes);
            string artPrefabNameLobby = null;
            if (bLobbyShow)
            {
                artPrefabNameLobby = info.GetArtPrefabNameLobby(0);
            }
            if (string.IsNullOrEmpty(artPrefabNameLobby))
            {
                artPrefabNameLobby = info.GetArtPrefabName(0, -1);
            }
            GameObject obj2 = null;
            if (!string.IsNullOrEmpty(artPrefabNameLobby))
            {
                obj2 = MonoSingleton<SceneMgr>.GetInstance().InstantiateLOD(artPrefabNameLobby, false, SceneObjType.ActionRes);
            }
            return obj2;
        }

        public static bool IsHaveRedDot(GameObject target)
        {
            return CUIRedDotSystem.IsHaveRedDot(target);
        }

        public static bool IsMatchOpened(ResRewardMatchTimeInfo inMatchTimeInfo)
        {
            bool flag = false;
            ResRewardMatchTimeInfo info = inMatchTimeInfo;
            if (info != null)
            {
                ulong ullStartDate = info.ullStartDate;
                ulong ullEndDate = info.ullEndDate;
                DateTime time = Utility.ULongToDateTime(ullStartDate);
                DateTime time2 = Utility.ULongToDateTime(ullEndDate);
                DateTime time3 = Utility.ToUtcTime2Local((long) CRoleInfo.GetCurrentUTCTime());
                if (((DateTime.Compare(time3, time) >= 0) && (DateTime.Compare(time3, time2) <= 0)) && (info.bIsOpen != 0))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static bool IsMatchOpened(RES_BATTLE_MAP_TYPE mapType, uint mapId)
        {
            ResRewardMatchTimeInfo info = null;
            GameDataMgr.matchTimeInfoDict.TryGetValue(GameDataMgr.GetDoubleKey((uint) mapType, mapId), out info);
            return IsMatchOpened(info);
        }

        public static bool JumpForm(RES_GAME_ENTRANCE_TYPE entranceType, int targetId = 0, int targetId2 = 0)
        {
            CUIEvent uiEvent = new CUIEvent();
            CUIFormScript form = null;
            switch (entranceType)
            {
                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_RECHARGE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Pay_OpenBuyDianQuanPanel);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_COUPONSSHOP:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Pay_OpenBuyDianQuanPanel);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Matching_OpenEntry);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_LADDER:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Matching_OpenLadder);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Explore_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_ANRENA:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Arena_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_BURNING:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Burn_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_HERO:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.HeroView_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_NOTICE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Activity_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SYMBOL:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Symbol_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PACKAGE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Bag_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_TASK:
                    uiEvent.m_eventID = enUIEventID.Task_OpenForm;
                    uiEvent.m_eventParams.tag = 0;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_GUILD:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Guild_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_FRIEND:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Friend_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_HERO:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.HeroInfo_GotoMall);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_SKIN:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_GoToSkinTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_SYMBOL:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_GoToSymbolTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_DISCOUNT:
                    uiEvent.m_eventID = enUIEventID.Mall_Open_Factory_Shop_Tab;
                    uiEvent.m_eventParams.tag2 = targetId;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_TREASURE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_GotoCouponsTreasureTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_MISTERY:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_GoToMysteryTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_DIAMOND_TREASURE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_GotoDianmondTreasureTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_NOBE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.NOBE_OPEN_Form);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_QQVIP:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.IDIP_QQVIP_OpenWealForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SYMBOL_MAKE:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Symbol_OpenForm_ToMakeTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_TASK_DAY:
                    uiEvent.m_eventID = enUIEventID.Task_OpenForm;
                    uiEvent.m_eventParams.tag = 1;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVE_ADV:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Adv_OpenChapterForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SHOP_NEWPROD:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mall_GoToRecommendHeroTab);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP_PLAYER:
                    form = Singleton<CUIManager>.GetInstance().GetForm(CMatchingSystem.PATH_MATCHING_ENTRY);
                    if (form == null)
                    {
                        Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Matching_OpenEntry);
                        form = Singleton<CUIManager>.GetInstance().GetForm(CMatchingSystem.PATH_MATCHING_ENTRY);
                    }
                    uiEvent.m_srcFormScript = form;
                    uiEvent.m_eventID = enUIEventID.Matching_BtnGroup_Click;
                    uiEvent.m_eventParams.tag = 1;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP_COMPUTER:
                    form = Singleton<CUIManager>.GetInstance().GetForm(CMatchingSystem.PATH_MATCHING_ENTRY);
                    if (form == null)
                    {
                        Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Matching_OpenEntry);
                        form = Singleton<CUIManager>.GetInstance().GetForm(CMatchingSystem.PATH_MATCHING_ENTRY);
                    }
                    uiEvent.m_srcFormScript = form;
                    uiEvent.m_eventID = enUIEventID.Matching_BtnGroup_Click;
                    uiEvent.m_eventParams.tag = 2;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_PVP_TRAIN:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Matching_GuidePanel);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_UNION_BATTLE_ENTRY:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Union_Battle_ClickEntry);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SEVENCHECKIN:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.SevenCheck_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_ADDSKILL:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.AddedSkill_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_MAIL:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mail_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_MAILSystem:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mail_OpenForm);
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Mail_TabSystem);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_HERO_PREVIEW:
                    uiEvent.m_eventID = enUIEventID.HeroInfo_OpenForm;
                    uiEvent.m_eventParams.openHeroFormPar.heroId = (uint) targetId;
                    uiEvent.m_eventParams.openHeroFormPar.openSrc = enHeroFormOpenSrc.HeroBuyClick;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_SKIN_PREVIEW:
                    uiEvent.m_eventID = enUIEventID.HeroInfo_OpenForm;
                    uiEvent.m_eventParams.openHeroFormPar.heroId = (uint) targetId;
                    uiEvent.m_eventParams.openHeroFormPar.skinId = (uint) targetId2;
                    uiEvent.m_eventParams.openHeroFormPar.openSrc = enHeroFormOpenSrc.SkinBuyClick;
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_14DayCheck:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Day14Check_OpenForm);
                    break;

                case RES_GAME_ENTRANCE_TYPE.RES_GAME_ENTRANCE_DRAW_SYMBOL:
                    Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Lottery_Open_Form);
                    break;

                default:
                    return false;
            }
            return true;
        }

        public static void LoadUIPrefab(string prefabPath, string prefabName, GameObject container, CUIFormScript formScript)
        {
            if (((formScript != null) && (container != null)) && (container.get_transform().Find(prefabName) == null))
            {
                GameObject uIPrefab = GetUIPrefab(prefabPath);
                uIPrefab.set_name(prefabName);
                RectTransform component = uIPrefab.GetComponent<RectTransform>();
                component.SetParent(container.get_transform());
                component.set_localPosition(Vector3.get_zero());
                component.set_localRotation(Quaternion.get_identity());
                component.set_localScale(Vector3.get_one());
                component.set_anchorMin(new Vector2(0f, 0f));
                component.set_anchorMax(new Vector2(1f, 1f));
                component.set_offsetMin(Vector2.get_zero());
                component.set_offsetMax(Vector2.get_zero());
                formScript.InitializeComponent(container);
            }
        }

        private static void NewHeroOrSkinFormClose(CUIEvent uiEventID)
        {
            s_isNewHeroOrSkinShowing = false;
            if (newHeroOrSkinList.Count == 0)
            {
                Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Common_NewHeroOrSkinFormClose, new CUIEventManager.OnUIEventHandler(CUICommonSystem.NewHeroOrSkinFormClose));
            }
            else
            {
                NewHeroOrSkinParams params = newHeroOrSkinList[0];
                Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(params.eventID);
                newHeroOrSkinList.RemoveAt(0);
                ProcessShowNewHeroOrSkin();
            }
        }

        private void OnGiftBag_ShowDetail(CUIEvent uiEvent)
        {
            CUseable iconUseable = uiEvent.m_eventParams.iconUseable;
            if ((iconUseable != null) && (iconUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP))
            {
                CItem item = iconUseable as CItem;
                if ((item != null) && (item.m_itemData.bIsView > 0))
                {
                    uint key = (uint) item.m_itemData.EftParam[0];
                    ResRandomRewardStore dataByKey = GameDataMgr.randomRewardDB.GetDataByKey(key);
                    ListView<CUseable> view = new ListView<CUseable>();
                    for (int i = 0; i < dataByKey.astRewardDetail.Length; i++)
                    {
                        if (dataByKey.astRewardDetail[i].bItemType != 0)
                        {
                            CUseable useable2 = null;
                            if (dataByKey.astRewardDetail[i].dwLowCnt != dataByKey.astRewardDetail[i].dwHighCnt)
                            {
                                useable2 = CUseableManager.CreateUsableByRandowReward((RES_RANDOM_REWARD_TYPE) dataByKey.astRewardDetail[i].bItemType, 1, dataByKey.astRewardDetail[i].dwItemID);
                            }
                            else
                            {
                                useable2 = CUseableManager.CreateUsableByRandowReward((RES_RANDOM_REWARD_TYPE) dataByKey.astRewardDetail[i].bItemType, (int) dataByKey.astRewardDetail[i].dwLowCnt, dataByKey.astRewardDetail[i].dwItemID);
                            }
                            if (useable2 != null)
                            {
                                view.Add(useable2);
                            }
                        }
                    }
                    if (view.Count != 0)
                    {
                        CUIFormScript script = Singleton<CUIManager>.GetInstance().OpenForm(GIFT_BAG_DETAIL_PATH, false, true);
                        if (script != null)
                        {
                            Text componetInChild = Utility.GetComponetInChild<Text>(script.get_gameObject(), "Panel/lblDesc");
                            if (componetInChild != null)
                            {
                                componetInChild.set_text(item.m_description);
                            }
                            CUIListScript script2 = Utility.GetComponetInChild<CUIListScript>(script.get_gameObject(), "Panel/itemGroup");
                            script2.SetElementAmount(view.Count);
                            for (int j = 0; j < view.Count; j++)
                            {
                                CUIListElementScript elemenet = script2.GetElemenet(j);
                                SetItemCell(elemenet.m_belongedFormScript, elemenet.GetWidget(0), view[j], true, false, true, false);
                                if (view[j].m_stackCount == 1)
                                {
                                    Utility.FindChild(elemenet.GetWidget(0), "lblIconCount").CustomSetActive(false);
                                }
                                else
                                {
                                    Utility.FindChild(elemenet.GetWidget(0), "lblIconCount").CustomSetActive(true);
                                }
                                if (view[j] != null)
                                {
                                    CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                                    if (masterRoleInfo != null)
                                    {
                                        if (view[j].m_type == COM_ITEM_TYPE.COM_OBJTYPE_HERO)
                                        {
                                            CHeroItem item2 = view[j] as CHeroItem;
                                            if (masterRoleInfo.IsOwnHero(item2.m_heroData.dwCfgID))
                                            {
                                                Utility.FindChild(elemenet.GetWidget(0), "HaveItemFlag").CustomSetActive(true);
                                            }
                                            else
                                            {
                                                Utility.FindChild(elemenet.GetWidget(0), "HaveItemFlag").CustomSetActive(false);
                                            }
                                        }
                                        else if (view[j].m_type == COM_ITEM_TYPE.COM_OBJTYPE_HEROSKIN)
                                        {
                                            CHeroSkin skin = view[j] as CHeroSkin;
                                            if (masterRoleInfo.IsHaveHeroSkin(skin.m_heroId, skin.m_skinId, false))
                                            {
                                                Utility.FindChild(elemenet.GetWidget(0), "HaveItemFlag").CustomSetActive(true);
                                            }
                                            else
                                            {
                                                Utility.FindChild(elemenet.GetWidget(0), "HaveItemFlag").CustomSetActive(false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void OnLoadNewHeroOrSkin3DModel(CUIEvent uiEvent)
        {
            CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
            if (srcFormScript.get_gameObject().get_activeSelf())
            {
                GameObject widget = srcFormScript.GetWidget(0);
                CUI3DImageScript script2 = (widget == null) ? null : widget.GetComponent<CUI3DImageScript>();
                if ((s_last3DModelPath != null) && (script2 != null))
                {
                    script2.RemoveGameObject(s_last3DModelPath);
                }
                uint heroId = uiEvent.m_eventParams.heroId;
                uint tag = (uint) uiEvent.m_eventParams.tag;
                bool flag = Convert.ToBoolean(uiEvent.m_eventParams.tag2);
                ObjNameData data = GetHeroPrefabPath(heroId, (int) tag, true);
                s_last3DModelPath = data.ObjectName;
                GameObject model = (script2 == null) ? null : script2.AddGameObject(s_last3DModelPath, false, false);
                CHeroAnimaSystem instance = Singleton<CHeroAnimaSystem>.GetInstance();
                instance.Set3DModel(model);
                if (model == null)
                {
                    s_last3DModelPath = null;
                }
                else
                {
                    if (data.ActorInfo != null)
                    {
                        model.get_transform().set_localScale(new Vector3(data.ActorInfo.LobbyScale, data.ActorInfo.LobbyScale, data.ActorInfo.LobbyScale));
                    }
                    if (flag)
                    {
                        instance.InitAnimatList();
                        instance.InitAnimatSoundList(heroId, tag);
                        instance.OnModePlayAnima("Come");
                    }
                }
            }
        }

        private void OnOperateFileFail(string fullPath, enFileOperation fileOperation)
        {
            string message = null;
            Exception e = null;
            switch (fileOperation)
            {
                case enFileOperation.ReadFile:
                    message = "ReadFileFail";
                    e = new ReadFileException(message);
                    break;

                case enFileOperation.WriteFile:
                    message = "WriteFileFail";
                    e = new WriteFileException(message);
                    break;

                case enFileOperation.DeleteFile:
                    message = "DeleteFileFail";
                    e = new DeleteFileException(message);
                    break;

                case enFileOperation.CreateDirectory:
                    message = "CreateDirectoryFail";
                    e = new CreateDirectoryException(message);
                    break;

                case enFileOperation.DeleteDirectory:
                    message = "DeleteDirectoryFail";
                    e = new DeleteDirectoryException(message);
                    break;
            }
            if (!string.IsNullOrEmpty(message))
            {
                Singleton<CUIManager>.GetInstance().OpenTips(message, true, 1.5f, null, new object[0]);
                BuglyAgent.ReportException(e, fullPath);
            }
        }

        private void OnSendMsgAlertClose(CUIEvent uiEvent)
        {
            Singleton<CUIManager>.GetInstance().CloseForm("UGUI/Form/Common/Form_SendMsgAlert.prefab");
        }

        private void OnSendMsgAlertOpen(CUIEvent uiEvent)
        {
            CUIFormScript script = Singleton<CUIManager>.GetInstance().OpenForm(FORM_SENDING_ALERT, false, false);
            if (script != null)
            {
                Transform transform = script.get_transform().Find("Timer");
                if (transform != null)
                {
                    CUITimerScript component = transform.GetComponent<CUITimerScript>();
                    if (component != null)
                    {
                        component.EndTimer();
                    }
                    if (!string.IsNullOrEmpty(uiEvent.m_eventParams.tagStr))
                    {
                        script.get_transform().Find("Panel/Panel").get_gameObject().CustomSetActive(true);
                        script.get_transform().Find("Panel/Image").get_gameObject().CustomSetActive(false);
                        script.get_transform().Find("Panel/Panel/Text").GetComponent<Text>().set_text(uiEvent.m_eventParams.tagStr);
                        if ((uiEvent.m_eventParams.tag != 0) && (component != null))
                        {
                            component.SetTotalTime((float) uiEvent.m_eventParams.tag);
                            if (uiEvent.m_eventParams.tag2 != 0)
                            {
                                component.SetTimerEventId(enTimerEventType.TimeUp, (enUIEventID) uiEvent.m_eventParams.tag2);
                            }
                        }
                    }
                    else if ((uiEvent.m_eventParams.tag != 0) && (component != null))
                    {
                        component.SetTotalTime((float) uiEvent.m_eventParams.tag);
                        if (uiEvent.m_eventParams.tag2 != 0)
                        {
                            component.SetTimerEventId(enTimerEventType.TimeUp, (enUIEventID) uiEvent.m_eventParams.tag2);
                        }
                    }
                    component.StartTimer();
                }
            }
        }

        private void OnTips_Close(CUIEvent uiEvent)
        {
            Singleton<CUIManager>.GetInstance().CloseForm(uiEvent.m_srcFormScript);
        }

        private void OnTips_CommonInfoClose(CUIEvent uiEvent)
        {
            CloseCommonTips();
        }

        private void OnTips_CommonInfoOpen(CUIEvent uiEvent)
        {
            OpenCommonTips(uiEvent.m_pointerEventData.get_pressPosition().x, uiEvent.m_pointerEventData.get_pressPosition().y, uiEvent.m_eventParams.tagStr, (enUseableTipsPos) uiEvent.m_eventParams.tag);
        }

        private void OnTips_ItemInfoClose(CUIEvent uiEvent)
        {
            CloseUseableTips();
        }

        private void OnTips_ItemInfoOpen(CUIEvent uiEvent)
        {
            this.OpenUseableTips(uiEvent.m_eventParams.iconUseable, uiEvent.m_pointerEventData.get_pressPosition().x, uiEvent.m_pointerEventData.get_pressPosition().y, (enUseableTipsPos) uiEvent.m_eventParams.tag);
        }

        public static void OpenApNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_Ap_Not_Enough");
            Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Purchase_OpenBuyActionPoint, enUIEventID.None, false);
        }

        public static void OpenArenaCoinNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_Arena_Coin_Not_Enough");
            Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Shop_GetArenaCoinConfirm, enUIEventID.None, false);
        }

        public static void OpenBurningCoinNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_Burning_Coin_Not_Enough");
            Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Shop_GetBurningCoinConfirm, enUIEventID.None, false);
        }

        public static void OpenCoinNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_GoldCoin_Not_Enough");
            Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Purchase_OpenBuyCoin, enUIEventID.None, false);
        }

        private static void OpenCommonTips(float srcX, float srcY, string strContent, enUseableTipsPos pos = 0)
        {
            CUIFormScript script = Singleton<CUIManager>.GetInstance().OpenForm(FORM_COMMON_TIPS, false, false);
            if (script != null)
            {
                GameObject obj2 = script.get_gameObject().get_transform().Find("Panel").get_gameObject();
                GameObject obj3 = script.get_gameObject().get_transform().Find("Panel/PanelTop").get_gameObject();
                Text component = script.get_gameObject().get_transform().Find("Panel/lblDesc").GetComponent<Text>();
                component.set_text(strContent);
                RectTransform transform = obj2.get_transform();
                float num = ((RectTransform) obj3.get_transform()).get_rect().get_height();
                transform.set_sizeDelta(new Vector2(transform.get_sizeDelta().x, num + component.get_preferredHeight()));
                float num2 = srcX;
                float num3 = srcY;
                float num4 = 60f;
                float num5 = script.ChangeFormValueToScreen(transform.get_rect().get_width());
                float num6 = script.ChangeFormValueToScreen(transform.get_rect().get_height());
                switch (pos)
                {
                    case enUseableTipsPos.enTop:
                        num2 -= num5 / 2f;
                        num3 += num4;
                        break;

                    case enUseableTipsPos.enLeft:
                        num2 -= num5 + num4;
                        num3 -= num6 / 2f;
                        break;

                    case enUseableTipsPos.enRight:
                        num2 += num4;
                        num3 -= num6 / 2f;
                        break;

                    case enUseableTipsPos.enBottom:
                        num2 -= num5 / 2f;
                        num3 -= num6 + num4;
                        break;
                }
                if (num2 < 0f)
                {
                    num2 = 0f;
                }
                else if ((num2 + num5) > Screen.get_width())
                {
                    num2 = Screen.get_width() - num5;
                }
                if (num3 < 0f)
                {
                    num3 = 0f;
                }
                else if ((num3 + num6) > Screen.get_height())
                {
                    num3 = Screen.get_height() - num6;
                }
                transform.set_position(CUIUtility.ScreenToWorldPoint(null, new Vector2(num2, num3), 0f));
            }
        }

        public static void OpenDianQuanNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_DianQuan_Not_Enough");
            Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Pay_OpenBuyDianQuanPanel, enUIEventID.None, false);
        }

        public static void OpenFps()
        {
        }

        public static void OpenGoldCoinNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_GoldCoin_Not_Enough_2");
            Singleton<CUIManager>.GetInstance().OpenMessageBox(text, false);
        }

        public static void OpenGuildCoinNotEnoughTip()
        {
            string text = Singleton<CTextManager>.GetInstance().GetText("Common_Guild_Coin_Not_Enough");
            Singleton<CUIManager>.GetInstance().OpenMessageBox(text, false);
        }

        public static void OpenUrl(string strUrl, bool isOpenInGame = false, int dir = 0)
        {
            if (isOpenInGame)
            {
                IApolloCommonService service = IApollo.Instance.GetService(8) as IApolloCommonService;
                if (service != null)
                {
                    service.OpenUrl(strUrl, (APOLLO_SCREENDIR) dir);
                }
            }
            else
            {
                Application.OpenURL(strUrl);
            }
        }

        public void OpenUseableTips(CUseable iconUseable, float srcX, float srcY, enUseableTipsPos pos = 0)
        {
            CUIFormScript formScript = Singleton<CUIManager>.GetInstance().OpenForm(FORM_ITEM_TIPS, false, false);
            if (formScript != null)
            {
                GameObject obj2 = formScript.get_gameObject().get_transform().Find("Panel").get_gameObject();
                GameObject obj3 = formScript.get_gameObject().get_transform().Find("Panel/PanelTop").get_gameObject();
                GameObject itemCell = formScript.get_gameObject().get_transform().Find("Panel/itemCell").get_gameObject();
                Text component = formScript.get_gameObject().get_transform().Find("Panel/titleContainer/lblName").GetComponent<Text>();
                Text text2 = formScript.get_gameObject().get_transform().Find("Panel/lblHave").GetComponent<Text>();
                Text text3 = formScript.get_gameObject().get_transform().Find("Panel/titleContainer/lblCount").GetComponent<Text>();
                Text text4 = formScript.get_gameObject().get_transform().Find("Panel/panelPrice/lblPrice").GetComponent<Text>();
                Text text5 = formScript.get_gameObject().get_transform().Find("Panel/lblDesc").GetComponent<Text>();
                GameObject obj5 = formScript.get_gameObject().get_transform().Find("Panel/panelPrice").get_gameObject();
                SetItemCell(formScript, itemCell, iconUseable, false, false, false, false);
                component.set_text(iconUseable.m_name);
                text5.set_text(iconUseable.m_description);
                text2.get_gameObject().CustomSetActive(false);
                text3.get_gameObject().CustomSetActive(false);
                obj5.get_gameObject().CustomSetActive(false);
                CUseableContainer useableContainer = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().GetUseableContainer(enCONTAINER_TYPE.ITEM);
                if (iconUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP)
                {
                    CItem item = iconUseable as CItem;
                    int useableStackCount = useableContainer.GetUseableStackCount(COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP, item.m_baseID);
                    if ((item.m_itemData.bClass != 2) && (item.m_itemData.bClass != 3))
                    {
                        text3.set_text("当前拥有 <color=#a52a2aff>" + useableStackCount + "</color>");
                        text3.get_gameObject().CustomSetActive(true);
                    }
                    text4.set_text(item.m_coinSale.ToString());
                    string[] values = new string[] { useableStackCount.ToString() };
                    text5.set_text(CUIUtility.StringReplace(text5.get_text(), values));
                }
                else if (iconUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMEQUIP)
                {
                    int num2 = useableContainer.GetUseableStackCount(COM_ITEM_TYPE.COM_OBJTYPE_ITEMEQUIP, iconUseable.m_baseID);
                    text3.set_text("当前拥有 <color=#a52a2aff>" + num2 + "</color>");
                    text3.get_gameObject().CustomSetActive(true);
                    text4.set_text(iconUseable.m_coinSale.ToString());
                    string[] textArray2 = new string[] { num2.ToString() };
                    text5.set_text(CUIUtility.StringReplace(text5.get_text(), textArray2));
                }
                else if (iconUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMSYMBOL)
                {
                    int num3 = useableContainer.GetUseableStackCount(COM_ITEM_TYPE.COM_OBJTYPE_ITEMSYMBOL, iconUseable.m_baseID);
                    text3.set_text("当前拥有 <color=#a52a2aff>" + num3 + "</color>");
                    text3.get_gameObject().CustomSetActive(true);
                    text4.set_text(iconUseable.m_coinSale.ToString());
                    string[] textArray3 = new string[] { num3.ToString() };
                    text5.set_text(CUIUtility.StringReplace(text5.get_text(), textArray3));
                }
                CVirtualItem item2 = iconUseable as CVirtualItem;
                if ((item2 != null) && (item2.m_virtualType == enVirtualItemType.enExp))
                {
                    CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                    text3.set_text(string.Concat(new object[] { "当前经验", masterRoleInfo.PvpExp, "/", masterRoleInfo.PvpNeedExp }));
                    text3.get_gameObject().CustomSetActive(true);
                }
                RectTransform transform = obj2.get_transform();
                float num4 = ((RectTransform) obj3.get_transform()).get_rect().get_height();
                transform.set_sizeDelta(new Vector2(transform.get_sizeDelta().x, num4 + text5.get_preferredHeight()));
                float num5 = srcX;
                float num6 = srcY;
                float num7 = 60f;
                float num8 = formScript.ChangeFormValueToScreen(transform.get_rect().get_width());
                float num9 = formScript.ChangeFormValueToScreen(transform.get_rect().get_height());
                switch (pos)
                {
                    case enUseableTipsPos.enTop:
                        num5 -= num8 / 2f;
                        num6 += num7;
                        break;

                    case enUseableTipsPos.enLeft:
                        num5 -= num8 + num7;
                        num6 -= num9 / 2f;
                        break;

                    case enUseableTipsPos.enRight:
                        num5 += num7;
                        num6 -= num9 / 2f;
                        break;

                    case enUseableTipsPos.enBottom:
                        num5 -= num8 / 2f;
                        num6 -= num9 + num7;
                        break;
                }
                if (num5 < 0f)
                {
                    num5 = 0f;
                }
                else if ((num5 + num8) > Screen.get_width())
                {
                    num5 = Screen.get_width() - num8;
                }
                if (num6 < 0f)
                {
                    num6 = 0f;
                }
                else if ((num6 + num9) > Screen.get_height())
                {
                    num6 = Screen.get_height() - num9;
                }
                transform.set_position(CUIUtility.ScreenToWorldPoint(null, new Vector2(num5, num6), 0f));
            }
        }

        public static stSkillPropertyPrams[] ParseSkillLevelUpProperty(ref ResDT_SkillDescription[] skillPropertyDesc, uint heroID)
        {
            stSkillPropertyPrams[] pramsArray = new stSkillPropertyPrams[skillPropertyDesc.Length];
            int index = 0;
            for (int i = 0; i < skillPropertyDesc.Length; i++)
            {
                if (skillPropertyDesc[i].szSkillDescType != string.Empty)
                {
                    pramsArray[index].name = skillPropertyDesc[i].szSkillDescType;
                    pramsArray[index].baseValue = ParseValue(skillPropertyDesc[i].szSkillDescBaseValue, heroID);
                    pramsArray[index].growthValue = ParseValue(skillPropertyDesc[i].szSkillDescGrowth, heroID);
                    pramsArray[index].valueType = skillPropertyDesc[i].dwSkillDescValueType;
                    index++;
                }
            }
            return pramsArray;
        }

        private static float ParseValue(string str, uint heroID)
        {
            int num2;
            if (str == string.Empty)
            {
                return 0f;
            }
            if (int.TryParse(str, out num2))
            {
                return (((float) num2) / 100f);
            }
            return GetSkillPropertyValue(str, heroID);
        }

        public static void PlayAnimation(Transform trans, string aniName = new string())
        {
            if (trans != null)
            {
                Animation component = trans.GetComponent<Animation>();
                if (component != null)
                {
                    if (string.IsNullOrEmpty(aniName))
                    {
                        component.Play();
                    }
                    else
                    {
                        component.Play(aniName);
                    }
                }
            }
        }

        public static void PlayAnimation(GameObject target, string aniName, bool forceRewind = false)
        {
            if (target != null)
            {
                CUIAnimationScript component = target.GetComponent<CUIAnimationScript>();
                if (component != null)
                {
                    component.PlayAnimation(aniName, forceRewind);
                }
            }
        }

        public static void PlayAnimator(GameObject target, string stateName)
        {
            CUIAnimatorScript script = (target == null) ? null : target.GetComponent<CUIAnimatorScript>();
            if (script != null)
            {
                script.PlayAnimator(stateName);
            }
        }

        private static void ProcessShowNewHeroOrSkin()
        {
            if (s_isNewHeroOrSkinShowing)
            {
                return;
            }
            if (newHeroOrSkinList.Count == 0)
            {
                Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Common_NewHeroOrSkinFormClose, new CUIEventManager.OnUIEventHandler(CUICommonSystem.NewHeroOrSkinFormClose));
                return;
            }
            s_isNewHeroOrSkinShowing = true;
            NewHeroOrSkinParams params = newHeroOrSkinList[0];
            Singleton<CTopLobbyEntry>.GetInstance().CloseForm();
            CUIFormScript formScript = null;
            if (!string.IsNullOrEmpty(params.prefabPath))
            {
                formScript = Singleton<CUIManager>.GetInstance().OpenForm(params.prefabPath, true, true);
            }
            else
            {
                formScript = Singleton<CUIManager>.GetInstance().OpenForm(s_newHeroOrSkinPath, false, true);
            }
            if (formScript == null)
            {
                return;
            }
            MonoSingleton<ShareSys>.GetInstance().m_bShowTimeline = false;
            DynamicShadow.EnableDynamicShow(formScript.get_gameObject(), true);
            formScript.SetPriority(params.priority);
            Transform transform = formScript.get_transform().Find("btnGroup/Button_Share");
            if (transform != null)
            {
                if (CSysDynamicBlock.bSocialBlocked || MonoSingleton<ShareSys>.GetInstance().m_bHide)
                {
                    transform.get_gameObject().CustomSetActive(false);
                }
                else
                {
                    transform.get_gameObject().CustomSetActive(true);
                }
            }
            GameObject widget = formScript.GetWidget(2);
            GameObject obj3 = formScript.GetWidget(3);
            GameObject obj4 = formScript.GetWidget(6);
            GameObject obj5 = formScript.GetWidget(7);
            GameObject obj6 = formScript.GetWidget(8);
            GameObject obj7 = formScript.GetWidget(1);
            GameObject obj8 = formScript.GetWidget(5);
            switch (params.rewardType)
            {
                case COM_REWARDS_TYPE.COM_REWARDS_TYPE_HERO:
                {
                    if (transform != null)
                    {
                        if ((CSysDynamicBlock.bSocialBlocked || MonoSingleton<ShareSys>.GetInstance().m_bHide) || (params.experienceDays > 0))
                        {
                            transform.get_gameObject().CustomSetActive(false);
                        }
                        else
                        {
                            transform.get_gameObject().CustomSetActive(true);
                        }
                    }
                    if (widget != null)
                    {
                        if (params.experienceDays > 0)
                        {
                            widget.CustomSetActive(false);
                            obj4.CustomSetActive(true);
                            obj6.CustomSetActive(true);
                            string[] args = new string[] { params.experienceDays.ToString() };
                            obj6.GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("ExpCard_NewHeroOrSkin_ExpTime", args));
                        }
                        else
                        {
                            widget.CustomSetActive(true);
                            obj4.CustomSetActive(false);
                            obj6.CustomSetActive(false);
                        }
                    }
                    if (obj3 != null)
                    {
                        obj3.CustomSetActive(false);
                    }
                    if ((params.convertCoin > 0) && (widget != null))
                    {
                        widget.CustomSetActive(false);
                        obj8.CustomSetActive(true);
                        GameObject obj9 = obj8.get_transform().FindChild("ConvertCoinText").get_gameObject();
                        obj9.CustomSetActive(true);
                        Text text = obj9.GetComponent<Text>();
                        if (params.rewardType == COM_REWARDS_TYPE.COM_REWARDS_TYPE_HERO)
                        {
                            text.set_text(string.Format(Singleton<CTextManager>.GetInstance().GetText("HeroConvertItem"), params.convertCoin));
                        }
                    }
                    else
                    {
                        if ((obj4 != null) && !obj4.get_activeSelf())
                        {
                            widget.CustomSetActive(true);
                        }
                        obj8.CustomSetActive(false);
                    }
                    if (obj7 != null)
                    {
                        obj7.CustomSetActive(true);
                        IHeroData data = CHeroDataFactory.CreateHeroData(params.heroId);
                        obj7.GetComponent<Text>().set_text(data.heroName);
                    }
                    MonoSingleton<ShareSys>.instance.m_ShareInfo.heroId = params.heroId;
                    MonoSingleton<ShareSys>.instance.m_ShareInfo.skinId = params.skinId;
                    MonoSingleton<ShareSys>.instance.m_ShareInfo.rewardType = params.rewardType;
                    MonoSingleton<ShareSys>.instance.m_ShareInfo.shareSkinUrl = MonoSingleton<ShareSys>.instance.m_ShareInfo.heroId.ToString();
                    ShareSys.CLoadReq loadReq = new ShareSys.CLoadReq();
                    loadReq.m_Url = MonoSingleton<ShareSys>.instance.m_ShareInfo.shareSkinUrl;
                    loadReq.m_CachePath = MonoSingleton<ShareSys>.instance.m_SharePicCDNCachePath;
                    loadReq.m_LoadError = ShareSys.ELoadError.None;
                    loadReq.m_Type = 1;
                    MonoSingleton<ShareSys>.GetInstance().PreLoadShareSkinImage(loadReq);
                    break;
                }
                case COM_REWARDS_TYPE.COM_REWARDS_TYPE_SKIN:
                    if (widget != null)
                    {
                        widget.CustomSetActive(false);
                    }
                    if (obj3 != null)
                    {
                        if (params.experienceDays > 0)
                        {
                            obj3.CustomSetActive(false);
                            obj5.CustomSetActive(true);
                            obj6.CustomSetActive(true);
                            string[] textArray2 = new string[] { params.experienceDays.ToString() };
                            obj6.GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("ExpCard_NewHeroOrSkin_ExpTime", textArray2));
                        }
                        else
                        {
                            obj3.CustomSetActive(true);
                            obj5.CustomSetActive(false);
                            obj6.CustomSetActive(false);
                        }
                    }
                    if ((params.convertCoin > 0) && (obj3 != null))
                    {
                        obj3.CustomSetActive(false);
                        obj8.CustomSetActive(true);
                        GameObject obj10 = obj8.get_transform().FindChild("ConvertCoinText").get_gameObject();
                        obj10.CustomSetActive(true);
                        Text text3 = obj10.GetComponent<Text>();
                        if (params.rewardType == COM_REWARDS_TYPE.COM_REWARDS_TYPE_SKIN)
                        {
                            text3.set_text(string.Format(Singleton<CTextManager>.GetInstance().GetText("SkinConvertItem"), params.convertCoin));
                        }
                    }
                    else
                    {
                        if ((obj5 != null) && !obj5.get_activeSelf())
                        {
                            obj3.CustomSetActive(true);
                        }
                        obj8.CustomSetActive(false);
                    }
                    if (obj7 != null)
                    {
                        obj7.CustomSetActive(true);
                        ResHeroSkin heroSkin = CSkinInfo.GetHeroSkin(params.heroId, params.skinId);
                        if (heroSkin != null)
                        {
                            Text text4 = obj7.GetComponent<Text>();
                            if (text4 != null)
                            {
                                text4.set_text(StringHelper.UTF8BytesToString(ref heroSkin.szSkinName));
                            }
                            MonoSingleton<ShareSys>.instance.m_ShareInfo.heroId = params.heroId;
                            MonoSingleton<ShareSys>.instance.m_ShareInfo.skinId = params.skinId;
                            MonoSingleton<ShareSys>.instance.m_ShareInfo.rewardType = params.rewardType;
                            MonoSingleton<ShareSys>.instance.m_ShareInfo.beginDownloadTime = Time.get_time();
                            MonoSingleton<ShareSys>.instance.m_ShareInfo.shareSkinUrl = heroSkin.szShareSkinUrl;
                            ResHeroSkinShop shop = null;
                            GameDataMgr.skinShopInfoDict.TryGetValue(heroSkin.dwID, out shop);
                            if (!string.IsNullOrEmpty(MonoSingleton<ShareSys>.instance.m_ShareInfo.shareSkinUrl))
                            {
                                if (transform != null)
                                {
                                    transform.get_gameObject().CustomSetActive(true);
                                }
                                CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                                if (masterRoleInfo != null)
                                {
                                    masterRoleInfo.m_nHeroSkinCount++;
                                }
                                ShareSys.CLoadReq req2 = new ShareSys.CLoadReq();
                                req2.m_Url = MonoSingleton<ShareSys>.instance.m_ShareInfo.shareSkinUrl;
                                req2.m_CachePath = MonoSingleton<ShareSys>.instance.m_SharePicCDNCachePath;
                                req2.m_LoadError = ShareSys.ELoadError.None;
                                req2.m_Type = 2;
                                MonoSingleton<ShareSys>.GetInstance().PreLoadShareSkinImage(req2);
                            }
                            else if (transform != null)
                            {
                                transform.get_gameObject().CustomSetActive(false);
                            }
                        }
                        else if (transform != null)
                        {
                            transform.get_gameObject().CustomSetActive(false);
                        }
                        if ((transform != null) && CSysDynamicBlock.bSocialBlocked)
                        {
                            transform.get_gameObject().CustomSetActive(false);
                        }
                    }
                    goto Label_071F;
            }
        Label_071F:
            formScript.m_eventIDs[1] = enUIEventID.Common_NewHeroOrSkinFormClose;
            GameObject obj11 = formScript.GetWidget(4);
            if (params.interactableTransition && (obj11 != null))
            {
                CUIAnimatorScript script2 = obj11.GetComponent<CUIAnimatorScript>();
                if (script2 == null)
                {
                    script2 = obj11.AddComponent<CUIAnimatorScript>();
                    if (script2 != null)
                    {
                        script2.Initialize(formScript);
                    }
                }
                if (script2 != null)
                {
                    script2.PlayAnimator("Interactable_Enabled");
                }
            }
            CUITimerScript component = formScript.GetComponent<CUITimerScript>();
            if (component != null)
            {
                component.SetTotalTime(0.38f);
                component.m_eventIDs[1] = enUIEventID.HeroSkin_LoadNewHeroOrSkin3DModel;
                stUIEventParams params2 = new stUIEventParams();
                params2.heroId = params.heroId;
                params2.tag = (int) params.skinId;
                params2.tag2 = Convert.ToInt32(params.bInitAnima);
                component.m_eventParams[1] = params2;
                component.StartTimer();
            }
        }

        public static void RefreshExperienceHeroLeftTime(GameObject txtHeroLeftTimeGo, GameObject timerGo, uint heroId)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            txtHeroLeftTimeGo.CustomSetActive(true);
            int experienceHeroLeftTime = masterRoleInfo.GetExperienceHeroLeftTime(heroId);
            string timeSpanFormatString = Utility.GetTimeSpanFormatString(experienceHeroLeftTime);
            if (Utility.IsOverOneDay(experienceHeroLeftTime))
            {
                txtHeroLeftTimeGo.GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("ExpCard_Hero_ExpTime") + timeSpanFormatString);
            }
            else
            {
                timerGo.CustomSetActive(true);
                txtHeroLeftTimeGo.GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("ExpCard_Hero_ExpTime"));
                CUITimerScript component = timerGo.GetComponent<CUITimerScript>();
                component.SetTotalTime((float) experienceHeroLeftTime);
                component.StartTimer();
            }
        }

        public static void RefreshExperienceSkinLeftTime(GameObject txtSkinLeftTimeGo, GameObject timerGo, uint heroId, uint skinId, string tipText = new string())
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo.IsValidExperienceSkin(heroId, skinId))
            {
                txtSkinLeftTimeGo.CustomSetActive(true);
                int experienceSkinLeftTime = masterRoleInfo.GetExperienceSkinLeftTime(heroId, skinId);
                string timeSpanFormatString = Utility.GetTimeSpanFormatString(experienceSkinLeftTime);
                if (string.IsNullOrEmpty(tipText))
                {
                    tipText = Singleton<CTextManager>.GetInstance().GetText("ExpCard_Skin_ExpTime");
                }
                if (Utility.IsOverOneDay(experienceSkinLeftTime))
                {
                    timerGo.CustomSetActive(false);
                    txtSkinLeftTimeGo.GetComponent<Text>().set_text(tipText + timeSpanFormatString);
                }
                else
                {
                    timerGo.CustomSetActive(true);
                    txtSkinLeftTimeGo.GetComponent<Text>().set_text(tipText);
                    CUITimerScript component = timerGo.GetComponent<CUITimerScript>();
                    component.SetTotalTime((float) experienceSkinLeftTime);
                    component.StartTimer();
                }
            }
            else
            {
                txtSkinLeftTimeGo.CustomSetActive(false);
            }
        }

        public static void RefreshSkillLevelUpProperty(GameObject skillPropertyInfoPanel, ref stSkillPropertyPrams[] skillPropertyParams, int skillSlotID)
        {
            if (skillPropertyInfoPanel != null)
            {
                if ((skillSlotID < 1) || (skillSlotID > 3))
                {
                    skillPropertyInfoPanel.CustomSetActive(false);
                }
                else
                {
                    int num;
                    if (skillSlotID == 3)
                    {
                        num = 3;
                    }
                    else
                    {
                        num = 6;
                    }
                    for (int i = 4; i <= 6; i++)
                    {
                        Utility.FindChild(skillPropertyInfoPanel, "Header/level" + i).CustomSetActive(num == 6);
                    }
                    int num3 = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        GameObject p = Utility.FindChild(skillPropertyInfoPanel, "InfoList/PropertyInfo_" + j);
                        if (p == null)
                        {
                            return;
                        }
                        if (skillPropertyParams[j].name != null)
                        {
                            Utility.GetComponetInChild<Text>(p, "PropertyName").set_text(skillPropertyParams[j].name);
                            for (int k = 1; k <= 6; k++)
                            {
                                Text componetInChild = Utility.GetComponetInChild<Text>(p, "PropertyValue/level" + k);
                                if (k <= num)
                                {
                                    string str;
                                    float num6 = skillPropertyParams[j].growthValue * (k - 1);
                                    float num7 = skillPropertyParams[j].baseValue + num6;
                                    int num8 = (int) num7;
                                    if (skillPropertyParams[j].valueType == 1)
                                    {
                                        str = num8.ToString() + "%";
                                    }
                                    else if (num7 > num8)
                                    {
                                        str = num7.ToString("0.0");
                                    }
                                    else
                                    {
                                        str = num8.ToString();
                                    }
                                    componetInChild.set_text(str);
                                }
                                else
                                {
                                    componetInChild.set_text(string.Empty);
                                }
                            }
                            num3++;
                            p.CustomSetActive(true);
                        }
                        else
                        {
                            p.CustomSetActive(false);
                        }
                    }
                    skillPropertyInfoPanel.CustomSetActive(num3 > 0);
                }
            }
        }

        public static void SetBaseValue(ValueDataInfo info, int val)
        {
            info.baseValue = val;
            info.growValue = 0;
            info.addValue = 0;
            info.decValue = 0;
            info.addRatio = 0;
            info.decRatio = 0;
        }

        public static void SetButtonEnable(Button btn, bool isEnable, bool isEventEnable, bool bChangeTextColor = true)
        {
            btn.get_gameObject().GetComponent<CUIEventScript>().set_enabled(isEventEnable);
            btn.set_interactable(isEnable);
            if (bChangeTextColor)
            {
                Text componentInChildren = btn.get_gameObject().GetComponentInChildren<Text>();
                if (componentInChildren != null)
                {
                    componentInChildren.set_color(!isEnable ? CUIUtility.s_Text_Color_Disable : CUIUtility.s_Text_Color_White);
                }
            }
        }

        public static void SetButtonEnableWithShader(Button btn, bool isEnable, bool bChangeTextColor = true)
        {
            btn.get_gameObject().GetComponent<CUIEventScript>().set_enabled(isEnable);
            if (isEnable)
            {
                btn.get_gameObject().CustomSetActive(true);
            }
            Image component = btn.get_gameObject().GetComponent<Image>();
            if (component != null)
            {
                if (isEnable)
                {
                    component.set_color(CUIUtility.s_Color_White);
                }
                else
                {
                    component.set_color(CUIUtility.s_Color_GrayShader);
                }
            }
            if (bChangeTextColor)
            {
                Text componentInChildren = btn.get_gameObject().GetComponentInChildren<Text>();
                if (componentInChildren != null)
                {
                    componentInChildren.set_color(!isEnable ? CUIUtility.s_Text_Color_Disable : CUIUtility.s_Text_Color_White);
                }
            }
        }

        public static void SetButtonName(GameObject btn, string strName)
        {
            Transform transform = btn.get_transform().Find("Text");
            if (transform != null)
            {
                Text component = transform.GetComponent<Text>();
                if (component != null)
                {
                    component.set_text(strName);
                }
            }
        }

        public static void SetCommonTipsEvent(CUIFormScript formScript, GameObject targetObj, string strContent, enUseableTipsPos tipPos = 0)
        {
            CUIEventScript component = targetObj.GetComponent<CUIEventScript>();
            if (component == null)
            {
                component = targetObj.AddComponent<CUIEventScript>();
                component.Initialize(formScript);
            }
            stUIEventParams eventParams = new stUIEventParams();
            eventParams.tag = (int) tipPos;
            eventParams.tagStr = strContent;
            component.SetUIEvent(enUIEventType.Down, enUIEventID.Tips_CommonInfoOpen, eventParams);
            component.SetUIEvent(enUIEventType.Up, enUIEventID.Tips_CommonInfoClose, eventParams);
        }

        public static void SetEquipIcon(ushort equipId, GameObject equipIcon, CUIFormScript formScript)
        {
            if (null != equipIcon)
            {
                ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint) equipId);
                if (dataByKey != null)
                {
                    string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_System_BattleEquip_Dir, dataByKey.szIcon);
                    CUIUtility.SetImageSprite(equipIcon.GetComponent<Image>(), prefabPath, formScript, true, false, false, false);
                }
            }
        }

        public static void SetGetInfoToList(CUIFormScript formScript, CUIListScript list, CUseable itemUseable)
        {
            ResDT_ItemSrc_Info[] astSrcInfo = null;
            ResDT_ItemSrc_Info[] infoArray2 = null;
            if (itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP)
            {
                astSrcInfo = ((CItem) itemUseable).m_itemData.astSrcInfo;
            }
            else if (itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMEQUIP)
            {
                astSrcInfo = ((CEquip) itemUseable).m_equipData.astSrcInfo;
            }
            else if (itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMSYMBOL)
            {
                astSrcInfo = ((CSymbolItem) itemUseable).m_SymbolData.astSrcInfo;
            }
            else
            {
                return;
            }
            if (CSysDynamicBlock.bLobbyEntryBlocked)
            {
                ListView<ResDT_ItemSrc_Info> inList = new ListView<ResDT_ItemSrc_Info>();
                for (int j = 0; j < astSrcInfo.Length; j++)
                {
                    ResDT_ItemSrc_Info info = astSrcInfo[j];
                    inList.Add(((info.bType != 2) && (info.bType != 3)) ? info : null);
                }
                infoArray2 = LinqS.ToArray<ResDT_ItemSrc_Info>(inList);
            }
            else
            {
                infoArray2 = astSrcInfo;
            }
            int amount = 0;
            for (int i = 0; i < infoArray2.Length; i++)
            {
                if ((infoArray2[i] == null) || (infoArray2[i].bType == 0))
                {
                    break;
                }
                amount++;
            }
            if (list != null)
            {
                list.SetElementAmount(amount);
                for (int k = 0; k < amount; k++)
                {
                    GameObject p = list.GetElemenet(k).get_gameObject();
                    GameObject obj3 = Utility.FindChild(p, "Item_Property");
                    GameObject obj4 = Utility.FindChild(p, "Item_Shop");
                    byte bType = infoArray2[k].bType;
                    int dwID = (int) infoArray2[k].dwID;
                    bool flag = false;
                    string str = string.Empty;
                    stUIEventParams eventParams = new stUIEventParams();
                    stItemGetInfoParams params2 = new stItemGetInfoParams();
                    eventParams.itemGetInfoParams = params2;
                    eventParams.itemGetInfoParams.getType = bType;
                    if (bType == 1)
                    {
                        obj3.CustomSetActive(true);
                        obj4.CustomSetActive(false);
                        Text component = Utility.FindChild(p, "Item_Property/Chapter").GetComponent<Text>();
                        Text text2 = Utility.FindChild(p, "Item_Property/Game_Name").GetComponent<Text>();
                        Text text3 = Utility.FindChild(p, "Item_Property/Game_Difficulty").GetComponent<Text>();
                        ResLevelCfgInfo dataByKey = GameDataMgr.levelDatabin.GetDataByKey((long) dwID);
                        object[] inParameters = new object[] { dwID };
                        DebugHelper.Assert(dataByKey != null, "ResLevelCfgInfo[{0}] can not be find!", inParameters);
                        flag = Singleton<CAdventureSys>.GetInstance().IsLevelOpen(dataByKey.iCfgID);
                        eventParams.itemGetInfoParams.levelInfo = dataByKey;
                        eventParams.itemGetInfoParams.isCanDo = flag;
                        if (!flag)
                        {
                            eventParams.itemGetInfoParams.errorStr = Singleton<CTextManager>.instance.GetText("Hero_Level_Not_Open");
                        }
                        string[] args = new string[] { dataByKey.iChapterId.ToString() };
                        component.set_text(Singleton<CTextManager>.instance.GetText("PVE_Level_Chapter_Number", args));
                        text2.set_text(Utility.UTF8Convert(dataByKey.szName));
                        text3.get_gameObject().CustomSetActive(false);
                        uint num7 = !flag ? 0 : Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().pveLevelDetail[dataByKey.bLevelDifficulty - 1].ChapterDetailList[dataByKey.iChapterId - 1].LevelDetailList[dataByKey.bLevelNo - 1].PlayNum;
                        uint dwChallengeNum = dataByKey.dwChallengeNum;
                        if (dataByKey.bLevelDifficulty == 2)
                        {
                            string[] textArray2 = new string[] { (dwChallengeNum - num7).ToString(), dwChallengeNum.ToString() };
                            text3.set_text(Singleton<CTextManager>.instance.GetText("Hero_Info_Text4", textArray2));
                            text3.get_gameObject().CustomSetActive(true);
                        }
                    }
                    else if (bType == 2)
                    {
                        RES_SHOP_TYPE res_shop_type = (RES_SHOP_TYPE) dwID;
                        RES_SPECIALFUNCUNLOCK_TYPE type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_NONE;
                        obj3.CustomSetActive(false);
                        obj4.CustomSetActive(true);
                        obj4.GetComponentInChildren<Text>().set_text(Singleton<CTextManager>.instance.GetText(string.Format("Shop_Type_{0}", dwID)));
                        switch (res_shop_type)
                        {
                            case RES_SHOP_TYPE.RES_SHOPTYPE_ARENA:
                                type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_ARENA;
                                break;

                            case RES_SHOP_TYPE.RES_SHOPTYPE_BURNING:
                                type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_LIUGUOYUANZHENG;
                                break;

                            case RES_SHOP_TYPE.RES_SHOPTYPE_PVPSYMBOL:
                                type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SYMBOLSHOP;
                                break;

                            case RES_SHOP_TYPE.RES_SHOPTYPE_GUILD:
                                type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_GUILDSHOP;
                                break;

                            case RES_SHOP_TYPE.RES_SHOPTYPE_FIXED:
                                type = RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_SHOP;
                                break;
                        }
                        if ((type == RES_SPECIALFUNCUNLOCK_TYPE.RES_SPECIALFUNCUNLOCKTYPE_NONE) || Singleton<CFunctionUnlockSys>.instance.FucIsUnlock(type))
                        {
                            flag = true;
                        }
                        else
                        {
                            str = Utility.UTF8Convert(GameDataMgr.specialFunUnlockDatabin.GetDataByKey((uint) type).szLockedTip);
                        }
                        eventParams.itemGetInfoParams.isCanDo = flag;
                        eventParams.itemGetInfoParams.errorStr = str;
                        eventParams.tag = dwID;
                    }
                    else if (bType == 3)
                    {
                        flag = true;
                        eventParams.itemGetInfoParams.isCanDo = flag;
                        eventParams.itemGetInfoParams.errorStr = string.Empty;
                        obj3.CustomSetActive(false);
                        obj4.CustomSetActive(true);
                        obj4.GetComponentInChildren<Text>().set_text(Singleton<CTextManager>.instance.GetText("Mall"));
                    }
                    p.GetComponent<CUIEventScript>().SetUIEvent(enUIEventType.Click, enUIEventID.Tips_ItemSourceElementClick, eventParams);
                }
                list.get_gameObject().CustomSetActive(true);
            }
        }

        public static void SetHeadItemCell(GameObject headItemCell, string headUrl, int vipLevel, int headBkId)
        {
            if (!CSysDynamicBlock.bSocialBlocked && (headItemCell != null))
            {
                Transform transform = headItemCell.get_transform();
                Transform transform2 = transform.FindChild("HeadIcon");
                Transform transform3 = transform.FindChild("HeadFrame");
                Transform transform4 = transform.FindChild("NobeIcon");
                if (transform2 != null)
                {
                    transform2.GetComponent<CUIHttpImageScript>().SetImageUrl(headUrl);
                }
                if (transform3 != null)
                {
                    MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(transform3.GetComponent<Image>(), headBkId);
                }
                if (transform4 != null)
                {
                    MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(transform4.GetComponent<Image>(), vipLevel, false);
                }
            }
        }

        public static void SetHeroItemData(CUIFormScript formScript, GameObject item, IHeroData data, enHeroHeadType headType = 0, bool bIconGray = false, bool isShowSpecMatrial = true)
        {
            if ((item != null) && (data != null))
            {
                Transform transform = item.get_transform();
                if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo() != null)
                {
                    SetHeroItemImage(formScript, item, CSkinInfo.GetHeroSkinPic(data.cfgID, 0), headType, bIconGray, isShowSpecMatrial);
                }
                Transform transform2 = transform.Find("profession");
                Transform transform3 = transform.Find("heroNameText");
                if (transform2 != null)
                {
                    SetHeroJob(formScript, transform2.get_gameObject(), (enHeroJobType) data.heroType);
                }
                if (transform3 != null)
                {
                    transform3.GetComponent<Text>().set_text(data.heroName);
                }
            }
        }

        public static void SetHeroItemImage(CUIFormScript formScript, GameObject item, string imgPath, enHeroHeadType headType = 1, bool bGray = false, bool isShowSpecMatrial = false)
        {
            if (null != item)
            {
                if (bGray)
                {
                    isShowSpecMatrial = false;
                }
                Transform transform = item.get_transform().Find("imageIcon");
                if (transform != null)
                {
                    Image component = transform.GetComponent<Image>();
                    if (component != null)
                    {
                        component.set_color(!bGray ? CUIUtility.s_Color_White : CUIUtility.s_Color_GrayShader);
                        string str = CUIUtility.s_Sprite_Dynamic_Icon_Dir;
                        if (headType == enHeroHeadType.enBust)
                        {
                            str = CUIUtility.s_Sprite_Dynamic_BustHero_Dir;
                        }
                        else if (headType == enHeroHeadType.enBustCircle)
                        {
                            str = CUIUtility.s_Sprite_Dynamic_BustCircle_Dir;
                        }
                        if (component != null)
                        {
                            if (headType == enHeroHeadType.enBust)
                            {
                                if (bGray)
                                {
                                    CUIUtility.SetImageGrayMatrial(component);
                                }
                                component.SetSprite(str + imgPath, formScript, false, true, true, isShowSpecMatrial);
                            }
                            else
                            {
                                component.SetSprite(str + imgPath, formScript, false, true, true, false);
                            }
                        }
                    }
                }
            }
        }

        public static void SetHeroJob(CUIFormScript formScript, GameObject root, enHeroJobType jobType)
        {
            root.GetComponent<Image>().SetSprite(CUIUtility.s_Sprite_Dynamic_Profession_Dir + jobType.ToString(), formScript, true, false, false, false);
        }

        public static void SetHeroProficiencyBgImage(CUIFormScript formScript, GameObject proficiencyBg, int proficiencyLV, bool bLoading)
        {
            if ((null != formScript) && (null != proficiencyBg))
            {
                int num = 1;
                int maxProficiency = CHeroInfo.GetMaxProficiency();
                proficiencyLV = Math.Max(num, Math.Min(maxProficiency, proficiencyLV));
                Image component = proficiencyBg.GetComponent<Image>();
                if (component != null)
                {
                    string str = string.Empty;
                    if (proficiencyLV != maxProficiency)
                    {
                        str = bLoading ? string.Format("{0}{1}", "HeroProficiency_Bg_Level_1", "_loading") : "HeroProficiency_Bg_Level_1";
                        component.SetSprite(string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Proficiency_Dir, str), formScript, true, false, false, false);
                    }
                    else
                    {
                        str = bLoading ? string.Format("{0}{1}", "HeroProficiency_Bg_Level_Max", "_loading") : "HeroProficiency_Bg_Level_Max";
                        component.SetSprite(string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Proficiency_Dir, str), formScript, true, false, false, false);
                    }
                }
            }
        }

        public static void SetHeroProficiencyIconImage(CUIFormScript formScript, GameObject proficiencyIcon, int proficiencyLV)
        {
            if ((null != formScript) && (null != proficiencyIcon))
            {
                int num = 1;
                int maxProficiency = CHeroInfo.GetMaxProficiency();
                proficiencyLV = Math.Max(num, Math.Min(maxProficiency, proficiencyLV));
                Image component = proficiencyIcon.GetComponent<Image>();
                if (component != null)
                {
                    component.SetSprite(string.Format("{0}{1}{2}", CUIUtility.s_Sprite_Dynamic_Proficiency_Dir, "HeroProficiency_Level_", proficiencyLV), formScript, true, false, false, false);
                }
            }
        }

        public static void SetHeroSkinLabelPic(CUIFormScript formScript, GameObject labelObj, uint heroId, uint skinId)
        {
            if (null != labelObj)
            {
                ResHeroSkinShop shop = null;
                GameDataMgr.skinShopInfoDict.TryGetValue(CSkinInfo.GetSkinCfgId(heroId, skinId), out shop);
                if (shop == null)
                {
                    labelObj.CustomSetActive(false);
                }
                else
                {
                    RectTransform transform = labelObj.get_transform() as RectTransform;
                    string szLimitLabelPic = string.Empty;
                    if (shop.bIsLimitSkin > 0)
                    {
                        if (!string.IsNullOrEmpty(shop.szLimitLabelPic))
                        {
                            szLimitLabelPic = shop.szLimitLabelPic;
                        }
                        transform.set_sizeDelta(new Vector2(120f, 38f));
                    }
                    else
                    {
                        ResSkinQualityPicInfo dataByKey = GameDataMgr.skinQualityPicDatabin.GetDataByKey((uint) shop.bSkinQuality);
                        if ((dataByKey != null) && !string.IsNullOrEmpty(dataByKey.szLabelPicPath))
                        {
                            szLimitLabelPic = dataByKey.szLabelPicPath;
                        }
                        transform.set_sizeDelta(new Vector2(95f, 46f));
                    }
                    if (string.IsNullOrEmpty(szLimitLabelPic))
                    {
                        labelObj.CustomSetActive(false);
                    }
                    else
                    {
                        labelObj.CustomSetActive(true);
                        szLimitLabelPic = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_SkinQuality_Dir, szLimitLabelPic);
                        CUIUtility.SetImageSprite(labelObj.GetComponent<Image>(), szLimitLabelPic, formScript, true, false, false, false);
                    }
                }
            }
        }

        public static void SetHeroSkinQualityPic(CUIFormScript formScript, GameObject qualityObj, uint heroId, uint skinId)
        {
            if (null != qualityObj)
            {
                ResHeroSkinShop shop = null;
                GameDataMgr.skinShopInfoDict.TryGetValue(heroId, out shop);
                if (shop != null)
                {
                    if (shop.bIsLimitSkin > 0)
                    {
                        string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_SkinQuality_Dir, StringHelper.UTF8BytesToString(ref shop.szLimitQualityPic));
                        CUIUtility.SetImageSprite(qualityObj.GetComponent<Image>(), prefabPath, formScript, true, false, false, false);
                    }
                    else
                    {
                        ResSkinQualityPicInfo dataByKey = GameDataMgr.skinQualityPicDatabin.GetDataByKey((uint) shop.bSkinQuality);
                        if (dataByKey != null)
                        {
                            string str2 = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_SkinQuality_Dir, StringHelper.UTF8BytesToString(ref dataByKey.szQualityPicPath));
                            CUIUtility.SetImageSprite(qualityObj.GetComponent<Image>(), str2, formScript, true, false, false, false);
                        }
                    }
                }
            }
        }

        private static void SetHeroValueArr(uint heroId, ref ValueDataInfo[] dataArr)
        {
            if (s_heroId != heroId)
            {
                s_heroId = heroId;
                ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey(heroId);
                if (dataByKey != null)
                {
                    SetBaseValue(dataArr[1], (int) dataByKey.iBaseATT);
                    SetBaseValue(dataArr[2], (int) dataByKey.iBaseINT);
                    SetBaseValue(dataArr[3], (int) dataByKey.iBaseDEF);
                    SetBaseValue(dataArr[4], (int) dataByKey.iBaseRES);
                    SetBaseValue(dataArr[5], (int) dataByKey.iBaseHP);
                    SetBaseValue(dataArr[6], (int) dataByKey.iCritRate);
                    SetBaseValue(dataArr[7], 0);
                    SetBaseValue(dataArr[8], 0);
                    SetBaseValue(dataArr[9], 0);
                    SetBaseValue(dataArr[10], 0);
                    SetBaseValue(dataArr[11], 0);
                    SetBaseValue(dataArr[12], (int) dataByKey.iCritEft);
                    SetBaseValue(dataArr[13], 0);
                    SetBaseValue(dataArr[14], 0);
                    SetBaseValue(dataArr[15], (int) dataByKey.iBaseSpeed);
                    SetBaseValue(dataArr[0x10], (int) dataByKey.iBaseHPAdd);
                    SetBaseValue(dataArr[0x11], 0);
                    SetBaseValue(dataArr[0x12], (int) dataByKey.iBaseAtkSpd);
                    SetBaseValue(dataArr[0x13], 0);
                    SetBaseValue(dataArr[20], 0);
                    SetBaseValue(dataArr[0x15], dataByKey.iSightR);
                    SetBaseValue(dataArr[0x16], 0);
                    SetBaseValue(dataArr[0x17], 0);
                }
            }
        }

        public static void SetHostHeadItemCell(GameObject headItemCell)
        {
            if (!CSysDynamicBlock.bSocialBlocked && (headItemCell != null))
            {
                Transform transform = headItemCell.get_transform();
                Transform transform2 = transform.FindChild("HeadIcon");
                Transform transform3 = transform.FindChild("HeadFrame");
                Transform transform4 = transform.FindChild("NobeIcon");
                CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                if (masterRoleInfo != null)
                {
                    if (transform2 != null)
                    {
                        transform2.GetComponent<CUIHttpImageScript>().SetImageUrl(masterRoleInfo.HeadUrl);
                    }
                    if (transform3 != null)
                    {
                        MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(transform3.GetComponent<Image>(), (int) masterRoleInfo.GetNobeInfo().stGameVipClient.dwHeadIconId);
                    }
                    if (transform4 != null)
                    {
                        MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(transform4.GetComponent<Image>(), (int) masterRoleInfo.GetNobeInfo().stGameVipClient.dwCurLevel, false);
                    }
                }
            }
        }

        public static void SetItemCell(CUIFormScript formScript, GameObject itemCell, CUseable itemUseable, bool isHaveClickEvent = true, bool displayAll = false, bool cntForceDisplay = false, bool isClickToShowDetail = false)
        {
            if ((itemUseable != null) && (itemCell != null))
            {
                CUIUtility.SetImageSprite(itemCell.get_transform().Find("imgIcon").GetComponent<Image>(), itemUseable.GetIconPath(), formScript, false, false, false, false);
                string prefabPath = string.Format("{0}Common_slotBg{1}", "UGUI/Sprite/Common/", itemUseable.m_grade + 1);
                CUIUtility.SetImageSprite(itemCell.GetComponent<Image>(), prefabPath, formScript, true, false, false, false);
                Transform transform = itemCell.get_transform().Find("lblIconCount");
                if (transform != null)
                {
                    if (cntForceDisplay)
                    {
                        transform.get_gameObject().CustomSetActive(true);
                    }
                    Text component = transform.GetComponent<Text>();
                    if ((itemUseable.m_stackCount < 0x2710) || displayAll)
                    {
                        component.set_text(itemUseable.m_stackCount.ToString());
                    }
                    else
                    {
                        component.set_text((itemUseable.m_stackCount / 0x2710) + "万");
                    }
                    AppendMultipleText(component, itemUseable.m_stackMulti);
                    if (itemUseable.m_stackCount <= 0)
                    {
                        component.get_gameObject().CustomSetActive(false);
                    }
                    if (itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMSYMBOL)
                    {
                        if (((CSymbolItem) itemUseable).IsGuildSymbol())
                        {
                            component.set_text(string.Empty);
                        }
                        else
                        {
                            component.set_text(itemUseable.GetSalableCount().ToString());
                        }
                    }
                }
                Transform transform2 = itemCell.get_transform().Find("imgSuipian");
                if (transform2 != null)
                {
                    Image image2 = transform2.GetComponent<Image>();
                    image2.get_gameObject().CustomSetActive(false);
                    if (itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP)
                    {
                        CItem item = itemUseable as CItem;
                        if ((item.m_itemData.bClass == 2) || (item.m_itemData.bClass == 3))
                        {
                            image2.get_gameObject().CustomSetActive(true);
                        }
                    }
                }
                Transform transform3 = itemCell.get_transform().Find("ItemName");
                if (transform3 != null)
                {
                    Text text2 = transform3.get_gameObject().GetComponent<Text>();
                    if (text2 != null)
                    {
                        text2.set_text(itemUseable.m_name);
                    }
                }
                Transform transform4 = itemCell.get_transform().Find("imgExperienceMark");
                if (transform4 != null)
                {
                    if ((itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP) && CItem.IsHeroExperienceCard(itemUseable.m_baseID))
                    {
                        transform4.get_gameObject().CustomSetActive(true);
                        transform4.GetComponent<Image>().SetSprite(CUIUtility.GetSpritePrefeb(CExperienceCardSystem.HeroExperienceCardMarkPath, false, false), false);
                    }
                    else if ((itemUseable.m_type == COM_ITEM_TYPE.COM_OBJTYPE_ITEMPROP) && CItem.IsSkinExperienceCard(itemUseable.m_baseID))
                    {
                        transform4.get_gameObject().CustomSetActive(true);
                        transform4.GetComponent<Image>().SetSprite(CUIUtility.GetSpritePrefeb(CExperienceCardSystem.SkinExperienceCardMarkPath, false, false), false);
                    }
                    else
                    {
                        transform4.get_gameObject().CustomSetActive(false);
                    }
                }
                if (isHaveClickEvent)
                {
                    CUIEventScript script = itemCell.GetComponent<CUIEventScript>();
                    if (script == null)
                    {
                        script = itemCell.AddComponent<CUIEventScript>();
                        script.Initialize(formScript);
                    }
                    stUIEventParams eventParams = new stUIEventParams();
                    eventParams.iconUseable = itemUseable;
                    script.SetUIEvent(enUIEventType.Down, enUIEventID.Tips_ItemInfoOpen, eventParams);
                    script.SetUIEvent(enUIEventType.Up, enUIEventID.Tips_ItemInfoClose, eventParams);
                }
                if (isClickToShowDetail)
                {
                    CItem item2 = itemUseable as CItem;
                    if ((item2 != null) && (item2.m_itemData.bIsView > 0))
                    {
                        CUIEventScript script2 = itemCell.GetComponent<CUIEventScript>();
                        if (script2 == null)
                        {
                            script2 = itemCell.AddComponent<CUIEventScript>();
                            script2.Initialize(formScript);
                        }
                        stUIEventParams params2 = new stUIEventParams();
                        params2.iconUseable = itemUseable;
                        script2.SetUIEvent(enUIEventType.Click, enUIEventID.GiftBag_OnShowDetail, params2);
                    }
                }
            }
        }

        public static void SetListProp(GameObject listObj, ref int[] propArr, ref int[] propPctArr)
        {
            int num = 0x25;
            if (((listObj != null) && (propArr != null)) && (((propPctArr != null) && (propArr.Length == num)) && (propPctArr.Length == num)))
            {
                int index = 0;
                int amount = 0;
                index = 0;
                while (index < num)
                {
                    if ((propArr[index] > 0) || (propPctArr[index] > 0))
                    {
                        amount++;
                    }
                    index++;
                }
                CUIListScript component = listObj.GetComponent<CUIListScript>();
                component.SetElementAmount(amount);
                amount = 0;
                for (index = 0; index < num; index++)
                {
                    if ((propArr[index] > 0) || (propPctArr[index] > 0))
                    {
                        CUIListElementScript elemenet = component.GetElemenet(amount);
                        Text text = (elemenet == null) ? null : elemenet.get_gameObject().get_transform().Find("titleText").GetComponent<Text>();
                        Text text2 = (elemenet == null) ? null : elemenet.get_gameObject().get_transform().Find("valueText").GetComponent<Text>();
                        if (text != null)
                        {
                            text.set_text(s_attNameList[index]);
                        }
                        if (text2 != null)
                        {
                            if (propArr[index] > 0)
                            {
                                text2.set_text(string.Format("+{0}", propArr[index]));
                            }
                            else if (propPctArr[index] > 0)
                            {
                                text2.set_text(string.Format("+{0}", GetValuePercent(propPctArr[index])));
                            }
                        }
                        amount++;
                    }
                }
            }
        }

        public static void SetListProp(GameObject listObj, ref int[] propArr, ref int[] propPctArr, ref int[] propValAddArr, ref int[] propPctAddArr, bool bShowAdd = true)
        {
            int num = 0x25;
            if ((((listObj != null) && (propArr != null)) && ((propPctArr != null) && (propValAddArr != null))) && (((propPctAddArr != null) && (propArr.Length == num)) && (((propPctArr.Length == num) && (propValAddArr.Length == num)) && (propPctAddArr.Length == num))))
            {
                int index = 0;
                int amount = 0;
                for (index = 0; index < num; index++)
                {
                    if (((propArr[index] > 0) || (propPctArr[index] > 0)) || (((propValAddArr[index] / 0x2710) > 0) || (propPctAddArr[index] > 0)))
                    {
                        amount++;
                    }
                }
                CUIListScript component = listObj.GetComponent<CUIListScript>();
                component.SetElementAmount(amount);
                amount = 0;
                for (index = 0; index < num; index++)
                {
                    if (((propArr[index] > 0) || (propPctArr[index] > 0)) || (((propValAddArr[index] / 0x2710) > 0) || (propPctAddArr[index] > 0)))
                    {
                        CUIListElementScript elemenet = component.GetElemenet(amount);
                        if (elemenet != null)
                        {
                            Text text = elemenet.get_gameObject().get_transform().Find("titleText").GetComponent<Text>();
                            Text text2 = elemenet.get_gameObject().get_transform().Find("valueText").GetComponent<Text>();
                            Text text3 = elemenet.get_gameObject().get_transform().Find("addValText").GetComponent<Text>();
                            GameObject obj2 = elemenet.get_gameObject().get_transform().Find("GreenPointer").get_gameObject();
                            DebugHelper.Assert(text3 != null);
                            if (text3 != null)
                            {
                                text3.get_gameObject().CustomSetActive(bShowAdd);
                            }
                            obj2.CustomSetActive(bShowAdd);
                            if (text != null)
                            {
                                text.set_text(s_attNameList[index]);
                            }
                            if ((text2 != null) && (text3 != null))
                            {
                                if ((propArr[index] > 0) || (propValAddArr[index] > 0))
                                {
                                    text2.set_text(string.Format("{0}", propArr[index]));
                                    text3.set_text(string.Format("{0}", propValAddArr[index]));
                                }
                                else if ((propPctArr[index] > 0) || (propPctAddArr[index] > 0))
                                {
                                    text2.set_text(GetValuePercent(propPctArr[index]));
                                    text3.set_text(string.Format("{0}", GetValuePercent(propPctAddArr[index])));
                                }
                            }
                        }
                        amount++;
                    }
                }
            }
        }

        public static void SetMaterialDirectBuy(CUIFormScript form, GameObject target, CUseable useable, bool autoHide = true)
        {
            if (target != null)
            {
                if (((useable == null) || (useable.m_stackCount == 0)) && (autoHide && target.get_activeSelf()))
                {
                    target.CustomSetActive(false);
                }
                CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                if ((masterRoleInfo != null) && (useable != null))
                {
                    if (((useable.m_dianQuanDirectBuy == 0) && autoHide) && target.get_activeSelf())
                    {
                        target.CustomSetActive(false);
                    }
                    if (useable.m_stackCount > masterRoleInfo.MaterialDirectBuyLimit)
                    {
                        useable.m_stackCount = masterRoleInfo.MaterialDirectBuyLimit;
                    }
                    CUIEventScript component = target.GetComponent<CUIEventScript>();
                    if (component == null)
                    {
                        component = target.AddComponent<CUIEventScript>();
                        component.Initialize(form);
                    }
                    target.CustomSetActive(true);
                    stUIEventParams eventParams = new stUIEventParams();
                    eventParams.iconUseable = useable;
                    component.SetUIEvent(enUIEventType.Click, enUIEventID.HeroInfo_Material_Direct_Buy, eventParams);
                }
            }
        }

        public static void SetObjActive(GameObject targetObj, bool isActive)
        {
            if (targetObj != null)
            {
                targetObj.CustomSetActive(isActive);
            }
        }

        public static void SetObjActive(Transform targetTrans, bool isActive)
        {
            if (targetTrans != null)
            {
                targetTrans.get_gameObject().CustomSetActive(isActive);
            }
        }

        public static void SetProgressBarData(GameObject progressBar, int curVal, int totalVal, bool bShowTxt = false)
        {
            progressBar.get_transform().Find("Image").GetComponent<Image>().CustomFillAmount(((float) curVal) / ((float) totalVal));
            Text component = progressBar.get_transform().Find("Text").GetComponent<Text>();
            if (bShowTxt)
            {
                component.set_text(curVal.ToString() + "/" + totalVal.ToString());
            }
            else
            {
                component.set_text(string.Empty);
            }
        }

        public static void SetRankDisplay(uint rankNumber, Transform rankTransform)
        {
            GameObject obj2 = rankTransform.Find("imgRank1st").get_gameObject();
            GameObject obj3 = rankTransform.Find("imgRank2nd").get_gameObject();
            GameObject obj4 = rankTransform.Find("imgRank3rd").get_gameObject();
            GameObject txtRank = rankTransform.Find("txtRank").get_gameObject();
            GameObject txtNotInRank = rankTransform.Find("txtNotInRank").get_gameObject();
            SetRankDisplay(rankNumber, obj2, obj3, obj4, txtRank, txtNotInRank);
        }

        private static void SetRankDisplay(uint rankNumber, GameObject imgRank1st, GameObject imgRank2nd, GameObject imgRank3rd, GameObject txtRank, GameObject txtNotInRank)
        {
            if (rankNumber == 0)
            {
                imgRank1st.CustomSetActive(false);
                imgRank2nd.CustomSetActive(false);
                imgRank3rd.CustomSetActive(false);
                txtRank.CustomSetActive(false);
                txtNotInRank.CustomSetActive(true);
            }
            else if (rankNumber == 1)
            {
                imgRank1st.CustomSetActive(true);
                imgRank2nd.CustomSetActive(false);
                imgRank3rd.CustomSetActive(false);
                txtRank.CustomSetActive(false);
                txtNotInRank.CustomSetActive(false);
            }
            else if (rankNumber == 2)
            {
                imgRank1st.CustomSetActive(false);
                imgRank2nd.CustomSetActive(true);
                imgRank3rd.CustomSetActive(false);
                txtRank.CustomSetActive(false);
                txtNotInRank.CustomSetActive(false);
            }
            else if (rankNumber == 3)
            {
                imgRank1st.CustomSetActive(false);
                imgRank2nd.CustomSetActive(false);
                imgRank3rd.CustomSetActive(true);
                txtRank.CustomSetActive(false);
                txtNotInRank.CustomSetActive(false);
            }
            else
            {
                imgRank1st.CustomSetActive(false);
                imgRank2nd.CustomSetActive(false);
                imgRank3rd.CustomSetActive(false);
                txtRank.CustomSetActive(true);
                txtNotInRank.CustomSetActive(false);
                txtRank.GetComponent<Text>().set_text(rankNumber.ToString());
            }
        }

        public static void SetTextColorSize(GameObject textObj, Color color, enFontSize size)
        {
            if (textObj != null)
            {
                Text component = textObj.get_transform().GetComponent<Text>();
                if (component != null)
                {
                    component.set_color(color);
                    component.set_fontSize((int) size);
                }
            }
        }

        public static void SetTextContent(GameObject textObj, string strContent)
        {
            if ((textObj != null) && (strContent != null))
            {
                Text component = textObj.get_transform().GetComponent<Text>();
                if (component != null)
                {
                    component.set_text(strContent);
                }
            }
        }

        public static void SetTextContent(Transform textObjTrans, string strContent)
        {
            if ((textObjTrans != null) && (strContent != null))
            {
                Text component = textObjTrans.GetComponent<Text>();
                if (component != null)
                {
                    component.set_text(strContent);
                }
            }
        }

        public static void ShowNewHeroOrSkin(uint heroId, uint skinId, enUIEventID eventID, bool bInitAnima = true, COM_REWARDS_TYPE rewardType = 5, bool interactableTransition = false, string prefabPath = new string(), enFormPriority priority = 1, uint convertCoin = 0, int experienceDays = 0)
        {
            NewHeroOrSkinParams item = new NewHeroOrSkinParams();
            if ((heroId == 0) && (skinId > 0))
            {
                CSkinInfo.ResolveHeroSkin(skinId, out heroId, out skinId);
            }
            item.heroId = heroId;
            item.skinId = skinId;
            item.eventID = eventID;
            item.bInitAnima = bInitAnima;
            item.rewardType = rewardType;
            item.interactableTransition = interactableTransition;
            item.prefabPath = prefabPath;
            item.priority = priority;
            item.convertCoin = convertCoin;
            item.experienceDays = experienceDays;
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Common_NewHeroOrSkinFormClose, new CUIEventManager.OnUIEventHandler(CUICommonSystem.NewHeroOrSkinFormClose));
            newHeroOrSkinList.Add(item);
            ProcessShowNewHeroOrSkin();
        }

        public static bool ShowSymbol(CUseableContainer container, enUIEventID eventID = 0)
        {
            CUIFormScript formScript = Singleton<CUIManager>.GetInstance().OpenForm(s_newSymbolFormPath, false, true);
            if (((formScript == null) || (formScript.get_gameObject() == null)) || (container == null))
            {
                Singleton<CUIManager>.GetInstance().CloseForm(s_newSymbolFormPath);
                return false;
            }
            int curUseableCount = container.GetCurUseableCount();
            GameObject obj2 = Utility.FindChild(formScript.get_gameObject(), "Panel_NewSymbol/ContentOne");
            GameObject obj3 = Utility.FindChild(formScript.get_gameObject(), "Panel_NewSymbol/ContentMulti");
            GameObject root = null;
            formScript.m_eventIDs[1] = eventID;
            switch (curUseableCount)
            {
                case 0:
                    Singleton<CUIManager>.GetInstance().CloseForm(s_newSymbolFormPath);
                    return false;

                case 1:
                    obj2.CustomSetActive(true);
                    obj3.CustomSetActive(false);
                    root = obj2;
                    break;

                default:
                    obj2.CustomSetActive(false);
                    obj3.CustomSetActive(true);
                    root = Utility.FindChild(obj3, "ScrollRect/Container/Content");
                    if (root != null)
                    {
                        Utility.SetChildrenActive(root, false);
                    }
                    break;
            }
            if (root == null)
            {
                Singleton<CUIManager>.GetInstance().CloseForm(s_newSymbolFormPath);
                return false;
            }
            for (int i = 0; i < curUseableCount; i++)
            {
                GameObject obj5 = Utility.FindChild(root, string.Format("NewSymbol{0}", i));
                if (obj5 != null)
                {
                    obj5.CustomSetActive(true);
                    Image componetInChild = Utility.GetComponetInChild<Image>(obj5, "Icon_Img");
                    Text text = Utility.GetComponetInChild<Text>(obj5, "Symbol_Name");
                    Text text2 = Utility.GetComponetInChild<Text>(obj5, "SymbolDesc");
                    if (((componetInChild == null) || (text == null)) || (text2 == null))
                    {
                        return false;
                    }
                    CSymbolItem useableByIndex = container.GetUseableByIndex(i) as CSymbolItem;
                    if (useableByIndex != null)
                    {
                        CUIUtility.SetImageSprite(componetInChild, useableByIndex.GetIconPath(), formScript, true, false, false, false);
                        text.set_text(useableByIndex.m_name);
                        text2.set_text(CSymbolSystem.GetSymbolAttString(useableByIndex, true));
                    }
                }
            }
            return true;
        }
    }
}

