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

    void Awake () {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            ballFactory = MainFactory.GetBallFactory();
            GameMeditor.Instance.Initinal();
        }
        else if (this!=instance)
        {
            Destroy(gameObject);
        }      

        
    }

    void Update(){
        GameMeditor.Instance.Update();
    }


    public void ResetStage(){
        // GameMeditor.Instance.SetStageData(level - 1);
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