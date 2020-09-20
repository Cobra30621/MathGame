using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LoadStageData 
{    
    // CVS資料
    private TextAsset stageInfoFile;
    private TextAsset stageBoxInfoFile;

    // 變成List的資料
    List<StageInfo> stageInfos = new List<StageInfo> ();
    public Dictionary<string, IStageDataBox> stageBoxs = new Dictionary<string, IStageDataBox> ();
    public List<IStageData> stageDatas = new List<IStageData>(); // 儲存所有的StageData

    List<string> saveLevelNames = new List<string>(); // 關卡的名稱清單

    public Dictionary<string, IStageDataBox> GetAllStageDataBox(){
        return CreateAllStageDataBox();
    }

    public List<string> GetStageNames(){
        return saveLevelNames;
    }

    // 讀取資料
    public void LoadData(){
        Dictionary<string, int> saveLevelIDs = InitLevelData();  // 關卡資料建立基本架構

        // 將資料讀進架構中，如果不在原資料的東西就定為0
        saveLevelNames = new List<string>();
        foreach ( var data in saveLevelIDs.Keys)
        {
            saveLevelNames.Add(data);
        }

        GameMeditor.Instance.LoadByJson(saveLevelNames); // 讀取關卡進度資料;
        SaveData saveData = GameMeditor.Instance.GetSaveData(); // 讀取資料
        Debug.Log(saveData);
        Dictionary<string, int> saveLevelIDsLoad = saveData.saveLevelIDs;  // 讀取關卡進度資料;

        foreach (string stageName in saveLevelNames)
        {
                // string stageName = "第一農場";
            if(saveLevelIDsLoad.ContainsKey(stageName)) // 如果讀的資料有在裡面，賦予值
            {
                saveLevelIDs[stageName] = saveLevelIDsLoad[stageName];
            }
            else  // 如果讀的資料有在裡面，值給予0
            {
                saveLevelIDs[stageName] = 0;
                Debug.Log($"初始化{stageName}為0");
            }
        }
        
        // 將所有的資料放入
        foreach ( string stageName in saveLevelNames )
        {
            if(stageBoxs.ContainsKey(stageName))
                stageBoxs[stageName].saveLevelID = saveLevelIDs[stageName];
            else
                Debug.LogError($"找不到在stageBoxs的{stageName}，有點問題歐");
            stageBoxs[stageName].InitNowLevelID(); // 初始化所有的NowLevelID;
        }
    }

    private Dictionary<string, int> InitLevelData(){
        Dictionary<string, int> saveLevelIDs = new Dictionary<string, int>(); 

        // 將現在所有關卡進度加入 saveLevelIDs
        foreach ( var stage in stageBoxs )
        {
            saveLevelIDs.Add(stage.Key, 0);
        }
        return saveLevelIDs;
    }

    private Dictionary<string, IStageDataBox> CreateAllStageDataBox(){
        InitstageInfo(); // 產生所有小關卡的資料

        // 產生所有的小關卡
        foreach(StageInfo info in stageInfos){
            stageDatas.Add(CreateStageData(info));
        }

        // 初始化所有的StageDataBox
        stageInfoFile = Resources.Load<TextAsset>("StageData/stageDataBox");
        string [] data = stageInfoFile.text.Split('\n'); 

        for(int i  = 2;  i < data.Length ; i ++){
            string[] row = data[i].Split(',');

            string stageName = row[0]; // 關卡名稱
            int[] numRange = Array.ConvertAll(row[1].Split(' '), int.Parse); // 數字範圍
            string preStage = row[2]; // 上一個關卡

            CreateStageDataBox(stageName, numRange, preStage); // 創造關卡
        }

        LoadData();// 讀取關卡進度資料
        return stageBoxs;
    }

    // 產生單一StageBox
    private void CreateStageDataBox(string stageName, int[] numRange, string preStage){
        IStageDataBox StageBox = new IStageDataBox(stageName);
        
        foreach(IStageData stageData in stageDatas){ // 設置子關卡
            if(stageData._boxName == stageName) 
            StageBox.AddStageData(stageData);
        }
        
        StageBox.SetNumRange(numRange[0],numRange[1]);

        stageBoxs.Add(stageName, StageBox); // 加入StageSystem管理器
        
        if(preStage == "null") // 沒有前一關
        {
            StageBox.SetCardState(CardState.HasNotStart); // 解鎖
            return;
        }
             

        IStageDataBox preStageBox = GetStageDataBox(preStage); // 上一個關卡
        preStageBox.SetNetStageBox(StageBox); // 把自己設為下一個關卡
    }

    // 取得stageDataBox
    private IStageDataBox GetStageDataBox(string BoxName){
        if(stageBoxs.ContainsKey(BoxName))
            return stageBoxs[BoxName];
        else
        {
            Debug.LogError($"找不到關卡{BoxName}");
            return null;
        }
    }

    // 產生單一StageData
    private IStageData CreateStageData (StageInfo info){
        IStageData stageData = new BallCatchTomatoData(info._CoolDown ,info._ballCounts, info._primes, info._composites);
        int [] p = info._probability; // 機率
        stageData.SetBallProbability(p[0],p[1], p[2],p[3]); // Judge_Prime, Judge_Composite, P_prime, P_composites, Plus ，出現機率加總為10
        stageData.SetStageBallStrategy(new BarCatchPrimeStrategy(info._ballMoveMethon));
        stageData.SetBallCountCreateOnceTime(info._ballCountOnce);
        stageData.SetBallSpeed(info._fallingSpeed);
        stageData._startText = info._startText;
        stageData.SetBGM(info._bgm);
        stageData._boxName = info._stageName;
        
        return stageData;
    }

    // 產生所有小關卡StageData的資料
    private void InitstageInfo(){
        stageInfoFile = Resources.Load<TextAsset>("StageData/stageData");

        string [] data = stageInfoFile.text.Split('\n'); 
        for(int i  = 2;  i < data.Length ; i ++){
            string[] row = data[i].Split(',');

            StageInfo info = new StageInfo();

            info._stageName = row[0];
            info._primes = Array.ConvertAll(row[1].Split(' '), int.Parse);
            Debug.Log("row[2].Split(' ')"+ row[2].Split(' '));
            info._composites = Array.ConvertAll(row[2].Split(' '), int.Parse) ;
            info._probability = Array.ConvertAll(row[3].Split(' '), int.Parse) ;
            info._startText = row[4];
            info._ballCounts = int.Parse(row[5]);
            info._fallingSpeed = float.Parse(row[6]);
            info._ballCountOnce = int.Parse(row[7]);
            info._CoolDown = int.Parse(row[8]);

            // 球的路徑
            switch (row[9]){
                case "S":
                    info._ballMoveMethon = BallMoveMethon.Straight;
                    break;
                case "R":
                    info._ballMoveMethon = BallMoveMethon.Ramdom;
                    break;
                default :
                    Debug.Log("路徑輸入錯誤"+ row[9]);
                    break;
            }

            // BGM
            string bgm = row[10];
            switch (bgm){
                case "Boss":
                    info._bgm = BGM.Boss;
                    break;
                case "Normal":
                    info._bgm = BGM.Normal;
                    break;
                default :
                    Debug.Log("輸入錯誤"+ row[10]);
                    break;
            }

            /*
            if (row[10] == "Boss")
            {
                info._bgm = BGM.Boss;
                Debug.Log($"BGM.Boss:{row[10]}");
            }
                
            if (row[10] == "Normal")
            {
                Debug.Log($"BGM.Normal:{row[10]}");
                info._bgm = BGM.Normal;
            }
            */
            

            Debug.Log($"info._bgm:{info._bgm}");
            //probability	startText	ballCounts	fallingSpeed	ballCountCreate	CoolDown	BallStrategy

            stageInfos.Add(info);
        }

    }

}