using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChangeStageUI : MonoBehaviour
{
    public static ChangeStageUI instance;

    [SerializeField] private GameObject nextLevelPanel, completePanel, deadPanel, mainPanel, buyPanel;

    // nextPanel
    [SerializeField] private Text lab_next_addMoney,lab_next_title, lab_next_nowLevel, lab_next_nextLevel;

    // DeadPanel
    [SerializeField] private Text lab_dead_addMoney,  lab_dead_title, lab_dead_nowLevel, lab_dead_nextLevel;
    [SerializeField] private GameObject gameProcessBar;

    // completePanel
    [SerializeField] private Text lab_complete_stageName, lab_complete_addMoney ,lab_complete_nowLevel, lab_complete_nextLevel ;

    // BuyPanel
    [SerializeField] private Text lab_NeedMoney, lab_heartNum; 
    [SerializeField] private Button buyButt;
    private IStageData _stageData;


    void Awake()
    {
        instance = this;
        Close(); 
    }

    // 顯示前往下一關
    public static void ShowNextLevel(IStageData stageData){
        instance._stageData = stageData;
        instance.WhetherEnd();
    }

    public void WhetherEnd(){ // 這應該夜放在stageData中
        // AudioSourceController.PlaySound("pass"); // 播放音效
        MusicManager.PlayMusicAtOnce(BGM.Pass);
        
        IStageDataBox stageBox = _stageData._stageDataBox;
        if(stageBox.WhetherCompleteStage(_stageData.stageID))
        {
            StageComplete();
        }
        else{
            NextLeval();
        }
    }

    
    // 顯示CompletePanel
    public void StageComplete(){
        mainPanel.SetActive(true);
        completePanel.SetActive(true);

        lab_complete_stageName.text = _stageData._stageDataBox.stageName;

        // 關卡
        int level = _stageData.stageID ;
        lab_complete_nowLevel.text = $"{level}";
        lab_complete_nextLevel.text = _stageData.GetNextLevelText();

        // 記錄目前的關卡
        _stageData._stageDataBox.SetNowLevelID(0); // 重新開始
        _stageData._stageDataBox.SetCardState(CardState.Complete); // 完成關卡
    }

    // 顯示NextPanel
    public void NextLeval(){
        mainPanel.SetActive(true);
        nextLevelPanel.SetActive(true);
        ShowBuyPanel();

        /*
        // 分數
        int point = _stageData.point;
        if(point < 0)
            lab_next_addMoney.text = "$ +0";
        else
            lab_next_addMoney.text = $"$ +{point}";
        */

        // 關卡
        int level = _stageData.stageID ;
        lab_next_title.text = $"第{level + 1}號番茄樹";
        lab_next_nowLevel.text = $"{level}";
        lab_next_nextLevel.text = _stageData.GetNextLevelText();

        _stageData._stageDataBox.SetNowLevelID(_stageData.stageID + 1); // 記錄為下一關

    }

    public static void ShowDead(IStageData stageData){
        instance._stageData = stageData;
        instance.Dead();
    }

    // 顯示Dead Panel
    public void Dead(){
        mainPanel.SetActive(true);
        deadPanel.SetActive(true);
        ShowBuyPanel();
        MusicManager.PlayMusicAtOnce(BGM.Dead);
        // AudioSourceController.PlaySound("dead"); // 播放音效
        /*
        // 分數
        int point = _stageData.point;
        if(point < 0)
            lab_dead_addMoney.text = "$ +0";
        else
            lab_dead_addMoney.text = $"$ +{point}";
        */

        // 關卡
        int level = _stageData.stageID;
        lab_dead_title.text = $"第{level}號番茄樹";
        lab_dead_nowLevel.text = $"{level}";
        lab_dead_nextLevel.text = _stageData.GetNextLevelText();

        Vector3 scale = gameProcessBar.transform.localScale;
        gameProcessBar.transform.localScale = new Vector3(0,scale.y, scale.z ); 
        gameProcessBar.transform.DOScaleX(_stageData._stageCompleteRate, 0.2f);
    }

    public void ShowBuyPanel(){
        buyPanel.SetActive(true);
        int needMoney = GameMeditor.Instance.GetHpNeedMoney();
        lab_NeedMoney.text = $"{needMoney}";

        int nowHp = GameMeditor.Instance.GetMaxHp();
        lab_heartNum.text = $"{nowHp} > {nowHp+1}";


        // 設定按鈕顏色
        ColorBlock cb= buyButt.colors;
        // 是否可以買關卡
        needMoney = GameMeditor.Instance.GetHpNeedMoney();
        if(GameMeditor.Instance.WhetherBuyStage(needMoney))
        {
            cb.normalColor = Color.white; // 按鈕設定為白色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
            buyButt.interactable = true; // 可以點擊
        }
        else
        {
            cb.normalColor = Color.gray; // 按鈕設定為灰色
            cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
            buyButt.interactable = false; // 無法點擊
        }
        buyButt.colors = cb; // 設定按鈕顏色
    }


    public void Close(){
        mainPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        completePanel.SetActive(false);
        deadPanel.SetActive(false);
        buyPanel.SetActive(false);
    }

    public void GoToNextLevel(){
        // 數據切到下一關
        _stageData.GoToNextLevel();
        GameMeditor.Instance.SetGameProcess(GameProcess.Start);
        Close();
    }

    public void GoToFirstLevel(){
        _stageData.GoToFirstLevel();
        GameMeditor.Instance.SetGameProcess(GameProcess.Start);
        Close();
    }

    public void Retry(){
        GameMeditor.Instance.SetGameProcess(GameProcess.Start);
        Close();
    }

    public void Leave(){
        Close();
        GameMeditor.Instance.LeaveStage();
    }

    public void BuyHp(){
        GameMeditor.Instance.BuyHp();
        ShowBuyPanel(); // 重置畫面
        GradeInfoUI.RefreshInfo();
    }


}
