using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMeditor
{
    //------------------------------------------------------------------------
	// Singleton模版
	private static GameMeditor _instance;
	public static GameMeditor Instance
	{
		get
		{
			if (_instance == null)			
				_instance = new GameMeditor();
			return _instance;
		}
	}

	// 遊戲系統
	private BallSystem ballSystem; 
	private StageSystem stageSystem;

	private GameMeditor(){}

	public void Initinal(){
		ballSystem = new BallSystem(this);
		stageSystem = new StageSystem(this);

	}

	public void Update(){
		ballSystem.Update();
		stageSystem.Update();
	}

	// ----------------BallSystem------------------
	public void AddBall(Ball ball){
        ballSystem.AddBall(ball);
    }

	public Ball GetBall(int index){
		return ballSystem.GetBall(index);
	}

	public List<Ball> GetBalls(){
		return ballSystem.GetBalls();
	}

	public void RemoveBall(Ball ball){
		ballSystem.RemoveBall(ball);
	}

	// ----------------StageSystem------------------
	public void ResetStage(){
		stageSystem.ReSet();
	}
    public void AddPoint(int poi){
        stageSystem.AddPoint(poi);
    }

    public void LessPoint(int poi){
        stageSystem.LessPoint(poi);
    }
    public void AddCombol(){
        stageSystem.AddCombol();
    }
    
    public void EndCombol (){
        stageSystem.EndCombol();
    }

	public int GetPoint(){
		return stageSystem.GetPoint();
	}

	public int GetCombol(){
		return stageSystem.GetCombol();
	}

}