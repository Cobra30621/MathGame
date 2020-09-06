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

        // 製作初步動畫
        Sequence sequence = DOTween.Sequence() 
        .Append(BackGround.transform.DOScaleY(0, 0))
        .AppendInterval(0.5f)
        .Append(SetBackGroundAnime());
        
         // ========== 製作質數球清單 ==========
        // 設定質數球，最多3個
        int count = 0;
        if (primes.Length == 0 || stageData.P_prime == 0)
        {
            PrimePanel.SetActive(false); // 關閉清單
        }
        else
        {
            PrimePanel.SetActive(true); // 開啟清單
            for (int i =0; i<primes.Length; i++){
                if(count < 2)
                {
                    SetBall(primes[i], PrimeBalls, BallColor.Prime);
                    count ++;
                }            
            }
            SetBall(primes[primes.Length -1], PrimeBalls, BallColor.Prime);
            sequence.Append(SetPanelAnime(PrimeBalls));
        }

        
        

        // ========== 製作合數球清單 ==========
        if (composites.Length == 0 || stageData.P_composite == 0)
        {
            CompositePanel.SetActive(false); // 關閉清單
        }
        else
        {
            CompositePanel.SetActive(true); // 開啟清單
            // 設定合數球，最多3個
            count = 0;
            for (int i =0; i<composites.Length; i++){
                if(count < 2)
                {
                    SetBall(composites[i], CompsiteBalls, BallColor.Composite);
                    count ++;
                }            
            }
            SetBall(composites[primes.Length -1], CompsiteBalls, BallColor.Composite);
            sequence.Append(SetPanelAnime(CompsiteBalls));
        }
        
        // ========== 製作判斷球清單 ==========
        // 如果判斷球得出球機率為0則不顯示
        if ( stageData.P_JudgeComposite == 0 && stageData.P_JudgePrime == 0)
        {
            JudgePanel.SetActive(false); // 關閉清單
        }
        else
        {
            JudgePanel.SetActive(true); // 開啟清單
            // 設定合數球2個，質數球1個
            SetBall(primes[0], JudgeBalls, BallColor.Judge);
            SetBall(composites[0], JudgeBalls, BallColor.Judge);
            SetBall(composites[composites.Length -1], JudgeBalls, BallColor.Judge);

            sequence.Append(SetPanelAnime(JudgeBalls));
        }
        Debug.Log("PrimePanel:" + PrimePanel.GetComponent<RectTransform>().anchoredPosition);
        AnimeInit();  // 將物件移動

        // 新增等待點擊動畫
        sequence.AppendInterval(0.5f)
        .Append(SetTouchAnime());

        _stageData.AnimeHadFinish = false;
        Debug.Log("PrimePanel:" + PrimePanel.GetComponent<RectTransform>().anchoredPosition);
    }

    private void AnimeInit(){
        mainPanel.SetActive(true); // 打開Panel

        // 移動
        /*
        Debug.Log("PrimePanel:" + PrimePanel.GetComponent<RectTransform>().anchoredPosition);
        PrimePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(1000,200,0);
        CompositePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(1000,-200,0);
        JudgePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(1000,-200,0);
        Debug.Log("PrimePanel:" + PrimePanel.GetComponent<RectTransform>().anchoredPosition);
        */

        SetBigPanelActive(PrimePanel ,false);
        SetBigPanelActive(CompositePanel, false);
        SetBigPanelActive(JudgePanel, false);

        Color touchColor = Touch.color;
        touchColor.a = 0f;
        Touch.color = touchColor; 
        
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
    public Sequence SetPanelAnime(GameObject panel){ // 播放文字動畫
        GameObject BigPanel = panel.transform.parent.gameObject;
        Sequence sequence = DOTween.Sequence()
        .PrependCallback(() => PanelAnimeInit(BigPanel)) // 更改文字，初始化動畫位置
        .Append(BigPanel.GetComponent<RectTransform>().DOAnchorPosX(0, TextShowDur).SetEase(Ease.OutBack));
        return sequence;
    }

    private void PanelAnimeInit(GameObject panel){
        SetBigPanelActive(panel, true);

        Vector2 vec = panel.GetComponent<RectTransform>().anchoredPosition;
        vec.x = 800;
        panel.GetComponent<RectTransform>().anchoredPosition = vec;
    }

    // 將PrimePanel....裡的BallPanel與Text關掉或打開
    private void SetBigPanelActive(GameObject panel, bool bo){
        panel.transform.GetChild(0).gameObject.SetActive(bo);
        panel.transform.GetChild(1).gameObject.SetActive(bo);
    }

    public Sequence SetTouchAnime(){
        Sequence sequence = DOTween.Sequence()
        .Append(Touch.DOFade(1,1).SetEase(Ease.OutQuad).SetLoops(100, LoopType.Yoyo));
        return sequence;
    }

    public Sequence SetBackGroundAnime(){
        Sequence sequence = DOTween.Sequence()
        .Append(BackGround.transform.DOScaleY(1, 0.5f));
        return sequence;
    }


}
