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

    public void Show(){
        RefreshInfo(new StageInfoValue(stageData));
    }

    public void EnterStage(){

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

        int primeStart = stageData.m_primes[0];
        int primeEnd = stageData.m_primes[-1];
        Text_primeRange = $"質數:{primeStart}~{primeEnd}";

        int compositeStart = stageData.m_composites[0];
        int compositeEnd = stageData.m_composites[-1];
        Text_compositeRange = $"質數:{compositeStart}~{compositeEnd}";

        Text_bestpoint = $"最佳紀錄{stageData.m_bestPoint}";

        // 關卡完成狀況
        switch (stageData.m_stageComplete){
            case StageComplete.UnComplete:
                Text_compositeRange = "UnComplete";
                break;
            case StageComplete.Complete:
                Text_compositeRange = "Complete";
                break;
            case StageComplete.FullCombol:
                Text_compositeRange = "FullCombol";
                break;
        }

        // 關卡解鎖：以stagestate判斷
        Text_stageStage = "開始";
    }

}