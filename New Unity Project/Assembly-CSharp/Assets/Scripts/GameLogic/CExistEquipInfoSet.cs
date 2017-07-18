﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Framework;
    using ResData;
    using System;

    public class CExistEquipInfoSet
    {
        public int m_existEquipInfoCount = 0;
        public stExistEquipInfo[] m_existEquipInfos = new stExistEquipInfo[6];
        public bool m_used = true;

        public void Clear()
        {
            for (int i = 0; i < 6; i++)
            {
                this.m_existEquipInfos[i].m_equipID = 0;
                this.m_existEquipInfos[i].m_amount = 0;
                this.m_existEquipInfos[i].m_calculateAmount = 0;
                this.m_existEquipInfos[i].m_unitBuyPrice = 0;
            }
            this.m_existEquipInfoCount = 0;
        }

        public void Clone(CExistEquipInfoSet existEquipInfoSet)
        {
            if (existEquipInfoSet != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    this.m_existEquipInfos[i] = existEquipInfoSet.m_existEquipInfos[i];
                }
                this.m_existEquipInfoCount = existEquipInfoSet.m_existEquipInfoCount;
            }
        }

        public bool Equals(CExistEquipInfoSet other)
        {
            bool flag = true;
            for (int i = 0; i < 6; i++)
            {
                flag = flag && this.m_existEquipInfos[i].Equals(other.m_existEquipInfos[i]);
            }
            return (flag && (this.m_existEquipInfoCount == other.m_existEquipInfoCount));
        }

        public override bool Equals(object obj)
        {
            return (((obj != null) && (base.GetType() == obj.GetType())) && this.Equals((CExistEquipInfoSet) obj));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Refresh(stEquipInfo[] equipInfos)
        {
            if (equipInfos != null)
            {
                this.Clear();
                for (int i = 0; i < equipInfos.Length; i++)
                {
                    if (equipInfos[i].m_equipID != 0)
                    {
                        bool flag = false;
                        for (int j = 0; j < this.m_existEquipInfoCount; j++)
                        {
                            if (this.m_existEquipInfos[j].m_equipID == equipInfos[i].m_equipID)
                            {
                                flag = true;
                                this.m_existEquipInfos[j].m_amount += equipInfos[i].m_amount;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this.m_existEquipInfos[this.m_existEquipInfoCount].m_equipID = equipInfos[i].m_equipID;
                            this.m_existEquipInfos[this.m_existEquipInfoCount].m_amount = equipInfos[i].m_amount;
                            ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint) equipInfos[i].m_equipID);
                            this.m_existEquipInfos[this.m_existEquipInfoCount].m_unitBuyPrice = (dataByKey == null) ? 0 : ((CrypticInt32) dataByKey.dwBuyPrice);
                            this.m_existEquipInfoCount++;
                        }
                    }
                }
            }
        }

        public void ResetCalculateAmount()
        {
            for (int i = 0; i < this.m_existEquipInfoCount; i++)
            {
                this.m_existEquipInfos[i].m_calculateAmount = this.m_existEquipInfos[i].m_amount;
            }
        }
    }
}

