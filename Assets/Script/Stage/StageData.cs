using UnityEngine;
using System.Collections.Generic;

// 一般關卡資訊
public class StageData : IStageData 
{
	private float m_CoolDown = 0;		// 產生球的間隔時間
	private float m_MaxCoolDown = 0;	// 
    private float m_gameTime = 0;
    private float m_MaxGameTime = 30f;
	private bool timeUp = false;
    private int[] m_primes;
    private int[] m_composites;
    private int[] m_bossNums;
    private BallFactory ballFactory;
    public bool hasSet = false;
    private bool hasCreateBoss = false;

	// 設定相關數據
	public StageData(float CoolDown ,int[] primes, int[] composites, int[] bossNums)
	{
		m_MaxCoolDown = CoolDown;
		m_CoolDown = m_MaxCoolDown;
        m_gameTime = m_MaxGameTime;

        m_primes = primes;
        m_composites = composites;
        m_bossNums = bossNums;
        hasSet = true;
	}

	// 重置
	public override	void Reset()
	{
        m_CoolDown = m_MaxCoolDown;
        m_gameTime = m_MaxGameTime;
		timeUp = false;
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
            timeUp = true;
            return;
        }
        m_gameTime -= Time.deltaTime;

		// 是否可以產生
		m_CoolDown -= Time.deltaTime;
		if( m_CoolDown > 0)
			return ;
		m_CoolDown = m_MaxCoolDown;

        ballFactory = MainFactory.GetBallFactory();
        CreateBall();
	}

    public void CreateBall(){
        int PrimeOrComposite = Random.Range(0,2);
        if(PrimeOrComposite == 0)
            CreatePrimeBall();
        else
            CreateCompositeBall();        
    }

    public void CreatePrimeBall(){
        if(m_primes.Length == 0){
            Debug.Log("primes是空的");
            return;
        }
        int index = Random.Range(0, m_primes.Length);
        int num = m_primes[index];
        ballFactory.CreateBall(num);
    }

    public void CreateCompositeBall(){
        if(m_composites.Length == 0){
            Debug.Log("primes是空的");
            return;
        }
        int index = Random.Range(0, m_composites.Length);
        int num = m_composites[index];
        ballFactory.CreateBall(num);
    }

    public void CreateBossBall(){
        if(m_bossNums.Length == 0){
            Debug.Log("primes是空的");
            return;
        }
        int index = Random.Range(0, m_bossNums.Length);
        int num = m_bossNums[index];
        ballFactory.CreateBall(num);
    }

    public float GetGameTime(){
        return m_gameTime;
    }

	// 是否完成
	public override	bool IsFinished()
	{
		return timeUp;
	}
}
