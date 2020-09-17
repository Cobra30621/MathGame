using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GradeInfoUI : MonoBehaviour
{
    private static GradeInfoUI instance;
    private IStageData _stageData;

    [SerializeField] private GameObject gameProcessBar, heartBar, moneyIcon;
    [SerializeField] private Text lab_point , lab_Info, lab_nowLevel, lab_nextLevel, lab_heart, lab_process;
    [SerializeField] private  Text lab_money, lab_stageName;
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
        lab_nowLevel.text = $"{stageData.stageID}";
        lab_nextLevel.text = $"{stageData.stageID + 1}";
        lab_process.text = "0%";
        

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
        /*
        int point = GameMeditor.Instance.GetPoint();
        if(point < 0)
            lab_point.text = "$ 0";
        else
            lab_point.text = $"$ {point}";
        */
        int levelCount = _stageData.GetStageDataCount(); 
        int nowLevel = _stageData.stageID + 1;
        string stageName = _stageData._stageDataBox.stageName;
        lab_stageName.text =  $"{stageName}({nowLevel}/{levelCount})";
        
    
        int money = GameMeditor.Instance.GetMoney();
        lab_money.text = $"{money}";

        float rate = 0;    
        if(_stageData._stageCompleteRate <= 1)
            rate = _stageData._stageCompleteRate;
        else
            rate = 1;
        
        // 進度條顯示
        gameProcessBar.transform.DOScaleX(rate, 0.2f);
        lab_process.text = $"{_stageData._stageCompleteRateHun}%";

        // 血量顯示
        float maxHeart = _stageData.maxHeart;
        heartBar.transform.DOScaleX(nowHeart/maxHeart, 0.2f);
        lab_heart.text = $"血量：{nowHeart}/{maxHeart}";

        // 用ID表示關卡
        // lab_nowLevel.text = $"{_stageData.stageID + 1}";
        // lab_nextLevel.text = _stageData.GetNextLevelText();

        // lab_nowLevel.text = $"{_stageData.stageLevel}";
        // lab_nextLevel.text = $"{_stageData.stageLevel + 1}";
        
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

    // 賺錢時Icon變大
    public static void PlayChangeMoneyAnime(){
        instance.PlayScaleAnime(instance.moneyIcon);
        instance.PlayScaleAnime(instance.lab_money.gameObject);
    }

    private void PlayScaleAnime(GameObject gameObject){
        float rawScale = gameObject.transform.localScale.x;
        float newScale =  rawScale * 1.2f;
        Sequence scaleSequence = DOTween.Sequence()
        .Append(gameObject.transform.DOScale(newScale, 0.3f).SetEase(Ease.OutQuart))
        .Append(gameObject.transform.DOScale(rawScale, 0.3f).SetEase(Ease.OutQuart));
    }


}
