using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBar : MonoBehaviour
{
    public int speed = 1;
    private bool WhetherExist;
    // Update is called once per frame
    void Update()
    {
        InputProcess();
    }

    private void InputProcess(){
        if(Input.GetKey(KeyCode.RightArrow))
            transform.position += new Vector3(speed * 0.1f,0,0);
        if(Input.GetKey(KeyCode.LeftArrow))
            transform.position += new Vector3(-speed* 0.1f,0,0);
        if(Input.GetKey(KeyCode.Space))
        {
            WhetherExist = false;
            GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
        }
        else
        {
            WhetherExist = true;
            GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        }

    }

    void OnTriggerEnter2D(Collider2D ball) 
    {    
        Debug.Log(ball.ToString()+ "Enter");
        if (ball.gameObject.tag == "ball") 
        {    
            OnBallEnter(ball.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D ball) 
    {    
        Debug.Log(ball.ToString()+ "Enter");
        if (ball.gameObject.tag == "ball") 
        {    
            OnBallEnter(ball.gameObject);
        }
    }

    public void OnBallEnter(GameObject ball){
        if (WhetherExist == false){return;} //判斷玩家是否按空白鍵

        Ball onBall = ball.gameObject.GetComponent<Ball>();
        BallType ballType = onBall.ballType;

        if (onBall.GetWhetherTouch() == false){return;}
        onBall.SetWhetherTouch(false);

        switch(ballType){
            case BallType.Prime:
                onBall.BecomeBlackBall();
                onBall.ReboundY();
                break;
            case BallType.Black:
                onBall.ReboundY();
                break;
            case BallType.Composite:
                onBall.BecomeTwoBall(); 
                GameMeditor.Instance.AddCombol();
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }
    }
}
