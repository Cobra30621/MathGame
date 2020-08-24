using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWall : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D ball) //aaa為自定義碰撞事件
    {    
        Debug.Log(ball.ToString()+ "Enter");
        if (ball.gameObject.tag == "ball") //如果aaa碰撞事件的物件標籤名稱是test
        {    
            OnBallEnter(ball.gameObject);
            //Destroy(aaa.gameObject); //刪除碰撞到的物件(CubeA)
        }
    }


    public virtual void OnBallEnter(GameObject ball){
        Ball onBall = ball.gameObject.GetComponent<Ball>();
        BallType ballType = onBall.ballType;

        switch(ballType){
            case BallType.Prime:
                break;
            case BallType.Composite:
                break;
            case BallType.Black:
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }
    }
    
}