using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageInfoPanel : MonoBehaviour
{
    private StageSystem stageSystem;
    [SerializeField] GameObject stageDataCardPrefab;
    [SerializeField] GameObject stageCardBoxPrefab;
    

    private GameObject test;


    void Awake(){
        stageSystem = GameMeditor.Instance.GetStageSystem();
        Initialize();
    }

    void Update(){
        if(Input.GetKey(KeyCode.RightArrow))
             test.transform.position += new Vector3(10,0,100);
    }

    private void Initialize(){
        // 讀取關卡Box資料，產生字卡
        Dictionary<string, IStageDataBox> stages = stageSystem.stageBoxs;
        foreach ( var stage in stages )
        {
            CreateStageDataCard(stage.Value);
        }
        
    }

    /// <summary>
    /// 製作一個CreateStageDataCard
    /// </summary>
    public void CreateStageDataCard(IStageDataBox stageBox)
    {
        var g = Instantiate(stageCardBoxPrefab, transform);
        // g.transform.SetParent(transform);

        var l = g.GetComponent<IStageDataCardBox>();
        l.Initialize(stageBox); // 產生StageCrad
        // g.transform.DOMoveY(100f,100);
    }


    /*
    private void Initialize(){
        // 讀取關卡資料，產生字卡
        Dictionary<string, IStageData> stages = stageSystem.stages;
        foreach ( var stage in stages )
        {
            CreateStageDataCard(stage.Value);
        }
    }

    /// <summary>
    /// 製作一個CreateStageDataCard，自動移動至錢錢顯示位置後加錢
    /// </summary>
    public void CreateStageDataCard(IStageData stageData)
    {
        var g = Instantiate(stageDataCardPrefab, transform);
        // g.transform.SetParent(transform);

        var l = g.GetComponent<IStageDataCard>();
        l.Initialize(stageData);
    }

    */

}