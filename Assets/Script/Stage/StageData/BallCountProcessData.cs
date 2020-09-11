using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// 一般關卡資訊
public class BallCountProcessData : IStageData
{
    



    // ================= 初始設定方法 ===================
	public BallCountProcessData(float CoolDown , string name, int ballCount, int[] primes, int[] composites):base(CoolDown , name, ballCount, primes, composites)
	{
		// 設定冷卻時間
		m_MaxCoolDown = CoolDown;
		m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;

        // 球的數量
        maxBallCount = ballCount;
        hasCreateBallCount = 0;
        hasHitBallCount = 0;

		// 設定關卡資訊
        stageName = name;
        m_primes = primes;
        m_composites = composites;

		// 設定遊戲狀況
		m_stageState = StageState.Lock; // 預設為關閉
		m_stageComplete = StageComplete.UnComplete;
		m_bestPoint = 0;

        m_bgm = BGM.Normal;

        // 設定球回擊模式，預設回擊兩顆
        ballStrategy = new BarCatchPrimeStrategy();

        // 遊戲流程
        m_gameProcess = GameProcess.WaitStart;
		hasSet = true;
	}

    public override void PlayGameProcess(){
        switch(m_gameProcess){
            case GameProcess.Start:
                GameStartProcess();
                break;
            case GameProcess.WaitTouch:
                GameWaitTouchProcess();
                break;
            case GameProcess.StartAnime:
                GameStartAnimeProcess();
                break;
            case GameProcess.NormalTime : // 遊戲開始
                NormalTimeProcess();
                break;
            case GameProcess.TimeUp : // 時間到，等待場上沒球
                WaitNoBallProcess();
                break;
            case GameProcess.EndAnime : // 播放結束動畫
                EndAnimeProcess();
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
    public override void NormalTimeProcess(){
        // 判斷是否打完球了
        if (hasHitBallCount == maxBallCount){
            SetGameProcess( GameProcess.TimeUp);
        }

		// 球是否生產完了
        if(hasCreateBallCount == maxBallCount)
            return;

        // 球的冷卻時間
		m_CoolDown -= Time.deltaTime;
		if( m_CoolDown > 0)
			return ;
		m_CoolDown = m_MaxCoolDown;

        // 產生球
        ballFactory = MainFactory.GetBallFactory();
        ballFactory.SetBallSpeed(m_ballSpeed); // 設定球掉落速度
        CreateBall();
        hasCreateBallCount++; // 記錄多生產球
    }

    public override void WaitNoBallProcess(){
        int ballCount = GameMeditor.Instance.GetBallCount();
        if (ballCount == 0)
            SetGameProcess(GameProcess.EndAnime); // 遊戲結束

    }

    public override void Reset()
	{
        // 球的數量
        hasCreateBallCount = 0;
        hasHitBallCount = 0;

        // 繼承原本的Reset
        base.Reset();
	}

    
}

