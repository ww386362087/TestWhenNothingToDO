﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.GameLogic;
    using CSProtocol;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class WatchScoreHud
    {
        private Text _campDragonText_1;
        private Text _campDragonText_2;
        private Text _campMoneyText_1;
        private Text _campMoneyText_2;
        private Text _campScoreText_1;
        private Text _campScoreText_2;
        private Text _campTowerText_1;
        private Text _campTowerText_2;
        private int _lastTime;
        private GameObject _root;
        private Text _timeText;

        public WatchScoreHud(GameObject root)
        {
            this._root = root;
            this._timeText = Utility.GetComponetInChild<Text>(root, "Time");
            this._campScoreText_1 = Utility.GetComponetInChild<Text>(root, "Camp_1/Score");
            this._campScoreText_2 = Utility.GetComponetInChild<Text>(root, "Camp_2/Score");
            this._campMoneyText_1 = Utility.GetComponetInChild<Text>(root, "Camp_1/Money");
            this._campMoneyText_2 = Utility.GetComponetInChild<Text>(root, "Camp_2/Money");
            this._campTowerText_1 = Utility.GetComponetInChild<Text>(root, "Camp_1/Tower");
            this._campTowerText_2 = Utility.GetComponetInChild<Text>(root, "Camp_2/Tower");
            this._campDragonText_1 = Utility.GetComponetInChild<Text>(root, "Camp_1/Dragon");
            this._campDragonText_2 = Utility.GetComponetInChild<Text>(root, "Camp_2/Dragon");
            this._lastTime = 0;
            this._timeText.set_text(string.Format("{0:D2}:{1:D2}", 0, 0));
            this._campTowerText_1.set_text("0");
            this._campTowerText_2.set_text("0");
            this._campDragonText_1.set_text("0");
            this._campDragonText_2.set_text("0");
            this._campScoreText_1.set_text("0");
            this._campScoreText_2.set_text("0");
            Singleton<EventRouter>.instance.AddEventHandler(EventID.BATTLE_TOWER_DESTROY_CHANGED, new Action(this, (IntPtr) this.OnCampTowerChange));
            Singleton<EventRouter>.instance.AddEventHandler(EventID.BATTLE_DRAGON_KILL_CHANGED, new Action(this, (IntPtr) this.OnCampDragonChange));
            Singleton<GameEventSys>.instance.AddEventHandler<SCampScoreUpdateParam>(GameEventDef.Event_CampScoreUpdated, new RefAction<SCampScoreUpdateParam>(this.OnBattleScoreChange));
        }

        public void Clear()
        {
            Singleton<GameEventSys>.instance.RmvEventHandler<SCampScoreUpdateParam>(GameEventDef.Event_CampScoreUpdated, new RefAction<SCampScoreUpdateParam>(this.OnBattleScoreChange));
            Singleton<EventRouter>.instance.RemoveEventHandler(EventID.BATTLE_TOWER_DESTROY_CHANGED, new Action(this, (IntPtr) this.OnCampTowerChange));
            Singleton<EventRouter>.instance.RemoveEventHandler(EventID.BATTLE_DRAGON_KILL_CHANGED, new Action(this, (IntPtr) this.OnCampDragonChange));
        }

        public void LateUpdate()
        {
            int num = Singleton<BattleLogic>.GetInstance().CalcCurrentTime();
            if (num != this._lastTime)
            {
                this._lastTime = num;
                this._timeText.set_text(string.Format("{0:D2}:{1:D2}", num / 60, num % 60));
            }
        }

        private void OnBattleScoreChange(ref SCampScoreUpdateParam param)
        {
            if (this._root == null)
            {
                this.Clear();
            }
            else if (param.HeadPoints >= 0)
            {
                SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
                if (param.CampType == COM_PLAYERCAMP.COM_PLAYERCAMP_1)
                {
                    if (curLvelContext.m_headPtsUpperLimit > 0)
                    {
                        this._campScoreText_1.set_text(string.Format(Singleton<CTextManager>.GetInstance().GetText("ScoreBoard_FireHole_1"), param.HeadPoints, curLvelContext.m_headPtsUpperLimit));
                    }
                    else
                    {
                        this._campScoreText_1.set_text(string.Format(Singleton<CTextManager>.GetInstance().GetText("ScoreBoard_Normal_1"), param.HeadPoints));
                    }
                }
                else if (param.CampType == COM_PLAYERCAMP.COM_PLAYERCAMP_2)
                {
                    if (curLvelContext.m_headPtsUpperLimit > 0)
                    {
                        this._campScoreText_2.set_text(string.Format(Singleton<CTextManager>.GetInstance().GetText("ScoreBoard_FireHole_2"), param.HeadPoints, curLvelContext.m_headPtsUpperLimit));
                    }
                    else
                    {
                        this._campScoreText_2.set_text(string.Format(Singleton<CTextManager>.GetInstance().GetText("ScoreBoard_Normal_2"), param.HeadPoints));
                    }
                }
            }
        }

        private void OnCampDragonChange()
        {
            if (this._root == null)
            {
                this.Clear();
            }
            else
            {
                int num = 0;
                int num2 = 0;
                DictionaryView<uint, PlayerKDA>.Enumerator enumerator = Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<uint, PlayerKDA> current = enumerator.Current;
                    PlayerKDA rkda = current.Value;
                    if (rkda.PlayerCamp == COM_PLAYERCAMP.COM_PLAYERCAMP_1)
                    {
                        ListView<HeroKDA>.Enumerator enumerator2 = rkda.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            num += enumerator2.Current.numKillDragon + enumerator2.Current.numKillBaron;
                        }
                    }
                    else if (rkda.PlayerCamp == COM_PLAYERCAMP.COM_PLAYERCAMP_2)
                    {
                        ListView<HeroKDA>.Enumerator enumerator3 = rkda.GetEnumerator();
                        while (enumerator3.MoveNext())
                        {
                            num2 += enumerator3.Current.numKillDragon + enumerator3.Current.numKillBaron;
                        }
                    }
                }
                this._campDragonText_1.set_text(num.ToString());
                this._campDragonText_2.set_text(num2.ToString());
            }
        }

        private void OnCampTowerChange()
        {
            if (this._root == null)
            {
                this.Clear();
            }
            else
            {
                this._campTowerText_1.set_text(Singleton<BattleLogic>.GetInstance().battleStat.GetCampInfoByCamp(COM_PLAYERCAMP.COM_PLAYERCAMP_1).destoryTowers.ToString());
                this._campTowerText_2.set_text(Singleton<BattleLogic>.GetInstance().battleStat.GetCampInfoByCamp(COM_PLAYERCAMP.COM_PLAYERCAMP_2).destoryTowers.ToString());
            }
        }

        public void ValidateMoney(int campMoney_1, int campMoney_2)
        {
            this._campMoneyText_1.set_text(string.Format("{0:N1}k", campMoney_1 * 0.001f));
            this._campMoneyText_2.set_text(string.Format("{0:N1}k", campMoney_2 * 0.001f));
        }
    }
}

