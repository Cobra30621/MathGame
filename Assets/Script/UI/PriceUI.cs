using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceUI : MonoBehaviour
{
    private static PriceUI instance;
    private Animator m_animator;
    [SerializeField] private GameObject panel;
    [SerializeField] private Text lab_addMoney, lab_money, lab_stageComplete, lab_stageName;

    void Awake()
    {
        instance = this;
        m_animator = GetComponent<Animator>();
        Initialize();
    }

    public static void Initialize(){
        RefreshInfo();
    }

    public static void RefreshInfo(){
        instance.Refresh();
    }

    public void Refresh(){
        int money = GameMeditor.Instance.GetMoney();
        lab_money.text = $"$ {money}";

        int point = GameMeditor.Instance.GetPoint();
        if(point < 0)
            lab_addMoney.text = "$ +0";
        else
            lab_addMoney.text = $"$ +{point}";

        // 設定分數狀態
        StageComplete stageComplete = GameMeditor.Instance.GetStageComplete();
        switch (stageComplete){
            case StageComplete.Complete:
                lab_stageComplete.text = "Complete";

                break;
            case StageComplete.FullCombol:
                lab_stageComplete.text = "FullCombol";
                lab_addMoney.text = $"$ +{point} (x2)"; // 給錢加成
                break;
        }



        string stageName = GameMeditor.Instance.GetStageName();
        lab_stageName.text = stageName;
        
    }

    // =============介面開啟與關閉============

    public static void Show(){
        instance.Open();
    }

    public static void Close(){
        instance.ClosePanel();
    }

    public void Open(){
        // playAnime;
        Refresh();
        panel.SetActive(true);
    }

    public void ClosePanel(){
        // playAnime;
        panel.SetActive(false);
    }

    public void Retry(){
        ClosePanel();
        GameMeditor.Instance.ResetStage();
    }

    public void Leave(){
        ClosePanel();
        GameMeditor.Instance.LeaveStage();
    }
}