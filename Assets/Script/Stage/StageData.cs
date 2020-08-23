using UnityEngine;
using System.Collections.Generic;

// 一般關卡資訊
public class StageData : IStageData 
{
	private float m_CoolDown = 0;		// 產生球的間隔時間
	private float m_MaxCoolDown = 0;	// 
    private float m_startCoolDown = 1f;
    private float m_gameTime = 0;
    private float m_MaxGameTime = 30f;
    private int missConbol;
	private bool timeUp = false;
    private int[] m_primes;
    private int[] m_composites;
    private int[] m_plusNums;
    private int[] m_bossNums;
    private string stageName;
    private BallFactory ballFactory;
    public bool hasSet = false;
    private bool hasCreateBoss = false;

	// 設定相關數據
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
        missConbol = 0;
        hasSet = true;
	}

	// 重置
	public override	void Reset()
	{
        m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;
		timeUp = false;
        missConbol = 0;
        hasCreateBoss = false;
	}

	// 更新
	public override void Update()
	{
        if (m_gameTime <= 0){
            m_gameTime = 0;
            if (hasCreateBoss == false){
                CreateBossBall();
                hasCreateBoss = true;
            } 

            if (hasCreateBoss == true && timeUp == true) // 跑結束畫面
            {
                int BallCounts = GameMeditor.Instance.GetBallCount();
                if(BallCounts == 0)
                    GameEnd(); // 遊戲結束
            }
            timeUp = true;
            return;
        }
        m_gameTime -= Time.deltaTime;
        GradeInfoUI.UpdateTime(); // 更新介面時間

		// 是否可以產生
		m_CoolDown -= Time.deltaTime;
		if( m_CoolDown > 0)
			return ;
		m_CoolDown = m_MaxCoolDown;

        ballFactory = MainFactory.GetBallFactory();
        CreateBall();
	}

    public void GameEnd(){
        GradeInfoUI.ShowWhetherFullCombol(); // 顯示是否Full Combol
    }

    public void CreateBall(){ // 隨機產生球
        int PrimeOrComposite = Random.Range(0,5);
        if(PrimeOrComposite >= 0 && PrimeOrComposite < 2) // 2/5
            CreatePrimeBall();
        else if(PrimeOrComposite >= 2 && PrimeOrComposite < 4) // 2/5
            CreateCompositeBall();
        else // 1/5
            CreatePlusBall();      
    }

    public void CreatePrimeBall(){ // 建立質數球
        if(m_primes.Length == 0){
            Debug.Log("primes是空的，產生plusBall");
            CreatePlusBall();
            return;
        }
        int index = Random.Range(0, m_primes.Length);
        int num = m_primes[index];
        ballFactory.CreateBall(num);
    }

    public void CreateCompositeBall(){ // 建立合數球
        if(m_composites.Length == 0){
            Debug.Log("Composites是空的，產生plusBall");
            CreatePlusBall();
            return;
        }
        int index = Random.Range(0, m_composites.Length);
        int num = m_composites[index];
        ballFactory.CreateBall(num);
    }

    public void CreatePlusBall(){ // 建立加分數球
        if(m_plusNums.Length == 0){
            Debug.Log("PlusBalls是空的，改產生compositeBall");
            CreateCompositeBall();
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

    public float GetGameTime(){
        return m_gameTime;
    }

    public void AddMissCombol(){
        missConbol ++;
    }

    public int GetMissCombol(){
        return missConbol;
    }

	// 是否完成
	public override	bool IsFinished()
	{
		return timeUp;
	}

    public string GetStageName(){
        return stageName;
    }
}
