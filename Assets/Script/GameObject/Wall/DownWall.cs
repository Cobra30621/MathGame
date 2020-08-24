using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownWall : IWall
{
    private int point = 100;
    public override void OnBallEnter(GameObject ball){
        Ball onBall = ball.gameObject.GetComponent<Ball>();
        BallType ballType = onBall.ballType;
         
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
        }
    }

    public void JudgeCreateWhichLabel(Ball onBall){
        BallColor color = onBall.GetBallColor();

        if(color == BallColor.Purple || color == BallColor.Green) // 魔王球
        {
            BallPointUI.CreatePointLabelBoss(onBall.GetPosition(), onBall.GetPoint());// 顯示加分
        }
        else // 一般球
        {
            BallPointUI.CreateGetPointLabel(onBall.GetPosition(), onBall.GetPoint()); // 顯示加分
        }


    }
}