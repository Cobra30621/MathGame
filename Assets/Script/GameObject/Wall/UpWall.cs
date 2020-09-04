using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpWall : IWall
{

    private IBallStrategy _ballStrategy;  // 球回擊策略

    void Awake()
    {
        IStageData.SetBallStrategy += SetBallStrategy; // 訂閱設定球回擊策略
    }

    // 設定球回擊得策略
    public void SetBallStrategy(IBallStrategy ballStrategy){
        _ballStrategy = ballStrategy;
    }

    public override void OnBallEnter(GameObject ball){
        Ball onBall = ball.gameObject.GetComponent<Ball>();
        BallType ballType = onBall.ballType;

        _ballStrategy.UpWallOnBallEnter(onBall);
        
        /*
        switch(ballType){
            case BallType.Prime:
            case BallType.Composite:
                onBall.ReboundY();
                onBall.SetWhetherTouch(true);
                break;
            case BallType.Black:
                GameMeditor.Instance.MissCombol();
                BallPointUI.CreateMissLabel(onBall.GetPosition());
                onBall.Release(); 
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }*/
    }
}