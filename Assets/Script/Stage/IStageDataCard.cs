using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStageDataCard : MonoBehaviour
{
    [SerializeField] private Text lab_Stagename, lab_primeRange, lab_compositeRange;
    [SerializeField] private Text lab_bestpoint, lab_stageComplete, lab_ButtonText;
    [SerializeField] private Button startButt;
    private IStageData stageData;

    public void Initialize(IStageData stageData){
        this.stageData = stageData;
        RefreshInfo(new StageInfoValue(stageData));
    }

    public void Show(){
        RefreshInfo(new StageInfoValue(stageData));
    }

    public void RefreshInfo(StageInfoValue stageInfoValue){
        lab_Stagename.text = stageInfoValue.Text_stageName;
        lab_primeRange.text = stageInfoValue.Text_primeRange;
        lab_compositeRange.text = stageInfoValue.Text_compositeRange;
        lab_bestpoint.text = stageInfoValue.Text_bestpoint;
        lab_stageComplete.text = stageInfoValue.Text_stageComplete;
        RefershButton();
    }

    public void RefershButton(){
        // 設定按鈕狀態
        switch (stageData.m_stageState){
            case StageState.Open:
                break;
            case StageState.NeedMoney:
            case StageState.MoneyEnough:
                if(GameMeditor.Instance.WhetherBuyStage(stageData.m_stagePrice)) // 如果錢夠
                {
                    stageData.m_stageState = StageState.MoneyEnough;
                }
                else
                {
                    stageData.m_stageState = StageState.NeedMoney;
                }
                break;
        }

        
        // 設定按鈕顏色
        ColorBlock cb= startButt.colors;
        switch (stageData.m_stageState){
            case StageState.Open:
                startButt.onClick.AddListener(delegate
                { 
                    EnterStage(); // 進入遊戲
                });
                lab_ButtonText.text = "開始";
                cb.normalColor = Color.white; // 按鈕設定為白色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case StageState.NeedMoney:
                startButt.onClick.AddListener(delegate
                { 
                    // 沒東西
                });
                lab_ButtonText.text = $"$ {stageData.m_stagePrice}";
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case StageState.MoneyEnough:
                startButt.onClick.AddListener(delegate
                { 
                    BuyStage(); // 購買關卡
                });
                lab_ButtonText.text = $"$ {stageData.m_stagePrice}";
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
        }
        startButt.colors = cb; // 設定按鈕顏色
    }


     // 進入遊戲
    public void EnterStage(){
        GameMeditor.Instance.EnterStage(stageData.stageName);
    }

    // 購買關卡
    public void BuyStage(){
        int stagePrice = stageData.m_stagePrice;
        if(GameMeditor.Instance.WhetherBuyStage(stagePrice)) // 如果錢夠
        {
            // PlayBuyAnime()
            GameMeditor.Instance.BuyThing(stagePrice);
            stageData.m_stageState = StageState.Open;
        }
        else
        {
            // 顯示錢不夠
        }
        RefershButton();
    }

}


public class StageInfoValue{

    public string Text_stageName;
    public string Text_primeRange;
    public string Text_compositeRange;

    public string Text_stageStage;
    public string Text_bestpoint;
    public string Text_stageComplete;

    public bool StateOpen;

    public StageInfoValue(IStageData stageData){
        Text_stageName = stageData.stageName;

        // 設定質數文字
        if (stageData.m_primes.Length == 0){
            Text_primeRange = "技巧型關卡";
        }
        else
        {
            int primeStart = stageData.m_primes[0];
            int primeEnd = stageData.m_primes[stageData.m_primes.Length - 1]; // 最後的數字
            Text_primeRange = $"質數: {primeStart}~{primeEnd}";
        }
        
        // 設定合數文字
        if (stageData.m_composites.Length == 0)
        {
            Text_compositeRange = "";
        }
        else
        {
            int compositeStart = stageData.m_composites[0];
            int compositeEnd = stageData.m_composites[stageData.m_composites.Length - 1]; // 取最後的數字
            Text_compositeRange = $"合數: {compositeStart}~{compositeEnd}";

        }
        Text_bestpoint = $"Best: {stageData.m_bestPoint}%";

        

        // 關卡完成狀況
        switch (stageData.m_stageComplete){
            case StageComplete.UnComplete:
                Text_stageComplete = "UnComplete";
                break;
            case StageComplete.Complete:
                Text_stageComplete = "Complete";
                break;
            case StageComplete.FullCombol:
                Text_stageComplete = "FullCombol";
                break;
        }

    }

}