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

    public void Initialize(){
        m_money = 0; // 之後改讀取
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



}
