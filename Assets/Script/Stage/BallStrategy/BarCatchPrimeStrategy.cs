using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarCatchPrimeStrategy : IBallStrategy 
{
    // 建構值
    public BarCatchPrimeStrategy():base(){
        _ballMoveMethon = BallMoveMethon.Ramdom;
    }

    public BarCatchPrimeStrategy(BallMoveMethon ballMoveMethon):base(ballMoveMethon){

    }

    public override void BarOnBallEnter(Ball onBall)
    {
        BallType ballType = onBall.ballType;
        switch(ballType){
            case BallType.Prime:
                GameMeditor.Instance.AddCombol();
                GameMeditor.Instance.AddMoney(onBall.GetPoint());
                GradeInfoUI.PlayChangeMoneyAnime(); // 播放獲得錢的動畫
                BallPointUI.CreateGetPointLabel(onBall.GetPosition(), onBall.GetPoint());
                GameMeditor.Instance.AddHasHitBallCount(); //增加球的進度
                AudioSourceController.PlaySound("getPoint"); // 播放音效
                onBall.Release();
                break;
            case BallType.Composite:
                GameMeditor.Instance.MissCombol();
                BallPointUI.CreateLossPointLabel(onBall.GetPosition(), onBall.GetPoint());
                GameMeditor.Instance.LossHeart(onBall.GetPoint()); // 損血
                AudioSourceController.PlaySound("miss"); // 播放音效
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

    public override void DownWallOnBallEnter(Ball onBall){
        BallType ballType = onBall.ballType;
         
        switch(ballType){
            case BallType.Prime:
                GameMeditor.Instance.MissCombol();
                GameMeditor.Instance.LossHeart(onBall.GetPoint()); // 損血
                // BallPointUI.CreateLossPointLabel()
                BallPointUI.CreateLossPointLabel(onBall.GetPosition(), onBall.GetPoint());
                AudioSourceController.PlaySound("miss"); // 播放音效
                onBall.Release();
                break;
            case BallType.Composite:
                BallPointUI.CreateAddCombolLabel(onBall.GetPosition());
                GameMeditor.Instance.AddCombol();
                // AudioSourceController.PlaySound("getPoint"); // 播放音效
                onBall.Release();
                break;
            case BallType.Black:
                Debug.LogError("Black不該出現");
                break;
            default :
                Debug.LogError("無法找到BallType");
                break;
        }
        // GameMeditor.Instance.AddHasHitBallCount(); //增加球的進度
    }

    public override void UpWallOnBallEnter(Ball onBall){
        BallType ballType = onBall.ballType;
        
        Debug.LogError("球不該碰到上方");

    }

    // 累積最大Combol數
    public override int GetMaxCombol(int num){
        return 1; // 球只會回擊一次
    }

}