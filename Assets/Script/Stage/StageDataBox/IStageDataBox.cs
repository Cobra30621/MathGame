using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardState{
    Lock , NeedMoney, EnoughMoney, HasBuy, Complete
}

public class IStageDataBox 
{
    public Dictionary<int, IStageData> stageDatas = new Dictionary<int, IStageData>();

    public string stageName;
    public int _stagePrice;
    public CardState _cardState; //此卡的狀態
    public IStageDataBox _nextStageBox; // 下一關


    public IStageDataBox(string name, int price){
        stageName = name;
        _stagePrice = price;

    }

    // 設定下一關
    public void SetNetStageBox(IStageDataBox box){
        _nextStageBox = box;
    }

    public void UnlockNextStageBox(){
        if(_nextStageBox == null)
            Debug.Log("關卡全破了");
        else
        {
            _nextStageBox._cardState = CardState.NeedMoney; // 可以買下一關
        }
    }

    public void Init(){
        // 所有關卡關閉
        SetStagesState(StageState.Lock);

        // 如果價格為零，直接開啟
        if (_stagePrice == 0)
        {
            _cardState = CardState.HasBuy;
            stageDatas[0].m_stageState = StageState.Open; // 開啟第一關
        }
    }

    public void UpdateCardState(){
        // 如果價格為零，直接開啟
        if (_stagePrice == 0)
        {
            _cardState = CardState.HasBuy;
        }


        switch (_cardState){
            case CardState.Lock:
                SetStagesState(StageState.Lock);
                break;
            case CardState.NeedMoney:
            case CardState.EnoughMoney:
                SetStagesState(StageState.Lock);
                if(GameMeditor.Instance.WhetherBuyStage(_stagePrice)) // 如果錢夠
                {
                    _cardState = CardState.EnoughMoney;
                }
                else
                {
                    _cardState = CardState.NeedMoney;
                }
                break;
            case CardState.HasBuy:
                stageDatas[0].m_stageState = StageState.Open; // 開啟第一關
                break;
            case CardState.Complete:
                SetStagesState(StageState.Open);
                break;
            default:
                Debug.LogError("出現不該出現的CardState");
                break;
        }
    }

    public void SetStagesState(StageState stageState){
        foreach (var stage in stageDatas)
        {
            stage.Value.m_stageState = stageState;
        }
    }

    // ================= 給小關卡使用==================

    public void AddStageData(IStageData stageData){
        int id = stageDatas.Count;
        stageData.stageID = id; // 設置卡片的ID
        stageData.BoxName = stageName; // 設置CardBox的名稱
        stageData._stageDataBox = this; // 加入自己給對方
        stageDatas.Add(id, stageData);
    }

    public void CompleteStage(int id){
        if (id == (stageDatas.Count-1)) // 所有關卡完成
        {
            _cardState = CardState.Complete;
            Debug.Log($"完成{stageName}所有關卡");
            UnlockNextStageBox(); // 可以買下一關
        }
        else
        {
            stageDatas[id + 1].m_stageState = StageState.Open; // 開啟下一關
        }
    }

    public bool WhetherCompleteStage(int id){
        if (id == (stageDatas.Count-1)) // 所有關卡完成
        {
            _cardState = CardState.Complete;
            Debug.Log($"完成{stageName}所有關卡");
            UnlockNextStageBox(); // 可以買下一關
            return true;
        }
        return false;
    }

    public int GetStageDataCount(){
        return stageDatas.Count;
    }

}
