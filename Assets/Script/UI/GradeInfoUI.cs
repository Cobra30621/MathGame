using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GradeInfoUI : MonoBehaviour
{
    private static GradeInfoUI instance;
    private IStageData _stageData;

    [SerializeField] private GameObject gameProcessBar, heartBar;
    [SerializeField] private Text lab_point , lab_Info, lab_nowLevel, lab_nextLevel, lab_heart;
    float TextShowDur = 1f;
    float lastGameProcess;
    float nowGameProcess;

    // 現在血量
    float nowHeart{
        get
        {
            if(_stageData.nowHeart <=0)
                return 0;
            else
                return _stageData.nowHeart;
        }
    }
    

    void Awake()
    {
        instance = this;
    }

    public static void Initialize(IStageData stageData){
        instance.SetStageData(stageData);
        instance.Refresh();
    }

    public void Init(IStageData stageData){
        SetStageData(stageData);
        lab_nowLevel.text = $"{stageData.stageLevel}";
        lab_nextLevel.text = $"{stageData.stageLevel + 1}";

        Vector3 scale = gameProcessBar.transform.localScale;
        gameProcessBar.transform.localScale = new Vector3(0,scale.y, scale.z );
        Refresh();
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

        // 進度條顯示
        gameProcessBar.transform.DOScaleX(_stageData._stageCompleteRate, 0.2f);

        // 血量顯示
        float maxHeart = _stageData.maxHeart;
        heartBar.transform.DOScaleX(nowHeart/maxHeart, 0.2f);
        lab_heart.text = $"{nowHeart}/{maxHeart}";
        
        // Debug.Log($"<color=B500FF></color>");
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
