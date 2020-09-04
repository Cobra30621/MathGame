using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownWall : IWall
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

        _ballStrategy.DownWallOnBallEnter(onBall);
         
        /*
        switch(ballType){
            case BallType.Prime:
                GameMeditor.Instance.AddCombol();
                GameMeditor.Instance.AddPoint(onBall.GetPoint());
                JudgeCreateWhichLabel(onBall); // 顯示加分
                onBall.Release();
                break;
            case BallType.Composite:
                // GameMeditor.Instance.LessPoint(point);
                GameMeditor.Instance.MissCombol();
                BallPointUI.CreateMissLabel(onBall.GetPosition());
                onBall.Release();
                break;
            case BallType.Black:
                Debug.LogError("Black不該出現");
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }*/
    }

}