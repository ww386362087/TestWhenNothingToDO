﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic;
    using CSProtocol;
    using ResData;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CRoleInfoManager : Singleton<CRoleInfoManager>, IUpdateLogic
    {
        [CompilerGenerated]
        private ulong <masterUUID>k__BackingField;
        private CRoleInfoContainer s_roleInfoContainer;

        public void CalculateKDA(COMDT_GAME_INFO gameInfo)
        {
            CRoleInfo masterRoleInfo = this.GetMasterRoleInfo();
            DebugHelper.Assert(masterRoleInfo != null, "masterRoleInfo is null");
            if (masterRoleInfo != null)
            {
                PlayerKDA hostKDA = Singleton<BattleStatistic>.GetInstance().m_playerKDAStat.GetHostKDA();
                if (hostKDA != null)
                {
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    int num9 = 0;
                    int num10 = 0;
                    int num11 = 0;
                    int num12 = 0;
                    ListView<HeroKDA>.Enumerator enumerator = hostKDA.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current != null)
                        {
                            num += enumerator.Current.LegendaryNum;
                            num2 += enumerator.Current.PentaKillNum;
                            num3 += enumerator.Current.QuataryKillNum;
                            num4 += enumerator.Current.TripleKillNum;
                            num5 += enumerator.Current.DoubleKillNum;
                            num8 += !enumerator.Current.bHurtMost ? 0 : 1;
                            num9 += !enumerator.Current.bHurtTakenMost ? 0 : 1;
                            num10 += !enumerator.Current.bGetCoinMost ? 0 : 1;
                            num11 += !enumerator.Current.bAsssistMost ? 0 : 1;
                            num12 += !enumerator.Current.bKillMost ? 0 : 1;
                        }
                    }
                    if (gameInfo.bGameResult == 1)
                    {
                        uint mvpPlayer = Singleton<BattleStatistic>.instance.GetMvpPlayer(hostKDA.PlayerCamp, true);
                        if (mvpPlayer != 0)
                        {
                            num6 = (mvpPlayer != hostKDA.PlayerId) ? 0 : 1;
                        }
                    }
                    else if (gameInfo.bGameResult == 2)
                    {
                        uint num14 = Singleton<BattleStatistic>.instance.GetMvpPlayer(hostKDA.PlayerCamp, false);
                        if (num14 != 0)
                        {
                            num7 = (num14 != hostKDA.PlayerId) ? 0 : 1;
                        }
                    }
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;
                    bool flag5 = false;
                    bool flag6 = false;
                    bool flag7 = false;
                    bool flag8 = false;
                    bool flag9 = false;
                    bool flag10 = false;
                    bool flag11 = false;
                    bool flag12 = false;
                    int index = 0;
                    ListView<COMDT_STATISTIC_KEY_VALUE_INFO> inList = new ListView<COMDT_STATISTIC_KEY_VALUE_INFO>();
                    while (index < masterRoleInfo.pvpDetail.stKVDetail.dwNum)
                    {
                        COMDT_STATISTIC_KEY_VALUE_INFO comdt_statistic_key_value_info = masterRoleInfo.pvpDetail.stKVDetail.astKVDetail[index];
                        switch (comdt_statistic_key_value_info.dwKey)
                        {
                            case 13:
                                comdt_statistic_key_value_info.dwValue += (uint) num6;
                                flag6 = true;
                                break;

                            case 14:
                                comdt_statistic_key_value_info.dwValue += (uint) num7;
                                flag7 = true;
                                break;

                            case 15:
                                comdt_statistic_key_value_info.dwValue += (uint) num;
                                flag5 = true;
                                break;

                            case 0x10:
                                comdt_statistic_key_value_info.dwValue += (uint) num5;
                                flag = true;
                                break;

                            case 0x11:
                                comdt_statistic_key_value_info.dwValue += (uint) num4;
                                flag2 = true;
                                break;

                            case 0x1b:
                                comdt_statistic_key_value_info.dwValue += (uint) num3;
                                flag3 = true;
                                break;

                            case 0x1c:
                                comdt_statistic_key_value_info.dwValue += (uint) num2;
                                flag4 = true;
                                break;

                            case 0x1d:
                                comdt_statistic_key_value_info.dwValue += (uint) num8;
                                flag8 = true;
                                break;

                            case 30:
                                comdt_statistic_key_value_info.dwValue += (uint) num10;
                                flag10 = true;
                                break;

                            case 0x1f:
                                comdt_statistic_key_value_info.dwValue += (uint) num9;
                                flag9 = true;
                                break;

                            case 0x20:
                                comdt_statistic_key_value_info.dwValue += (uint) num11;
                                flag11 = true;
                                break;

                            case 0x21:
                                comdt_statistic_key_value_info.dwValue += (uint) num12;
                                flag12 = true;
                                break;
                        }
                        index++;
                    }
                    COMDT_STATISTIC_KEY_VALUE_INFO item = null;
                    if (!flag)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x10;
                        item.dwValue = (uint) num5;
                        inList.Add(item);
                    }
                    if (!flag2)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x11;
                        item.dwValue = (uint) num4;
                        inList.Add(item);
                    }
                    if (!flag3)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x1b;
                        item.dwValue = (uint) num3;
                        inList.Add(item);
                    }
                    if (!flag4)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x1c;
                        item.dwValue = (uint) num2;
                        inList.Add(item);
                    }
                    if (!flag5)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 15;
                        item.dwValue = (uint) num;
                        inList.Add(item);
                    }
                    if (!flag6)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 13;
                        item.dwValue = (uint) num6;
                        inList.Add(item);
                    }
                    if (!flag7)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 14;
                        item.dwValue = (uint) num7;
                        inList.Add(item);
                    }
                    if (!flag8)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x1d;
                        item.dwValue = (uint) num8;
                        inList.Add(item);
                    }
                    if (!flag9)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x1f;
                        item.dwValue = (uint) num9;
                        inList.Add(item);
                    }
                    if (!flag10)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 30;
                        item.dwValue = (uint) num10;
                        inList.Add(item);
                    }
                    if (!flag11)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x20;
                        item.dwValue = (uint) num11;
                        inList.Add(item);
                    }
                    if (!flag12)
                    {
                        item = new COMDT_STATISTIC_KEY_VALUE_INFO();
                        item.dwKey = 0x21;
                        item.dwValue = (uint) num12;
                        inList.Add(item);
                    }
                    if (inList.Count > 0)
                    {
                        masterRoleInfo.pvpDetail.stKVDetail.dwNum += (uint) inList.Count;
                        inList.AddRange(masterRoleInfo.pvpDetail.stKVDetail.astKVDetail);
                        masterRoleInfo.pvpDetail.stKVDetail.astKVDetail = LinqS.ToArray<COMDT_STATISTIC_KEY_VALUE_INFO>(inList);
                    }
                }
            }
        }

        public void CalculateWins(COMDT_PVPBATTLE_INFO battleInfo, int bGameResult)
        {
            battleInfo.dwTotalNum++;
            if (bGameResult == 1)
            {
                battleInfo.dwWinNum++;
            }
        }

        public void Clean()
        {
            this.s_roleInfoContainer.Clear();
        }

        public void ClearMasterRoleInfo()
        {
            CRoleInfo info = !this.hasMasterUUID ? null : this.GetMasterRoleInfo();
            if (info != null)
            {
                info.Clear();
            }
        }

        public CRoleInfo CreateRoleInfo(enROLEINFO_TYPE type, ulong uuID, int logicWorldID = 0)
        {
            ulong num = this.s_roleInfoContainer.AddRoleInfoByType(type, uuID, logicWorldID);
            return this.GetRoleInfoByUUID(num);
        }

        public CRoleInfo GetMasterRoleInfo()
        {
            return this.GetRoleInfoByUUID(this.masterUUID);
        }

        public CRoleInfo GetRoleInfoByUUID(ulong uuID)
        {
            return this.s_roleInfoContainer.FindRoleInfoByID(uuID);
        }

        public uint GetSelfRankClass()
        {
            if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo() != null)
            {
                return (byte) Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_rankClass;
            }
            return 0;
        }

        public override void Init()
        {
            this.s_roleInfoContainer = new CRoleInfoContainer();
            this.masterUUID = 0L;
        }

        public void InsertHonorOnDuplicateUpdate(ref Dictionary<int, COMDT_HONORINFO> dic, int type, int defaultPoint = 1)
        {
            COMDT_HONORINFO honorInfo = new COMDT_HONORINFO();
            if (!dic.ContainsKey(type))
            {
                honorInfo.iHonorID = type;
                honorInfo.iHonorLevel = 0;
                honorInfo.iHonorPoint = defaultPoint;
                this.JudgeHonorLevelUp(honorInfo);
                dic.Add(type, honorInfo);
            }
            else if (dic.TryGetValue(type, out honorInfo))
            {
                honorInfo.iHonorPoint = defaultPoint;
                this.JudgeHonorLevelUp(honorInfo);
                dic[type] = honorInfo;
            }
        }

        private void JudgeHonorLevelUp(COMDT_HONORINFO honorInfo)
        {
            int iHonorID = honorInfo.iHonorID;
            int iHonorPoint = honorInfo.iHonorPoint;
            int num3 = -1;
            ResHonor dataByKey = GameDataMgr.resHonor.GetDataByKey((long) iHonorID);
            if (dataByKey != null)
            {
                for (int i = 0; i < dataByKey.astHonorLevel.Length; i++)
                {
                    if (iHonorPoint < dataByKey.astHonorLevel[i].iMaxPoint)
                    {
                        break;
                    }
                    num3++;
                }
                if (num3 >= dataByKey.astHonorLevel.Length)
                {
                    num3--;
                }
            }
            honorInfo.iHonorLevel = num3;
        }

        public void SetMaterUUID(ulong InMaterUUID)
        {
            this.masterUUID = InMaterUUID;
        }

        public void UpdateLogic(int delta)
        {
            if (this.hasMasterUUID)
            {
                CRoleInfo masterRoleInfo = this.GetMasterRoleInfo();
                if (masterRoleInfo != null)
                {
                    masterRoleInfo.UpdateLogic(delta);
                }
            }
        }

        public bool hasMasterUUID
        {
            get
            {
                return (this.masterUUID != 0L);
            }
        }

        public ulong masterUUID
        {
            [CompilerGenerated]
            get
            {
                return this.<masterUUID>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<masterUUID>k__BackingField = value;
            }
        }
    }
}

