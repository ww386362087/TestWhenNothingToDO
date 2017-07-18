﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.GameSystem;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class CTreasureHud
    {
        private Image icon;
        private Text label;
        private int m_DropNum;
        private GameObject node;
        private GameObject Num;

        public void Clear()
        {
            LeanTween.cancelAll(false);
            this.node = null;
            this.icon = null;
            this.label = null;
            this.Num = null;
            Singleton<EventRouter>.instance.RemoveEventHandler(EventID.DropTreasure, new Action(this, (IntPtr) this.onGetTreasure));
        }

        public void Hide()
        {
            if (this.node != null)
            {
                this.node.CustomSetActive(false);
            }
        }

        public void Init(GameObject obj)
        {
            this.node = obj;
            this.Hide();
            this.icon = Utility.GetComponetInChild<Image>(this.node, "Treasure/Icon");
            this.label = Utility.GetComponetInChild<Text>(this.node, "Treasure/Text");
            this.Num = Utility.FindChildByName(this.node, "TreasureNum");
            Singleton<EventRouter>.instance.AddEventHandler(EventID.DropTreasure, new Action(this, (IntPtr) this.onGetTreasure));
            UT.If_Null_Error<GameObject>(this.node);
            UT.If_Null_Error<Image>(this.icon);
            UT.If_Null_Error<Text>(this.label);
            UT.If_Null_Error<GameObject>(this.Num);
        }

        public void onGetTreasure()
        {
            this.Show();
            this.m_DropNum = Singleton<TreasureChestMgr>.instance.droppedCount;
            if (this.Num != null)
            {
                Utility.GetComponetInChild<Text>(this.Num, "TxtNum").set_text(this.m_DropNum.ToString());
            }
            Singleton<CSoundManager>.instance.PlayBattleSound2D("UI_Prompt_get_box");
        }

        public void Show()
        {
            if (this.node != null)
            {
                this.node.CustomSetActive(true);
            }
        }
    }
}

