﻿namespace Assets.Scripts.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CUIJoystickScript : CUIComponent, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
    {
        private Vector2 m_axis;
        private Vector2 m_axisCurrentScreenPosition;
        public float m_axisFadeoutAlpha = 0.6f;
        private Image m_axisImage;
        private Vector2 m_axisOriginalScreenPosition;
        private RectTransform m_axisRectTransform;
        public Vector2 m_axisScreenPositionOffsetMax;
        public Vector2 m_axisScreenPositionOffsetMin;
        private Vector2 m_axisTargetScreenPosition;
        private CanvasGroup m_borderCanvasGroup;
        private RectTransform m_borderRectTransform;
        public float m_cursorDisplayMaxRadius = 128f;
        private Image m_cursorImage;
        private RectTransform m_cursorRectTransform;
        public float m_cursorRespondMinRadius = 15f;
        public bool m_isAxisMoveable = true;
        [HideInInspector]
        public enUIEventID m_onAxisChangedEventID;
        public stUIEventParams m_onAxisChangedEventParams;
        [HideInInspector]
        public enUIEventID m_onAxisDownEventID;
        public stUIEventParams m_onAxisDownEventParams;
        [HideInInspector]
        public enUIEventID m_onAxisReleasedEventID;
        public stUIEventParams m_onAxisReleasedEventParams;

        private void AxisFadeIn()
        {
            if (this.m_axisImage != null)
            {
                this.m_axisImage.set_color(new Color(1f, 1f, 1f, 1f));
            }
            if (this.m_cursorImage != null)
            {
                this.m_cursorImage.set_color(new Color(1f, 1f, 1f, 1f));
            }
        }

        private void AxisFadeout()
        {
            if (this.m_axisImage != null)
            {
                this.m_axisImage.set_color(new Color(1f, 1f, 1f, this.m_axisFadeoutAlpha));
            }
            if (this.m_cursorImage != null)
            {
                this.m_cursorImage.set_color(new Color(1f, 1f, 1f, this.m_axisFadeoutAlpha));
            }
        }

        private void DispatchOnAxisChangedEvent()
        {
            if (this.m_onAxisChangedEventID != enUIEventID.None)
            {
                CUIEvent uIEvent = Singleton<CUIEventManager>.GetInstance().GetUIEvent();
                uIEvent.m_eventID = this.m_onAxisChangedEventID;
                uIEvent.m_eventParams = this.m_onAxisChangedEventParams;
                uIEvent.m_srcFormScript = base.m_belongedFormScript;
                uIEvent.m_srcWidgetBelongedListScript = base.m_belongedListScript;
                uIEvent.m_srcWidgetIndexInBelongedList = base.m_indexInlist;
                uIEvent.m_srcWidget = base.get_gameObject();
                uIEvent.m_srcWidgetScript = this;
                uIEvent.m_pointerEventData = null;
                base.DispatchUIEvent(uIEvent);
            }
        }

        private void DispatchOnAxisDownEvent()
        {
            if (this.m_onAxisDownEventID != enUIEventID.None)
            {
                CUIEvent uIEvent = Singleton<CUIEventManager>.GetInstance().GetUIEvent();
                uIEvent.m_eventID = this.m_onAxisDownEventID;
                uIEvent.m_eventParams = this.m_onAxisDownEventParams;
                uIEvent.m_srcFormScript = base.m_belongedFormScript;
                uIEvent.m_srcWidgetBelongedListScript = base.m_belongedListScript;
                uIEvent.m_srcWidgetIndexInBelongedList = base.m_indexInlist;
                uIEvent.m_srcWidget = base.get_gameObject();
                uIEvent.m_srcWidgetScript = this;
                uIEvent.m_pointerEventData = null;
                base.DispatchUIEvent(uIEvent);
            }
        }

        private void DispatchOnAxisReleasedEvent()
        {
            if (this.m_onAxisReleasedEventID != enUIEventID.None)
            {
                CUIEvent uIEvent = Singleton<CUIEventManager>.GetInstance().GetUIEvent();
                uIEvent.m_eventID = this.m_onAxisReleasedEventID;
                uIEvent.m_eventParams = this.m_onAxisReleasedEventParams;
                uIEvent.m_srcFormScript = base.m_belongedFormScript;
                uIEvent.m_srcWidgetBelongedListScript = base.m_belongedListScript;
                uIEvent.m_srcWidgetIndexInBelongedList = base.m_indexInlist;
                uIEvent.m_srcWidget = base.get_gameObject();
                uIEvent.m_srcWidgetScript = this;
                uIEvent.m_pointerEventData = null;
                base.DispatchUIEvent(uIEvent);
            }
        }

        public Vector2 GetAxis()
        {
            return this.m_axis;
        }

        private Vector2 GetFixAixsScreenPosition(Vector2 axisScreenPosition)
        {
            axisScreenPosition.x = CUIUtility.ValueInRange(axisScreenPosition.x, this.m_axisOriginalScreenPosition.x + base.m_belongedFormScript.ChangeFormValueToScreen(this.m_axisScreenPositionOffsetMin.x), this.m_axisOriginalScreenPosition.x + base.m_belongedFormScript.ChangeFormValueToScreen(this.m_axisScreenPositionOffsetMax.x));
            axisScreenPosition.y = CUIUtility.ValueInRange(axisScreenPosition.y, this.m_axisOriginalScreenPosition.y + base.m_belongedFormScript.ChangeFormValueToScreen(this.m_axisScreenPositionOffsetMin.y), this.m_axisOriginalScreenPosition.y + base.m_belongedFormScript.ChangeFormValueToScreen(this.m_axisScreenPositionOffsetMax.y));
            return axisScreenPosition;
        }

        private void HideBorder()
        {
            if ((this.m_borderRectTransform != null) && (this.m_borderCanvasGroup != null))
            {
                if (this.m_borderCanvasGroup.get_alpha() != 0f)
                {
                    this.m_borderCanvasGroup.set_alpha(0f);
                }
                if (this.m_borderCanvasGroup.get_blocksRaycasts())
                {
                    this.m_borderCanvasGroup.set_blocksRaycasts(false);
                }
            }
        }

        public override void Initialize(CUIFormScript formScript)
        {
            if (!base.m_isInitialized)
            {
                base.Initialize(formScript);
                this.m_axisRectTransform = base.get_gameObject().get_transform().FindChild("Axis") as RectTransform;
                if (this.m_axisRectTransform != null)
                {
                    this.m_axisRectTransform.set_anchoredPosition(Vector2.get_zero());
                    this.m_axisOriginalScreenPosition = CUIUtility.WorldToScreenPoint(base.m_belongedFormScript.GetCamera(), this.m_axisRectTransform.get_position());
                    this.m_axisImage = this.m_axisRectTransform.get_gameObject().GetComponent<Image>();
                    this.m_cursorRectTransform = this.m_axisRectTransform.FindChild("Cursor") as RectTransform;
                    if (this.m_cursorRectTransform != null)
                    {
                        this.m_cursorRectTransform.set_anchoredPosition(Vector2.get_zero());
                        this.m_cursorImage = this.m_cursorRectTransform.get_gameObject().GetComponent<Image>();
                    }
                    this.AxisFadeout();
                }
                this.m_borderRectTransform = base.get_gameObject().get_transform().FindChild("Axis/Border") as RectTransform;
                if (this.m_borderRectTransform != null)
                {
                    this.m_borderCanvasGroup = this.m_borderRectTransform.get_gameObject().GetComponent<CanvasGroup>();
                    if (this.m_borderCanvasGroup == null)
                    {
                        this.m_borderCanvasGroup = this.m_borderRectTransform.get_gameObject().AddComponent<CanvasGroup>();
                    }
                    this.HideBorder();
                }
            }
        }

        private void MoveAxis(Vector2 position, bool isDown)
        {
            if (isDown || ((this.m_axisCurrentScreenPosition == Vector2.get_zero()) && (this.m_axisTargetScreenPosition == Vector2.get_zero())))
            {
                this.m_axisCurrentScreenPosition = this.GetFixAixsScreenPosition(position);
                this.m_axisTargetScreenPosition = this.m_axisCurrentScreenPosition;
                DebugHelper.Assert(base.m_belongedFormScript != null);
                this.m_axisRectTransform.set_position(CUIUtility.ScreenToWorldPoint((base.m_belongedFormScript == null) ? null : base.m_belongedFormScript.GetCamera(), this.m_axisCurrentScreenPosition, this.m_axisRectTransform.get_position().z));
            }
            Vector2 axis = position - this.m_axisCurrentScreenPosition;
            Vector2 vector2 = axis;
            float num = axis.get_magnitude();
            float num2 = num;
            if (base.m_belongedFormScript != null)
            {
                num2 = base.m_belongedFormScript.ChangeScreenValueToForm(num);
                vector2.x = base.m_belongedFormScript.ChangeScreenValueToForm(axis.x);
                vector2.y = base.m_belongedFormScript.ChangeScreenValueToForm(axis.y);
            }
            DebugHelper.Assert(this.m_cursorRectTransform != null);
            this.m_cursorRectTransform.set_anchoredPosition((num2 <= this.m_cursorDisplayMaxRadius) ? vector2 : ((Vector2) (vector2.get_normalized() * this.m_cursorDisplayMaxRadius)));
            if (this.m_isAxisMoveable && (num2 > this.m_cursorDisplayMaxRadius))
            {
                DebugHelper.Assert(base.m_belongedFormScript != null);
                this.m_axisTargetScreenPosition = this.m_axisCurrentScreenPosition + (position - CUIUtility.WorldToScreenPoint((base.m_belongedFormScript == null) ? null : base.m_belongedFormScript.GetCamera(), this.m_cursorRectTransform.get_position()));
                this.m_axisTargetScreenPosition = this.GetFixAixsScreenPosition(this.m_axisTargetScreenPosition);
            }
            if (num2 < this.m_cursorRespondMinRadius)
            {
                this.UpdateAxis(Vector2.get_zero());
            }
            else
            {
                this.UpdateAxis(axis);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.MoveAxis(eventData.get_position(), false);
        }

        protected override void OnDestroy()
        {
            this.m_axisRectTransform = null;
            this.m_cursorRectTransform = null;
            this.m_axisImage = null;
            this.m_cursorImage = null;
            this.m_borderRectTransform = null;
            this.m_borderCanvasGroup = null;
            base.OnDestroy();
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.MoveAxis(eventData.get_position(), false);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.DispatchOnAxisDownEvent();
            this.MoveAxis(eventData.get_position(), true);
            this.AxisFadeIn();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.ResetAxis();
            this.DispatchOnAxisReleasedEvent();
        }

        public void ResetAxis()
        {
            this.m_axisRectTransform.set_anchoredPosition(Vector2.get_zero());
            this.m_cursorRectTransform.set_anchoredPosition(Vector2.get_zero());
            this.m_axisOriginalScreenPosition = CUIUtility.WorldToScreenPoint(base.m_belongedFormScript.GetCamera(), this.m_axisRectTransform.get_position());
            this.m_axisCurrentScreenPosition = Vector2.get_zero();
            this.m_axisTargetScreenPosition = Vector2.get_zero();
            this.UpdateAxis(Vector2.get_zero());
            this.AxisFadeout();
        }

        private void ShowBorder(Vector2 axis)
        {
            if ((this.m_borderRectTransform != null) && (this.m_borderCanvasGroup != null))
            {
                if (this.m_borderCanvasGroup.get_alpha() != 1f)
                {
                    this.m_borderCanvasGroup.set_alpha(1f);
                }
                if (!this.m_borderCanvasGroup.get_blocksRaycasts())
                {
                    this.m_borderCanvasGroup.set_blocksRaycasts(true);
                }
                this.m_borderRectTransform.set_right(axis);
            }
        }

        private void Update()
        {
            if (((base.m_belongedFormScript == null) || !base.m_belongedFormScript.IsClosed()) && this.m_isAxisMoveable)
            {
                this.UpdateAxisPosition();
            }
        }

        private void UpdateAxis(Vector2 axis)
        {
            if (this.m_axis != axis)
            {
                this.m_axis = axis;
                this.DispatchOnAxisChangedEvent();
            }
            if (this.m_axis == Vector2.get_zero())
            {
                this.HideBorder();
            }
            else
            {
                this.ShowBorder(this.m_axis);
            }
        }

        private void UpdateAxisPosition()
        {
            if (this.m_axisCurrentScreenPosition != this.m_axisTargetScreenPosition)
            {
                Vector2 vector = this.m_axisTargetScreenPosition - this.m_axisCurrentScreenPosition;
                Vector2 vector2 = (Vector2) ((this.m_axisTargetScreenPosition - this.m_axisCurrentScreenPosition) / 3f);
                if (vector.get_sqrMagnitude() <= 1f)
                {
                    this.m_axisCurrentScreenPosition = this.m_axisTargetScreenPosition;
                }
                else
                {
                    this.m_axisCurrentScreenPosition += vector2;
                }
                this.m_axisRectTransform.set_position(CUIUtility.ScreenToWorldPoint(base.m_belongedFormScript.GetCamera(), this.m_axisCurrentScreenPosition, this.m_axisRectTransform.get_position().z));
            }
        }
    }
}

