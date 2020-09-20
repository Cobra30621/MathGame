using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardState{
    Lock , HasNotStart, Processing, NeedMoney, EnoughMoney, HasBuy, Complete
}

public class IStageDataBox 
{
    public Dictionary<int, IStageData> stageDatas = new Dictionary<int, IStageData>();

    public string stageName;
    public int _stagePrice;
    public CardState _cardState; //此卡的狀態
    public IStageDataBox _nextStageBox; // 下一關
    public int nowLevelID; // 現在進行的關卡ID
    
    public int saveLevelID;
    // 儲存用資料
    /*
    public int saveLevelID{
        get{
            return saveLevelID;
        }
        set {
            if(_cardState == CardState.Complete)
                saveLevelID =  stageDatas.Count -1;
            else
                saveLevelID = value;
        }
    }*/

    public string nextLevelText{ 
        get{
        if(WhetherCompleteStage(nowLevelID))
            return "End";
        else
            return $"{nowLevelID + 1}";
        }
    }
    public int[] numRange = {1,1}; // 此大關數字的Range

    public IStageDataBox(string name){
        stageName = name;
        _stagePrice = 0;
        _cardState = CardState.Lock; // 預設鎖的
        nowLevelID = 0;
    }

    public IStageDataBox(string name, int price){
        stageName = name;
        _stagePrice = price;
        _cardState = CardState.Lock; // 預設鎖的
        nowLevelID = 0;
    }

    // 設定下一大關
    public void SetNetStageBox(IStageDataBox box){
        _nextStageBox = box;
    }

    public void UnlockNextStageBox(){
        if(_nextStageBox == null)
            Debug.Log("關卡全破了");
        else
        {
            _nextStageBox._cardState = CardState.HasNotStart; // 可以買玩下一關
        }
    }

    public void SetNumRange(int firstNum, int lastNum){
        numRange[0] = firstNum;
        numRange[1] = lastNum;
    }

    public void SetCardState(CardState cardState){
        _cardState = cardState;
    }

    public void InitNowLevelID(){
        Debug.Log($"將{stageName}的nowLevelID設為{saveLevelID}");
        if(saveLevelID == stageDatas.Count) // 如果進度為滿的，CardState.Complete
        {
            nowLevelID = 0; // 關卡重置
            SetCardState(CardState.Complete);
            UnlockNextStageBox();
        }
        else{
            nowLevelID = saveLevelID;
        }
            
    }

    public void UpdateCardState(){
        // 如果價格為零，直接開啟
        /*
        if (_stagePrice == 0)
        {
            _cardState = CardState.HasBuy;
            _cardState = CardState.HasNotStart; // 尚未開始
        }
        */


        switch (_cardState){
            case CardState.Lock:
                // SetStagesState(StageState.Lock);
                break;
            case CardState.NeedMoney:
            case CardState.EnoughMoney:
                // SetStagesState(StageState.Lock);
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
            case CardState.HasNotStart:
                // stageDatas[0].m_stageState = StageState.Open; // 開啟第一關
                break;
            case CardState.Complete:
                // SetStagesState(StageState.Open);
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
        stageData._boxName = stageName; // 設置CardBox的名稱
        stageData._stageDataBox = this; // 加入自己給對方
        stageDatas.Add(id, stageData);
    }

    public void CompleteStage(int id){
        if (id == (stageDatas.Count-1)) // 所有關卡完成
        {
            _cardState = CardState.Complete;
            saveLevelID = stageDatas.Count; 
            Debug.Log($"完成{stageName}所有關卡");
            UnlockNextStageBox(); // 可以買下一關
        }
        else
        {
            Debug.Log($"完成{stageName}的地{id+1}關:");
            if(_cardState != CardState.Complete)
                Debug.Log($"完成{stageName}的地{id+1}關");
                saveLevelID = id + 1;
            // stageDatas[id + 1].m_stageState = StageState.Open; // 開啟下一關
        }
        GameMeditor.Instance.SaveLevelData(); // 純檔
    }

    public bool WhetherCompleteStage(int id){
        if (id == (stageDatas.Count-1)) // 所有關卡完成
        {
            return true;
        }
        return false;
    }

    public int GetStageDataCount(){
        return stageDatas.Count;
    }

    public void SetNowLevelID(int num){
        nowLevelID = num;
    }

}
