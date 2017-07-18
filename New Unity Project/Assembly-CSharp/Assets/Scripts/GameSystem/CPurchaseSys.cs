﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.UI;
    using CSProtocol;
    using ResData;
    using System;

    [MessageHandlerClass]
    internal class CPurchaseSys : Singleton<CPurchaseSys>
    {
        private CBuyActPtPanel m_BuyActPtPanel;
        private CBuyCoinPanel m_BuyCoinPanel;
        private CBuySkillPt m_BuySkillPt;

        [MessageHandler(0x45c)]
        public static void BuyCoinRsp(CSPkg msg)
        {
            Singleton<CPurchaseSys>.GetInstance().m_BuyCoinPanel.BuyCoinRsp(msg);
            Singleton<CSoundManager>.instance.PostEvent("UI_Add_Gold", null);
        }

        public static ResShopInfo GetCfgShopInfo(RES_SHOPBUY_TYPE type, int subType)
        {
            uint shopInfoCfgId = GetShopInfoCfgId(type, subType);
            return GameDataMgr.resShopInfoDatabin.GetDataByKey(shopInfoCfgId);
        }

        public static uint GetShopInfoCfgId(RES_SHOPBUY_TYPE type, int subType)
        {
            int num = ((int) (type * ((RES_SHOPBUY_TYPE) 100))) + subType;
            return (uint) num;
        }

        public override void Init()
        {
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_OpenBuyActionPoint, new CUIEventManager.OnUIEventHandler(this.onOpenBuyActionPoint));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_OpenBuyCoin, new CUIEventManager.OnUIEventHandler(this.onOpenBuyCoin));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_OpenBuySkillPt, new CUIEventManager.OnUIEventHandler(this.onOpenBuySkillPt));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_CloseBuyActionPoint, new CUIEventManager.OnUIEventHandler(this.onCloseBuyActionPoint));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_CloseBuyCoin, new CUIEventManager.OnUIEventHandler(this.onCloseBuyCoin));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_CloseBuySkillPt, new CUIEventManager.OnUIEventHandler(this.onCloseBuySkillPt));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_BuyActPt, new CUIEventManager.OnUIEventHandler(this.onBuyActPt));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_BuyCoinOne, new CUIEventManager.OnUIEventHandler(this.onBuyCoinOne));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_BuyCoinTen, new CUIEventManager.OnUIEventHandler(this.onBuyCoinTen));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_BuySkillPt, new CUIEventManager.OnUIEventHandler(this.onBuySkillPt));
            Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_BuyDiamond, new CUIEventManager.OnUIEventHandler(this.onBuyDiamond));
            Singleton<EventRouter>.instance.AddEventHandler<CSPkg>("ShopBuyPurchase", new Action<CSPkg>(this.PurchaseRes));
            Singleton<EventRouter>.instance.AddEventHandler<int>("SkillPointChange", new Action<int>(this.onSkillPointChange));
            this.m_BuyActPtPanel = new CBuyActPtPanel();
            this.m_BuyActPtPanel.init();
            this.m_BuyCoinPanel = new CBuyCoinPanel();
            this.m_BuyCoinPanel.init();
            this.m_BuySkillPt = new CBuySkillPt();
            this.m_BuySkillPt.init();
        }

        private void onBuyActPt(CUIEvent uiEvent)
        {
            this.m_BuyActPtPanel.Buy();
        }

        private void onBuyCoinOne(CUIEvent uiEvent)
        {
            this.m_BuyCoinPanel.Buy(1);
        }

        private void onBuyCoinTen(CUIEvent uiEvent)
        {
            if (uiEvent.m_eventParams.tag != 0)
            {
                this.m_BuyCoinPanel.Buy(10);
            }
        }

        private void onBuyDiamond(CUIEvent uiEvent)
        {
            int tag = uiEvent.m_eventParams.tag;
            Singleton<ApolloHelper>.GetInstance().Pay(tag.ToString(), string.Empty);
        }

        private void onBuySkillPt(CUIEvent uiEvent)
        {
            this.m_BuySkillPt.Buy();
        }

        private void onCloseBuyActionPoint(CUIEvent uiEvent)
        {
            this.m_BuyActPtPanel.close();
        }

        private void onCloseBuyCoin(CUIEvent uiEvent)
        {
            this.m_BuyCoinPanel.close();
        }

        private void onCloseBuySkillPt(CUIEvent uiEvent)
        {
            this.m_BuySkillPt.close();
        }

        private void onOpenBuyActionPoint(CUIEvent uiEvent)
        {
            Singleton<CPurchaseSys>.GetInstance();
            this.m_BuyActPtPanel.open();
        }

        private void onOpenBuyCoin(CUIEvent uiEvent)
        {
            Singleton<CPurchaseSys>.GetInstance();
            this.m_BuyCoinPanel.open();
        }

        private void onOpenBuySkillPt(CUIEvent uiEvent)
        {
            Singleton<CPurchaseSys>.GetInstance();
            this.m_BuySkillPt.open();
        }

        private void onSkillPointChange(int Cnt)
        {
            if (Cnt > 0)
            {
                this.m_BuySkillPt.close();
            }
        }

        public void PurchaseRes(CSPkg msg)
        {
            switch (msg.stPkgData.stShopBuyRsp.iBuyType)
            {
                case 4:
                    Singleton<CPurchaseSys>.GetInstance().m_BuyActPtPanel.BuyRsp(msg);
                    break;

                case 5:
                    Singleton<CPurchaseSys>.GetInstance().m_BuySkillPt.BuyRsp(msg);
                    break;
            }
        }

        [MessageHandler(0x138b)]
        public static void ReceiveRandHeroCount(CSPkg msg)
        {
            Singleton<CHeroSelectBaseSystem>.instance.ResetRandHeroLeftCount((int) msg.stPkgData.stEntertainmentRandHeroCnt.dwRandHeroCnt);
        }

        public void SetSvrData(ref CSDT_ACNT_SHOPBUY_INFO ShopBuyInfo)
        {
            if (ShopBuyInfo != null)
            {
                this.m_BuyCoinPanel.SetSvrData(ref ShopBuyInfo);
                this.m_BuyActPtPanel.SetSvrData(ref ShopBuyInfo);
                this.m_BuySkillPt.SetSvrData(ref ShopBuyInfo);
            }
        }

        [MessageHandler(0x45a)]
        public static void ShopBuyRsp(CSPkg msg)
        {
            switch (msg.stPkgData.stShopBuyRsp.iBuyType)
            {
                case 1:
                case 2:
                case 3:
                case 8:
                case 10:
                case 11:
                    Singleton<EventRouter>.instance.BroadCastEvent<CSPkg>("ShopBuyDraw", msg);
                    break;

                case 4:
                case 5:
                    Singleton<EventRouter>.instance.BroadCastEvent<CSPkg>("ShopBuyPurchase", msg);
                    Singleton<CSoundManager>.instance.PostEvent("UI_Add_Physical", null);
                    break;

                case 6:
                    Singleton<EventRouter>.instance.BroadCastEvent<CSPkg>("ShopBuyLvlChallengeTime", msg);
                    break;

                case 7:
                    CSymbolWearController.OnSymbolBuySuccess(msg.stPkgData.stShopBuyRsp.iChgValue, msg.stPkgData.stShopBuyRsp.iBuySubType);
                    break;

                case 9:
                    Singleton<CArenaSystem>.GetInstance().ResetFightTimes(msg.stPkgData.stShopBuyRsp.iBuySubType, msg.stPkgData.stShopBuyRsp.iChgValue);
                    break;

                case 12:
                    CSymbolWearController.OnSymbolGridBuySuccess(msg.stPkgData.stShopBuyRsp.iChgValue);
                    break;

                case 13:
                    Singleton<CHeroSelectNormalSystem>.instance.OnHeroCountBought();
                    break;
            }
        }

        public override void UnInit()
        {
            base.UnInit();
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_OpenBuyActionPoint, new CUIEventManager.OnUIEventHandler(this.onOpenBuyActionPoint));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_OpenBuyCoin, new CUIEventManager.OnUIEventHandler(this.onOpenBuyCoin));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_OpenBuySkillPt, new CUIEventManager.OnUIEventHandler(this.onOpenBuySkillPt));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_CloseBuyActionPoint, new CUIEventManager.OnUIEventHandler(this.onCloseBuyActionPoint));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_CloseBuyCoin, new CUIEventManager.OnUIEventHandler(this.onCloseBuyCoin));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_CloseBuySkillPt, new CUIEventManager.OnUIEventHandler(this.onCloseBuySkillPt));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_BuyActPt, new CUIEventManager.OnUIEventHandler(this.onBuyActPt));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_BuyCoinOne, new CUIEventManager.OnUIEventHandler(this.onBuyCoinOne));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_BuyCoinTen, new CUIEventManager.OnUIEventHandler(this.onBuyCoinTen));
            Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_BuySkillPt, new CUIEventManager.OnUIEventHandler(this.onBuySkillPt));
            Singleton<EventRouter>.instance.RemoveEventHandler<CSPkg>("ShopBuyPurchase", new Action<CSPkg>(this.PurchaseRes));
            this.m_BuyActPtPanel.unInit();
            this.m_BuyCoinPanel.unInit();
            this.m_BuySkillPt.unInit();
            this.m_BuyActPtPanel = null;
            this.m_BuyCoinPanel = null;
            this.m_BuySkillPt = null;
        }
    }
}

