using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSystem : IGameSystem
{
    private List<Ball> Balls = new List<Ball>();

    public BallSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();
	}


    public override void Update(){
        foreach(Ball ball in Balls){
            ball.Update();
        }
    }

    public void AddBall(Ball ball){
        Balls.Add(ball);
    }

    public Ball GetBall(int index){
        if (index < Balls.Count)
            if(Balls[index] != null)
                return Balls[index];
        Debug.Log("找不到球");
        return null;
    }

    public List<Ball> GetBalls(){
        return Balls;
    }

    public int GetBallCount(){
        return Balls.Count;
    }

    public void RemoveAllBall(){
        foreach(Ball ball in Balls){
            ball.Release();
        }
    }
    public void RemoveBall(Ball ball){
        Balls.Remove(ball);
    }

}