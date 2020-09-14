using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CompositeToOneStrategy : IBallStrategy 
{
    // 建構值
    public CompositeToOneStrategy():base(){

    }

    public CompositeToOneStrategy(BallMoveMethon ballMoveMethon):base(ballMoveMethon){

    }

    public override void BarOnBallEnter(Ball onBall)
    {
        BallType ballType = onBall.ballType;
        switch(ballType){
            case BallType.Prime:
                onBall.BecomeBlackBall();
                GameMeditor.Instance.MissCombol();
                BallPointUI.CreateMissLabel(onBall.GetPosition());
                onBall.ReboundY();
                break;
            case BallType.Black:
                onBall.ReboundY();
                break;
            case BallType.Composite:
                onBall.BecomeBlackBall(BallType.CompostieBlack);
                onBall.ReboundY();
                GameMeditor.Instance.AddMoney(onBall.GetPoint());
                JudgeCreateWhichLabel(onBall); // 顯示加分
                GameMeditor.Instance.AddCombol();
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }
    }

    public override void DownWallOnBallEnter(Ball onBall){
        BallType ballType = onBall.ballType;
         
        switch(ballType){
            case BallType.Prime:
                GameMeditor.Instance.AddCombol();
                GameMeditor.Instance.AddMoney(onBall.GetPoint());
                JudgeCreateWhichLabel(onBall); // 顯示加分
                onBall.Release();
                break;
            case BallType.Composite:
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

        if(color == BallColor.Boss ) // 魔王球
        {
            BallPointUI.CreatePointLabelBoss(onBall.GetPosition(), onBall.GetPoint());// 顯示加分
        }
        else  // 一般球
        {
            BallPointUI.CreateGetPointLabel(onBall.GetPosition(), onBall.GetPoint()); // 顯示加分
        }


    }

    public override void UpWallOnBallEnter(Ball onBall){
        BallType ballType = onBall.ballType;
        
        switch(ballType){
            case BallType.Prime:
            case BallType.Composite:
                onBall.ReboundY();
                onBall.SetWhetherTouch(true);
                break;
            case BallType.Black:
                onBall.Release(); 
                break;
            case BallType.CompostieBlack : 
                onBall.Release(); 
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }
    }

    // 累積最大Combol數
    public override int GetMaxCombol(int num){
        return 1; // 球只會回擊一次
    }

}