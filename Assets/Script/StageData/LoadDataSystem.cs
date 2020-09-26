using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class LoadDataSystem : IGameSystem
{
    public LoadDataSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();
	}

    // CVS資料
    private TextAsset stageInfoFile;
    private TextAsset stageBoxInfoFile;

    // 小關卡StageData
    List<StageInfo> stageInfos = new List<StageInfo> ();
    public List<IStageData> stageDatas = new List<IStageData>(); // 儲存所有的StageData

    // 大關卡StageBox
    public Dictionary<string, IStageDataBox> stageBoxs = new Dictionary<string, IStageDataBox> ();
    public List<string> stageBoxNames = new List<string>(); // 關卡的名稱清單

    public override void Initialize(){
        CreateAllStageDataBox();// 產生所有小關卡的資料
        LoadStageBoxNames(); // 產生關卡的名稱清單
    }

    public Dictionary<string, IStageDataBox> GetStageBoxs(){
        return stageBoxs;
    }

    public List<string> GetStageBoxNames(){
        return stageBoxNames;
    }

    // ========= 內部使用============

    private void LoadStageBoxNames(){
        foreach ( var data in stageBoxs.Keys)
        {
            stageBoxNames.Add(data);
        }
    }

    private Dictionary<string, IStageDataBox> CreateAllStageDataBox(){
        LoadStageInfo(); // 產生所有小關卡的資料

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
    private void LoadStageInfo(){
        stageInfoFile = Resources.Load<TextAsset>("StageData/stageData");

        string [] data = stageInfoFile.text.Split('\n'); 
        for(int i  = 2;  i < data.Length ; i ++){
            string[] row = data[i].Split(',');

            StageInfo info = new StageInfo();

            info._stageName = row[0];
            info._primes = Array.ConvertAll(row[1].Split(' '), int.Parse);
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
            
            stageInfos.Add(info);
        }

    }

}