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
        nowStageData = stageDatas[3]; // 設定現在是第幾關
    }

    public override void Update(){
        if(nowStageData.hasSet == true)
            nowStageData.Update();
    }

    public void ReSet(){
        nowStageData.Reset(); // 關卡CD重開
        point = 0;
        combol = 0;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
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
        int[] primes ={2, 3, 5};
        int[] composites = {4,6,8,9};
        int[] bossNums = {11,12,16,18};
        StageData stageData = new StageData(3f ,primes, composites, bossNums);
        stageDatas.Add(stageData);
    }

    private void CreateStageData2(){// 第二關
        int[] primes2 ={ 11, 13, 17, 23, 29, 31, 37};
        int[] composites2 = {10,12,16,18,21, 24, 30};
        int[] bossNums2 = {43, 47, 53, 59, 61, 67,51, 57, 69, 81 };
        StageData stageData = new StageData(5f ,primes2, composites2, bossNums2);
        stageDatas.Add(stageData);
    }

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
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public int GetPoint(){
        return point;
    }
    public int GetCombol(){
        return combol;
    }

}