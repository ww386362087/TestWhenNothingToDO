﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Common;
    using ResData;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class AttackModeTargetSearcher : Singleton<AttackModeTargetSearcher>
    {
        private TargetPropertyLessEqualFilter bossFiler = new TargetPropertyLessEqualFilter();
        private List<ActorRoot> bossList = new List<ActorRoot>();
        private PoolObjHandle<ActorRoot> curActorPtr;
        private TargetPropertyLessEqualFilter heroFilter = new TargetPropertyLessEqualFilter();
        private List<ActorRoot> heroList = new List<ActorRoot>();
        private SceneManagement.Process LowestHpHandler;
        private TargetPropertyLessEqualFilter monsterFiler = new TargetPropertyLessEqualFilter();
        private List<ActorRoot> monsterList = new List<ActorRoot>();
        private TargetPropertyLessEqualFilter monsterNotInBattleFiler = new TargetPropertyLessEqualFilter();
        private List<ActorRoot> monsterNotInBattleList = new List<ActorRoot>();
        private SceneManagement.Process NearestHandler;
        private TargetPropertyLessEqualFilter organFiler = new TargetPropertyLessEqualFilter();
        private List<ActorRoot> organList = new List<ActorRoot>();
        private VInt3 searchPosition;
        private int searchRadius;
        private uint searchTypeMask;

        private void Clear()
        {
            this.curActorPtr.Release();
            this.heroList.Clear();
            this.bossList.Clear();
            this.monsterList.Clear();
            this.monsterNotInBattleList.Clear();
            this.organList.Clear();
            this.heroFilter.Initial(this.heroList, ulong.MaxValue);
            this.bossFiler.Initial(this.bossList, ulong.MaxValue);
            this.monsterFiler.Initial(this.monsterList, ulong.MaxValue);
            this.monsterNotInBattleFiler.Initial(this.monsterNotInBattleList, ulong.MaxValue);
            this.organFiler.Initial(this.organList, ulong.MaxValue);
        }

        private void FilterLowestHpActor(ref PoolObjHandle<ActorRoot> _actorPtr)
        {
            if ((_actorPtr.handle.HorizonMarker.IsVisibleFor(this.curActorPtr.handle.TheActorMeta.ActorCamp) && this.curActorPtr.handle.CanAttack((ActorRoot) _actorPtr)) && ((this.curActorPtr != _actorPtr) && ((this.searchTypeMask & (((int) 1) << this.curActorPtr.handle.TheActorMeta.ActorType)) <= 0L)))
            {
                if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Hero))
                {
                    if (DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.heroFilter.Searcher((ActorRoot) _actorPtr, RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP, new PropertyDelegate(TargetProperty.GetPropertyHpRate));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Organ))
                {
                    if (DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.organFiler.Searcher((ActorRoot) _actorPtr, RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP, new PropertyDelegate(TargetProperty.GetPropertyHpRate));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Monster, false))
                {
                    if (DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        if (this.MonsterNotInBattle(ref _actorPtr))
                        {
                            this.monsterNotInBattleFiler.Searcher((ActorRoot) _actorPtr, RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP, new PropertyDelegate(TargetProperty.GetPropertyHpRate));
                        }
                        else
                        {
                            this.monsterFiler.Searcher((ActorRoot) _actorPtr, RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP, new PropertyDelegate(TargetProperty.GetPropertyHpRate));
                        }
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Monster, true) && DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                {
                    this.bossFiler.Searcher((ActorRoot) _actorPtr, RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP, new PropertyDelegate(TargetProperty.GetPropertyHpRate));
                }
            }
        }

        private void FilterNearestActor(ref PoolObjHandle<ActorRoot> _actorPtr)
        {
            if ((_actorPtr.handle.HorizonMarker.IsVisibleFor(this.curActorPtr.handle.TheActorMeta.ActorCamp) && this.curActorPtr.handle.CanAttack((ActorRoot) _actorPtr)) && ((this.curActorPtr != _actorPtr) && ((this.searchTypeMask & (((int) 1) << this.curActorPtr.handle.TheActorMeta.ActorType)) <= 0L)))
            {
                if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Hero))
                {
                    if (DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.heroFilter.Searcher((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Organ))
                {
                    if (DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.organFiler.Searcher((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Monster, false))
                {
                    if (DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        if (this.MonsterNotInBattle(ref _actorPtr))
                        {
                            this.monsterNotInBattleFiler.Searcher((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                        }
                        else
                        {
                            this.monsterFiler.Searcher((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                        }
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Monster, true) && DistanceSearchCondition.Fit((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, this.searchRadius))
                {
                    this.bossFiler.Searcher((ActorRoot) this.curActorPtr, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                }
            }
        }

        private void FilterNearestActorByPosition(ref PoolObjHandle<ActorRoot> _actorPtr)
        {
            if ((_actorPtr.handle.HorizonMarker.IsVisibleFor(this.curActorPtr.handle.TheActorMeta.ActorCamp) && this.curActorPtr.handle.CanAttack((ActorRoot) _actorPtr)) && (this.curActorPtr != _actorPtr))
            {
                if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Hero))
                {
                    if (DistanceSearchCondition.Fit(this.searchPosition, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.heroFilter.Searcher(this.searchPosition, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Organ))
                {
                    if (DistanceSearchCondition.Fit(this.searchPosition, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.organFiler.Searcher(this.searchPosition, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Monster, false))
                {
                    if (DistanceSearchCondition.Fit(this.searchPosition, (ActorRoot) _actorPtr, this.searchRadius))
                    {
                        this.monsterFiler.Searcher(this.searchPosition, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                    }
                }
                else if (TypeSearchCondition.Fit((ActorRoot) _actorPtr, ActorTypeDef.Actor_Type_Monster, true) && DistanceSearchCondition.Fit(this.searchPosition, (ActorRoot) _actorPtr, this.searchRadius))
                {
                    this.bossFiler.Searcher(this.searchPosition, (ActorRoot) _actorPtr, new DistanceDelegate(TargetDistance.GetDistance));
                }
            }
        }

        private uint GetSearchPriorityTarget(bool bIncludeMonsterNotInBattle = true)
        {
            if (this.heroList.Count >= 1)
            {
                return this.heroList[0].ObjID;
            }
            if (this.bossList.Count >= 1)
            {
                return this.bossList[0].ObjID;
            }
            if (this.monsterList.Count >= 1)
            {
                return this.monsterList[0].ObjID;
            }
            if (this.organList.Count >= 1)
            {
                return this.organList[0].ObjID;
            }
            if ((this.monsterNotInBattleList.Count >= 1) && bIncludeMonsterNotInBattle)
            {
                return this.monsterNotInBattleList[0].ObjID;
            }
            return 0;
        }

        private uint GetSearchPriorityTargetInLastHitMode(bool bIncludeMonsterNotInBattle = true)
        {
            if (this.bossList.Count >= 1)
            {
                return this.bossList[0].ObjID;
            }
            if (this.monsterList.Count >= 1)
            {
                return this.monsterList[0].ObjID;
            }
            if (this.organList.Count >= 1)
            {
                return this.organList[0].ObjID;
            }
            if (this.heroList.Count >= 1)
            {
                return this.heroList[0].ObjID;
            }
            if ((this.monsterNotInBattleList.Count >= 1) && bIncludeMonsterNotInBattle)
            {
                return this.monsterNotInBattleList[0].ObjID;
            }
            return 0;
        }

        private bool MonsterNotInBattle(ref PoolObjHandle<ActorRoot> monster)
        {
            if (monster != 0)
            {
                MonsterWrapper actorControl = monster.handle.ActorControl as MonsterWrapper;
                if (actorControl != null)
                {
                    ResMonsterCfgInfo cfgInfo = actorControl.cfgInfo;
                    if ((cfgInfo != null) && (cfgInfo.bMonsterType == 2))
                    {
                        switch (monster.handle.ActorAgent.GetCurBehavior())
                        {
                            case ObjBehaviMode.State_Idle:
                            case ObjBehaviMode.State_Dead:
                            case ObjBehaviMode.State_Null:
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public uint SearchLowestHpTarget(ref PoolObjHandle<ActorRoot> _actorPtr, int _srchR, uint _typeMask, bool bIncludeMonsterNotInBattle = true, SearchTargetPriority priority = 0)
        {
            this.Clear();
            this.curActorPtr = _actorPtr;
            this.searchRadius = _srchR;
            this.searchTypeMask = _typeMask;
            this.LowestHpHandler = new SceneManagement.Process(this.FilterLowestHpActor);
            SceneManagement instance = Singleton<SceneManagement>.GetInstance();
            SceneManagement.Coordinate coord = new SceneManagement.Coordinate();
            instance.GetCoord_Center(ref coord, _actorPtr.handle.location.xz, _srchR);
            instance.UpdateDirtyNodes();
            instance.ForeachActors(coord, this.LowestHpHandler);
            if (priority == SearchTargetPriority.CommonAttack)
            {
                return this.GetSearchPriorityTarget(bIncludeMonsterNotInBattle);
            }
            return this.GetSearchPriorityTargetInLastHitMode(bIncludeMonsterNotInBattle);
        }

        public uint SearchNearestTarget(ref PoolObjHandle<ActorRoot> _actorPtr, VInt3 _position, int _srchR)
        {
            this.Clear();
            this.curActorPtr = _actorPtr;
            this.searchRadius = _srchR;
            this.searchPosition = _position;
            this.NearestHandler = new SceneManagement.Process(this.FilterNearestActorByPosition);
            SceneManagement instance = Singleton<SceneManagement>.GetInstance();
            SceneManagement.Coordinate coord = new SceneManagement.Coordinate();
            instance.GetCoord_Center(ref coord, _position.xz, _srchR);
            instance.UpdateDirtyNodes();
            instance.ForeachActors(coord, this.NearestHandler);
            return this.GetSearchPriorityTarget(true);
        }

        public uint SearchNearestTarget(ref PoolObjHandle<ActorRoot> _actorPtr, int _srchR, uint _typeMask, bool bIncludeMonsterNotInBattle = true, SearchTargetPriority priority = 0)
        {
            this.Clear();
            this.curActorPtr = _actorPtr;
            this.searchRadius = _srchR;
            this.searchTypeMask = _typeMask;
            this.NearestHandler = new SceneManagement.Process(this.FilterNearestActor);
            SceneManagement instance = Singleton<SceneManagement>.GetInstance();
            SceneManagement.Coordinate coord = new SceneManagement.Coordinate();
            instance.GetCoord_Center(ref coord, _actorPtr.handle.location.xz, _srchR);
            instance.UpdateDirtyNodes();
            instance.ForeachActors(coord, this.NearestHandler);
            if (priority == SearchTargetPriority.CommonAttack)
            {
                return this.GetSearchPriorityTarget(bIncludeMonsterNotInBattle);
            }
            return this.GetSearchPriorityTargetInLastHitMode(bIncludeMonsterNotInBattle);
        }
    }
}

