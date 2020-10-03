using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BallMoveMethon{
    Straight, Ramdom
}


public class IBallStrategy 
{
    public BallMoveMethon _ballMoveMethon;
    public BallFactory ballFactory;
    // 建構值
    public IBallStrategy(){
        _ballMoveMethon = BallMoveMethon.Ramdom; // 預設隨機
    }


    public IBallStrategy(BallMoveMethon ballMoveMethon){
        _ballMoveMethon = ballMoveMethon; // 可以改成非隨機
    }

    public virtual void BarOnBallEnter(Ball onBall){} // 底下Bar的回擊方式

    public virtual void DownWallOnBallEnter(Ball onBall){} // 底部牆壁的回擊方式

    public virtual void UpWallOnBallEnter(Ball onBall){} // 上方牆壁的回擊方式

    public virtual int GetMaxCombol(int num){ return 0;} // 計算Combol的方法

    public void CreateBall(int num, BallColor color){
        if (ballFactory == null) // 產生球工廠
            ballFactory = MainFactory.GetBallFactory();

        // 判斷要產生直球還是隨機球
        if(_ballMoveMethon == BallMoveMethon.Ramdom)
            ballFactory.CreateBall(num, color);
        else if(_ballMoveMethon == BallMoveMethon.Straight)
            ballFactory.CreateStraightBall(num, color);

    }

}