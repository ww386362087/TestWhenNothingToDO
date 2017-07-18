﻿namespace AGE
{
    using Assets.Scripts.Common;
    using System;
    using UnityEngine;

    [EventCategory("MMGame/System")]
    public class ChangeActorMeshDuration : DurationCondition
    {
        private GameObject actorMesh;
        [AssetReference(AssetRefType.Prefab)]
        public string prefabName = string.Empty;
        private bool switchFinished;
        [ObjectTemplate(new Type[] {  })]
        public int targetId;

        private void ChangeMesh(ref PoolObjHandle<ActorRoot> srcActor, GameObject newMesh)
        {
            if (newMesh != null)
            {
                Transform transform = newMesh.get_transform();
                transform.SetParent(srcActor.handle.myTransform);
                transform.set_localPosition(Vector3.get_zero());
                transform.set_localRotation(Quaternion.get_identity());
                srcActor.handle.SetActorMesh(newMesh);
            }
        }

        public override bool Check(Action _action, Track _track)
        {
            return this.switchFinished;
        }

        public override BaseEvent Clone()
        {
            ChangeActorMeshDuration duration = ClassObjPool<ChangeActorMeshDuration>.Get();
            duration.CopyData(this);
            return duration;
        }

        protected override void CopyData(BaseEvent src)
        {
            base.CopyData(src);
            ChangeActorMeshDuration duration = src as ChangeActorMeshDuration;
            this.targetId = duration.targetId;
            this.prefabName = duration.prefabName;
            this.switchFinished = duration.switchFinished;
            this.actorMesh = duration.actorMesh;
        }

        public override void Enter(Action _action, Track _track)
        {
            PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.targetId);
            if (actorHandle != 0)
            {
                this.switchFinished = false;
                this.actorMesh = actorHandle.handle.ActorMesh;
                this.actorMesh.CustomSetActive(false);
                GameObject newMesh = MonoSingleton<SceneMgr>.GetInstance().GetPooledGameObjLOD(this.prefabName, false, SceneObjType.ActionRes, Vector3.get_zero());
                this.ChangeMesh(ref actorHandle, newMesh);
                base.Enter(_action, _track);
            }
        }

        public override void Leave(Action _action, Track _track)
        {
            PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.targetId);
            if (actorHandle != 0)
            {
                this.switchFinished = true;
                this.ChangeMesh(ref actorHandle, this.actorMesh);
                this.actorMesh.CustomSetActive(true);
                this.actorMesh = null;
                base.Leave(_action, _track);
            }
        }

        public override void OnUse()
        {
            base.OnUse();
            this.targetId = 0;
            this.prefabName = string.Empty;
            this.switchFinished = false;
            this.actorMesh = null;
        }

        public override void Process(Action _action, Track _track, int _localTime)
        {
            base.Process(_action, _track, _localTime);
        }

        public override bool SupportEditMode()
        {
            return true;
        }
    }
}

