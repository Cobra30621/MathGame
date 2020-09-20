using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveSystem : IGameSystem
{
    // 儲存資料
    public SaveData _saveData;

    public SaveSystem(GameMeditor meditor):base(meditor)
	{
		Initialize();
	}

    public override void Initialize(){
        // LoadByJson();
    }

    public SaveData GetSaveData(){
        return _saveData;
    }

    public void SetSaveData(SaveData saveData){
        _saveData = saveData;
        SaveByJson();
    }

    public void ClearSavaData(){
        _saveData.InitSaveData();
        // LoadByJson(GameMeditor.Instance.GetStageNames());
        SaveByJson();
    }

    public void SaveByJson(){
        //將save物件轉換為Json格式的字串
        string saveJsonStr = JsonUtility.ToJson(_saveData);
        //將這個字串寫入到檔案中
        PlayerPrefs.SetString("saveData", saveJsonStr);
        Debug.Log($"<color=blue>儲存：{saveJsonStr}</color>");

        // 將現在所有關卡進度加入 saveLevelIDs
        foreach ( var stage in _saveData.saveLevelIDs )
        {
            PlayerPrefs.SetInt(stage.Key, stage.Value);
            Debug.Log($"<color=blue>存檔{stage.Key}為{stage.Value}</color>");
        }
    }

    public void LoadByJson(List<string> stageNames)
    { 
        //將讀取Json字串
        string saveJsonStr = PlayerPrefs.GetString("saveData");
        //將Json字串轉換為save物件
        _saveData = JsonUtility.FromJson<SaveData>(saveJsonStr);
        Debug.Log($"<color=blue>讀取：{saveJsonStr}</color>");


        // 建立用來存stageName的清單
        // List<string> saveLevelNames = new List<string>();
        /*
        foreach ( var data in _saveData.saveLevelIDs.Keys)
        {
            saveLevelNames.Add(data);
        }*/

        foreach (string stageName in stageNames)
        {
            // string stageName = "第一農場";
            _saveData.saveLevelIDs.Add( stageName,  PlayerPrefs.GetInt(stageName));
            Debug.Log($"<color=blue>讀檔{stageName}為{_saveData.saveLevelIDs[stageName]}</color>");
        }

        // 將現在所有關卡進度加入 saveLevelIDs
        
    }
}