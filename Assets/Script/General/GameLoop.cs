using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public int num = 2;
    private BallFactory ballFactory;
    
    int [] numList =  {2, 3, 4,5,6,7,8,9};

    void Awake(){
        ballFactory = MainFactory.GetBallFactory();
        GameMeditor.Instance.Initinal();
    }

    void Update(){
        GameMeditor.Instance.Update();
    }

    public void CreateBalls(){
        int index = Random.Range(0, numList.Length);
        num = numList[index];
        ballFactory.CreateBall(num);
    }

    public void ResetStage(){
		GameMeditor.Instance.ResetStage();
	}

}