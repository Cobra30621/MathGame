using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BallType{
    Prime, Composite, Black
}

public enum BallColor{//White, Red, Orange, Yellow, Green
    Blue, Gray, Black , Purple, Orange
}

public class Ball : MonoBehaviour
{
    private int Number;
    [SerializeField]
    private Vector3 speed;
    [SerializeField]
    private Text NumText;
    [SerializeField]
    private int point = 0;
    private Image img_ball;
    private BallColor ballColor;
    private bool WhetherTouch;


    // private GameObject gameObject;
    public BallType ballType;

    public void Update(){
        Move();
    }

    public void SetNumber(int num){
        Number = num;
        NumText.text = num + "";
    }
    public int GetNumber(){
        return Number;
    }

    public void SetPoint(int num){
        point = num;
    }

    public int GetPoint(){
        return point;
    }

    public void SetSpeed(Vector3 vec){
        speed = vec;
    }

    public void SetPosition(Vector3 vec){
        transform.position = vec;
    }

    public Vector3 GetPosition(){
        return transform.position;
    }

    public void SetBallType(BallType ballType){
        this.ballType = ballType; 
    }

    public void SetBallColor(BallColor ballColor){
        this.ballColor = ballColor;
    }

    public void SetBallImage(Image img){
        img_ball = img;
    }

    public bool GetWhetherTouch(){
        return WhetherTouch;
    }
    public void SetWhetherTouch(bool bo){
        WhetherTouch = bo;
    }

    public void Move(){
        transform.Translate(speed * Time.deltaTime);
    }

    public void ReboundY(){
        speed.y *= -1; 
    }

    public void ReboundX(){
        speed.x *= -1; 
    }

    public void BecomeBlackBall(){
        ballType = BallType.Black;
        img_ball.color = Color.gray ;
    }

    public void BecomeTwoBall(){
        BallFactory ballFactory = MainFactory.GetBallFactory();
        ballFactory.CreateTwoBall(this);
    }

    public void Release(){
        Debug.Log(ballType + "Ball消失了");
        GameMeditor.Instance.RemoveBall(this); // 移出管理器
        GameObject.Destroy(this.gameObject);
    }

}