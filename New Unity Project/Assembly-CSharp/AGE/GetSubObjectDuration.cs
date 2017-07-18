﻿namespace AGE
{
    using Assets.Scripts.Common;
    using System;
    using UnityEngine;

    [EventCategory("Utility")]
    public class GetSubObjectDuration : DurationEvent
    {
        public bool isGetByName;
        [ObjectTemplate(new Type[] {  })]
        public int parentId = -1;
        public string subObjectName = string.Empty;
        [ObjectTemplate(true)]
        public int targetId = -1;

        public override BaseEvent Clone()
        {
            GetSubObjectDuration duration = ClassObjPool<GetSubObjectDuration>.Get();
            duration.CopyData(this);
            return duration;
        }

        protected override void CopyData(BaseEvent src)
        {
            base.CopyData(src);
            GetSubObjectDuration duration = src as GetSubObjectDuration;
            this.targetId = duration.targetId;
            this.parentId = duration.parentId;
            this.isGetByName = duration.isGetByName;
            this.subObjectName = duration.subObjectName;
        }

        public override void Enter(Action _action, Track _track)
        {
            GameObject gameObject = _action.GetGameObject(this.parentId);
            if ((gameObject != null) && (this.targetId >= 0))
            {
                _action.ExpandGameObject(this.targetId);
                GameObject go = _action.GetGameObject(this.targetId);
                if (go == null)
                {
                    if (this.isGetByName)
                    {
                        Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < componentsInChildren.Length; i++)
                        {
                            if (componentsInChildren[i].get_gameObject().get_name() == this.subObjectName)
                            {
                                go = componentsInChildren[i].get_gameObject();
                                break;
                            }
                        }
                    }
                    else if (gameObject.get_transform().get_childCount() > 0)
                    {
                        go = gameObject.get_transform().GetChild(0).get_gameObject();
                    }
                    _action.SetGameObject(this.targetId, go);
                }
            }
        }

        public override void Leave(Action _action, Track _track)
        {
            if ((this.targetId >= 0) && (_action.GetGameObject(this.targetId) != null))
            {
                _action.SetGameObject(this.targetId, null);
            }
        }

        public override void OnUse()
        {
            base.OnUse();
            this.targetId = -1;
            this.parentId = -1;
            this.isGetByName = false;
            this.subObjectName = string.Empty;
        }

        public override bool SupportEditMode()
        {
            return true;
        }
    }
}

