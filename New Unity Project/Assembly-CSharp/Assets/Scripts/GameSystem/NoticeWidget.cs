﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.UI;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class NoticeWidget : ActivityWidget
    {
        private Text _datePeriod;
        private Text _descContent;
        private GameObject _jumpBtn;
        private Text _jumpBtnLabel;
        private ScrollRect _scrollRect;

        public NoticeWidget(GameObject node, ActivityView view) : base(node, view)
        {
            this._datePeriod = Utility.GetComponetInChild<Text>(node, "DatePeriod");
            this._scrollRect = Utility.GetComponetInChild<ScrollRect>(node, "ScrollRect");
            this._descContent = Utility.GetComponetInChild<Text>(node, "ScrollRect/DescContent");
            this._jumpBtn = Utility.FindChild(node, "JumpBtn");
            this._jumpBtnLabel = Utility.GetComponetInChild<Text>(this._jumpBtn, "Text");
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Activity_NoticeJump, new CUIEventManager.OnUIEventHandler(this.OnClickJump));
            this.Validate();
        }

        public override void Clear()
        {
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Activity_NoticeJump, new CUIEventManager.OnUIEventHandler(this.OnClickJump));
        }

        private void OnClickJump(CUIEvent evt)
        {
            NoticeActivity activity = base.view.activity as NoticeActivity;
            if (activity != null)
            {
                base.view.form.Close();
                activity.Jump();
            }
        }

        public override void OnShow()
        {
            this._scrollRect.set_verticalNormalizedPosition(1f);
        }

        public override void Validate()
        {
            this._datePeriod.set_text(base.view.activity.PeriodText);
            this._descContent.set_text(base.view.activity.Content);
            NoticeActivity activity = base.view.activity as NoticeActivity;
            if (activity != null)
            {
                if (activity.timeState == Activity.TimeState.Going)
                {
                    string jumpLabel = activity.JumpLabel;
                    if (string.IsNullOrEmpty(jumpLabel))
                    {
                        this._jumpBtn.CustomSetActive(false);
                    }
                    else
                    {
                        this._jumpBtn.CustomSetActive(true);
                        this._jumpBtnLabel.set_text(jumpLabel);
                    }
                }
                else
                {
                    this._jumpBtn.CustomSetActive(false);
                }
            }
        }
    }
}

