﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic;
    using Assets.Scripts.UI;
    using System;
    using UnityEngine;

    public class BattleTaskView
    {
        private GameTask _curTask;
        private GameObject _root;
        private int _timer;

        public void Clear()
        {
            Singleton<CTimerManager>.instance.RemoveTimerSafely(ref this._timer);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskReady -= new GameTaskSys.TaskEventDelegate(this.onBattleTaskStart);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskStart -= new GameTaskSys.TaskEventDelegate(this.onBattleTaskStart);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskGoing -= new GameTaskSys.TaskEventDelegate(this.onBattleTaskGoing);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskClose -= new GameTaskSys.TaskEventDelegate(this.onBattleTaskClose);
        }

        public void Init(GameObject obj)
        {
            this._root = obj;
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskReady += new GameTaskSys.TaskEventDelegate(this.onBattleTaskStart);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskStart += new GameTaskSys.TaskEventDelegate(this.onBattleTaskStart);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskGoing += new GameTaskSys.TaskEventDelegate(this.onBattleTaskGoing);
            Singleton<BattleLogic>.instance.battleTaskSys.OnTaskClose += new GameTaskSys.TaskEventDelegate(this.onBattleTaskClose);
        }

        private bool IsHostTask(GameTask gt)
        {
            SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
            int playerCamp = (int) Singleton<GamePlayerCenter>.instance.GetHostPlayer().PlayerCamp;
            return ((((curLvelContext != null) && (playerCamp >= 0)) && (playerCamp < curLvelContext.m_battleTaskOfCamps.Length)) && (gt.ID == curLvelContext.m_battleTaskOfCamps[playerCamp]));
        }

        private void onBattleTaskClose(GameTask gt)
        {
            if (!this.IsHostTask(gt))
            {
            }
        }

        private void onBattleTaskGoing(GameTask gt)
        {
            if (this.IsHostTask(gt))
            {
                this.TrackTask(gt);
            }
        }

        private void onBattleTaskStart(GameTask gt)
        {
            if (this.IsHostTask(gt))
            {
                this.TrackTask(gt);
                if (this._timer == 0)
                {
                    this._timer = Singleton<CTimerManager>.instance.AddTimer(0x3e8, -1, new CTimer.OnTimeUpHandler(this.onTimerUpdate));
                }
            }
        }

        private void onTimerUpdate(int timer)
        {
            this.UpdateView();
        }

        private void TrackTask(GameTask gt)
        {
            this._curTask = gt;
            while ((this._curTask != null) && this._curTask.IsGroup)
            {
                this._curTask = ((GameTaskGroup) this._curTask).ActiveChild;
            }
            if (this._curTask == null)
            {
                this._curTask = gt;
            }
            this.UpdateView();
        }

        private void UpdateView()
        {
            if ((this._curTask != null) && this.IsHostTask(this._curTask))
            {
                try
                {
                    int num = Mathf.RoundToInt(this._curTask.TimeRemain * 0.001f);
                    Utility.GetComponetInChild<Text>(this._root, "Time").set_text(string.Format("{0:D2}", num / 60) + ":" + string.Format("{0:D2}", num % 60));
                    Utility.GetComponetInChild<Text>(this._root, "Name").set_text(this._curTask.Name);
                    Utility.GetComponetInChild<Image>(this._root, "Progress/Fore").CustomFillAmount(this._curTask.Progress);
                }
                catch (Exception)
                {
                    Singleton<CTimerManager>.instance.RemoveTimerSafely(ref this._timer);
                }
            }
        }

        public bool Visible
        {
            get
            {
                return this._root.get_activeSelf();
            }
            set
            {
                this._root.CustomSetActive(value);
            }
        }
    }
}

