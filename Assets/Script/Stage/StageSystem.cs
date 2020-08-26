using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSystem : IGameSystem
{

    public StageSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();

	}

    public int point;
    public int combol;
    public int missCombol;
    public StageData nowStageData;
    public List<StageData> stageDatas = new List<StageData>();
    public Dictionary<string, StageData> stages = new Dictionary<string, StageData>(); 


    // Dictionary方法
    public void EnterStage(string stageID){
        StageData stageData;
        if(stages.ContainsKey(stageID))
            stageData = stages[stageID];
        else
        {
            Debug.Log("找不到關卡" + stageID);
            return;
        }

        nowStageData = stageData;
        SceneManager.LoadScene("MainGame");
    }

    public void SetStageData(string id){
        nowStageData = stages[id];
        // ReSet(); // 關卡重置 :不知道哪邊出了問題
    }

    public override void Initialize(){
        InitializeStageData();
        SetStageData(1); // 設定現在是第幾關
    }

    public override void Update(){
        if(nowStageData.hasSet == true)
            nowStageData.Update();
    }

    public void ReSet(){
        nowStageData.Reset(); // 關卡CD重開
        point = 0;
        combol = 0;
        missCombol = 0;
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
        CreateStageData5();
        CreateStageData6();
        CreateStageData7();
        CreateStageData8();

    }

    private void CreateStageData1(){// 第一關
        string stageName = "基礎";
        int[] primes ={2, 3};
        int[] composites = {4,6,9};
        int[] plusNums = {};
        int[] bossNums = {13,14, 15,19};
        StageData stageData = new StageData(4f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(5,5); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData2(){// 第二關
        string stageName = "中等";
        int[] primes ={2, 3, 5};
        int[] composites = {4,6,9};
        int[] plusNums = {8, 12,16, 18};
        int[] bossNums = {21,22, 23, 26,29};
        StageData stageData = new StageData(3f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(4,4); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData3(){// 第三關
        string stageName = "困難：數學面";
        int[] primes ={29, 31, 37,41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127};
        int[] composites = {33, 39, 46, 49, 51, 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {101, 123, 107, 109, 113 , 111, 115, 99,106,  111, 115 , 123,129};
        StageData stageData = new StageData(2f, stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(5,5); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData4(){// 第四關
        string stageName = "困難：遊戲面";
        int[] primes = {};
        int[] composites =  {};
        int[] plusNums = {24, 30, 36, 42, 48, 54, 60, 64};
        int[] bossNums = {1024};
        StageData stageData = new StageData(5f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData5(){// 第四關
        string stageName = "高難度";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };//
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {24, 30, 36, 42, 48, 54, 60, 64, 128};
        int[] bossNums = {3628800};
        StageData stageData = new StageData(1f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(4,4); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData6(){// 第五關
        string stageName = "地獄:數學性";
        int[] primes = { 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997};//
        int[] composites =  CreateCompositeIntList();
        int[] plusNums = {};
        int[] bossNums = { 1009, 1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097, 1103, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181, 1187, 1193, 1201, 1213, 1217, 1223};
        StageData stageData = new StageData(3f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(5,5); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData7(){// 第五關
        string stageName = "地獄:遊戲性";
        int[] primes = { };//
        int[] composites =  { };
        int[] plusNums = CreatePlusIntList();
        int[] bossNums = {3628800};
        StageData stageData = new StageData(1f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    private void CreateStageData8(){// 第五關
        string stageName = "地獄:這出球率";
        int[] primes = { };//
        int[] composites =  { };
        int[] plusNums = {24, 30, 36, 42, 48, 54, 60, 64, 128};
        int[] bossNums = {3628800};
        StageData stageData = new StageData(0.4f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0); // P_prime, P_composites ，三顆球出現機率加總為10
        stageDatas.Add(stageData);
        stages.Add(stageName, stageData);
    }

    // -----------數學方法----------

    private int[] CreateCompositeIntList(){
        int count = 20;
        int [] nums = new int[count];
        for(int i = 0; i<count ; i++){
            nums[i] = CreateCompositeInt();
        }
        return nums;
    }

    private int CreateCompositeInt(){
        int [] nums = { 7, 11, 13, 17, 19, 23, 29, 31, 37};
        int count = 2;
        int num = 1;
        for(int i = 0; i<count ; i++){
            int index = Random.Range(0,nums.Length);
            num *= nums[index];
        }
        return num;
    }

    // 產生加分球數字

    private int[] CreatePlusIntList(){
        int count = 8;
        int [] nums = new int[count];
        for(int i = 0; i<count ; i++){
            nums[i] = CreatePlusInt();
        }
        return nums;
    }

    private int CreatePlusInt(){
        int [] nums = {5,7,11,13,17,19};
        int count = Random.Range(3,6);
        int num = 1;
        for(int i = 0; i<count ; i++){
            int index = Random.Range(0,nums.Length);
            num *= nums[index];
        }
        return num;
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
    
    public void MissCombol (){
        combol = 0;
        missCombol ++;
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
        return missCombol;
    }

    public string GetStageName(){
        return nowStageData.GetStageName();
    }

    public void BossComingAnimeEnd(){  // Boss球動畫播完
        nowStageData.BossComingAnimeEnd();
    }

    
}