using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData{
    [SerializeField]
    public Dictionary<string, int> stageBoxProgresses = new Dictionary<string, int>();
    
    [SerializeField]
    public int money; 
    [SerializeField]
    public int maxHp;

    public SaveData(){
        InitSaveData();
    }

    public void InitSaveData(){
        money = 0;
        maxHp = 10;
        stageBoxProgresses = CreateStageBoxProgressesFrame();
    }
    
    public void AddStageBoxProcress(string name, int progress){
        if(stageBoxProgresses.ContainsKey(name))
            stageBoxProgresses[name] = progress;
        else
            stageBoxProgresses.Add(name, progress);
    }

    private Dictionary<string, int> CreateStageBoxProgressesFrame(){
        Dictionary<string, int> stageBoxProgresses = new Dictionary<string, int>(); 

        List<string> stageNames = GameMeditor.Instance.GetStageBoxNames();
        // 將現在所有關卡進度加入 stageBoxProgresses
        foreach ( string name in stageNames )
        {
            stageBoxProgresses.Add(name, 0);
        }
        return stageBoxProgresses;
    }

}