﻿namespace AGE
{
    using Assets.Scripts.Common;
    using System;
    using UnityEngine;

    public class GetSubObjectTick : TickEvent
    {
        public bool isGetByName;
        [ObjectTemplate(new Type[] {  })]
        public int parentId = -1;
        public string subObjectName = "Mesh";
        [ObjectTemplate(new Type[] {  })]
        public int targetId = -1;

        public override BaseEvent Clone()
        {
            GetSubObjectTick tick = ClassObjPool<GetSubObjectTick>.Get();
            tick.CopyData(this);
            return tick;
        }

        protected override void CopyData(BaseEvent src)
        {
            base.CopyData(src);
            GetSubObjectTick tick = src as GetSubObjectTick;
            this.targetId = tick.targetId;
            this.parentId = tick.parentId;
            this.isGetByName = tick.isGetByName;
            this.subObjectName = tick.subObjectName;
        }

        public override void OnUse()
        {
            base.OnUse();
            this.targetId = -1;
            this.parentId = -1;
            this.isGetByName = false;
            this.subObjectName = "Mesh";
        }

        public override void Process(Action _action, Track _track)
        {
            GameObject gameObject = _action.GetGameObject(this.parentId);
            if (gameObject != null)
            {
                GameObject go = _action.GetGameObject(this.targetId);
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
}

