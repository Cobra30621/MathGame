using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameTimeUI : MonoBehaviour
{
    public static GameTimeUI instance;
    private BallCountProcessData _stageData;

    [SerializeField] private Slider gameProcessSlider;
    [SerializeField] private Text lab_point , lab_Info;
    float TextShowDur = 1f;

    void Awake()
    {
        instance = this;
    }

    public static void Initialize(BallCountProcessData stageData){
        instance.SetStageData(stageData);
        instance.Refresh();
    }

    public static void RefreshInfo(){
        instance.Refresh();
    }

    public void SetStageData(BallCountProcessData stageData){
        _stageData = stageData;
    }

    public void Refresh(){
        int point = GameMeditor.Instance.GetPoint();
        if(point < 0)
            lab_point.text = "$ 0";
        else
            lab_point.text = $"$ {point}";

        // 進度條顯示
        float gameProcess = (_stageData._stageCompleteRate) / 100;
        gameProcessSlider.value = gameProcess;
        
        
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

    public static void PlayTextShowOnceAnime(string str){
        instance.PlayTextOneAnime(str);
    }

    public void PlayTextOneAnime(string str){ // 播放文字動畫
        _stageData.AnimeHadFinish = false;
        
        Sequence moveSequence = DOTween.Sequence()
        .PrependCallback(() => TextAnimeInit(str)) // 更改文字，初始化動畫位置
        .Append(lab_Info.rectTransform.DOAnchorPos(new Vector2(0, 0), TextShowDur).SetEase(Ease.OutQuart))
        .AppendInterval(1);
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
