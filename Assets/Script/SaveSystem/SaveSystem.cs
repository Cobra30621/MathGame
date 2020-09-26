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
        LoadByJson();
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
        SaveByJson();
    }

    public void SaveByJson(){
        //將save物件轉換為Json格式的字串
        string saveJsonStr = JsonUtility.ToJson(_saveData);
        //將這個字串寫入到檔案中
        PlayerPrefs.SetString("saveData", saveJsonStr);
        Debug.Log($"<color=blue>儲存：{saveJsonStr}</color>");

        // 將現在所有關卡進度加入 saveLevelIDs
        foreach ( var stage in _saveData.stageBoxProgresses )
        {
            PlayerPrefs.SetInt(stage.Key, stage.Value);
            Debug.Log($"<color=blue>存檔{stage.Key}為{stage.Value}</color>");
        }
    }

    public void LoadByJson()
    { 
        //將讀取Json字串
        string saveJsonStr = PlayerPrefs.GetString("saveData");

        // 第一次進行遊戲
        if(saveJsonStr == "")
        {
            _saveData = new SaveData();
        }
        else
        {
            //將Json字串轉換為save物件
            _saveData = JsonUtility.FromJson<SaveData>(saveJsonStr);
            Debug.Log($"<color=blue>讀取：{saveJsonStr}</color>");
        }

        // 讀取關卡進度資料
        List<string> stageNames = meditor.GetStageBoxNames();
        foreach (string stageName in stageNames)
        {
            if(PlayerPrefs.HasKey(stageName))
            {
                _saveData.AddStageBoxProcress( stageName,  PlayerPrefs.GetInt(stageName));
                Debug.Log($"<color=blue>讀檔{stageName}為{_saveData.stageBoxProgresses[stageName]}</color>");
            }
            else
            {
                _saveData.AddStageBoxProcress( stageName , 0);
                Debug.Log($"<color=blue>建立檔按{stageName}為0</color>");
            }
                
        }
        
    }
}