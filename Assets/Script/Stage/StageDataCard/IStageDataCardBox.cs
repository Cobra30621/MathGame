using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStageDataCardBox : MonoBehaviour
{
    public enum CardState{
        Unlocked , NeedMoney, EnoughMoney, HasBuy, Complete
    }
    public Dictionary<int, IStageDataCard> stageCards = new Dictionary<int, IStageDataCard>();
    [SerializeField] private Text lab_name, lab_Butt;
    [SerializeField] private Button buyButt;

    
    public int stagePrice;
    public CardState _cardState; //此卡的狀態
    public IStageDataCardBox _nextStageBox; // 下一關

    public void Initialize(IStageData stageData){

    }

    public void SetStageCard(IStageDataCard card){
        int id = stageCards.Count;
        card.BoxId = id; // 設置卡片的ID
        stageCards.Add(id, card);
    }

    // 設定下一關
    public void SetNetStageBox(IStageDataCardBox box){
        _nextStageBox = box;
    }

    public void CompleteStage(int id){
        if (id == (stageCards.Count-1)) // 所有關卡完成
        {
            _cardState = CardState.Complete;
            UnlockNextStageBox(); // 可以買下一關
        }
        else
        {
            stageCards[id + 1].SetStageState(StageState.Open); // 開啟下一關
        }
    }

    // 購買關卡
    public void BuyStage(){
        if(GameMeditor.Instance.WhetherBuyStage(stagePrice)) // 如果錢夠
        {
            // PlayBuyAnime()
            GameMeditor.Instance.BuyThing(stagePrice);
            _cardState = CardState.HasBuy;
            // stageData.m_stageState = StageState.Open;
        }
        else
        {
            // 顯示錢不夠
        }
        RefershButton();
    }

    public void RefershButton(){
        // 設定按鈕狀態
        switch (_cardState){
            case CardState.Unlocked:
            case CardState.HasBuy:
            case CardState.Complete:
                break;
            case CardState.NeedMoney:
            case CardState.EnoughMoney:
                if(GameMeditor.Instance.WhetherBuyStage(stagePrice)) // 如果錢夠
                {
                    _cardState = CardState.EnoughMoney;
                }
                else
                {
                    _cardState = CardState.NeedMoney;
                }
                break;
            default:
                Debug.LogError("出現不該出現的CardState");
                break;
        }

        
        // 設定按鈕顏色
        ColorBlock cb= buyButt.colors;
        switch (_cardState){
            case CardState.Unlocked:
                lab_Butt.text = "Unlock";
                
                break;
            case CardState.HasBuy:
            case CardState.Complete:
                break;
            case CardState.NeedMoney:
            case CardState.EnoughMoney:
                if(GameMeditor.Instance.WhetherBuyStage(stagePrice)) // 如果錢夠
                {
                    _cardState = CardState.EnoughMoney;
                }
                else
                {
                    _cardState = CardState.NeedMoney;
                }
                break;
            default:
                Debug.LogError("出現不該出現的CardState");
                break;
        }
        /*
        switch (stageData.m_stageState){
            case StageState.Open:
                buyButt.onClick.AddListener(delegate
                { 
                    EnterStage(); // 進入遊戲
                });
                lab_Butt.text = "開始";
                cb.normalColor = Color.white; // 按鈕設定為白色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case StageState.NeedMoney:
                buyButt.onClick.AddListener(delegate
                { 
                    // 沒東西
                });
                lab_Butt.text = $"$ {stageData.m_stagePrice}";
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case StageState.MoneyEnough:
                buyButt.onClick.AddListener(delegate
                { 
                    BuyStage(); // 購買關卡
                });
                lab_Butt.text = $"$ {stageData.m_stagePrice}";
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
        }*/
        buyButt.colors = cb; // 設定按鈕顏色
    }



    public void UnlockNextStageBox(){
        if(_nextStageBox == null)
            Debug.Log("關卡全破了");
        else
        {
            _nextStageBox._cardState = CardState.NeedMoney; // 可以買下一關
        }
    }

    

    public void RefreshInfo(StageInfoValue stageInfoValue){

    }


}
