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
	private ShopSystem shopSystem;

	private GameMeditor(){}

	public void Initinal(){
		ballSystem = new BallSystem(this);
		stageSystem = new StageSystem(this);
		shopSystem = new ShopSystem(this);

	}

	public void Update(){
		ballSystem.Update();
		stageSystem.Update();
		shopSystem.Update();
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

	public void RemoveAllBall(){
		ballSystem.RemoveAllBall();
	}

	public int GetBallCount(){
		return ballSystem.GetBallCount();
	}

	// 依照策略取得球的分數
    public int GetBallPoint(BallColor color, Ball ball){
        return ballSystem.GetBallPoint(color, ball);
    }

	// ----------------StageSystem------------------
	public StageSystem GetStageSystem(){
		return stageSystem;
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
    
    public void MissCombol (){
        stageSystem.MissCombol();
    }

	public int GetPoint(){
		return stageSystem.GetPoint();
	}

	public int GetCombol(){
		return stageSystem.GetCombol();
	}

	public float GetGameTime(){
		return stageSystem.GetGameTime();
	}

	public void SetGameTime(float time){
        stageSystem.SetGameTime(time);
    }

	public int GetMissCombol(){
		return stageSystem.GetMissCombol();
	}

	public StageComplete GetStageComplete(){
        return stageSystem.GetStageComplete();
    }

	public void SetStageData(int num){
		stageSystem.SetStageData(num);
	}

	public string GetStageName(){
		return stageSystem.GetStageName();
	}

	public void BossComingAnimeEnd(){  // Boss球動畫播完
        stageSystem.BossComingAnimeEnd();
    }

	// 以ID進入關卡
	public void EnterStage(string stageID){
		stageSystem.EnterStage(stageID);
	}

	// 以BoxName, BoxId進入關卡
	public void EnterStage(string BoxName, int stageID){
		stageSystem.EnterStage(BoxName, stageID);
	}

	// 離開關卡
	public void LeaveStage(){
		stageSystem.LeaveStage();
	}
	
	public void SetGameProcess(GameProcess gameProcess){
		stageSystem.SetGameProcess(gameProcess);
	}

	public GameProcess GetNowGameProcess(){
        return  stageSystem.GetNowGameProcess();
    }

	public void ResetStage(){
		stageSystem.ReSet();
	}
	// 給予完成率
    public int GetCompletionRate(){
        return stageSystem.GetCompletionRate();
    }

	 // IStageDataCardBox 方法
    public void AddStageCardBox(IStageDataCardBox cardBox){
        stageSystem.AddStageCardBox(cardBox);
    }

    public void RefreshAllCard(){
        stageSystem.RefreshAllCard();
    }

	

	// =============ShopSystem================
	public void SetBallStyle(BallStyle ballStyle){
        shopSystem.SetBallStyle(ballStyle);
    }

    public BallStyle GetBallStyle(){
        return shopSystem.GetBallStyle();
    }


	public bool BuyThing(int price){
        return shopSystem.BuyThing(price);
    }

    public bool WhetherBuyStage(int price){
        return shopSystem.WhetherBuyStage(price);
    }

	public void AddMoney(int money){
        shopSystem.AddMoney(money);
    }

    public void LessMoney(int money){
        shopSystem.LessMoney(money);
    }

    public int GetMoney(){
        return shopSystem.GetMoney();
    }

}