using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// 一般關卡資訊
public class CatchPrimeStageData : IStageData
{

    // ================= 初始設定方法 ===================
	public CatchPrimeStageData(float CoolDown , string name, int[] primes, int[] composites, int[] plusNums, int[] bossNums):base(CoolDown , name, primes, composites, plusNums, bossNums)
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
        ballStrategy = new BarCatchPrimeStrategy();

        // 遊戲流程
        m_gameProcess = GameProcess.WaitStart;
		hasSet = true;
	}


    // ------------------ 關卡流程 ---------------------- 
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

    public override void GameStartProcess(){
        Reset();
        MusicManager.PlayMusic();
        SetGameProcess( GameProcess.WaitTouch);
        PhoneInputUI.EnableMoveBar(false); // 無法透過點擊螢幕來移動Bar
        
	}

    public override void GameWaitTouchProcess(){
        if(!AnimeHasPlay)
        {
            AnimeHasPlay = true;
            StartPanel.Show(this);
        }
        if(AnimeHadFinish)
        {
            SetGameProcess( GameProcess.NormalTime);
            InitAnimeVariable();
            PhoneInputUI.EnableMoveBar(true); // 可以透過點擊螢幕來移動Bar
        }
    }

    public override void WaitNoBallProcess(){
        int ballCount = GameMeditor.Instance.GetBallCount();
        if (ballCount == 0)
            SetGameProcess(GameProcess.EndAnime); // 遊戲結束

    }

}
