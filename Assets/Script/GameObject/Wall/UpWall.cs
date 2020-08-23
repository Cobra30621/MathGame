using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpWall : IWall
{
    public override void OnBallEnter(GameObject ball){
        Ball onBall = ball.gameObject.GetComponent<Ball>();
        BallType ballType = onBall.ballType;
         
        switch(ballType){
            case BallType.Prime:
            case BallType.Composite:
                onBall.ReboundY();
                onBall.SetWhetherTouch(true);
                break;
            case BallType.Black:
                GameMeditor.Instance.EndCombol();
                BallPointUI.CreateNoPointLabel(onBall.GetPosition());
                onBall.Release(); 
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }
    }
}