﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.UI;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    internal class CAdventureView
    {
        private static bool bLevelUp;
        private static int CurrentItemPos;
        private static int CurrentRewardPos;
        public static string MAT_BOSS_LEVEL = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Red_Normal.mat";
        public static string MAT_BOSS_LEVEL_HIGHLIGHT = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Red_Open.mat";
        public static string MAT_BOSS_LEVEL_LOCK = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Red_Lock.mat";
        public static string MAT_ELITE_LEVEL = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Golden_Normal.mat";
        public static string MAT_ELITE_LEVEL_HIGHLIGHT = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Golden_Open.mat";
        public static string MAT_ELITE_LEVEL_LOCK = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Golden_Lock.mat";
        public static string MAT_NORMAL_LEVEL = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Bule_Normal.mat";
        public static string MAT_NORMAL_LEVEL_HIGHLIGHT = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Bule_Open.mat";
        public static string MAT_NORMAL_LEVEL_LOCK = "UGUI/Form/System/PvE/Adv/Material/PVE_Adv_Bule_Lock.mat";
        private static uint newLevel;
        private static uint oldLevel;
        private static SCDT_SWEEP_REWARD rewardRef;
        public static Color[] s_Adv_Chaptper_Colors = new Color[] { new Color(0.3019608f, 1f, 1f), new Color(0.8941177f, 0.8941177f, 0.8941177f), new Color(0.3529412f, 0.3529412f, 0.3529412f) };
        public static Color[] s_Adv_Difficult_Bg_Color = new Color[] { new Color(0.1176471f, 1f, 1f), new Color(0f, 0.7843137f, 1f), new Color(1f, 0.2784314f, 0.2784314f), new Color(1f, 0f, 0.5568628f) };
        public static Color[] s_Adv_Difficult_Circle_Color = new Color[] { new Color(0.3686275f, 1f, 1f), new Color(0.3686275f, 0.682353f, 1f), new Color(1f, 0f, 0f), new Color(1f, 0f, 0.9176471f) };
        public static Color[] s_Adv_Difficult_Color = new Color[] { new Color(1f, 1f, 1f), new Color(0.1176471f, 1f, 1f), new Color(1f, 0f, 0.1254902f), new Color(1f, 0f, 0.5568628f), new Color(1f, 1f, 1f), new Color(0.2588235f, 0.7843137f, 1f), new Color(1f, 0f, 0.1254902f), new Color(1f, 0f, 0.5568628f) };
        public static Color s_Adv_Difficulty_Gray_Color = new Color(0.5882353f, 0.5882353f, 0.5882353f);
        public static Color[] s_Adv_Level_Colors = new Color[] { new Color(1f, 1f, 1f), new Color(0.8941177f, 0.8941177f, 0.8941177f), new Color(0.5019608f, 0.5254902f, 0.5882353f) };

        public static void CheckMopupLevelUp()
        {
            if (bLevelUp)
            {
                CUIEvent uiEvent = new CUIEvent();
                uiEvent.m_eventID = enUIEventID.Settle_OpenLvlUp;
                uiEvent.m_eventParams.tag = (int) oldLevel;
                uiEvent.m_eventParams.tag2 = (int) newLevel;
                Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uiEvent);
                bLevelUp = false;
                oldLevel = newLevel = 0;
            }
        }

        public static void CloseChapterRewardPanel(GameObject root)
        {
            root.get_transform().Find("ChapterRewardPanel").get_gameObject().CustomSetActive(false);
        }

        private static string GetChapterBgPath(int ChapterId)
        {
            return string.Format("{0}Chapter_{1}", CUIUtility.s_Sprite_Dynamic_Adventure_Dir, ChapterId);
        }

        private static string GetChapterIcon(int ChapterId)
        {
            ResChapterInfo dataByKey = GameDataMgr.chapterInfoDatabin.GetDataByKey((uint) ChapterId);
            if (dataByKey != null)
            {
                return StringHelper.UTF8BytesToString(ref dataByKey.szChapterIcon);
            }
            return string.Empty;
        }

        public static string GetChapterName(int ChapterId)
        {
            ResChapterInfo dataByKey = GameDataMgr.chapterInfoDatabin.GetDataByKey((uint) ChapterId);
            if (dataByKey != null)
            {
                return StringHelper.UTF8BytesToString(ref dataByKey.szChapterName);
            }
            return string.Empty;
        }

        private static string GetDifficultIcon(int difficult)
        {
            return string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Adventure_Dir, ((RES_LEVEL_DIFFICULTY_TYPE) difficult).ToString());
        }

        private static string GetLevelBgPath(int ChapterId, int LevelNo, int difficult)
        {
            return string.Format("{0}{1}_{2}", CUIUtility.s_Sprite_Dynamic_Adventure_Dir, ChapterId.ToString("D2"), LevelNo.ToString("D2"));
        }

        private static string GetLevelFramePath(int difficult)
        {
            return string.Format("{0}Adventure_Level_Frame_{1}", CUIUtility.s_Sprite_Dynamic_Adventure_Dir, difficult);
        }

        private static string GetLevelSelectFramePath(int difficult)
        {
            return string.Format("{0}Adventure_Level_Selected_Frame_{1}", CUIUtility.s_Sprite_Dynamic_Adventure_Dir, difficult);
        }

        public static void InitChapterElement(CUIFormScript formScript, int currentChapter, int levelNo, int difficulty)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                PVE_ADV_COMPLETE_INFO pve_adv_complete_info = masterRoleInfo.pveLevelDetail[difficulty - 1];
                PVE_CHAPTER_COMPLETE_INFO pve_chapter_complete_info = pve_adv_complete_info.ChapterDetailList[currentChapter - 1];
                PVE_LEVEL_COMPLETE_INFO[] levelDetailList = pve_chapter_complete_info.LevelDetailList;
                ResLevelCfgInfo dataByKey = GameDataMgr.levelDatabin.GetDataByKey((long) levelDetailList[levelNo - 1].iLevelID);
                if (dataByKey != null)
                {
                    formScript.get_transform().FindChild("ChapterElement/ChapterImg").GetComponent<Image>().SetSprite(GetLevelBgPath(currentChapter, levelNo, difficulty), formScript, true, false, false, false);
                    formScript.get_transform().FindChild("ChapterElement/ChapterNameText").GetComponent<Text>().set_text(Utility.UTF8Convert(dataByKey.szName));
                    formScript.get_transform().FindChild("ChapterElement/ChapterDEscText").GetComponent<Text>().set_text(Utility.UTF8Convert(dataByKey.szLevelDesc));
                    string[] args = new string[] { dataByKey.RecommendLevel[difficulty - 1].ToString() };
                    formScript.get_transform().FindChild("ChapterElement/RecPlayerLvlText").GetComponent<Text>().set_text(Singleton<CTextManager>.instance.GetText("Level_Recommend_Tips_1", args));
                    formScript.get_transform().FindChild("Bg").GetComponent<Image>().set_color(s_Adv_Difficult_Bg_Color[difficulty - 1]);
                }
            }
        }

        public static void InitChapterForm(CUIFormScript formScript, int currentChapter, int levelNo, int difficulty)
        {
            if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo() != null)
            {
                InitChapterList(formScript, currentChapter, levelNo, difficulty);
                InitLevelList(formScript, currentChapter, levelNo, difficulty);
                InitDifficultList(formScript, currentChapter, levelNo, difficulty);
                InitChapterElement(formScript, currentChapter, levelNo, difficulty);
            }
        }

        public static void InitChapterList(CUIFormScript formScript, int currentChapter, int levelNo, int difficulty)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                CUIListScript component = formScript.get_transform().FindChild("ChapterList").GetComponent<CUIListScript>();
                PVE_ADV_COMPLETE_INFO pve_adv_complete_info = masterRoleInfo.pveLevelDetail[difficulty - 1];
                PVE_CHAPTER_COMPLETE_INFO chapterInfo = null;
                CUIListElementScript elemenet = null;
                int stars = 0;
                string prefabPath = string.Empty;
                string chapterName = string.Empty;
                component.SetElementAmount(CAdventureSys.CHAPTER_NUM);
                for (int i = 0; i < CAdventureSys.CHAPTER_NUM; i++)
                {
                    ResChapterInfo dataByIndex = GameDataMgr.chapterInfoDatabin.GetDataByIndex(i);
                    DebugHelper.Assert(dataByIndex != null);
                    bool bActive = Singleton<CFunctionUnlockSys>.instance.ChapterIsUnlock(dataByIndex.dwChapterId);
                    bool flag2 = i == (Singleton<CAdventureSys>.instance.bNewChapterId - 1);
                    chapterInfo = pve_adv_complete_info.ChapterDetailList[i];
                    elemenet = component.GetElemenet(i);
                    stars = CAdventureSys.GetChapterTotalStar(chapterInfo);
                    SetRewardItem(elemenet.get_gameObject(), chapterInfo, stars, i);
                    chapterName = GetChapterName(i + 1);
                    elemenet.get_transform().FindChild("ChapterNameText").GetComponent<Text>().set_text(chapterName);
                    if ((currentChapter == (i + 1)) && bActive)
                    {
                        elemenet.get_transform().FindChild("ChapterNameText").GetComponent<Text>().set_color(s_Adv_Chaptper_Colors[0]);
                    }
                    else if (bActive)
                    {
                        elemenet.get_transform().FindChild("ChapterNameText").GetComponent<Text>().set_color(s_Adv_Chaptper_Colors[1]);
                    }
                    else
                    {
                        elemenet.get_transform().FindChild("ChapterNameText").GetComponent<Text>().set_color(s_Adv_Chaptper_Colors[2]);
                    }
                    elemenet.GetComponent<CUIEventScript>().m_onClickEventID = enUIEventID.Adv_SelectChapter;
                    elemenet.GetComponent<CUIEventScript>().m_onClickEventParams.tag = i + 1;
                    elemenet.get_transform().FindChild("Lock").get_gameObject().CustomSetActive(!bActive);
                    elemenet.get_transform().FindChild("Unlock").get_gameObject().CustomSetActive(bActive);
                    prefabPath = GetChapterBgPath(i + 1);
                    elemenet.get_transform().FindChild("BackgroundImg").GetComponent<Image>().SetSprite(prefabPath, component.m_belongedFormScript, true, false, false, false);
                    elemenet.get_transform().FindChild("Lock/SelectedImg").GetComponent<Image>().SetSprite(prefabPath, component.m_belongedFormScript, true, false, false, false);
                    elemenet.get_transform().FindChild("Lock/LockText").GetComponent<Text>().set_text(Utility.UTF8Convert(dataByIndex.szLockedTip));
                    elemenet.get_transform().FindChild("New").get_gameObject().CustomSetActive(flag2);
                }
                component.SelectElement(currentChapter - 1, true);
                component.MoveElementInScrollArea(currentChapter - 1, true);
            }
        }

        public static void InitDifficultList(CUIFormScript form, int currentChapter, int levelNo, int difficulty)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                CUIListScript component = form.get_transform().FindChild("DifficultList").GetComponent<CUIListScript>();
                CUIListElementScript elemenet = null;
                string text = string.Empty;
                string prefabPath = string.Empty;
                int amount = CAdventureSys.LEVEL_DIFFICULT_OPENED;
                component.SetElementAmount(amount);
                for (int i = 0; i < amount; i++)
                {
                    bool flag = CAdventureSys.IsDifOpen(currentChapter, i + 1);
                    prefabPath = GetDifficultIcon(i + 1);
                    elemenet = component.GetElemenet(i);
                    Image image = elemenet.get_transform().FindChild("DifficultImg").GetComponent<Image>();
                    image.SetSprite(prefabPath, form, true, false, false, false);
                    image.set_color(!flag ? s_Adv_Difficulty_Gray_Color : Color.get_white());
                    elemenet.get_transform().FindChild("SelectedFrame").GetComponent<Image>().SetSprite(prefabPath, form, true, false, false, false);
                    text = Singleton<CTextManager>.instance.GetText(string.Format("Adventure_Level_{0}", i + 1));
                    elemenet.get_transform().FindChild("DifficultImg/DifficultText").GetComponent<Text>().set_text(text);
                    elemenet.get_transform().FindChild("SelectedFrame/DifficultText").GetComponent<Text>().set_text(text);
                    elemenet.GetComponent<CUIEventScript>().m_onClickEventID = enUIEventID.Adv_SelectDifficult;
                    elemenet.GetComponent<CUIEventScript>().m_onClickEventParams.tag = i + 1;
                    elemenet.get_transform().FindChild("SelectedFrame/Frame_circle").GetComponent<Image>().set_color(s_Adv_Difficult_Circle_Color[i]);
                    PVE_ADV_COMPLETE_INFO pve_adv_complete_info = masterRoleInfo.pveLevelDetail[i];
                    PVE_CHAPTER_COMPLETE_INFO chapterInfo = pve_adv_complete_info.ChapterDetailList[currentChapter - 1];
                    int chapterTotalStar = CAdventureSys.GetChapterTotalStar(chapterInfo);
                    elemenet.get_transform().FindChild("SelectedFrame/RewardBox").get_gameObject().CustomSetActive((chapterTotalStar == (CAdventureSys.LEVEL_PER_CHAPTER * CAdventureSys.STAR_PER_LEVEL)) && (chapterInfo.bIsGetBonus == 0));
                    elemenet.get_transform().FindChild("Lock").get_gameObject().CustomSetActive(!flag);
                }
                component.SelectElement(difficulty - 1, true);
            }
        }

        public static void InitLevelForm(CUIFormScript formScript, int chapterNo, int LevelNo, int difficulty)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                PVE_ADV_COMPLETE_INFO pve_adv_complete_info = masterRoleInfo.pveLevelDetail[difficulty - 1];
                PVE_LEVEL_COMPLETE_INFO pve_level_complete_info = pve_adv_complete_info.ChapterDetailList[chapterNo - 1].LevelDetailList[LevelNo - 1];
                GameObject obj2 = formScript.get_gameObject();
                ResLevelCfgInfo dataByKey = GameDataMgr.levelDatabin.GetDataByKey((long) pve_level_complete_info.iLevelID);
                if (dataByKey != null)
                {
                    string str = StringHelper.UTF8BytesToString(ref dataByKey.szName);
                    formScript.get_transform().Find("PanelLeft/DifficultText").GetComponent<Text>().set_text(Singleton<CTextManager>.instance.GetText(string.Format("Adventure_Level_{0}", difficulty)));
                    formScript.get_transform().Find("Panel_Main/ImgMapNameBg/MapNameText").GetComponent<Text>().set_text(str);
                    formScript.get_transform().Find("PanelLeft/MapNameText").GetComponent<Text>().set_text(str);
                    formScript.get_transform().Find("PanelLeft/MapDescText").GetComponent<Text>().set_text(StringHelper.UTF8BytesToString(ref dataByKey.szLevelDesc));
                    string[] args = new string[] { dataByKey.RecommendLevel[difficulty - 1].ToString() };
                    formScript.get_transform().Find("PanelLeft/RecPlayerLvlText").GetComponent<Text>().set_text(Singleton<CTextManager>.instance.GetText("Level_Recommend_Tips_1", args));
                    formScript.get_transform().Find("PanelLeft/ChapterImg").GetComponent<Image>().SetSprite(GetLevelBgPath(chapterNo, LevelNo, difficulty), formScript, true, false, false, false);
                    formScript.get_transform().Find("PanelLeft/DifficultImg").GetComponent<Image>().SetSprite(GetDifficultIcon(difficulty), formScript, true, false, false, false);
                    for (int i = 1; i <= CAdventureSys.STAR_PER_LEVEL; i++)
                    {
                        GameObject descCon = obj2.get_transform().Find("PanelRight/WinCondition" + i).get_gameObject();
                        SetStarConditionDesc(formScript, descCon, (uint) dataByKey.astStarDetail[i - 1].iParam, CAdventureSys.IsStarGained(pve_level_complete_info.bStarBits, i));
                    }
                    GameObject itemCell = obj2.get_transform().Find("PanelRight/itemCell").get_gameObject();
                    SetReward(formScript, itemCell, dataByKey, difficulty);
                    GameObject obj5 = obj2.get_transform().Find("PanelRight/HeroList").get_gameObject();
                    int teamPower = 0;
                    List<uint> heroListForBattleListID = Singleton<CHeroSelectBaseSystem>.instance.GetHeroListForBattleListID(dataByKey.dwBattleListID);
                    SetTeamHeroList(obj5, heroListForBattleListID, out teamPower);
                    SetStartBtnEnable(obj2.get_transform().Find("BtnStart").get_gameObject(), heroListForBattleListID);
                    formScript.get_gameObject().get_transform().FindChild("Bg").get_gameObject().GetComponent<Image>().set_color(s_Adv_Difficult_Bg_Color[difficulty - 1]);
                }
                else
                {
                    object[] inParameters = new object[] { pve_level_complete_info.iLevelID };
                    DebugHelper.Assert(false, "Can't find level info -- id: {0}", inParameters);
                }
            }
        }

        public static void InitLevelList(CUIFormScript form, int currentChapter, int levelNo, int difficulty)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            if (masterRoleInfo != null)
            {
                ResChapterInfo dataByKey = GameDataMgr.chapterInfoDatabin.GetDataByKey((long) currentChapter);
                DebugHelper.Assert(dataByKey != null);
                bool flag = Singleton<CFunctionUnlockSys>.instance.ChapterIsUnlock(dataByKey.dwChapterId);
                PVE_ADV_COMPLETE_INFO pve_adv_complete_info = masterRoleInfo.pveLevelDetail[difficulty - 1];
                PVE_CHAPTER_COMPLETE_INFO pve_chapter_complete_info = pve_adv_complete_info.ChapterDetailList[currentChapter - 1];
                PVE_LEVEL_COMPLETE_INFO[] levelDetailList = pve_chapter_complete_info.LevelDetailList;
                CUIListScript component = form.get_transform().FindChild("LevelList").GetComponent<CUIListScript>();
                component.SetElementAmount(levelDetailList.Length);
                CUIListElementScript elemenet = null;
                Sprite sprite = CUIUtility.GetSpritePrefeb(GetLevelFramePath(difficulty), false, false).GetComponent<SpriteRenderer>().get_sprite();
                GameObject prefab = CUIUtility.GetSpritePrefeb(GetLevelSelectFramePath(difficulty), false, false);
                for (int i = 0; i < levelDetailList.Length; i++)
                {
                    elemenet = component.GetElemenet(i);
                    ResLevelCfgInfo info3 = GameDataMgr.levelDatabin.GetDataByKey((long) levelDetailList[i].iLevelID);
                    object[] inParameters = new object[] { levelDetailList[i].iLevelID };
                    DebugHelper.Assert(info3 != null, "Can't find LevelConfig = {0}", inParameters);
                    bool bActive = (levelDetailList[i].levelStatus == 0) || !flag;
                    bool flag3 = (levelDetailList[i].levelStatus == 1) && flag;
                    int starNum = CAdventureSys.GetStarNum(levelDetailList[i].bStarBits);
                    elemenet.get_transform().FindChild("Unlock/star1").GetComponent<Image>().set_color((starNum < 1) ? CUIUtility.s_Color_GrayShader : Color.get_white());
                    elemenet.get_transform().FindChild("Unlock/star2").GetComponent<Image>().set_color((starNum < 2) ? CUIUtility.s_Color_GrayShader : Color.get_white());
                    elemenet.get_transform().FindChild("Unlock/star3").GetComponent<Image>().set_color((starNum < 3) ? CUIUtility.s_Color_GrayShader : Color.get_white());
                    elemenet.get_transform().FindChild("TxtLevelNameText").GetComponent<Text>().set_text(Utility.UTF8Convert(info3.szName));
                    if ((levelNo == (i + 1)) && !bActive)
                    {
                        elemenet.get_transform().FindChild("TxtLevelNameText").GetComponent<Text>().set_color(s_Adv_Level_Colors[0]);
                    }
                    else if (!bActive)
                    {
                        elemenet.get_transform().FindChild("TxtLevelNameText").GetComponent<Text>().set_color(s_Adv_Level_Colors[1]);
                    }
                    else
                    {
                        elemenet.get_transform().FindChild("TxtLevelNameText").GetComponent<Text>().set_color(s_Adv_Level_Colors[2]);
                    }
                    elemenet.get_transform().FindChild("SelectedFrame").GetComponent<Image>().set_color(s_Adv_Difficult_Color[difficulty - 1]);
                    elemenet.get_transform().FindChild("SelectedFrame/Image1").GetComponent<Image>().set_color(s_Adv_Difficult_Color[((s_Adv_Difficult_Color.Length / 2) + difficulty) - 1]);
                    elemenet.get_transform().FindChild("SelectedFrame/Image2").GetComponent<Image>().set_color(s_Adv_Difficult_Color[((s_Adv_Difficult_Color.Length / 2) + difficulty) - 1]);
                    elemenet.get_transform().FindChild("SelectedFrame/SelectedFrame").GetComponent<Image>().SetSprite(prefab, false);
                    elemenet.get_transform().FindChild("New").get_gameObject().CustomSetActive(flag3);
                    elemenet.GetComponent<CUIEventScript>().m_onClickEventID = enUIEventID.Adv_SelectLevel;
                    elemenet.GetComponent<CUIEventScript>().m_onClickEventParams.tag = i + 1;
                    elemenet.get_transform().FindChild("Unlock").get_gameObject().CustomSetActive(!bActive);
                    elemenet.get_transform().FindChild("Lock").get_gameObject().CustomSetActive(bActive);
                    elemenet.m_selectedSprite = sprite;
                    elemenet.GetComponent<Image>().SetSprite(((levelNo - 1) != i) ? elemenet.m_defaultSprite : sprite, elemenet.m_selectedLayout);
                }
                component.SelectElement(levelNo - 1, true);
            }
        }

        private static bool isVip()
        {
            return true;
        }

        public static void OpenChapterRewardPanel(CUIFormScript formScript, GameObject root, int ChapterId, int difficulty, bool bCanGet)
        {
            GameObject obj2 = root.get_transform().Find("ChapterRewardPanel").get_gameObject();
            obj2.CustomSetActive(true);
            ResChapterInfo dataByKey = GameDataMgr.chapterInfoDatabin.GetDataByKey((uint) ChapterId);
            object[] inParameters = new object[] { ChapterId };
            DebugHelper.Assert(dataByKey != null, "Can't find chapter config with ID: {0}", inParameters);
            ResDT_ChapterRewardInfo[] astNormalRewardDetail = null;
            astNormalRewardDetail = dataByKey.astNormalRewardDetail;
            if (difficulty == 1)
            {
                astNormalRewardDetail = dataByKey.astNormalRewardDetail;
            }
            else if (difficulty == 2)
            {
                astNormalRewardDetail = dataByKey.astEliteRewardDetail;
            }
            else if (difficulty == 3)
            {
                astNormalRewardDetail = dataByKey.astMasterRewardDetail;
            }
            else if (difficulty == 4)
            {
                astNormalRewardDetail = dataByKey.astAbyssRewardDetail;
            }
            object[] objArray2 = new object[] { ChapterId, difficulty };
            DebugHelper.Assert(astNormalRewardDetail != null, "Chapter RewardArr is NULL! -- ID: {0}, Difficulty: {1}", objArray2);
            for (int i = 0; i < astNormalRewardDetail.Length; i++)
            {
                ResDT_ChapterRewardInfo info2 = astNormalRewardDetail[i];
                GameObject obj3 = obj2.get_transform().Find(string.Format("RewardCon/itemCell{0}", i + 1)).get_gameObject();
                if (info2.bType != 0)
                {
                    obj3.CustomSetActive(true);
                    CUseable itemUseable = CUseableManager.CreateUsableByServerType((RES_REWARDS_TYPE) info2.bType, (int) info2.dwNum, info2.dwID);
                    CUICommonSystem.SetItemCell(formScript, obj3, itemUseable, true, false, false, false);
                }
                else
                {
                    obj3.CustomSetActive(false);
                }
            }
            GameObject obj4 = obj2.get_transform().Find("BtnGetReward").get_gameObject();
            if (bCanGet)
            {
                obj4.GetComponentInChildren<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("PVE_Level_Can_Get_The_Box"));
                obj4.GetComponent<Button>().set_interactable(true);
            }
            else
            {
                obj4.GetComponentInChildren<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("PVE_Level_Not_Enough_Starts"));
                obj4.GetComponent<Button>().set_interactable(false);
            }
        }

        private static void SetMopupEnable(GameObject Mopup, byte StarBitsMask, RES_LEVEL_DIFFICULTY_TYPE difficulty, int LeftPlayNum)
        {
            Button component = Mopup.GetComponent<Button>();
            bool isEnable = false;
            if (difficulty == RES_LEVEL_DIFFICULTY_TYPE.RES_LEVEL_DIFFICULTY_TYPE_NORMAL)
            {
                isEnable = CAdventureSys.GetStarNum(StarBitsMask) == CAdventureSys.STAR_PER_LEVEL;
            }
            else if (difficulty == RES_LEVEL_DIFFICULTY_TYPE.RES_LEVEL_DIFFICULTY_TYPE_NIGHTMARE)
            {
                isEnable = (CAdventureSys.GetStarNum(StarBitsMask) == CAdventureSys.STAR_PER_LEVEL) && (LeftPlayNum > 0);
            }
            else
            {
                object[] inParameters = new object[] { difficulty };
                DebugHelper.Assert(false, "Invalid difficulty -- {0}", inParameters);
                return;
            }
            CUICommonSystem.SetButtonEnable(component, isEnable, true, true);
        }

        public static void SetMopupLevelUp(uint oldLvl, uint newLvl)
        {
            bLevelUp = true;
            oldLevel = oldLvl;
            newLevel = newLvl;
        }

        private static void SetMopupTenEnable(GameObject Mopup, byte StarBitsMask, RES_LEVEL_DIFFICULTY_TYPE difficulty, int LeftPlayNum)
        {
            Button component = Mopup.GetComponent<Button>();
            bool isEnable = false;
            CUIEventScript script = component.get_gameObject().GetComponent<CUIEventScript>();
            stUIEventParams params = new stUIEventParams();
            if (difficulty == RES_LEVEL_DIFFICULTY_TYPE.RES_LEVEL_DIFFICULTY_TYPE_NORMAL)
            {
                string[] args = new string[] { "10" };
                Utility.GetComponetInChild<Text>(Mopup, "Text").set_text(Singleton<CTextManager>.GetInstance().GetText("PVE_Level_Sweep_Number", args));
                isEnable = (CAdventureSys.GetStarNum(StarBitsMask) == CAdventureSys.STAR_PER_LEVEL) && isVip();
                params.tag = 10;
                script.m_onClickEventParams = params;
            }
            else if (difficulty == RES_LEVEL_DIFFICULTY_TYPE.RES_LEVEL_DIFFICULTY_TYPE_NIGHTMARE)
            {
                Text componetInChild = Utility.GetComponetInChild<Text>(Mopup, "Text");
                if (LeftPlayNum > 0)
                {
                    string[] textArray2 = new string[] { LeftPlayNum.ToString() };
                    componetInChild.set_text(Singleton<CTextManager>.GetInstance().GetText("PVE_Level_Sweep_Number", textArray2));
                }
                else
                {
                    componetInChild.set_text(Singleton<CTextManager>.GetInstance().GetText("PVE_Level_Can_Not_Sweep"));
                }
                isEnable = ((CAdventureSys.GetStarNum(StarBitsMask) == CAdventureSys.STAR_PER_LEVEL) && isVip()) && (LeftPlayNum > 0);
                params.tag = LeftPlayNum;
                script.m_onClickEventParams = params;
            }
            else
            {
                object[] inParameters = new object[] { difficulty };
                DebugHelper.Assert(false, "Invalid difficulty -- {0}", inParameters);
                return;
            }
            CUICommonSystem.SetButtonEnable(component, isEnable, true, true);
        }

        private static void SetReward(CUIFormScript formScript, GameObject itemCell, ResLevelCfgInfo resLevelInfo, int difficulty)
        {
            ResDT_PveRewardShowInfo info = resLevelInfo.astRewardShowDetail[difficulty - 1];
            CUseable itemUseable = CUseableManager.CreateUseable((COM_ITEM_TYPE) info.bRewardType, info.dwRewardID, 0);
            if (itemUseable != null)
            {
                CUICommonSystem.SetItemCell(formScript, itemCell, itemUseable, true, false, false, false);
            }
        }

        private static void SetRewardItem(GameObject item, PVE_CHAPTER_COMPLETE_INFO chapterInfo, int stars, int chapterNo)
        {
            item.CustomSetActive(true);
            int num = CAdventureSys.LEVEL_PER_CHAPTER * CAdventureSys.STAR_PER_LEVEL;
            if (stars == num)
            {
                if (chapterInfo.bIsGetBonus > 0)
                {
                    item.get_transform().FindChild("Unlock/RewardBox").GetComponent<Image>().set_color(Color.get_gray());
                    item.get_transform().FindChild("Unlock/StarText").GetComponent<Text>().set_text(Singleton<CTextManager>.instance.GetText("hasGot"));
                }
                else
                {
                    item.get_transform().FindChild("Unlock/RewardBox").GetComponent<Image>().set_color(Color.get_white());
                    item.get_transform().FindChild("Unlock/StarText").GetComponent<Text>().set_text(string.Format("{0}/{1}", stars, num));
                }
            }
            else
            {
                item.get_transform().FindChild("Unlock/RewardBox").GetComponent<Image>().set_color(Color.get_gray());
                item.get_transform().FindChild("Unlock/StarText").GetComponent<Text>().set_text(string.Format("{0}/{1}", stars, num));
            }
            item.get_transform().FindChild("Unlock/RewardBox").GetComponent<CUIEventScript>().m_onClickEventParams.tag = chapterNo + 1;
        }

        private static void SetStarConditionDesc(CUIFormScript formScript, GameObject descCon, uint conditionId, bool bAchieved)
        {
            Text component = descCon.get_transform().Find("TxtWinCondtion").get_gameObject().GetComponent<Text>();
            ResEvaluateStarInfo dataByKey = GameDataMgr.evaluateCondInfoDatabin.GetDataByKey(conditionId);
            object[] inParameters = new object[] { conditionId };
            DebugHelper.Assert(dataByKey != null, "Can't find star condition config with ID: {0}", inParameters);
            component.set_text(StringHelper.UTF8BytesToString(ref dataByKey.szCondDesc));
            component.set_color(!bAchieved ? Color.get_white() : Color.get_green());
            descCon.get_transform().Find("ImgStar").get_gameObject().GetComponent<Image>().SetSprite("UGUI/Sprite/System/" + (!bAchieved ? "Adventure/empty_big_star" : "Adventure/big_star"), formScript, true, false, false, false);
        }

        private static void SetStartBtnEnable(GameObject Start, List<uint> heros)
        {
            Button component = Start.GetComponent<Button>();
            bool isEnable = (heros != null) && (heros.Count > 0);
            CUICommonSystem.SetButtonEnable(component, isEnable, isEnable, true);
            component.set_interactable(isEnable);
        }

        private static void SetTeamHeroList(GameObject list, List<uint> heroIds, out int teamPower)
        {
            CUIListScript component = list.GetComponent<CUIListScript>();
            CUIListElementScript elemenet = null;
            teamPower = 0;
            component.SetElementAmount(heroIds.Count);
            for (int i = 0; i < heroIds.Count; i++)
            {
                elemenet = component.GetElemenet(i);
                GameObject item = elemenet.get_gameObject().get_transform().Find("heroItemCell").get_gameObject();
                if (heroIds[i] > 0)
                {
                    IHeroData data = CHeroDataFactory.CreateHeroData(heroIds[i]);
                    teamPower += data.combatEft;
                    CUICommonSystem.SetHeroItemData(elemenet.m_belongedFormScript, item, data, enHeroHeadType.enIcon, false, true);
                    elemenet.get_gameObject().CustomSetActive(true);
                    item.get_gameObject().CustomSetActive(true);
                }
                else
                {
                    elemenet.get_gameObject().CustomSetActive(false);
                    item.get_gameObject().CustomSetActive(false);
                }
            }
        }

        private static void SetTeamHeroList(GameObject list, COMDT_BATTLELIST_LIST battleList, uint battleListID, out int teamPower)
        {
            CUIListScript component = list.GetComponent<CUIListScript>();
            CUIListElementScript elemenet = null;
            teamPower = 0;
            component.SetElementAmount(0);
            if ((battleList != null) && (battleList.dwListNum != 0))
            {
                for (int i = 0; i < battleList.dwListNum; i++)
                {
                    if (battleList.astBattleList[i].dwBattleListID == battleListID)
                    {
                        if (battleList.astBattleList[i].stBattleList.wHeroCnt == 0)
                        {
                            return;
                        }
                        component.SetElementAmount(battleList.astBattleList[i].stBattleList.wHeroCnt);
                        int num2 = 0;
                        for (int j = 0; j < battleList.astBattleList[i].stBattleList.wHeroCnt; j++)
                        {
                            elemenet = component.GetElemenet(j);
                            GameObject item = elemenet.get_gameObject().get_transform().Find("heroItemCell").get_gameObject();
                            if (battleList.astBattleList[i].stBattleList.BattleHeroList[j] > 0)
                            {
                                IHeroData data = CHeroDataFactory.CreateHeroData(battleList.astBattleList[i].stBattleList.BattleHeroList[j]);
                                teamPower += data.combatEft;
                                CUICommonSystem.SetHeroItemData(elemenet.m_belongedFormScript, item, data, enHeroHeadType.enIcon, false, true);
                                elemenet.get_gameObject().CustomSetActive(true);
                                item.get_gameObject().CustomSetActive(true);
                                num2++;
                            }
                            else
                            {
                                elemenet.get_gameObject().CustomSetActive(false);
                                item.get_gameObject().CustomSetActive(false);
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}

