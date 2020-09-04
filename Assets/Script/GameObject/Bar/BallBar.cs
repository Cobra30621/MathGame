using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBar : MonoBehaviour
{
    public int speed = 1;
    private bool WhetherExist;

    private SpriteRenderer spriteRenderer;

    private IBallStrategy _ballStrategy;  // 球回擊策略

    void Awake()
    {
        IStageData.SetBallStrategy += SetBallStrategy; // 訂閱設定球回擊策略

        // 設定造型
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResourceAssetFactory resourceAssetFactory = MainFactory.GetResourceAssetFactory(); 
        spriteRenderer.sprite = resourceAssetFactory.LoadImageSprite("Bar");
    }

    // 設定球回擊得策略
    public void SetBallStrategy(IBallStrategy ballStrategy){
        _ballStrategy = ballStrategy;
    }

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
            spriteRenderer.color = new Color(1f,1f,1f,0.5f);
        }
        else
        {
            WhetherExist = true;
            spriteRenderer.color = new Color(1f,1f,1f,1f);
        }

    }

    void OnTriggerEnter2D(Collider2D ball) 
    {    
        if (ball.gameObject.tag == "ball") 
        {    
            OnBallEnter(ball.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D ball) 
    {    
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

        if(_ballStrategy == null)
            Debug.LogError("_ballStrategy為null，請確定有賦值");

        _ballStrategy.BarOnBallEnter(onBall); // 球回擊的方式
        /*
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
        }*/
    }
}
