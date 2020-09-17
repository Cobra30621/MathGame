using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStageDataCardBox : MonoBehaviour
{
    [SerializeField] private Text lab_name, lab_Butt, lab_numRange, lab_stageComplete, lab_nowLevel, lab_nextLevel, lab_process;
    [SerializeField] private Button buyButt;
    [SerializeField] private GameObject processBar;

    public IStageDataBox _stageBox;
    // public List<IStageDataCard> _stateCards;
    // 製作CardList，RefreshButton

    // ================= 初始化介面==================
    public void Initialize(IStageDataBox stageBox){
        // 設置StageBataBox
        _stageBox = stageBox;
        // 加入管理器
        GameMeditor.Instance.AddStageCardBox(this);

        // 重置介面
        RefreshInfo();
        
    }

    public void RefreshInfo(){
        StageBoxInfoValue info = new StageBoxInfoValue(_stageBox);
        RefreshStartButton(); // 重置開始按鈕

        lab_name.text = info.Text_stageName;
        // lab_nowLevel.text = info.Text_nowLevel;
        // lab_nextLevel.text = info.Text_nextLevel;
        lab_process.text = info.Text_process;
        lab_numRange.text = info.Text_numRange;
        lab_stageComplete.text = info.Text_stageComplete;

        // 依據是否完成關卡，顯使Bar的樣式
        Vector3 scale = processBar.transform.localScale;        
        if (_stageBox._cardState == CardState.Complete)
            processBar.transform.localScale = new Vector3(1,scale.y, scale.z ); 
        else
        {
            float completeRate = (float)_stageBox.nowLevelID / (float)_stageBox.GetStageDataCount();
            Debug.Log($"_stageBox.GetStageDataCount(){_stageBox.GetStageDataCount()}");
            processBar.transform.localScale = new Vector3(completeRate,scale.y, scale.z ); 
        }
            
    
    }

    public void RefreshStartButton(){
        _stageBox.UpdateCardState(); // 更新卡片狀態
        
        // 設定按鈕顏色
        ColorBlock cb= buyButt.colors;
        switch (_stageBox._cardState){
            case CardState.Lock:
                buyButt.onClick.AddListener(delegate{ });

                lab_Butt.text = "未開啟"; // 設定按鈕文字
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case CardState.HasNotStart:
            case CardState.Complete:
                buyButt.onClick.AddListener(delegate{ EnterStage(); });

                lab_Butt.text = "開始"; // 設定按鈕文字
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case CardState.Processing:
                buyButt.onClick.AddListener(delegate{ EnterStage(); });

                lab_Butt.text = "繼續"; // 設定按鈕文字
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            default:
                Debug.LogError("出現不該出現的CardState");
                break;
        }
         
        buyButt.colors = cb; // 設定按鈕顏色
    }

    // 進入遊戲
    public void EnterStage(){
        // GameMeditor.Instance.EnterStage(stageData.stageName);
        GameMeditor.Instance.EnterStage(_stageBox.stageName, _stageBox.nowLevelID);
    }




    // ===測試用====
    public void OverComeStage(){
        _stageBox._cardState = CardState.HasNotStart;
        RefreshInfo();
        // _stageBox.SetStagesState(StageState.Open); // 開啟所有關卡
        //_stageBox.UnlockNextStageBox(); // 開啟下一關
        // RefreshCard(); // 更新卡片
        // GameMeditor.Instance.RefreshAllCard(); // 更新所有卡片
    }
}


// ====================== 購買關卡方法 ========================= 
/*
    // 購買關卡
    public void BuyStage(){
        if(GameMeditor.Instance.WhetherBuyStage(_stageBox._stagePrice)) // 如果錢夠
        {
            // PlayBuyAnime()
            GameMeditor.Instance.BuyThing(_stageBox._stagePrice);
            _stageBox._cardState = CardState.HasBuy;
            _stageBox.stageDatas[0].m_stageState = StageState.Open; // 開啟第一關
        }
        else
        {
            // 顯示錢不夠
        }
        RefreshInfo();
    }

    

    // 更新購買按鈕顏色
    public void RefershBuyButton(){
        _stageBox.UpdateCardState(); // 更新卡片狀態
        
        // 設定按鈕顏色
        ColorBlock cb= buyButt.colors;
        switch (_stageBox._cardState){
            case CardState.Lock:
                lab_Butt.text = "Unlock"; // 設定按鈕文字
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case CardState.NeedMoney:
                buyButt.onClick.AddListener(delegate{  });

                lab_Butt.text = $" ${_stageBox._stagePrice}"; // 設定按鈕文字
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case CardState.EnoughMoney:
                buyButt.onClick.AddListener(delegate
                { 
                    BuyStage(); // 購買關卡
                });

                lab_Butt.text = $" ${_stageBox._stagePrice}";
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case CardState.HasBuy:
                buyButt.onClick.AddListener(delegate{  });

                lab_Butt.text = "已解鎖"; // 設定按鈕文字
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case CardState.Complete:
                buyButt.onClick.AddListener(delegate{  });

                lab_Butt.text = "Complete"; // 設定按鈕文字
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            default:
                Debug.LogError("出現不該出現的CardState");
                break;
        }
         
        buyButt.colors = cb; // 設定按鈕顏色
    }

    // ================== 製作小卡方法 ===============

        private GameObject stageDataCardPrefab;
    // 讀取字卡Prefabs
        ResourceAssetFactory factory = MainFactory.GetResourceAssetFactory();
        stageDataCardPrefab = factory.LoadStageInfoCard();
    // 更新所有的卡片
    public void RefreshCard(){
        foreach(IStageDataCard card in _stateCards){
            card.RefreshInfo();
        }
    }

        /// <summary>
    /// 製作一個CreateStageDataCard，自動移動至錢錢顯示位置後加錢
    /// </summary>
    public void CreateStageDataCard(IStageData stageData)
    {
        var g = Instantiate(stageDataCardPrefab, transform);
        // g.transform.SetParent(transform);

        var l = g.GetComponent<IStageDataCard>();
        l.Initialize(stageData);
        _stateCards.Add(l);
    }

    */


public class StageBoxInfoValue{

    public string Text_stageName;
    public string Text_numRange;
    public string Text_stageComplete;
    public string Text_nowLevel;
    public string Text_nextLevel;
    public string Text_process;

    public StageBoxInfoValue(IStageDataBox stageDatabox){
        Text_stageName = stageDatabox.stageName;

        // 設定數字Range
        
        int numStart = stageDatabox.numRange[0];
        int numEnd = stageDatabox.numRange[1]; // 最後的數字
        Text_numRange = $"數字: {numStart}~{numEnd}";

        
        // 關卡完成狀況
        switch (stageDatabox._cardState){
            case CardState.Lock:
                Text_stageComplete = "未開始";
                break;
            case CardState.Processing:
            case CardState.HasNotStart:
                Text_stageComplete = "進行中";
                break;
            case CardState.Complete:
                Text_stageComplete = "通關！！";
                break;
        }

        // 關卡進度
        // 關卡數
        int maxLevel = stageDatabox.GetStageDataCount() ;
        if(stageDatabox._cardState == CardState.Complete)
            Text_process = $"{maxLevel}/{maxLevel}";
        else
            Text_process = $"{stageDatabox.nowLevelID}/{maxLevel}";
        
        
        

        // 關卡數
        if(stageDatabox._cardState == CardState.Complete){
            maxLevel = stageDatabox.GetStageDataCount() ;
            Text_nowLevel = $"{maxLevel}";
            Text_nextLevel = "End";
            
        }
        else
        {
            Text_nowLevel = $"{stageDatabox.nowLevelID}";
            Text_nextLevel = stageDatabox.nextLevelText;
        }
        

    }

}
