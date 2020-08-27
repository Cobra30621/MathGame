using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStageDataCard : MonoBehaviour
{
    [SerializeField] private Text lab_Stagename, lab_primeRange, lab_compositeRange;
    [SerializeField] private Text lab_bestpoint, lab_stageComplete;
    [SerializeField] private Button startButt;
    private StageData stageData;

    public void Initialize(StageData stageData){
        this.stageData = stageData;
        RefreshInfo(new StageInfoValue(stageData));
    }

    public void Show(){
        RefreshInfo(new StageInfoValue(stageData));
    }

    // 進入遊戲
    public void EnterStage(){
        GameMeditor.Instance.EnterStage(stageData.stageName);
    }

    public void RefreshInfo(StageInfoValue stageInfoValue){
        lab_Stagename.text = stageInfoValue.Text_stageName;
        lab_primeRange.text = stageInfoValue.Text_primeRange;
        lab_compositeRange.text = stageInfoValue.Text_compositeRange;
        lab_bestpoint.text = stageInfoValue.Text_bestpoint;
        lab_stageComplete.text = stageInfoValue.Text_stageComplete;
    }

}


public class StageInfoValue{

    public string Text_stageName;
    public string Text_primeRange;
    public string Text_compositeRange;

    public string Text_stageStage;
    public string Text_bestpoint;
    public string Text_stageComplete;

    public StageInfoValue(StageData stageData){
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
            // 改設定加分球
            int plusStart = stageData.m_plusNums[0];
            int plusEnd = stageData.m_plusNums[stageData.m_plusNums.Length - 1]; // 最後的數字
            Text_compositeRange = $"合數: {plusStart}~{plusEnd}";
        }
        else
        {
            int compositeStart = stageData.m_composites[0];
            int compositeEnd = stageData.m_composites[stageData.m_composites.Length - 1]; // 取最後的數字
            Text_compositeRange = $"合數: {compositeStart}~{compositeEnd}";

        }
        Text_bestpoint = $"最佳紀錄 : {stageData.m_bestPoint}";

        

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

        // 關卡解鎖：以stagestate判斷
        Text_stageStage = "開始";
    }

}