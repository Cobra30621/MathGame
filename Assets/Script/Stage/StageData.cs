using UnityEngine;
using System.Collections.Generic;

// 遊戲流程
public enum GameProcess{
   Normol, TimeUp, BossComingAnime, NoNormalBall, BossTime, GameEnd, WaitStart
}

// 一般關卡資訊
public class StageData : IStageData 
{
    // 關卡資訊
    private string stageName;
    private int[] m_primes;
    private int[] m_composites;
    private int[] m_plusNums;
    private int[] m_bossNums;

    // 球產生的機率，為 P / 10
    private int m_P_prime = 4;
    private int m_P_composite = 4;
    
    // 產生球的間隔時間
	private float m_CoolDown = 0;		
	private float m_MaxCoolDown = 0;	// 
    private float m_startCoolDown = 1f;

    // 遊戲時間與流程
    private float m_gameTime = 0;
    private float m_MaxGameTime = 30f;

    private GameProcess m_gameProcess;


    // 其他
    private BallFactory ballFactory;
    public bool hasSet = false;

	// 設定關卡資訊
	public StageData(float CoolDown , string name, int[] primes, int[] composites, int[] plusNums, int[] bossNums)
	{
		m_MaxCoolDown = CoolDown;
		m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;
        stageName = name;

        m_primes = primes;
        m_composites = composites;
        m_bossNums = bossNums;
        m_plusNums = plusNums;
        hasSet = true;
	}

	// 重置
	public override	void Reset()
	{
        m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;
        SetGameProcess(GameProcess.Normol);
	}

	// 更新
	public override void Update()
	{
        PlayGameProcess();
	}

    // ------------------ 關卡流程 ---------------------- 
    public void PlayGameProcess(){
        switch(m_gameProcess){
            case GameProcess.Normol : // 遊戲開始
                NormalProcess();
                break;
            case GameProcess.TimeUp : // 時間到，等待場上沒球
                WaitNoBallProcess();
                break;
            case GameProcess.NoNormalBall :
                PlayBossComingAnime();  // 播放BossComingAnime
                break;
            case GameProcess.BossComingAnime : // 等待BossComingAnime播完
                break;
            case GameProcess.BossTime :  // 以產生Boss球，等待場上沒球
                WaitNoBossBallProcess();
                break;
            case GameProcess.GameEnd :  // 遊戲結束
                GameEndProcess();
                break;
            case GameProcess.WaitStart : // 等待遊戲開始
                break;
            default:
                Debug.LogError("出現異常程序："+ m_gameProcess);
                break;
        }
    }

    // 時間倒數流程
    public void NormalProcess(){
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
        CreateBall();
    }

    public void WaitNoBallProcess(){
        int ballCount = GameMeditor.Instance.GetBallCount();
        if (ballCount == 0)
            SetGameProcess(GameProcess.NoNormalBall);

    }

    public void PlayBossComingAnime(){
        GradeInfoUI.PlayBossComingAnime();
        SetGameProcess(GameProcess.BossComingAnime);
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

    public void GameEndProcess(){
         GradeInfoUI.ShowWhetherFullCombol(); // 顯示是否Full Combol
         SetGameProcess(GameProcess.WaitStart);
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

    public void SetBallProbability(int P_prime, int P_composite){
        m_P_prime = P_prime;
        m_P_composite = P_composite;

    }

    // ------------------產生球相關方法-----------------------

    public void CreateBall(){ // 隨機產生球
        int PrimeOrComposite = Random.Range(0,10);
        if(PrimeOrComposite >= 0 && PrimeOrComposite < m_P_prime) // 2/5
            CreatePrimeBall();
        else if(PrimeOrComposite >= m_P_prime && PrimeOrComposite < m_P_prime + m_P_composite) // 2/5
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
        ballFactory.CreateBall(num);
    }

    public void CreateCompositeBall(){ // 建立合數球
        if(m_composites.Length == 0){
            Debug.Log("Composites是空的");
            return;
        }
        int index = Random.Range(0, m_composites.Length);
        int num = m_composites[index];
        ballFactory.CreateBall(num);
    }

    public void CreatePlusBall(){ // 建立加分數球
        if(m_plusNums.Length == 0){
            Debug.Log("PlusBalls是空的");
            return;
        }
        int index = Random.Range(0, m_plusNums.Length);
        int num = m_plusNums[index];
        ballFactory.CreatePlusBall(num);
    }

    public void CreateBossBall(){ // 建立魔王球
        if(m_bossNums.Length == 0){
            Debug.LogError("BossBalls是空的");
            return;
        }
        int index = Random.Range(0, m_bossNums.Length);
        int num = m_bossNums[index];
        ballFactory.CreateBossBall(num);
    }
}
