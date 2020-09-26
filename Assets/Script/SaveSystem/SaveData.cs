using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData{
    [SerializeField]
    public Dictionary<string, int> saveLevelIDs = new Dictionary<string, int>();
    
    [SerializeField]
    public int money; 
    [SerializeField]
    public int maxHp;

    public SaveData(){
        money = 0;
        maxHp = 10;
    }

    public void InitSaveData(){
        money = 0;
        maxHp = 10;
        saveLevelIDs = InitLevelData();
    }

    private Dictionary<string, int> InitLevelData(){
        Dictionary<string, int> saveLevelIDs = new Dictionary<string, int>(); 

        List<string> stageNames = GameMeditor.Instance.GetStageNames();
        // 將現在所有關卡進度加入 saveLevelIDs
        foreach ( string name in stageNames )
        {
            saveLevelIDs.Add(name, 0);
        }
        return saveLevelIDs;
    }

}