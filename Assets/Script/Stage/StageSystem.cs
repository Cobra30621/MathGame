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

    public override void Initialize(){
        stageBoxs =  meditor.GetStageBoxs() ; // 讀取所有關卡
        LoadStageBoxsData(); // 讀取關卡進度
        SetStageData("第一農場", 0); // 設定現在是第幾關
    }

    // 儲存關卡進度資料
    public void SaveLevelData(){
        SaveData saveData = GameMeditor.Instance.GetSaveData();

        // 將現在所有關卡進度儲存
        Dictionary<string, int> stageBoxProgresses  = new Dictionary<string, int>();
        foreach ( var stage in stageBoxs )
        {
            stageBoxProgresses.Add(stage.Key, stage.Value.saveLevelID);
        }

        saveData.stageBoxProgresses = stageBoxProgresses;

        GameMeditor.Instance.SetSaveData(saveData); // 存入
    }

    public void LoadStageBoxsData(){
        List<string> stageBoxNames = meditor.GetStageBoxNames();
        Dictionary<string, int> stageDataProgress = meditor.GetSaveData().stageBoxProgresses;

        // 將所有的資料放入
        foreach ( string stageName in stageBoxNames )
        {
            if(stageBoxs.ContainsKey(stageName))
                stageBoxs[stageName].saveLevelID = stageDataProgress[stageName];
            else
                Debug.LogError($"找不到在stageBoxs的{stageName}，有點問題歐");
            stageBoxs[stageName].InitNowLevelID(); // 初始化所有的NowLevelID;
        }
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