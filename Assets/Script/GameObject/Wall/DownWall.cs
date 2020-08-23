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
                int pointWithCombol = point + (point /10) * GameMeditor.Instance.GetCombol(); // combol分數加分
                GameMeditor.Instance.AddPoint(pointWithCombol);
                BallPointUI.CreateGetPointLabel(onBall.GetPosition(), pointWithCombol); // 顯示加分
                onBall.Release();
                break;
            case BallType.Composite:
                GameMeditor.Instance.LessPoint(point);
                GameMeditor.Instance.EndCombol();
                BallPointUI.CreateLossPointLabel(onBall.GetPosition(), 100); // 顯示扣分
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
}