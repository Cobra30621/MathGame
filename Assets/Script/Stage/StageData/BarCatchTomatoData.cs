using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// 一般關卡資訊
public class BallCatchTomatoData : IStageData
{

    // ================= 初始設定方法 ===================
	public BallCatchTomatoData(float CoolDown , string name, int ballCount, int[] primes, int[] composites):base(CoolDown , name, ballCount, primes, composites)
	{
		// 設定冷卻時間
		m_MaxCoolDown = CoolDown;
		m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;

        // 球的數量
        maxBallCount = ballCount;
        hasHitBallCount = 0;

		// 設定關卡資訊
        stageName = name;
        m_primes = primes;
        m_composites = composites;
        _startText = $"採集{maxBallCount}顆成熟(質數)番茄";

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

    public BallCatchTomatoData(float CoolDown , int ballCount, int[] primes, int[] composites):base(CoolDown , ballCount, primes, composites)
	{
		// 設定冷卻時間
		m_MaxCoolDown = CoolDown;
		m_CoolDown = m_startCoolDown;
        m_gameTime = m_MaxGameTime;

        // 球的數量
        maxBallCount = ballCount;
        hasHitBallCount = 0;

		// 設定關卡資訊
        m_primes = primes;
        m_composites = composites;
        _startText = $"採集{maxBallCount}顆成熟(質數)番茄";

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
            case GameProcess.GameOver :  // 遊戲結束
                GameOverProcess();
                break;        
            case GameProcess.WaitStart : // 等待遊戲開始
                break;
            default:
                Debug.LogError("出現異常程序："+ m_gameProcess);
                break;
        }
    }

    public override void GameStartProcess(){
        Reset();
        MusicManager.SwitchMusic(m_bgm);
        MusicManager.PlayMusic();
        SetGameProcess( GameProcess.StartAnime);
	}


    public override void WaitNoBallProcess(){
        int ballCount = GameMeditor.Instance.GetBallCount();
        if (ballCount == 0)
            SetGameProcess(GameProcess.EndAnime); // 遊戲結束

    }


    public override void EndAnimeProcess(){
        if(!AnimeHasPlay)
        {
            AnimeHasPlay = true;
            GradeInfoUI.PlayTextShowOnceAnime(_endText);
        }
        if(AnimeHadFinish)
        {
            SetGameProcess(GameProcess.GameEnd);
            InitAnimeVariable();
        }
    }

    public override void GameEndProcess(){

        int point = GameMeditor.Instance.GetPoint();

        JudgeStateComplete(); // 紀錄遊戲完成度
        int completerate = GetCompletionRate(); // 取得完成度
        if(completerate > m_bestPoint)
            m_bestPoint = completerate; // 記下最高紀錄


        // 增加錢
        // int money = point;

        // GameMeditor.Instance.AddMoney(point); // 增加錢
        ChangeStageUI.ShowNextLevel(this); // 顯示前往下一關
        // PriceUI.Show(); // 開啟獎勵界面
        // MusicManager.StopMusic(); // 關掉音樂，播放獎勵音樂
        // PhoneInputUI.EnableMoveBar(false); // 無法透過點擊螢幕來移動Bar

        CompleteStage(); // 完成關卡

        SetGameProcess(GameProcess.WaitStart);
    }

    public void GameOverProcess()
    {
        
        MusicManager.StopMusic(); // 關掉音樂，播放獎勵音樂
        ChangeStageUI.ShowDead(this); // 顯示死亡介面
        SetGameProcess(GameProcess.WaitStart);

    }

    public override void Reset()
	{
        // 球的數量
        hasHitBallCount = 0;

        // 繼承原本的Reset
        base.Reset();
	}

    
}

