using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLeftWall : IWall
{
    public override void OnBallEnter(GameObject ball){
        Ball onBall = ball.gameObject.GetComponent<Ball>();

        onBall.ReboundX();
    }
}