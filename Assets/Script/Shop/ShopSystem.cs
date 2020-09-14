using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BallStyle{
    tomato, snowBall
}

public class ShopSystem : IGameSystem
{
    // 目前球的造型
    public BallStyle _ballStyle = BallStyle.tomato;// 暫時先設為tomato

    public ShopSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();

	}

    public int m_money ;
    public int m_maxHp;
    

    public void Initialize(){
        m_money = 0; // 之後改讀取
        m_maxHp = 10; // 初始化為10
    }

    // =========造型=========
    public void SetBallStyle(BallStyle ballStyle){
        _ballStyle = ballStyle;
    }

    public BallStyle GetBallStyle(){
        return _ballStyle;
    }

    // ===========購買方法==========

    public bool BuyThing(int price){
        if(WhetherBuyStage(price)){
            LessMoney(price);
            StageSelectUI.RefreshInfo(); // 更新金錢介面
            return true;
        }
        else
            return false;
    }

    public bool WhetherBuyStage(int price){
        if(price <= m_money)
            return true;
        else
            return false;
    }

    // =========金錢===========

    public void AddMoney(int money){
        m_money += money;
    }

    public void LessMoney(int money){
        if(m_money > money)
            m_money -= money;
        else
            Debug.LogError("錢不夠卻扣錢");
    }

    public int GetMoney(){
        return m_money;
    }

    //======Hp=========
    public void BuyHp(){
        int hpPrice = (m_maxHp-5) / 5;
        if(WhetherBuyStage(hpPrice))
        {
            m_maxHp ++;
            LessMoney(hpPrice);
        }
    }

    public int GetHpNeedMoney(){
        return (m_maxHp-5) / 5;
    }



}
