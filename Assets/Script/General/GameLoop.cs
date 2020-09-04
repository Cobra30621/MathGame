using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public int level ;

    private BallFactory ballFactory;
    
    int [] numList =  {2, 3, 4,5,6,7,8,9};
    static GameLoop instance;

    public GameProcess gameProcess;

    void Awake () {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            GameMeditor.Instance.Initinal();
            ballFactory = MainFactory.GetBallFactory();
        }
        else if (this!=instance)
        {
            Destroy(gameObject);
        }      

    }

    void Update(){
        GameMeditor.Instance.Update();
        gameProcess =  GameMeditor.Instance.GetNowGameProcess(); // 確認現在遊戲流程
    }

    public void Test(){
        string stageName = "模擬新模式";
        int[] primes ={2, 3, 5};
        int[] composites = {4,6, 9};
        int[] plusNums = {};
        int[] bossNums = {3,4};
        NoBossStageData stageData = new NoBossStageData(4f , stageName, primes, composites, plusNums, bossNums);
        StartPanel.Show(stageData);
    }


    public void ResetStage(){
        GameMeditor.Instance.SetStageData(level - 1);
		GameMeditor.Instance.ResetStage();
	}

    public void GoToStageSelect(){
        /* GameMeditor.Instance.RemoveAllBall();
        MusicManager.StopMusic();
        GameMeditor.Instance.ResetStage();
        GameMeditor.Instance.SetGameProcess(GameProcess.WaitStart);
        SceneManager.LoadScene("StageSelect");
        */
        GameMeditor.Instance.LeaveStage();
    }

 

}