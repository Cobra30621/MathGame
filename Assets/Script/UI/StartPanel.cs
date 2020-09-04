using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : MonoBehaviour
{
    private static StartPanel instance;
    [SerializeField] private GameObject mainPanel, PrimeBalls, CompsiteBalls, JudgeBalls;
    [SerializeField] private GameObject PrimePanel, CompositePanel , JudgePanel ;
    [SerializeField] private GameObject BackGround;
    [SerializeField] private Text Touch;
    [SerializeField] private GameObject ShowBallPrefab;
    [SerializeField] private Button StartBut;
    public List<GameObject> Balls ;
    private BallFactory factory;
    private IStageData _stageData;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
        Initiaize(); // 取得BallPrefab
        factory = MainFactory.GetBallFactory();
    }

    private void Initiaize(){
        if (ShowBallPrefab == null)
            SetBallPrefabs();
    }

    private void SetBallPrefabs(){
        ResourceAssetFactory resourceAssetFactory = MainFactory.GetResourceAssetFactory();
        ShowBallPrefab = resourceAssetFactory.LoadBallPrefab("ShowBall");
        
    }

    public static void Show(IStageData stageData){
        instance.PlayAnime(stageData);
    }

    // 顯示動畫
    public void PlayAnime(IStageData stageData ){
        _stageData = stageData;
        ClearBall();
         
        int[] primes = stageData.m_primes;
        int[] composites = stageData.m_composites;

        // 設定質數球，最多3個
        int count = 0;
        for (int i =0; i<primes.Length; i++){
            if(count < 3)
            {
                SetBall(primes[i], PrimeBalls, BallColor.Prime);
                count ++;
            }            
        }

        // 設定合數球，最多3個
        count = 0;
        for (int i =0; i<composites.Length; i++){
            if(count < 3)
            {
                SetBall(composites[i], CompsiteBalls, BallColor.Composite);
                count ++;
            }            
        }

        _stageData.AnimeHadFinish = false;

        Sequence sequence = DOTween.Sequence()
        .PrependCallback(() => AnimeInit()) 
        .AppendInterval(0.5f)
        .Append(SetBackGroundAnime())
        .Append(SetPanelAnime(PrimeBalls, new Vector2(0,200)))
        .Append(SetPanelAnime(CompsiteBalls, new Vector2(0,-200)))
        .AppendInterval(0.5f)
        .Append(SetTouchAnime());
        
    }

    private void AnimeInit(){
        PrimePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(1000,200,0);
        CompositePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(1000,-200,0);
        
        // 將按鈕的透明度設定為0
        Color touchColor = Touch.color;
        touchColor.a = 0f;
        Touch.color = touchColor; 

        mainPanel.SetActive(true); // 打開Panel
    }

   

    public void GameStart(){
        mainPanel.SetActive(false); // 關閉Panel
        _stageData.AnimeHadFinish = true; // 傳通知給遊戲
        
    }


    // ===========設定球的模型============
    public void SetBall(int num, GameObject panel, BallColor color){
        // 建立物件模組
        GameObject ball ;
        ball= GameObject.Instantiate( ShowBallPrefab , panel.transform);
        ball.GetComponent<ShowBall>().SetNum(num); 
        ball.GetComponent<ShowBall>().SetImage(factory.GetBallImage(color));
        Balls.Add(ball);
    }

    private void ClearBall(){
        if (Balls.Count == 0)
            return;
        foreach(GameObject ball in Balls){
            GameObject.Destroy(ball); 
        }
    }

    // ==================== 設定播放動畫===================

    float TextShowDur = 0.5f;
    public Sequence SetPanelAnime(GameObject panel, Vector2 vec){ // 播放文字動畫
        GameObject BigPanel = panel.transform.parent.gameObject;
        Sequence sequence = DOTween.Sequence()
        .PrependCallback(() => PanelAnimeInit(BigPanel, vec)) // 更改文字，初始化動畫位置
        //.Append(panel.transform.DOMove(new Vector2(0, 0), TextShowDur).SetEase(Ease.OutQuart));
        .Append(BigPanel.GetComponent<RectTransform>().DOAnchorPos(vec, TextShowDur).SetEase(Ease.OutBack));
        return sequence;
    }

    private void PanelAnimeInit(GameObject panel, Vector2 vec){
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector3(700, vec.y ,0);
        // .anchoredPosition
    }

    public Sequence SetTouchAnime(){
        Sequence sequence = DOTween.Sequence()
        .Append(Touch.DOFade(1,1).SetEase(Ease.OutQuad).SetLoops(100, LoopType.Yoyo));
        return sequence;
    }

    public Sequence SetBackGroundAnime(){
        Sequence sequence = DOTween.Sequence()
        .Append(BackGround.transform.DOScaleY(0, 0))
        .Append(BackGround.transform.DOScaleY(1, 0.5f));
        return sequence;
    }


}
