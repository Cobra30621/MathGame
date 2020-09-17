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
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainGame");
    }

    // 等讀取完場景，再執行
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMeditor.Instance.SetGameProcess(GameProcess.Start); // 遊戲流程變成開始
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    public void TimeUp(){
        GameMeditor.Instance.SetGameTime(1);
    }

    public void AddMoney(){
        GameMeditor.Instance.AddMoney(10000);
        StageSelectUI.RefreshInfo(); // 更新金錢介面
    }

 

}