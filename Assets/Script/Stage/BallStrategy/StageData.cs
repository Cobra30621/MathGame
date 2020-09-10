using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// 一般關卡資訊
public class StageData : IStageData
{

    // ================= 初始設定方法 ===================
	public StageData(float CoolDown , string name, int[] primes, int[] composites, int[] plusNums, int[] bossNums):base(CoolDown , name, primes, composites, plusNums, bossNums)
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
            case GameProcess.NoNormalBall :
                PlayBossComingAnime();  // 播放BossComingAnime
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


    

}
