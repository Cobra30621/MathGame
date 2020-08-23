using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSystem : IGameSystem
{

    public StageSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();

	}

    public int point;
    public int combol;
    public StageData nowStageData;
    public List<StageData> stageDatas = new List<StageData>();

    public override void Initialize(){
        InitializeStageData();
        SetStageData(3); // 設定現在是第幾關
    }

    public override void Update(){
        if(nowStageData.hasSet == true)
            nowStageData.Update();
    }

    public void ReSet(){
        nowStageData.Reset(); // 關卡CD重開
        point = 0;
        combol = 0;
        GameMeditor.Instance.RemoveAllBall(); // 清除所有的球
        GradeInfoUI.Initialize(); // 重置分數介面
    }

    public void SetStageData(int level){
        nowStageData = stageDatas[level];
        // ReSet(); // 關卡重置 :不知道哪邊出了問題
    }

    public int GetStageDataCount(){
        return stageDatas.Count;
    }

    // 初始所有關卡
	private void InitializeStageData()
	{
        CreateStageData1();
        CreateStageData2();
        CreateStageData3();
        CreateStageData4();

    }

    private void CreateStageData1(){// 第一關
        string stageName = "基礎";
        int[] primes ={2, 3};
        int[] composites = {4,6,9};
        int[] plusNums = {};
        int[] bossNums = {13,14, 15,19};
        StageData stageData = new StageData(4f , stageName, primes, composites, plusNums, bossNums);
        stageDatas.Add(stageData);
    }

    private void CreateStageData2(){// 第二關
        string stageName = "中等";
        int[] primes ={2, 3, 5};
        int[] composites = {4,6,9};
        int[] plusNums = {8, 12,16, 18};
        int[] bossNums = {21,22, 23, 26,29};
        StageData stageData = new StageData(3f, stageName ,primes, composites, plusNums, bossNums);
        stageDatas.Add(stageData);
    }

    private void CreateStageData3(){// 第三關
        string stageName = "困難：數學面";
        int[] primes ={29, 41, 61, 67, 71, 73, 79, 89, 97};
        int[] composites = {33, 39, 51, 57, 69, 87};
        int[] plusNums = {};
        int[] bossNums = {101, 123, 107, 109, 113 , 111, 115};
        StageData stageData = new StageData(3.5f, stageName, primes, composites, plusNums, bossNums);
        stageDatas.Add(stageData);
    }

    private void CreateStageData4(){// 第四關
        string stageName = "困難：遊戲面";
        int[] primes = {};
        int[] composites =  {};
        int[] plusNums = {24, 30, 36, 42, 48, 54, 60, 64};
        int[] bossNums = {128};
        StageData stageData = new StageData(5f, stageName ,primes, composites, plusNums, bossNums);
        stageDatas.Add(stageData);
    }
    /*
    private void CreateStageData3(){// 第三關
        int[] primes3 ={29, 41, 61, 67, 71, 73, 79, 89, 97};
        int[] composites3 = {33, 39, 48, 51, 57, 64, 69, 81, 87};
        int[] bossNums3 = {101, 103, 107, 109, 113, 128 , 173};
        StageData stageData = new StageData(5f ,primes3, composites3, bossNums3);
        stageDatas.Add(stageData);
    }

    
    private void CreateStageData4(){// 第四關
        int[] primes4 ={24,64,128,60,72,48,96,84,80};
        int[] composites4 = {24,64,128,60,72,48,96,84,80};
        int[] bossNums4 = {101, 103, 107, 109, 113, 128 , 173};
        StageData stageData = new StageData(2f ,primes4, composites4, bossNums4);
        stageDatas.Add(stageData);
    }
    */

    // -------------------分數相關--------------------

    public void AddPoint(int poi){
        point += poi;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public void LessPoint(int poi){
        point -= poi;
        if(point < 0)
            point = 0;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public void AddCombol(){
        combol ++ ;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }
    
    public void EndCombol (){
        combol = 0;
        nowStageData.AddMissCombol();
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public int GetPoint(){
        return point;
    }
    public int GetCombol(){
        return combol;
    }

    public float GetGameTime(){
        return nowStageData.GetGameTime();
    }

    public int GetMissCombol(){
        return nowStageData.GetMissCombol();
    }

    public string GetStageName(){
        return nowStageData.GetStageName();
    }

}