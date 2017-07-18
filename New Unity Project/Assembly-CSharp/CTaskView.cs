﻿using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using ResData;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class CTaskView
{
    private Text day_huoyue_txt;
    private Image day_progress;
    private Image day_progress_bg;
    private GameObject jumpRewardGameObject;
    private static int LevelValue = 0x15;
    private CUIContainerScript m_container;
    private CUIFormScript m_CUIForm;
    public GameObject m_emptyTaskNode;
    public CUIListScript m_levelRewardList;
    public GameObject m_levelRewardNode;
    public Text m_levelRewardText;
    public GameObject m_mainTaskNode;
    public GameObject m_normalTaskNode;
    private float m_orgWeekgoldProgress = -0.1f;
    private int m_tabIndex = -1;
    public GameObject m_taskNode0;
    public GameObject m_taskNode1;
    public Text m_unlockInfoTxt;
    public GameObject m_unlockNode;
    private CUIListScript tablistScript;
    private CUIListScript tasklistScript_iwantgold;
    private CUIListScript tasklistScript_main;
    private CUIListScript tasklistScript_mishu;
    private CUIListScript tasklistScript_usual;
    private Text week_huoyue_text;
    private GameObject week_node1;
    private GameObject week_node2;

    private void _calc_red_dot(enTaskTab type)
    {
        int index = (type != enTaskTab.TAB_MAIN) ? 1 : 0;
        if (Singleton<CTaskSys>.instance.model.task_Data.GetTask_Count(type, CTask.State.Have_Done) > 0)
        {
            this.AddRedDot(index, enRedDotPos.enTopRight);
        }
        else
        {
            this.DelRedDot(index);
        }
        if (type == enTaskTab.TAB_MAIN)
        {
            if (Singleton<CTaskSys>.instance.model.IsShowMainTaskTab_RedDotCount())
            {
                this.jumpRewardGameObject.CustomSetActive(true);
                this.AddRedDot(0, enRedDotPos.enTopRight);
            }
            else
            {
                this.jumpRewardGameObject.CustomSetActive(false);
                this.DelRedDot(0);
            }
        }
    }

    private CTask _get_current_info(RES_TASK_TYPE taskType, int index)
    {
        ListView<CTask> listView = Singleton<CTaskSys>.instance.model.task_Data.GetListView((int) taskType);
        if (((listView != null) && (index >= 0)) && (index < listView.Count))
        {
            return listView[index];
        }
        return null;
    }

    private void _init_day_huoyue()
    {
        CTaskModel model = Singleton<CTaskSys>.instance.model;
        uint num = model.huoyue_data.max_dayhuoyue_num;
        float x = (this.day_progress_bg.get_transform() as RectTransform).get_sizeDelta().x;
        GameObject obj2 = null;
        int count = model.huoyue_data.day_huoyue_list.Count;
        for (int i = 0; i < count; i++)
        {
            ushort key = model.huoyue_data.day_huoyue_list[i];
            ResHuoYueDuReward reward = null;
            GameDataMgr.huoyueduDict.TryGetValue(key, out reward);
            if (reward != null)
            {
                float num6 = (((float) reward.dwHuoYueDu) / ((float) num)) * x;
                if (num6 > x)
                {
                    num6 = x;
                }
                int element = this.m_container.GetElement();
                obj2 = this.m_container.GetElement(element);
                if (obj2 != null)
                {
                    (obj2.get_transform() as RectTransform).set_anchoredPosition3D(new Vector3(num6, 0f, 0f));
                    obj2.get_gameObject().set_name(string.Format("box_{0}", reward.wID));
                    obj2.get_transform().Find("icon").GetComponent<CUIEventScript>().m_onDownEventParams.tagUInt = reward.wID;
                }
            }
        }
    }

    private void _refresh_Level_list(CUIListScript listScript, int count)
    {
        if (listScript != null)
        {
            ListView<LevelRewardData> levelRewardDataMap = Singleton<CTaskSys>.instance.model.m_levelRewardDataMap;
            listScript.SetElementAmount(count);
            for (int i = 0; i < count; i++)
            {
                CUIListElementScript elemenet = listScript.GetElemenet(i);
                if ((elemenet != null) && listScript.IsElementInScrollArea(i))
                {
                    this._ShowLevelNode(elemenet.get_gameObject(), levelRewardDataMap[i]);
                }
            }
        }
    }

    private void _refresh_list(CUIListScript listScript, ListView<CTask> data_list)
    {
        if (listScript != null)
        {
            int count = data_list.Count;
            listScript.SetElementAmount(count);
            for (int i = 0; i < count; i++)
            {
                CUIListElementScript elemenet = listScript.GetElemenet(i);
                if ((elemenet != null) && listScript.IsElementInScrollArea(i))
                {
                    CTaskShower component = elemenet.GetComponent<CTaskShower>();
                    CTask task = data_list[i];
                    if ((component != null) && (task != null))
                    {
                        component.ShowTask(task, this.m_CUIForm);
                    }
                }
            }
        }
    }

    private void _SetUnlockButton(Button btn, RES_GAME_ENTRANCE_TYPE entryType, bool bEnable)
    {
        RES_SPECIALFUNCUNLOCK_TYPE type = CUICommonSystem.EntryTypeToUnlockType(entryType);
        if (Singleton<CFunctionUnlockSys>.instance.FucIsUnlock(type) && bEnable)
        {
            CUICommonSystem.SetButtonEnableWithShader(btn, true, true);
            btn.GetComponent<CUIEventScript>().m_onClickEventParams.tag = (int) entryType;
        }
        else
        {
            CUICommonSystem.SetButtonEnableWithShader(btn, false, true);
        }
    }

    private void _show_day_box(GameObject box, ushort id)
    {
        Text component = box.get_transform().FindChild("num").GetComponent<Text>();
        Image image = box.get_transform().FindChild("mark").GetComponent<Image>();
        Image image2 = box.get_transform().FindChild("icon").GetComponent<Image>();
        GameObject obj2 = box.get_transform().FindChild("BaoShi").get_gameObject();
        ResHuoYueDuReward reward = null;
        GameDataMgr.huoyueduDict.TryGetValue(id, out reward);
        if (reward != null)
        {
            bool bActive = Singleton<CTaskSys>.instance.model.huoyue_data.BAlready_Reward(RES_HUOYUEDU_TYPE.RES_HUOYUEDU_TYPE_DAY, id);
            component.set_text(reward.dwHuoYueDu.ToString());
            image.get_gameObject().CustomSetActive(bActive);
            if (image2 != null)
            {
                ResDT_HuoYueDuReward_PeriodInfo info = Singleton<CTaskSys>.instance.model.huoyue_data.IsInTime(reward);
                if (info != null)
                {
                    image2.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + info.szIcon, this.m_CUIForm, true, false, false, false);
                }
                else
                {
                    image2.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + reward.szIcon, this.m_CUIForm, true, false, false, false);
                }
            }
            bool flag2 = !bActive && (Singleton<CTaskSys>.instance.model.huoyue_data.day_curNum >= reward.dwHuoYueDu);
            box.get_transform().FindChild("effect").get_gameObject().CustomSetActive(flag2);
            box.GetComponent<Animation>().set_enabled(flag2);
            obj2.CustomSetActive(flag2 || bActive);
        }
    }

    private void _ShowLevelNode(GameObject node, LevelRewardData data)
    {
        if ((node != null) && (data != null))
        {
            int index = Singleton<CTaskSys>.instance.model.GetLevelRewardData_Index(data);
            bool flag = index == (Singleton<CTaskSys>.instance.model.m_levelRewardDataMap.Count - 1);
            GameObject showNode = null;
            GameObject hideNode = null;
            this.GetShowAndHide_LevelNode(index, node, out showNode, out hideNode);
            showNode.CustomSetActive(true);
            hideNode.CustomSetActive(false);
            Text component = showNode.get_transform().Find("Text").GetComponent<Text>();
            if (component != null)
            {
                component.set_text(string.Format("LV.{0}", data.m_level));
            }
            GameObject obj4 = showNode.get_transform().Find("locked").get_gameObject();
            GameObject obj5 = showNode.get_transform().Find("normal").get_gameObject();
            if (Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo().PvpLevel >= data.m_level)
            {
                if (obj4 != null)
                {
                    obj4.CustomSetActive(false);
                }
                if (obj5 != null)
                {
                    obj5.CustomSetActive(true);
                }
            }
            else
            {
                if (obj4 != null)
                {
                    obj4.CustomSetActive(true);
                }
                if (obj5 != null)
                {
                    obj5.CustomSetActive(false);
                }
            }
            GameObject obj6 = showNode.get_transform().Find("curLevel").get_gameObject();
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo();
            bool bActive = (masterRoleInfo != null) && (masterRoleInfo.PvpLevel == data.m_level);
            if (obj6 != null)
            {
                obj6.CustomSetActive(bActive);
            }
            GameObject obj7 = showNode.get_transform().Find("selected").get_gameObject();
            if (Singleton<CTaskSys>.instance.model.curLevelRewardData != null)
            {
                bool flag3 = Singleton<CTaskSys>.instance.model.curLevelRewardData.IsEqual(data);
                obj7.CustomSetActive(flag3);
            }
            else
            {
                obj7.CustomSetActive(false);
            }
            GameObject obj8 = showNode.get_transform().Find("line").get_gameObject();
            if (flag)
            {
                if (obj8 != null)
                {
                    obj8.CustomSetActive(false);
                }
            }
            else if (obj8 != null)
            {
                obj8.CustomSetActive(true);
            }
            if (Singleton<CTaskSys>.instance.model.IsLevelNode_RedDot(data))
            {
                CUICommonSystem.AddRedDot(showNode, enRedDotPos.enTopRight, 0);
            }
            else
            {
                CUICommonSystem.DelRedDot(showNode);
            }
            CUICommonSystem.DelRedDot(hideNode);
        }
    }

    public void AddRedDot(int index, enRedDotPos redDotPos)
    {
        CUIListElementScript elemenet = this.tablistScript.GetElemenet(index);
        if (elemenet != null)
        {
            CUICommonSystem.AddRedDot(elemenet.get_gameObject(), redDotPos, 0);
        }
    }

    public void Bind_Week_Node(GameObject node, ushort week_id)
    {
        CTaskModel model = Singleton<CTaskSys>.instance.model;
        HuoyueData data = model.huoyue_data;
        ResHuoYueDuReward rewardCfg = model.huoyue_data.GetRewardCfg(week_id);
        if (rewardCfg != null)
        {
            Transform transform = node.get_transform().Find("node/box/icon");
            transform.GetComponent<CUIEventScript>().m_onDownEventParams.tagUInt = week_id;
            node.GetComponent<Text>().set_text(rewardCfg.dwHuoYueDu.ToString());
            Image component = transform.GetComponent<Image>();
            ResDT_HuoYueDuReward_PeriodInfo info = Singleton<CTaskSys>.instance.model.huoyue_data.IsInTime(rewardCfg);
            if (info != null)
            {
                component.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + info.szIcon, this.m_CUIForm, true, false, false, false);
            }
            else
            {
                component.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + rewardCfg.szIcon, this.m_CUIForm, true, false, false, false);
            }
            bool bActive = data.BAlready_Reward(RES_HUOYUEDU_TYPE.RES_HUOYUEDU_TYPE_WEEK, week_id);
            node.get_transform().FindChild("node/box/mark").get_gameObject().CustomSetActive(bActive);
            bool flag2 = !bActive && (data.week_curNum >= rewardCfg.dwHuoYueDu);
            node.get_transform().FindChild("node/box/effect").get_gameObject().CustomSetActive(flag2);
            node.get_transform().FindChild("node/box").GetComponent<Animation>().set_enabled(flag2);
        }
    }

    public void Clear()
    {
        this.m_tabIndex = -1;
        if (this.m_container != null)
        {
            this.m_container.RecycleAllElement();
        }
        this.m_container = null;
        this.day_progress_bg = (Image) (this.day_progress = null);
        this.day_huoyue_txt = null;
        this.tasklistScript_main = null;
        this.tasklistScript_usual = null;
        this.week_huoyue_text = null;
        this.week_node1 = null;
        this.week_node2 = null;
        this.tablistScript = null;
        this.m_CUIForm = null;
        this.m_mainTaskNode = null;
        this.m_unlockNode = null;
        this.m_levelRewardNode = null;
        this.m_levelRewardList = null;
        this.m_mainTaskNode = null;
        this.m_unlockNode = null;
        this.m_levelRewardNode = null;
        this.m_levelRewardList = null;
        this.m_normalTaskNode = null;
        this.m_emptyTaskNode = null;
        this.m_taskNode0 = null;
        this.m_taskNode1 = null;
        this.m_unlockInfoTxt = null;
        this.m_levelRewardText = null;
        this.jumpRewardGameObject = null;
        this.m_orgWeekgoldProgress = -0.1f;
        Singleton<CTaskSys>.instance.model.curLevelRewardData = null;
        if (Singleton<CUIManager>.GetInstance().GetForm(CTaskSys.TASK_LevelRewardFORM_PATH) != null)
        {
            Singleton<CUIManager>.GetInstance().CloseForm(CTaskSys.TASK_LevelRewardFORM_PATH);
        }
    }

    public void DelRedDot(int index)
    {
        CUIListElementScript elemenet = this.tablistScript.GetElemenet(index);
        if (elemenet != null)
        {
            CUICommonSystem.DelRedDot(elemenet.get_gameObject());
        }
    }

    private CUIListScript getListScript(int type)
    {
        if ((type != 0) && (type == 1))
        {
            return this.tasklistScript_usual;
        }
        return null;
    }

    private void GetShowAndHide_LevelNode(int index, GameObject node, out GameObject showNode, out GameObject hideNode)
    {
        if ((index % 2) == 0)
        {
            showNode = node.get_transform().Find("left_btn").get_gameObject();
            hideNode = node.get_transform().Find("right_btn").get_gameObject();
        }
        else
        {
            showNode = node.get_transform().Find("right_btn").get_gameObject();
            hideNode = node.get_transform().Find("left_btn").get_gameObject();
        }
    }

    public void MoveElementInScrollArea(int index)
    {
        if (this.m_levelRewardList != null)
        {
            this.m_levelRewardList.MoveElementInScrollArea(index, true);
        }
    }

    public void On_LevelRewardList_ElementEnable(CUIEvent uievent)
    {
        ListView<LevelRewardData> levelRewardDataMap = Singleton<CTaskSys>.instance.model.m_levelRewardDataMap;
        int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
        if (srcWidgetIndexInBelongedList < levelRewardDataMap.Count)
        {
            LevelRewardData data = levelRewardDataMap[srcWidgetIndexInBelongedList];
            if (data != null)
            {
                this._ShowLevelNode(uievent.m_srcWidget.get_gameObject(), data);
            }
        }
    }

    public void On_List_ElementEnable(CUIEvent uievent)
    {
        int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
        CTask task = this._get_current_info((RES_TASK_TYPE) this.m_tabIndex, srcWidgetIndexInBelongedList);
        CTaskShower component = uievent.m_srcWidget.GetComponent<CTaskShower>();
        if ((component != null) && (task != null))
        {
            component.ShowTask(task, this.m_CUIForm);
        }
    }

    public void On_Tab_Change(int index)
    {
        this.TabIndex = index;
    }

    public void OnCloseTaskForm()
    {
        this.Clear();
        Singleton<CUIManager>.GetInstance().CloseForm(CTaskSys.TASK_FORM_PATH);
    }

    public void OnEngineCloseForm()
    {
        this.Clear();
    }

    public void OnTask_JumpToReward(CUIEvent uievent)
    {
    }

    public void OpenForm(CUIEvent uiEvent)
    {
        this.m_CUIForm = Singleton<CUIManager>.GetInstance().OpenForm(CTaskSys.TASK_FORM_PATH, true, true);
        this.tablistScript = this.m_CUIForm.GetWidget(1).GetComponent<CUIListScript>();
        this.tasklistScript_usual = this.m_CUIForm.GetWidget(3).GetComponent<CUIListScript>();
        this.tasklistScript_mishu = this.m_CUIForm.GetWidget(4).GetComponent<CUIListScript>();
        this.tasklistScript_iwantgold = this.m_CUIForm.GetWidget(0x12).GetComponent<CUIListScript>();
        this.m_mainTaskNode = this.m_CUIForm.get_transform().Find("node/list_node_main").get_gameObject();
        this.m_unlockNode = this.m_CUIForm.get_transform().Find("node/list_node_main/unlock_node").get_gameObject();
        this.m_levelRewardNode = this.m_CUIForm.get_transform().Find("node/list_node_main/reward_node").get_gameObject();
        this.m_levelRewardList = this.m_CUIForm.get_transform().Find("node/list_node_main/levelList").GetComponent<CUIListScript>();
        this.m_unlockInfoTxt = this.m_CUIForm.get_transform().Find("node/list_node_main/unlock_node/Text").GetComponent<Text>();
        this.m_levelRewardText = this.m_CUIForm.get_transform().Find("node/list_node_main/reward_node/Text").GetComponent<Text>();
        this.m_normalTaskNode = this.m_CUIForm.get_transform().Find("node/list_node_main/task_node/normal").get_gameObject();
        this.m_emptyTaskNode = this.m_CUIForm.get_transform().Find("node/list_node_main/task_node/noTask").get_gameObject();
        this.m_taskNode0 = this.m_CUIForm.get_transform().Find("node/list_node_main/task_node/normal/task_0").get_gameObject();
        this.m_taskNode1 = this.m_CUIForm.get_transform().Find("node/list_node_main/task_node/normal/task_1").get_gameObject();
        this.jumpRewardGameObject = this.m_CUIForm.get_transform().Find("node/list_node_main/levelList/goto_btn").get_gameObject();
        DebugHelper.Assert(this.m_mainTaskNode != null, "ctaskview m_mainTaskNode == null");
        DebugHelper.Assert(this.m_unlockNode != null, "ctaskview m_unlockNode == null");
        DebugHelper.Assert(this.m_levelRewardNode != null, "ctaskview m_levelRewardNode == null");
        DebugHelper.Assert(this.m_levelRewardList != null, "ctaskview m_levelRewardList == null");
        DebugHelper.Assert(this.m_unlockInfoTxt != null, "ctaskview m_unlockInfoTxt == null");
        DebugHelper.Assert(this.m_levelRewardText != null, "ctaskview m_levelRewardText == null");
        DebugHelper.Assert(this.m_normalTaskNode != null, "ctaskview m_normalTaskNode == null");
        DebugHelper.Assert(this.m_emptyTaskNode != null, "ctaskview m_emptyTaskNode == null");
        DebugHelper.Assert(this.m_taskNode0 != null, "ctaskview m_taskNode0 == null");
        DebugHelper.Assert(this.m_taskNode1 != null, "ctaskview m_taskNode1 == null");
        DebugHelper.Assert(this.m_taskNode1 != null, "ctaskview jumpRewardGameObject == null");
        string[] strArray = null;
        CTaskModel model = Singleton<CTaskSys>.instance.model;
        strArray = new string[] { model.Daily_Quest_Career, model.Daily_Quest_NeedGrowing, model.Daily_Quest_NeedMoney, model.Daily_Quest_NeedSeal, model.Daily_Quest_NeedHero };
        this.tablistScript.SetElementAmount(strArray.Length);
        for (int i = 0; i < this.tablistScript.m_elementAmount; i++)
        {
            this.tablistScript.GetElemenet(i).get_gameObject().get_transform().FindChild("Text").GetComponent<Text>().set_text(strArray[i]);
        }
        this.tablistScript.m_alwaysDispatchSelectedChangeEvent = true;
        this.tablistScript.SelectElement(uiEvent.m_eventParams.tag, true);
        this.tablistScript.m_alwaysDispatchSelectedChangeEvent = false;
        this.week_huoyue_text = this.m_CUIForm.GetWidget(5).GetComponent<Text>();
        this.week_node1 = this.m_CUIForm.GetWidget(6);
        this.week_node2 = this.m_CUIForm.GetWidget(7);
        this.m_container = this.m_CUIForm.GetWidget(8).GetComponent<CUIContainerScript>();
        this.day_progress_bg = this.m_CUIForm.GetWidget(9).GetComponent<Image>();
        this.day_progress = this.m_CUIForm.GetWidget(10).GetComponent<Image>();
        this.day_huoyue_txt = this.m_CUIForm.GetWidget(11).GetComponent<Text>();
        this.Refresh_Tab_RedPoint();
        this._init_day_huoyue();
        this.Refresh_Huoyue();
        CTaskModel model2 = Singleton<CTaskSys>.instance.model;
        if (model2.curLevelRewardData == null)
        {
            uint pvpLevel = Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo().PvpLevel;
            model2.curLevelRewardData = model2.GetLevelRewardData((int) pvpLevel);
        }
    }

    public void Refresh()
    {
        if (this.m_CUIForm != null)
        {
            this.Refresh_Tab_RedPoint();
            this.Refresh_List(this.m_tabIndex);
            this.Refresh_LevelTab();
        }
    }

    public void refresh_Day_HuoYue()
    {
        CTaskModel model = Singleton<CTaskSys>.instance.model;
        this.day_huoyue_txt.set_text(model.huoyue_data.day_curNum.ToString());
        float x = (this.day_progress_bg.get_transform() as RectTransform).get_sizeDelta().x;
        float y = (this.day_progress.get_transform() as RectTransform).get_sizeDelta().y;
        uint num3 = model.huoyue_data.max_dayhuoyue_num;
        float num4 = (((float) model.huoyue_data.day_curNum) / ((float) num3)) * x;
        if (num4 > x)
        {
            num4 = x;
        }
        (this.day_progress.get_transform() as RectTransform).set_sizeDelta(new Vector2(num4, y));
        for (int i = 0; i < model.huoyue_data.day_huoyue_list.Count; i++)
        {
            ushort key = model.huoyue_data.day_huoyue_list[i];
            ResHuoYueDuReward reward = null;
            GameDataMgr.huoyueduDict.TryGetValue(key, out reward);
            if (reward != null)
            {
                GameObject box = this.m_container.get_gameObject().get_transform().FindChild(string.Format("box_{0}", reward.wID)).get_gameObject();
                DebugHelper.Assert(box != null);
                this._show_day_box(box, reward.wID);
            }
        }
    }

    public void Refresh_Huoyue()
    {
        if (this.m_CUIForm != null)
        {
            this.Refresh_Week_Huoyue();
            this.refresh_Day_HuoYue();
        }
    }

    private void Refresh_LevelTab()
    {
        if (this.m_tabIndex == 0)
        {
            this._refresh_Level_list(this.m_levelRewardList, Singleton<CTaskSys>.instance.model.m_levelRewardDataMap.Count);
            this.ShowLevelRightSide(Singleton<CTaskSys>.instance.model.curLevelRewardData);
        }
    }

    public void Refresh_List(int tIndex)
    {
        if (tIndex != 0)
        {
            ListView<CTask> listView = Singleton<CTaskSys>.instance.model.task_Data.GetListView(tIndex);
            CUIListScript listScript = this.getListScript(tIndex);
            if ((listScript != null) && (listView != null))
            {
                listScript.get_transform().get_parent().get_gameObject().CustomSetActive(true);
                this._refresh_list(listScript, listView);
            }
        }
    }

    public void Refresh_Tab_RedPoint()
    {
        this._calc_red_dot(enTaskTab.TAB_MAIN);
        this._calc_red_dot(enTaskTab.TAB_USUAL);
    }

    public void Refresh_Week_Huoyue()
    {
        CTaskModel model = Singleton<CTaskSys>.instance.model;
        this.Bind_Week_Node(this.week_node1, model.huoyue_data.week_reward1);
        this.Bind_Week_Node(this.week_node2, model.huoyue_data.week_reward2);
        this.week_huoyue_text.set_text(model.huoyue_data.week_curNum.ToString());
    }

    public void RefreshLevelList()
    {
        if ((this.m_levelRewardList != null) && (Singleton<CTaskSys>.instance.model.m_levelRewardDataMap != null))
        {
            this._refresh_Level_list(this.m_levelRewardList, Singleton<CTaskSys>.instance.model.m_levelRewardDataMap.Count);
        }
    }

    private void ShowLevelReward(LevelRewardData data, GameObject node)
    {
        if ((data != null) && (node != null))
        {
            GameObject obj2 = node.get_transform().Find("goto_btn").get_gameObject();
            GameObject obj3 = node.get_transform().Find("HasGetReward").get_gameObject();
            DebugHelper.Assert(obj2 != null, "CTaskView ShowLevelReward goto_btn is null, check out...");
            DebugHelper.Assert(obj3 != null, "CTaskView ShowLevelReward has_get is null, check out...");
            if (this.m_levelRewardText != null)
            {
                this.m_levelRewardText.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Task_Award_Text"), data.m_level));
            }
            if (data.GetConfigRewardCount() == 0)
            {
                obj2.CustomSetActive(false);
                obj3.CustomSetActive(false);
            }
            int num2 = 0;
            for (int i = 0; i < LevelRewardData.REWARD_MAX_COUNT; i++)
            {
                ResDT_LevelReward_Info info = data.m_resLevelReward.astRewardInfo[i];
                if (info.dwRewardNum != 0)
                {
                    Transform transform = node.get_transform().Find(string.Format("Ent_{0}", num2));
                    DebugHelper.Assert(transform != null, "CTaskView ShowLevelReward item is null, check out, name:" + "Ent_" + num2);
                    if (transform != null)
                    {
                        transform.get_gameObject().CustomSetActive(true);
                        Image component = transform.Find("icon").GetComponent<Image>();
                        DebugHelper.Assert(component != null, "CTaskView ShowLevelReward icon is null, check out...");
                        if (!string.IsNullOrEmpty(info.szIcon))
                        {
                            component.SetSprite(info.szIcon, this.m_CUIForm, true, false, false, false);
                        }
                        Text text = transform.Find("count").GetComponent<Text>();
                        DebugHelper.Assert(text != null, "CTaskView ShowLevelReward txt1 is null, check out...");
                        if (text != null)
                        {
                            text.set_text("x " + info.dwRewardNum);
                        }
                        Text text2 = transform.Find("name").GetComponent<Text>();
                        DebugHelper.Assert(text2 != null, "CTaskView ShowLevelReward txt2 is null, check out...");
                        if (text2 != null)
                        {
                            text2.set_text(info.szDesc);
                        }
                        num2++;
                    }
                }
            }
            for (int j = num2; j < LevelRewardData.UNLOCK_MAX_COUNT; j++)
            {
                Transform transform2 = node.get_transform().Find(string.Format("Ent_{0}", j));
                if (transform2 != null)
                {
                    transform2.get_gameObject().CustomSetActive(false);
                }
            }
            if (Singleton<CTaskSys>.instance.model.IsGetLevelReward(data.m_level))
            {
                if (obj3 != null)
                {
                    obj3.CustomSetActive(true);
                }
                if (obj2 != null)
                {
                    obj2.CustomSetActive(false);
                }
            }
            else
            {
                if (obj3 != null)
                {
                    obj3.CustomSetActive(false);
                }
                if (Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo().PvpLevel >= data.m_level)
                {
                    if (obj2 != null)
                    {
                        obj2.CustomSetActive(true);
                    }
                }
                else if (obj2 != null)
                {
                    obj2.CustomSetActive(false);
                }
            }
        }
    }

    public void ShowLevelRightSide(LevelRewardData data)
    {
        if (this.m_unlockNode != null)
        {
            this.m_unlockNode.CustomSetActive(data != null);
        }
        if (this.m_levelRewardNode != null)
        {
            this.m_levelRewardNode.CustomSetActive(data != null);
        }
        if (this.m_emptyTaskNode != null)
        {
            this.m_emptyTaskNode.get_transform().get_parent().get_gameObject().CustomSetActive(data != null);
        }
        this.ShowUnLock(data, this.m_unlockNode);
        this.ShowLevelReward(data, this.m_levelRewardNode);
        this.ShowLevelTask(data);
    }

    private void ShowLevelTask(LevelRewardData levelRewardData)
    {
        if (levelRewardData != null)
        {
            bool flag = levelRewardData.IsConfigTaskAllEmpty();
            bool flag2 = levelRewardData.GetValidTaskCount() > 0;
            bool flag3 = levelRewardData.IsAllLevelTask();
            if ((flag || (!flag2 && !levelRewardData.IsHasCltCalcCompeletedTask())) || flag3)
            {
                this.m_emptyTaskNode.CustomSetActive(true);
                this.m_normalTaskNode.CustomSetActive(false);
                Text component = this.m_CUIForm.get_transform().Find("node/list_node_main/task_node/noTask/Text").GetComponent<Text>();
                if (component != null)
                {
                    string text = string.Empty;
                    if (flag)
                    {
                        text = Singleton<CTextManager>.instance.GetText("Task_LevelRewardInfo_None");
                    }
                    if (!flag2 && !levelRewardData.IsHasCltCalcCompeletedTask())
                    {
                        text = Singleton<CTextManager>.instance.GetText("Task_LevelRewardInfo_NoGetTask");
                    }
                    if (flag3)
                    {
                        if (levelRewardData.m_level >= LevelValue)
                        {
                            text = Singleton<CTextManager>.instance.GetText("Task_LevelRewardInfo_Up21");
                        }
                        else
                        {
                            text = Singleton<CTextManager>.instance.GetText("Task_LevelRewardInfo_AllLevelTask");
                        }
                    }
                    component.set_text(text);
                }
            }
            else
            {
                this.m_emptyTaskNode.CustomSetActive(false);
                this.m_normalTaskNode.CustomSetActive(true);
                Text text2 = this.m_CUIForm.get_transform().Find("node/list_node_main/task_node/normal/Text").GetComponent<Text>();
                if (text2 != null)
                {
                    string[] args = new string[] { levelRewardData.m_level.ToString() };
                    text2.set_text(Singleton<CTextManager>.instance.GetText("Task_LevelRewardInfo_Normal", args));
                }
                int num = 0;
                for (int i = 0; i < levelRewardData.task_ids.Length; i++)
                {
                    ResTask task = levelRewardData.task_ids[i];
                    if (task != null)
                    {
                        CTask task2 = Singleton<CTaskSys>.instance.model.GetTask(task.dwTaskID);
                        bool flag4 = (task2 != null) && (task2.m_resTask.bTaskSubType == 0);
                        bool flag5 = task2 != null;
                        bool flag6 = false;
                        if (!flag5 && (task != null))
                        {
                            flag6 = Singleton<CTaskSys>.instance.model.IsInCltCalcCompletedTasks(task.dwTaskID);
                            flag4 = task.bTaskSubType == 0;
                        }
                        GameObject obj2 = null;
                        switch (num)
                        {
                            case 0:
                                obj2 = this.m_taskNode0;
                                break;

                            case 1:
                                obj2 = this.m_taskNode1;
                                break;
                        }
                        if (((obj2 != null) && (flag5 || flag6)) && !flag4)
                        {
                            CTaskShower shower = obj2.GetComponent<CTaskShower>();
                            if (shower == null)
                            {
                                return;
                            }
                            if (flag5)
                            {
                                obj2.CustomSetActive(true);
                                shower.ShowTask(task2, this.m_CUIForm);
                            }
                            else if (flag6)
                            {
                                obj2.CustomSetActive(true);
                                ResTask task3 = levelRewardData.task_ids[i];
                                if (task3 != null)
                                {
                                    shower.ShowTask(task3, this.m_CUIForm);
                                }
                            }
                            num++;
                        }
                    }
                }
                for (int j = num; j < LevelRewardData.TASK_MAX_COUNT; j++)
                {
                    GameObject obj3 = null;
                    switch (j)
                    {
                        case 0:
                            obj3 = this.m_taskNode0;
                            break;

                        case 1:
                            obj3 = this.m_taskNode1;
                            break;
                    }
                    if (obj3 != null)
                    {
                        obj3.CustomSetActive(false);
                    }
                }
            }
        }
    }

    public void ShowSelectedGameobject(GameObject node, int level, bool bShowSelected = true)
    {
        if (node != null)
        {
            GameObject obj2 = node.get_transform().Find("selected").get_gameObject();
            if (obj2 != null)
            {
                obj2.CustomSetActive(bShowSelected);
            }
        }
    }

    private void ShowUnLock(LevelRewardData data, GameObject node)
    {
        if ((data != null) && (node != null))
        {
            if (this.m_unlockInfoTxt != null)
            {
                this.m_unlockInfoTxt.set_text(string.Format(Singleton<CTextManager>.instance.GetText("Task_Unlock_Text"), data.m_level));
            }
            int num = 0;
            for (int i = 0; i < LevelRewardData.UNLOCK_MAX_COUNT; i++)
            {
                ResDT_LevelReward_UnlockInfo info = data.astLockInfo[i];
                if ((info != null) && !string.IsNullOrEmpty(info.szUnlockID))
                {
                    Transform transform = node.get_transform().Find(string.Format("Ent_{0}", num));
                    DebugHelper.Assert(transform != null, "CTaskView ShowUnLock item is null, check out, name:" + "Ent_" + num);
                    if (transform != null)
                    {
                        transform.get_gameObject().CustomSetActive(true);
                        Image component = transform.Find("icon").GetComponent<Image>();
                        DebugHelper.Assert(component != null, "CTaskView ShowUnLock icon is null, check out...");
                        component.SetSprite(info.szIcon, this.m_CUIForm, true, false, false, false);
                        Text text = transform.Find("name").GetComponent<Text>();
                        DebugHelper.Assert(text != null, "CTaskView ShowUnLock txt is null, check out...");
                        if (text != null)
                        {
                            text.set_text(info.szUnlockID);
                        }
                        Button btn = transform.Find("goto_btn").GetComponent<Button>();
                        this._SetUnlockButton(btn, (RES_GAME_ENTRANCE_TYPE) info.bGotoID, Singleton<CRoleInfoManager>.instance.GetMasterRoleInfo().PvpLevel >= data.m_level);
                        num++;
                    }
                }
            }
            for (int j = num; j < LevelRewardData.UNLOCK_MAX_COUNT; j++)
            {
                Transform transform2 = node.get_transform().Find(string.Format("Ent_{0}", j));
                if (transform2 != null)
                {
                    transform2.get_gameObject().CustomSetActive(false);
                }
            }
        }
    }

    public int TabIndex
    {
        get
        {
            return this.m_tabIndex;
        }
        set
        {
            if (this.m_tabIndex != value)
            {
                this.m_tabIndex = value;
                Singleton<CTaskSys>.instance.Increse(this.m_tabIndex);
                switch (this.m_tabIndex)
                {
                    case 0:
                        if (this.m_mainTaskNode != null)
                        {
                            this.m_mainTaskNode.CustomSetActive(true);
                        }
                        if (this.tasklistScript_usual != null)
                        {
                            this.tasklistScript_usual.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.tasklistScript_mishu != null)
                        {
                            this.tasklistScript_mishu.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.tasklistScript_iwantgold != null)
                        {
                            this.tasklistScript_iwantgold.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        break;

                    case 1:
                        if (this.tasklistScript_usual != null)
                        {
                            this.tasklistScript_usual.get_transform().get_parent().get_gameObject().CustomSetActive(true);
                        }
                        if (this.tasklistScript_mishu != null)
                        {
                            this.tasklistScript_mishu.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.m_mainTaskNode != null)
                        {
                            this.m_mainTaskNode.CustomSetActive(false);
                        }
                        if (this.tasklistScript_iwantgold != null)
                        {
                            this.tasklistScript_iwantgold.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        this.Refresh_List(this.m_tabIndex);
                        break;

                    case 2:
                        if (this.m_mainTaskNode != null)
                        {
                            this.m_mainTaskNode.CustomSetActive(false);
                        }
                        if (this.tasklistScript_usual != null)
                        {
                            this.tasklistScript_usual.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.tasklistScript_mishu != null)
                        {
                            this.tasklistScript_mishu.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.tasklistScript_iwantgold != null)
                        {
                            this.tasklistScript_iwantgold.get_transform().get_parent().get_gameObject().CustomSetActive(true);
                            Singleton<CMiShuSystem>.instance.InitList(this.m_tabIndex, this.tasklistScript_iwantgold);
                            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
                            if (masterRoleInfo != null)
                            {
                                string[] args = new string[] { masterRoleInfo.m_goldWeekCur.ToString(), masterRoleInfo.m_goldWeekLimit.ToString() };
                                this.m_CUIForm.GetWidget(0x11).GetComponent<Text>().set_text(Singleton<CTextManager>.GetInstance().GetText("GoldLimited_1", args));
                                Image component = this.m_CUIForm.GetWidget(0x10).GetComponent<Image>();
                                if (this.m_orgWeekgoldProgress < 0f)
                                {
                                    this.m_orgWeekgoldProgress = (component.get_transform() as RectTransform).get_sizeDelta().x;
                                }
                                float orgWeekgoldProgress = this.m_orgWeekgoldProgress;
                                float y = (component.get_transform() as RectTransform).get_sizeDelta().y;
                                uint goldWeekLimit = masterRoleInfo.m_goldWeekLimit;
                                float num4 = (((float) masterRoleInfo.m_goldWeekCur) / ((float) masterRoleInfo.m_goldWeekLimit)) * orgWeekgoldProgress;
                                if (num4 > orgWeekgoldProgress)
                                {
                                    num4 = orgWeekgoldProgress;
                                }
                                (component.get_transform() as RectTransform).set_sizeDelta(new Vector2(num4, y));
                            }
                        }
                        break;

                    default:
                        if (this.m_mainTaskNode != null)
                        {
                            this.m_mainTaskNode.CustomSetActive(false);
                        }
                        if (this.tasklistScript_usual != null)
                        {
                            this.tasklistScript_usual.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.tasklistScript_iwantgold != null)
                        {
                            this.tasklistScript_iwantgold.get_transform().get_parent().get_gameObject().CustomSetActive(false);
                        }
                        if (this.tasklistScript_mishu != null)
                        {
                            this.tasklistScript_mishu.get_transform().get_parent().get_gameObject().CustomSetActive(true);
                            Singleton<CMiShuSystem>.instance.InitList(this.m_tabIndex, this.tasklistScript_mishu);
                        }
                        break;
                }
            }
        }
    }

    public class CTaskUT
    {
        public static void ShowTaskAward(CUIFormScript formScript, ResTask task, GameObject awardContainer)
        {
            if (((formScript != null) && (awardContainer != null)) && ((task != null) && (task.dwTaskID != 0)))
            {
                ResTaskReward dataByKey = GameDataMgr.taskRewardDatabin.GetDataByKey(task.dwRewardID);
                if (dataByKey != null)
                {
                    for (int i = 0; i < dataByKey.astRewardInfo.Length; i++)
                    {
                        ResTaskRewardDetail detail = dataByKey.astRewardInfo[i];
                        Transform transform = awardContainer.get_transform().FindChild(string.Format("itemCell{0}", i));
                        if (transform != null)
                        {
                            GameObject itemCell = transform.get_gameObject();
                            if (itemCell != null)
                            {
                                if ((detail != null) && (detail.iCnt > 0))
                                {
                                    CUseable itemUseable = CUseableManager.CreateUsableByServerType((RES_REWARDS_TYPE) detail.dwRewardType, detail.iCnt, detail.dwRewardID);
                                    CUICommonSystem.SetItemCell(formScript, itemCell, itemUseable, true, false, false, false);
                                    itemCell.get_transform().FindChild("lblIconCount").GetComponent<Text>().set_text(string.Format("x{0}", detail.iCnt.ToString()));
                                    itemCell.get_gameObject().CustomSetActive(true);
                                }
                                else
                                {
                                    itemCell.CustomSetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ShowTaskAward(CUIFormScript formScript, CTask task, GameObject awardContainer, int awardItemcount = 2)
        {
            if (((formScript != null) && (awardContainer != null)) && (task.m_baseId != 0))
            {
                ResTaskReward resAward = task.resAward;
                if (resAward != null)
                {
                    for (int i = 0; i < awardItemcount; i++)
                    {
                        ResTaskRewardDetail detail = resAward.astRewardInfo[i];
                        GameObject itemCell = awardContainer.GetComponent<Transform>().FindChild(string.Format("itemCell{0}", i)).get_gameObject();
                        if ((detail != null) && (detail.iCnt > 0))
                        {
                            CUseable itemUseable = CUseableManager.CreateUsableByServerType((RES_REWARDS_TYPE) detail.dwRewardType, detail.iCnt, detail.dwRewardID);
                            CUICommonSystem.SetItemCell(formScript, itemCell, itemUseable, true, false, false, false);
                            itemCell.get_transform().FindChild("lblIconCount").GetComponent<Text>().set_text(string.Format("x{0}", detail.iCnt.ToString()));
                            itemCell.get_gameObject().CustomSetActive(true);
                        }
                        else
                        {
                            itemCell.CustomSetActive(false);
                        }
                    }
                }
            }
        }
    }

    public enum enTaskFormWidget
    {
        day_huoyue_txt = 11,
        day_progress = 10,
        day_progress_bg = 9,
        getReward_btn = 13,
        iwantgold_taskList = 15,
        iwantgold_text = 0x11,
        iwantgold_week_progress = 0x10,
        m_container = 8,
        None = -1,
        Reserve = 0,
        tablistScript = 1,
        task_Node = 14,
        tasklistScript_iwantgold = 0x12,
        tasklistScript_main = 2,
        tasklistScript_mishu = 4,
        tasklistScript_usual = 3,
        unlock_node = 12,
        week_huoyue_text = 5,
        week_node1 = 6,
        week_node2 = 7
    }

    public enum LevelRewardTaskWidget
    {
        Entity_0 = 1,
        Entity_1 = 2,
        Entity_2 = 3,
        None = -1,
        Reserve = 0
    }
}

