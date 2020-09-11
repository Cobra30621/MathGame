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
    public IStageData nowStageData;
    
    // 以下兩個用不到了
    public List<IStageData> stageDatas = new List<IStageData>();
    public Dictionary<string, IStageData> stages = new Dictionary<string, IStageData>(); 

    public Dictionary<string, IStageDataBox> stageBoxs = new Dictionary<string, IStageDataBox> ();
    public List<IStageDataCardBox> _cardBoxs = new List<IStageDataCardBox>();

    // ============== Dictionary方法 ======================= 
    public void EnterStage(string stageID){
        IStageData stageData;
        if(stages.ContainsKey(stageID))
            stageData = stages[stageID];
        else
        {
            Debug.Log("找不到關卡" + stageID);
            return;
        }

        nowStageData = stageData;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainGame");
    }

    public void EnterStage(string BoxName, int stageID){
        SetStageData(BoxName, stageID);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainGame");
    }

    public void SetStageData(string BoxName, int stageID){
        IStageDataBox stageBox;
        
        if(stageBoxs.ContainsKey(BoxName))
            stageBox = stageBoxs[BoxName];
        else
        {
            Debug.Log($"找不到關卡{BoxName}_{stageID}：{BoxName}");
            return;
        }

        IStageData stageData;
        if(stageBox.stageDatas.ContainsKey(stageID))
            stageData = stageBox.stageDatas[stageID];
        else
        {
            Debug.Log($"找不到關卡{BoxName}_{stageID}：{stageID}");
            return;
        }

        nowStageData = stageData;
    }

    // =============IStageDataCardBox=================
    public void AddStageCardBox(IStageDataCardBox cardBox){
        _cardBoxs.Add(cardBox);
    }

    public void RefreshAllCard(){
        foreach ( var box in _cardBoxs )
        {
            box.RefreshInfo();
        }
    }

    // 等讀取完場景，再執行
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetGameProcess(GameProcess.Start); // 遊戲流程變成開始
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SetStageData(string id){
        nowStageData = stages[id];
    }

    public override void Initialize(){
        InitializeStageData();
        SetStageData("0~9 V1", 0); // 設定現在是第幾關
    }

    public override void Update(){
        if(nowStageData.hasSet == true)
            nowStageData.Update();
    }

    public void ReSet(){
        // nowStageData.Reset(); // 關卡重置
        SetGameProcess(GameProcess.Start);
    }

    public void LeaveStage(){
        nowStageData.LeaveStage();
    }

    public void AddHasHitBallCount(){
        nowStageData.AddHasHitBallCount();
    }

    public void LossHeart(int num){
        nowStageData.LossHeart(num);
    }

    // 用不到了
    public void SetStageData(int level){
        nowStageData = stageDatas[level];
        // ReSet(); // 關卡重置 :不知道哪邊出了問題
    }

    public int GetStageDataCount(){
        return stageDatas.Count;
    }

    public IStageDataBox GetStageDataBox(string BoxName){
        if(stageBoxs.ContainsKey(BoxName))
            return stageBoxs[BoxName];
        else
        {
            Debug.LogError($"找不到關卡{BoxName}");
            return null;
        }
    }

    public void SetGameProcess(GameProcess gameProcess){
        nowStageData.SetGameProcess(gameProcess);
    }

    public GameProcess GetNowGameProcess(){
        return nowStageData.m_gameProcess;
    }

    // ================= 設定關卡資料 ===================

    // 初始所有關卡
	private void InitializeStageData()
	{
        CreateStageCardBox1();
        CreateStageCardBox2();
        CreateStageCardBox3();
        CreateStageCardBox10();
        CreateStageCardBox101();

    }

    /// =============================================
    /// ===================== 產生第一組關卡 ===========
    /// =============================================

    public void CreateStageCardBox1(){
        string name = "0~9 V1";
        IStageDataBox StageBox = new IStageDataBox(name, 0);
        StageBox.AddStageData(CreateStagedateTest ()); // 第一關
        StageBox.AddStageData(CreateStagedate0To9_2()); // 第二關
        StageBox.AddStageData(CreateStagedate0To9_3()); // 第三關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器
    }

    private IStageData CreateStagedateTest (){// 第一關
        string stageName = "01 接住紅蕃茄";
        int[] primes ={2, 3};
        int[] composites = {};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCountProcessData(4f , stageName, 3, primes, composites);
        stageData.SetBallProbability(0,0, 10,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData._startText = "接住紅番茄";
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(0.9f);
        return stageData;
    }

    private IStageData CreateStagedate0To9_1 (){// 第一關
        string stageName = "01 接住紅蕃茄";
        int[] primes ={2, 3};
        int[] composites = {};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(4f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0, 10,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData._startText = "接住紅番茄";
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(0.9f);
        return stageData;
    }

    private IStageData CreateStagedate0To9_2 (){// 第一關
        string stageName = "02 閃避綠蕃茄";
        int[] primes ={2, 3};
        int[] composites = {4,6};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(4f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0, 4,6); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData._startText = "閃避綠蕃茄";
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(0.9f);
        return stageData;
    }

    private IStageData CreateStagedate0To9_3 (){// 第一關
        string stageName = "03 番茄上的數字？";
        int[] primes ={2, 3, 5};
        int[] composites = {4, 6, 8};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(4f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0, 6,4); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }



    /// =============================================
    /// ===================== 產生第二組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox2(){
        string name = "0~9 V2";
        IStageDataBox StageBox = new IStageDataBox(name, 10);
        StageBox.AddStageData(CreateStagedate0To9_4 ()); // 第一關
        StageBox.AddStageData(CreateStagedate0To9_5()); // 第二關
        StageBox.AddStageData(CreateStagedate0To9_6()); // 第二關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("0~9 V1");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedate0To9_4 (){// 第一關
        string stageName = "04 紅蕃茄是質數";
        int[] primes ={2, 3, 5, 7};
        int[] composites = {4, 6, 8, 9};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(2f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0, 6,4); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedate0To9_5 (){// 第一關
        string stageName = "05 綠蕃茄是合數";
        int[] primes ={2, 3, 5, 7};
        int[] composites = {4, 6, 8, 9};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(2f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0, 6,4); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(0.8f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedate0To9_6 (){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={2, 3, 5, 7};
        int[] composites = {4, 6, 8, 9};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(3f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(1,1, 5,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(0.8f);
        stageData._startText = "番茄上的數字代表？";
        stageData.SetStagePrice(10); // 設定解鎖所需金錢
        return stageData;
    }

    /// =============================================
    /// ===================== 產生第三組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox3(){
        string name = "10~19 V1";
        IStageDataBox StageBox = new IStageDataBox(name, 10);
        StageBox.AddStageData(CreateStagedate10To19_4 ()); // 第一關
        StageBox.AddStageData(CreateStagedate10To19_5()); // 第二關
        StageBox.AddStageData(CreateStagedate10To19_6()); // 第二關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("0~9 V2");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedate10To19_4 (){// 第一關
        string stageName = "07 番茄長大了";
        int[] primes ={ 5, 7, 11, 13};
        int[] composites = {8, 9, 10, 12};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(3f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(2,1, 4,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(0.8f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedate10To19_5 (){// 第一關
        string stageName = "08 番茄掉落變快了";
        int[] primes ={ 5, 7, 11, 13, 17};
        int[] composites = {8, 9, 10, 12, 14};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(2f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(2,2, 3,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1.2f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedate10To19_6 (){// 第一關
        string stageName = "09 黃番茄好多啊";
        int[] primes ={ 5, 7, 11, 13, 17, 19};
        int[] composites = {8, 9, 10, 12, 14 ,15};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(3f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(4,3, 1,2); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(0.9f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    /// =============================================
    /// ===================== 產生第四組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox10(){
        string name = "40~130 V1";
        IStageDataBox StageBox = new IStageDataBox(name, 100);
        StageBox.AddStageData(CreateStagedateHard_1 ()); // 第一關
        StageBox.AddStageData(CreateStagedateHard_2()); // 第二關
        StageBox.AddStageData(CreateStagedateHard_3()); // 第二關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("10~19 V1");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedateHard_1 (){// 第一關
        string stageName = "100 番茄超進化";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(3f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(2,1, 4,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(0.8f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedateHard_2 (){// 第一關
        string stageName = "101 番茄掉落變快了";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(2f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(2,2, 3,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1.2f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedateHard_3 (){// 第一關
        string stageName = "102 全是黃番茄";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new CatchPrimeStageData(2f , stageName, primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(5,5, 0,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1f);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }



    /// =============================================
    /// ===================== 產生第五組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox101(){
        string name = "番茄會分裂啊";
        IStageDataBox StageBox = new IStageDataBox(name, 100);
        StageBox.AddStageData(CreateStagedateHard_4 ()); // 第一關
        StageBox.AddStageData(CreateStagedateHard_5()); // 第二關
        StageBox.AddStageData(CreateStagedateHard_6()); // 第二關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("40~130 V1");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedateHard_4 (){// 第一關
        string stageName = "綠番茄會分裂";
        int[] primes ={2, 3, 5};
        int[] composites = {4,6,9};
        int[] plusNums = {8, 12,16, 18};
        int[] bossNums = {21,22, 23, 26,29};
        StageData stageData = new StageData(3f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(2,2, 3,2);  // P_prime, P_composites ，三顆球出現機率加總為10
        stageData._startText = "番茄會分裂！";
        return stageData;
    }

    private IStageData CreateStagedateHard_5 (){// 第一關
        string stageName = "遊戲面難";
        int[] primes = {};
        int[] composites =  {24, 30, 36};
        int[] plusNums = {24, 30, 36, 42, 48, 54, 60, 64};
        int[] bossNums = {1024};
        StageData stageData = new StageData(5f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(0,0); // P_prime, P_composites ，三顆球出現機率加總為10
        stageData._startText = "番茄上的數字代表？";
        return stageData;
    }

    private IStageData CreateStagedateHard_6 (){// 第一關
        string stageName = "超高難度";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };//
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {24, 30, 36, 42, 48, 54, 60, 64, 128};
        int[] bossNums = {3628800};
        StageData stageData = new StageData(2f, stageName ,primes, composites, plusNums, bossNums);
        stageData.SetBallProbability(4,4); // P_prime, P_composites ，三顆球出現機率加總為10
        stageData.SetBGM(BGM.Boss);
        stageData._startText = "番茄上的數字代表？";
        return stageData;
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


    // -------------------分數相關--------------------

    public void AddPoint(int poi){
        nowStageData.point += poi;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public void LessPoint(int poi){
        nowStageData.point -= poi;
        if(nowStageData.point < 0)
            nowStageData.point = 0;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public void AddCombol(){
        nowStageData.combol ++ ; // 增加Combol數
        nowStageData.accumulateCombol ++ ;  // 增加累積Combol數
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }
    
    public void MissCombol (){
        nowStageData.combol = 0;
        nowStageData.missCombol ++;
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public int GetPoint(){
        return nowStageData.point;
    }
    public int GetCombol(){
        return nowStageData.combol;
    }

    public float GetGameTime(){
        return nowStageData.GetGameTime();
    }

    public void SetGameTime(float time){
        nowStageData.SetGameTime(time);
    }

    public int GetMissCombol(){
        return nowStageData.missCombol;
    }

    public string GetStageName(){
        return nowStageData.GetStageName();
    }

    // 給予完成率
    public int GetCompletionRate(){
        return nowStageData.GetCompletionRate();
    }

    public void BossComingAnimeEnd(){  // Boss球動畫播完
        nowStageData.BossComingAnimeEnd();
    }

    // ======== 服務方法========
    public StageComplete GetStageComplete(){
        return nowStageData.m_stageComplete;
    }

    
}