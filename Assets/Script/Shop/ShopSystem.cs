using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopSystem : IGameSystem
{

    public ShopSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();

	}

    public int m_money ;

    public void Initialize(){
        m_money = 0; // 之後改讀取
    }

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
