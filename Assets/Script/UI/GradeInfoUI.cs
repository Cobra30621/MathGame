using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GradeInfoUI : MonoBehaviour
{
    private static GradeInfoUI instance;
    private Animator m_animator;
    private IStageData _stageData;

    [SerializeField] private Text lab_point, lab_combol, lab_Time, lab_FullCombol, lab_stageName, lab_Info;

    float TextShowDur = 1.5f;

    void Awake()
    {
        instance = this;
        // m_animator = GetComponent<Animator>();
        // Refresh();
    }

    public static void Initialize(IStageData stageData){
        instance.SetStageData(stageData);
        instance.Refresh();
    }

    public static void RefreshInfo(){
        instance.Refresh();
    }

    public void SetStageData(IStageData stageData){
        _stageData = stageData;
    }

    public void Refresh(){
        int point = GameMeditor.Instance.GetPoint();
        if(point < 0)
            lab_point.text = "$ 0";
        else
            lab_point.text = $"$ {point}";

        int combol = GameMeditor.Instance.GetCombol();
        lab_combol.text = $"{combol}";

        /* combol加成
        int combolPlus = combol * 10;
        if (combol == 0)
            lab_combolPlus.text = "";
        else
            lab_combolPlus.text = $"+{combolPlus}%";
        */

        string stageName = GameMeditor.Instance.GetStageName();
        lab_stageName.text = stageName;

        RefreshTime();// 更新時間
        
    }

    public static void UpdateTime(){
        instance.RefreshTime();
    }

    public void RefreshTime(){
        float Rawtime = GameMeditor.Instance.GetGameTime();
        int time;
        if (Rawtime <= 0)
            time = 0;
        else
            time = Mathf.FloorToInt(Rawtime) + 1;

        if(time >= 31) // 避免時間顯示超過max
            time = 0;

        lab_Time.text = $"{time}";
    }

    // =================動畫庫=================

    public static void PlayTextShowAnime(string str){
        instance.PlayTextAnime(str);
    }

    public void PlayTextAnime(string str){ // 播放文字動畫
        _stageData.AnimeHadFinish = false;
        
        Sequence moveSequence = DOTween.Sequence()
        .PrependCallback(() => TextAnimeInit(str)) // 更改文字，初始化動畫位置
        .Append(lab_Info.rectTransform.DOAnchorPos(new Vector2(0, 0), TextShowDur).SetEase(Ease.OutQuart))
        .AppendInterval(1)
        .Append(lab_Info.rectTransform.DOAnchorPos(new Vector2(-900, 0), TextShowDur).SetEase(Ease.OutQuart));

        moveSequence.OnComplete(TextAnimeEnd); 
    }

    private void TextAnimeInit(string str){
        lab_Info.text = str;
        lab_Info.transform.position = new Vector3(900, 0 ,0);
    }

    private void TextAnimeEnd(){
        lab_Info.transform.position = new Vector3(900, 0 ,0);
        _stageData.AnimeHadFinish = true;
    }


}
