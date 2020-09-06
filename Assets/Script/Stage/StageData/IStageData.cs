using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// 遊戲流程
public enum GameProcess{
   	Start,WaitTouch, StartAnime, NormalTime,  TimeUp, NoNormalBall, BossTime, EndAnime, GameEnd, WaitStart
}

// 關卡解鎖狀態
public enum StageState{
    Open, NeedMoney , MoneyEnough
}

// 關卡完成度
public enum StageComplete{
    UnComplete, Complete, FullCombol
}

// 一般關卡資訊
public class IStageData 
{
    // 關卡資訊
    public string stageName;
    public int[] m_primes;
    public int[] m_composites;
    public int[] m_plusNums;
    public int[] m_bossNums;

    // 球產生的機率，為 P / 10
    public int P_JudgePrime = 4;
    public int P_JudgeComposite = 4;

    public int P_prime = 0;
    public int P_composite = 0;
    public int P_PlusBall =2;
    
    // 產生球的間隔時間
	public float m_CoolDown = 0;		
	public float m_MaxCoolDown = 0;	// 
    public float m_startCoolDown = 1f;
    public float m_ballSpeed = 1f; // 球掉落速度

    // 遊戲時間與流程
    public float m_gameTime = 0;
    public float m_MaxGameTime = 30f;
    public GameProcess m_gameProcess;
    public bool AnimeHasPlay;
    public bool AnimeHadFinish;

    public string _startText = "採集紅番茄!";
    public string _bossComingText =  "彩虹番茄要來了!";
    public string _endText =  "Game End!";

    // 解鎖相關
    public StageState m_stageState;
    public int m_stagePrice = 0;

	// 分數相關
    public StageComplete m_stageComplete; 
    public int point;
    public int combol;
    public int missCombol;
    public int accumulateCombol;
    public int MaxCombol;
	public int m_bestPoint;
    public int completionRate;

    public BGM m_bgm;
    public IBallStrategy ballStrategy;

	// 其他
    public BallFactory ballFactory;
    public bool hasSet = false;

	// ================= 初始設定方法 ===================
	public IStageData(float CoolDown , string name, int[] primes, int[] composites, int[] plusNums, int[] bossNums)
	{
		// 設定冷卻時間
		m_MaxCoolDown = CoolDown;
		m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;

		// 設定關卡資訊
        stageName = name;
        m_primes = primes;
        m_composites = composites;
        m_bossNums = bossNums;
        m_plusNums = plusNums;

		// 設定遊戲狀況
		m_stageState = StageState.Open;
		m_stageComplete = StageComplete.UnComplete;
		m_bestPoint = 0;

        m_bgm = BGM.Normal;

        // 設定球回擊模式，預設回擊兩顆
        ballStrategy = new CompositeToTwoStrategy();

        // 遊戲流程
        m_gameProcess = GameProcess.WaitStart;
		hasSet = true;
	}

    // 設定球的機率
	public void SetBallProbability(int P_prime, int P_composite){
        this.P_JudgePrime = P_prime;
        this.P_JudgeComposite = P_composite;

    }

	// 設定球的機率 
	public void SetBallProbability(int P_JudgePrime, int P_JudgeComposite, int P_prime, int P_composite){
        this.P_JudgePrime = P_JudgePrime;
        this.P_JudgeComposite = P_JudgeComposite;
        this.P_prime = P_prime;
        this.P_composite = P_composite;

    }

	// 設定解鎖所需要的錢
	public void SetStagePrice(int money){
		m_stagePrice = money;
        m_stageState = StageState.NeedMoney;
	}

    // 設定bgm;
    public void SetBGM(BGM bgm){
        m_bgm = bgm;
    }

    public void SetBallSpeed(float speed){
        m_ballSpeed = speed;
    }

    // ===========設定使用的策略=======
    public delegate void SetBallStrategyHandler(IBallStrategy ballStrategy);

    public static SetBallStrategyHandler SetBallStrategy;

    public void SetStrategy(){
        if (SetBallStrategy != null)
        {
            Debug.Log("設定策略"+  ballStrategy);
            SetBallStrategy(ballStrategy);
        }
    }

    public void SetStageBallStrategy(IBallStrategy ballStrategy){
        this.ballStrategy = ballStrategy;
    }

	// ================= 重置 ===================

	// 重置關卡
	public void Reset()
	{
        m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;
        point = 0;
        combol = 0;
        missCombol = 0;
        MaxCombol = 0;
        accumulateCombol = 0;

        // 設定球回擊策略
        SetStrategy( );
        // SetGameProcess(GameProcess.Start); // 遊戲流程變成開始
        MusicManager.StopMusic();  // 停止音樂
        GameMeditor.Instance.RemoveAllBall(); // 清除所有的球
        GradeInfoUI.Initialize(this); // 重置分數介面
        InitAnimeVariable();
	}

    // 離開關卡
    public void LeaveStage(){
        MusicManager.StopMusic();  // 停止音樂
        GameMeditor.Instance.RemoveAllBall(); // 清除所有的球
        SetGameProcess(GameProcess.WaitStart);  // 遊戲流程改成等待開始
        SceneManager.LoadScene("StageSelect");
    }

	// 更新
	public void Update()
	{
        PlayGameProcess();
	}

    // ------------------ 關卡流程 ---------------------- 
    public virtual void PlayGameProcess(){}

	public virtual void GameStartProcess(){
        Reset();
        // GradeInfoUI.PlayGameStartAnime();
        MusicManager.PlayMusic();
        // SetGameProcess( GameProcess.StartAnime);
        SetGameProcess( GameProcess.WaitTouch);
        PhoneInputUI.EnableMoveBar(false); // 無法透過點擊螢幕來移動Bar
	}

    public virtual void GameWaitTouchProcess(){
        if(!AnimeHasPlay)
        {
            AnimeHasPlay = true;
            // GradeInfoUI.PlayTextShowAnime(_startText);
            StartPanel.Show(this);
        }
        if(AnimeHadFinish)
        {
            SetGameProcess( GameProcess.StartAnime);
            InitAnimeVariable();
            PhoneInputUI.EnableMoveBar(true); // 可以透過點擊螢幕來移動Bar
        }
    }


    public void GameStartAnimeProcess(){
        if(!AnimeHasPlay)
        {
            AnimeHasPlay = true;
            GradeInfoUI.PlayTextShowAnime(_startText);
            // StartPanel.Show(this);
        }
        if(AnimeHadFinish)
        {
            SetGameProcess( GameProcess.NormalTime);
            InitAnimeVariable();
            PhoneInputUI.EnableMoveBar(true); // 可以透過點擊螢幕來移動Bar
        }
    }


    public void InitAnimeVariable(){
        AnimeHasPlay = false;
        AnimeHadFinish = false;
    }

    // 時間倒數流程
    public void NormalTimeProcess(){
        
        // 判斷時間否到了
        if (m_gameTime <= 0){
            m_gameTime = 0;
            SetGameProcess( GameProcess.TimeUp);
        }
        // 更新遊戲時間
        m_gameTime -= Time.deltaTime;
        GradeInfoUI.UpdateTime(); // 更新介面時間

		// 是否可以產生球
		m_CoolDown -= Time.deltaTime;
		if( m_CoolDown > 0)
			return ;
		m_CoolDown = m_MaxCoolDown;

        // 產生球
        ballFactory = MainFactory.GetBallFactory();
        ballFactory.SetBallSpeed(m_ballSpeed); // 設定球掉落速度
        CreateBall();
    }

    public virtual void WaitNoBallProcess(){
        int ballCount = GameMeditor.Instance.GetBallCount();
        if (ballCount == 0)
            SetGameProcess(GameProcess.NoNormalBall);

    }

    public void PlayBossComingAnime(){
        if(!AnimeHasPlay)
        {
            AnimeHasPlay = true;
            GradeInfoUI.PlayTextShowOnceAnime(_bossComingText);
        }
        if(AnimeHadFinish)
        {
            CreateBossBall(); // 產生魔王球
            SetGameProcess(GameProcess.BossTime);
            InitAnimeVariable();
        }
    }

    public void BossComingAnimeEnd(){   // 由anime去操控，產生Boss球
        CreateBossBall(); // 產生魔王球
        SetGameProcess(GameProcess.BossTime);
    }

    public void WaitNoBossBallProcess(){
        int ballCount = GameMeditor.Instance.GetBallCount();
        if (ballCount == 0)
            SetGameProcess(GameProcess.GameEnd);
    }

    public void EndAnimeProcess(){
        if(!AnimeHasPlay)
        {
            AnimeHasPlay = true;
            GradeInfoUI.PlayTextShowAnime(_endText);
        }
        if(AnimeHadFinish)
        {
            SetGameProcess(GameProcess.GameEnd);
            InitAnimeVariable();
        }
    }

    public void GameEndProcess(){

        int point = GameMeditor.Instance.GetPoint();

        JudgeStateComplete(); // 紀錄遊戲完成度
        int completerate = GetCompletionRate(); // 取得完成度
        if(completerate > m_bestPoint)
            m_bestPoint = completerate; // 記下最高紀錄


        // 增加錢
        int money = point;
        if(missCombol == 0)
            money *= 2;  // Full Combol 錢兩倍
        GameMeditor.Instance.AddMoney(point); // 增加錢
        PriceUI.Show(); // 開啟獎勵界面
        MusicManager.StopMusic(); // 關掉音樂，播放獎勵音樂
        PhoneInputUI.EnableMoveBar(false); // 無法透過點擊螢幕來移動Bar

        SetGameProcess(GameProcess.WaitStart);
    }

    public void JudgeStateComplete(){
        switch (m_stageComplete){
            case StageComplete.FullCombol:
                break;
            case StageComplete.Complete:
                if (GameMeditor.Instance.GetMissCombol() == 0)
                    m_stageComplete = StageComplete.FullCombol;
                break;
            case StageComplete.UnComplete:
                m_stageComplete = StageComplete.Complete;
                if (GameMeditor.Instance.GetMissCombol() == 0)
                    m_stageComplete = StageComplete.FullCombol;
                break;
        }
    }

    // 設置遊戲狀態

    public void SetGameProcess(GameProcess gameProcess){
        m_gameProcess = gameProcess;
    }

    // ---------------取的關卡資料------------------

    public float GetGameTime(){
        return m_gameTime;
    }

    public string GetStageName(){
        return stageName;
    }

    // ---------------取的關卡資料------------------


    // ------------------產生球相關方法-----------------------

    public void CreateBall(){ // 隨機產生球，方法超智障
        float PrimeOrComposite = Random.Range(0f,10f);

        int P_jp = P_JudgePrime;
        int P_jc = P_JudgePrime + P_JudgeComposite;
        int P_p = P_JudgePrime + P_JudgeComposite + P_prime;
        int P_c = P_JudgePrime + P_JudgeComposite + P_prime + P_composite;

        Debug.Log($"P_jp:{P_jp}");
        Debug.Log($"P_jc:{P_jc}");
        Debug.Log($"P_p:{P_p}");
        Debug.Log($"P_c:{P_c}");

        if(PrimeOrComposite >= 0 && PrimeOrComposite < P_jp) // 2/5
            CreatePrimeJudgeBall();
        else if(PrimeOrComposite >= P_jp && PrimeOrComposite < P_jc) // 2/5
            CreateCompositeJudgeBall();
        else if(PrimeOrComposite >=  P_jc && PrimeOrComposite < P_p) // 2/5
            CreatePrimeBall();
        else if(PrimeOrComposite >=  P_p && PrimeOrComposite <  P_c) // 2/5
            CreateCompositeBall();
        else // 1/5
            CreatePlusBall();      
    }

    public void CreatePrimeBall(){ // 建立質數球
        if(m_primes.Length == 0){
            Debug.Log("primes是空的");
            return;
        }
        int index = Random.Range(0, m_primes.Length);
        int num = m_primes[index];
        AddMaxCombol(num); // 每次產生球，累積最大Combol數
        ballStrategy.CreateBall(num, BallColor.Prime);
    }

    public void CreatePrimeJudgeBall(){ // 建立質數球
        if(m_primes.Length == 0){
            Debug.Log("primes是空的");
            return;
        }
        int index = Random.Range(0, m_primes.Length);
        int num = m_primes[index];
        AddMaxCombol(num); // 每次產生球，累積最大Combol數
        ballStrategy.CreateBall(num, BallColor.Judge);
    }

    public void CreateCompositeBall(){ // 建立合數球
        if(m_composites.Length == 0){
            Debug.Log("Composites是空的");
            return;
        }
        int index = Random.Range(0, m_composites.Length);
        int num = m_composites[index];
        AddMaxCombol(num); // 每次產生球，累積最大Combol數
        ballStrategy.CreateBall(num, BallColor.Composite);
    }

    public void CreateCompositeJudgeBall(){ // 建立判斷合數合數球
        if(m_composites.Length == 0){
            Debug.Log("Composites是空的");
            return;
        }
        int index = Random.Range(0, m_composites.Length);
        int num = m_composites[index];
        AddMaxCombol(num); // 每次產生球，累積最大Combol數
        ballStrategy.CreateBall(num, BallColor.Judge);
    }

    public void CreatePlusBall(){ // 建立大合數球
        if(m_plusNums.Length == 0){
            Debug.Log("PlusBalls是空的");
            return;
        }
        int index = Random.Range(0, m_plusNums.Length);
        int num = m_plusNums[index];
        AddMaxCombol(num); // 每次產生球，累積最大Combol數
        ballStrategy.CreateBall(num, BallColor.Composite);
    }

    public void CreateBossBall(){ // 建立魔王球
        if(m_bossNums.Length == 0){
            Debug.LogError("BossBalls是空的");
            return;
        }
        int index = Random.Range(0, m_bossNums.Length);
        int num = m_bossNums[index];
        AddMaxCombol(num); // 每次產生球，累積最大Combol數
        ballStrategy.CreateBall(num, BallColor.Boss);
    }

    // 累積最大Combol數
    public void AddMaxCombol(int num){
        // int max = ballFactory.GetNumMaxCombol(num);
        int max = ballStrategy.GetMaxCombol(num); // 取的球的最大值
        MaxCombol += max;
        Debug.Log($"Num{num}累積{max}");
        Debug.Log($"完成率:{accumulateCombol}/{MaxCombol}");
    }

    // 給予完成率
    public int GetCompletionRate(){
        float rawCompletionRate = (accumulateCombol * 100) / MaxCombol;
        int rate = Mathf.RoundToInt(rawCompletionRate);
        Debug.Log($"完成率:{accumulateCombol}/{MaxCombol}{rate}%");
        return rate;
    }
}
