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
    public IStageDataBox nowStageDataBox;
    

    public Dictionary<string, IStageDataBox> stageBoxs = new Dictionary<string, IStageDataBox> ();
    public List<IStageDataCardBox> _cardBoxs = new List<IStageDataCardBox>();
    private LoadStageData _loadStageData;

    public override void Initialize(){
        Debug.Log("CreateStageSystem");
        _loadStageData = new LoadStageData();
        stageBoxs =  _loadStageData.GetAllStageDataBox(); // 初始化所有關卡
        SetStageData("第一農場", 0); // 設定現在是第幾關
    }

    // 儲存關卡進度資料
    public void SaveLevelData(){
        SaveData saveData = GameMeditor.Instance.GetSaveData();
        Dictionary<string, int> saveLevelIDs  = new Dictionary<string, int>();

        // 將現在所有關卡進度加入 saveLevelIDs
        foreach ( var stage in stageBoxs )
        {
            // Debug.Log($"存檔{stage.Key}為{stage.Value.saveLevelID}");
            saveLevelIDs.Add(stage.Key, stage.Value.saveLevelID);
        }
        saveData.saveLevelIDs = saveLevelIDs;

        GameMeditor.Instance.SetSaveData(saveData); // 存入
    }

    // ============== Dictionary方法 ======================= 

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

        nowStageDataBox = stageBox; // 設定現在的stageBox

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

    public void AddHeart(int num){
        nowStageData.AddHeart(num);
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public void LossHeart(int num){
        nowStageData.LossHeart(num);
        GradeInfoUI.RefreshInfo(); // 刷新分數介面
    }

    public List<string> GetStageNames(){ // 取的關卡名稱清單
        return  _loadStageData.GetStageNames();
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
        CreateStageCardBox4();
        CreateStageCardBox100();
        /*
        CreateStageCardBox10();
        CreateStageCardBox101();*/

    }

    /// =============================================
    /// ===================== 產生第一組關卡 ===========
    /// =============================================

    public void CreateStageCardBox1(){
        string name = "第一農場";
        IStageDataBox StageBox = new IStageDataBox(name, 0);
        StageBox.SetNumRange(2,9);
        StageBox.SetCardState(CardState.HasNotStart);
        StageBox.AddStageData(CreateStagedate0To9_1()); // 第一關
        StageBox.AddStageData(CreateStagedate0To9_2()); // 第二關
        StageBox.AddStageData(CreateStagedate0To9_3()); // 第三關
        StageBox.AddStageData(CreateStagedate0To9_4()); // 第四關
        StageBox.AddStageData(CreateStagedate0To9_5()); // 第五關
        // StageBox.AddStageData(CreateStagedate0To9_6()); // 第六關
        
        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器
    }

    private IStageData CreateStagedate0To9_1 (){// 第一關
        string stageName = "01 ";
        int[] primes ={2, 3};
        int[] composites = {};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName, 3, primes, composites);
        stageData.SetBallProbability(0,0, 10,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(0.9f);
        stageData._startText = "接住成熟的紅蕃茄";
        return stageData;
    }

    private IStageData CreateStagedate0To9_2 (){// 第一關
        string stageName = "02 閃避綠蕃茄";
        int[] primes ={2, 3};
        int[] composites = {4,6};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName, 5, primes, composites);
        stageData.SetBallProbability(0,0, 7,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData._startText = "閃避不成熟的綠蕃茄";
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
        IStageData stageData = new BallCatchTomatoData(3f , stageName,5, primes, composites);
        stageData.SetBallProbability(0,0, 7,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData._startText = "番茄上有質數或合數";
        return stageData;
    }

    private IStageData CreateStagedate0To9_4 (){// 第一關
        string stageName = "04 紅蕃茄是質數";
        int[] primes ={2, 3, 5, 7};
        int[] composites = {4, 6, 8, 9};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,5, primes, composites);
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallProbability(0,0, 7,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1f);
        stageData._startText = "質數為成熟番茄\n合數為不成熟番茄";
        return stageData;
    }

    private IStageData CreateStagedate0To9_5 (){// 第一關
        string stageName = "05 綠蕃茄是合數";
        int[] primes ={2, 3, 5, 7};
        int[] composites = {4, 6, 8, 9};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,7, primes, composites);
        stageData.SetBallProbability(0,0, 7,3); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1.3f);
        stageData.SetBGM(BGM.Boss);
        stageData._startText = "番茄掉落速度變快";
        return stageData;
    }

    /// =============================================
    /// ===================== 產生第二組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox2(){
        string name = "第二農場";
        IStageDataBox StageBox = new IStageDataBox(name, 0);
        StageBox.SetNumRange(2,19);
        StageBox.AddStageData(CreateStagedata2_1 ()); // 第一關
        StageBox.AddStageData(CreateStagedata2_2()); // 第二關
        StageBox.AddStageData(CreateStagedata2_3()); // 第三關
        StageBox.AddStageData(CreateStagedata2_4()); // 第四關
        StageBox.AddStageData(CreateStagedata2_5()); // 第五關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("第一農場");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }


    private IStageData CreateStagedata2_1 (){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={2, 3, 5, 7};
        int[] composites = {4, 6, 8, 9};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(4f , stageName,5, primes, composites);
        stageData.SetBallProbability(1,1, 6,2); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(0.8f);
        stageData._startText = "黃番茄不知道是否成熟";
        return stageData;
    }

    private IStageData CreateStagedata2_2 (){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={ 5, 7, 11, 13};
        int[] composites = {8, 9, 10, 12};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(4f , stageName,6, primes, composites);
        stageData.SetBallProbability(2,1, 5,2); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1f);
        stageData._startText = "黃番茄要自行判斷\n為質數還是合數";
        return stageData;
    }

    private IStageData CreateStagedata2_3(){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={ 5, 7, 11, 13, 17};
        int[] composites = {8, 9, 10, 12, 14};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(4f , stageName,6, primes, composites);
        stageData.SetBallProbability(3,2, 4,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1f);
        
        return stageData;
    }

    private IStageData CreateStagedata2_4(){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={ 5, 7, 11, 13, 17, 19};
        int[] composites = {8, 9, 10, 12, 14, 15, 16, 18};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(4f , stageName,7, primes, composites);
        stageData.SetBallProbability(4,2, 3,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1.3f);
        stageData._startText = "番茄掉落速度加快";
        return stageData;
    }

    private IStageData CreateStagedata2_5(){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={ 5, 7, 11, 13, 17, 19};
        int[] composites = {8, 9, 10, 12, 14 ,15, 16, 18};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(4f , stageName,7, primes, composites);
        stageData.SetBallProbability(4,2, 3,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        // stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        // stageData.SetBallCountCreateOnceTime(2);
        stageData.SetBallSpeed(1f);
        stageData.SetBGM(BGM.Boss);
        stageData._startText = "最後一顆番茄樹";
        return stageData;
    }

    /// =============================================
    /// ===================== 產生第三組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox3(){
        string name = "第三農場";
        IStageDataBox StageBox = new IStageDataBox(name, 0);
         StageBox.SetNumRange(13,29);
        StageBox.AddStageData(CreateStagedata3_1 ()); // 第一關
        StageBox.AddStageData(CreateStagedata3_2()); // 第二關
        StageBox.AddStageData(CreateStagedata3_3()); // 第三關
        StageBox.AddStageData(CreateStagedata3_4()); // 第四關
        StageBox.AddStageData(CreateStagedata3_5()); // 第五關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("第二農場");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedata3_1 (){// 第一關
        int[] primes ={13, 17, 19, 23};
        int[] composites = {14 ,15, 16, 18, 20, 21};
        IStageData stageData = new BallCatchTomatoData(3f ,5, primes, composites);
        stageData.SetBallProbability(1,1, 6,2); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        return stageData;
    }

    private IStageData CreateStagedata3_2 (){// 第二關
        int[] primes ={13, 17, 19, 23, 29};
        int[] composites = {14 ,15, 16, 18, 20, 21, 22, 24, 25, 27, 29};
        IStageData stageData = new BallCatchTomatoData(3f ,7, primes, composites);
        stageData.SetBallProbability(3,2, 4,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        return stageData;
    }

    private IStageData CreateStagedata3_3(){// 第三關
        int[] primes ={13, 17, 19, 23, 29};
        int[] composites = {16, 18, 20, 21, 22, 24, 25, 27, 28};
        IStageData stageData = new BallCatchTomatoData(3f,7, primes, composites);
        stageData.SetBallProbability(5,2, 2,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData._startText = "大量黃番茄";
        return stageData;
    }

    private IStageData CreateStagedata3_4(){// 第四關
        int[] primes ={13, 17, 19, 23, 29};
        int[] composites = { 20, 21, 22, 24, 25, 27, 28};
        IStageData stageData = new BallCatchTomatoData(3f ,7, primes, composites);
        stageData.SetBallProbability(7,3, 0,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1.5f);
        stageData._startText = "番茄掉落變快了";
        return stageData;
    }

    private IStageData CreateStagedata3_5(){// 第五關
        int[] primes ={13, 17, 19, 23, 29};
        int[] composites = { 20, 21, 22, 24, 25, 27, 28};
        IStageData stageData = new BallCatchTomatoData(3f ,7, primes, composites);
        stageData.SetBallProbability(5,2, 2,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData._startText = "番茄不直線掉落了";
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }

    /// =============================================
    /// ===================== 產生第四組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox4(){
        string name = "第四農場";
        IStageDataBox StageBox = new IStageDataBox(name, 0);
        StageBox.SetNumRange(17,39);
        StageBox.AddStageData(CreateStagedata4_1 ()); // 第一關
        StageBox.AddStageData(CreateStagedata4_2()); // 第二關
        StageBox.AddStageData(CreateStagedata4_3()); // 第三關
        StageBox.AddStageData(CreateStagedata4_4()); // 第四關
        StageBox.AddStageData(CreateStagedata4_5()); // 第五關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("第三農場");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedata4_1 (){// 第一關
        int[] primes ={17, 19, 23, 29, 31, 37};
        int[] composites = { 22, 24, 25, 27, 28, 30, 32, 33,34, 35, 36, 38, 39};
        IStageData stageData = new BallCatchTomatoData(3f ,5, primes, composites);
        stageData.SetBallProbability(1,1, 6,2); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        return stageData;
    }

    private IStageData CreateStagedata4_2 (){// 第二關
        int[] primes ={ 17, 19, 23, 29, 31, 37};
        int[] composites = { 22, 24, 25, 27, 28, 30, 32, 33,34, 35, 36, 38, 39};
        IStageData stageData = new BallCatchTomatoData(3f ,7, primes, composites);
        stageData.SetBallProbability(5,2, 2,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData._startText = "大量黃番茄";
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        return stageData;
    }

    private IStageData CreateStagedata4_3(){// 第三關
        int[] primes ={17, 19, 23, 29, 31, 37};
        int[] composites = { 22, 24, 25, 27, 28, 30, 32, 33,34, 35, 36, 38, 39};
        IStageData stageData = new BallCatchTomatoData(3f,7, primes, composites);
        stageData.SetBallProbability(5,2, 2,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1.5f);
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData._startText = "番茄掉落變快了";
        return stageData;
    }

    private IStageData CreateStagedata4_4(){// 第四關
        int[] primes ={17, 19, 23, 29, 31, 37};
        int[] composites = { 22, 24, 25, 27, 28, 30, 32, 33,34, 35, 36, 38, 39};
        IStageData stageData = new BallCatchTomatoData(3f ,7, primes, composites);
        stageData.SetBallProbability(7,3, 0,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetBallSpeed(1f);
        stageData._startText = "番茄不直線掉落了";
        return stageData;
    }

    private IStageData CreateStagedata4_5(){// 第五關
        int[] primes ={17, 19, 23, 29, 31, 37};
        int[] composites = { 22, 24, 25, 27, 28, 30, 32, 33,34, 35, 36, 38, 39};
        IStageData stageData = new BallCatchTomatoData(3f ,10, primes, composites);
        stageData.SetBallProbability(5,2, 2,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallCountCreateOnceTime(2);
        stageData.SetBallSpeed(0.8f);
        stageData._startText = "一次掉落兩顆番茄";
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }





    
    /// =============================================
    /// ===================== 產生第三組關卡 ===========
    /// =============================================

    // 產生Box卡
    public void CreateStageCardBox100(){
        string name = "第一百農場";
        IStageDataBox StageBox = new IStageDataBox(name, 10);
         StageBox.SetNumRange(41,999);
        StageBox.AddStageData(CreateStagedata100_1 ()); // 第一關
        StageBox.AddStageData(CreateStagedata100_2()); // 第二關
        StageBox.AddStageData(CreateStagedata100_3()); // 第三關
        StageBox.AddStageData(CreateStagedata100_4()); // 第四關
        StageBox.AddStageData(CreateStagedata100_5()); // 第五關

        stageBoxs.Add(name, StageBox); // 加入StageSystem管理器

        IStageDataBox preStageBox = GetStageDataBox("第四農場");
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    private IStageData CreateStagedata100_1 (){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,7, primes, composites);
        stageData.SetBallProbability(1,1, 6,2); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1.8f);
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }

    private IStageData CreateStagedata100_2 (){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,15, primes, composites);
        stageData.SetBallProbability(3,2, 4,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallCountCreateOnceTime(3);
        stageData.SetBallSpeed(1f);
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }

    private IStageData CreateStagedata100_3(){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,15, primes, composites);
        stageData.SetBallProbability(7,3, 0,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        // stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1.5f);
        stageData.SetBallCountCreateOnceTime(3);
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }

    private IStageData CreateStagedata100_4(){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes = { 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127 };
        int[] composites =  { 51, 55, 57, 65 , 69,77,82,85, 87,91,95,99,106,  111, 115 , 123,129};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,20, primes, composites);
        stageData.SetBallProbability(7,3, 0,0); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallSpeed(1f);
        stageData.SetBallCountCreateOnceTime(5);
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }

    private IStageData CreateStagedata100_5(){// 第一關
        string stageName = "06 黃番茄登場";
        int[] primes ={ 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997};
        int[] composites = {278,273, 279,287,289, 291, 293,  300, 268, 819, 1011};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCatchTomatoData(3f , stageName,10, primes, composites);
        stageData.SetBallProbability(5,2, 2,1); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(BallMoveMethon.Straight));
        stageData.SetBallCountCreateOnceTime(2);
        stageData.SetBallSpeed(1f);
        stageData.SetBGM(BGM.Boss);
        
        return stageData;
    }


    /*

    private IStageData CreateStagedate10To19_4 (){// 第一關
        string stageName = "07 番茄長大了";
        int[] primes ={ 5, 7, 11, 13};
        int[] composites = {8, 9, 10, 12};
        int[] plusNums = {};
        int[] bossNums = {};
        IStageData stageData = new BallCountProcessData(3f , stageName, primes, composites, plusNums, bossNums);
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
        IStageData stageData = new BallCountProcessData(2f , stageName, primes, composites, plusNums, bossNums);
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
        IStageData stageData = new BallCountProcessData(3f , stageName, primes, composites, plusNums, bossNums);
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
        IStageData stageData = new BallCountProcessData(3f , stageName, primes, composites, plusNums, bossNums);
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
        IStageData stageData = new BallCountProcessData(2f , stageName, primes, composites, plusNums, bossNums);
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
        IStageData stageData = new BallCountProcessData(2f , stageName, primes, composites, plusNums, bossNums);
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

    */
    


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