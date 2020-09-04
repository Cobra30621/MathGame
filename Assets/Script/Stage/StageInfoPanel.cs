using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoPanel : MonoBehaviour
{
    private StageSystem stageSystem;
    [SerializeField] GameObject stageDataCardPrefab;


    void Awake(){
        stageSystem = GameMeditor.Instance.GetStageSystem();
        Initialize();
    }

    private void Initialize(){
        // 讀取關卡資料，產生字卡
        Dictionary<string, IStageData> stages = stageSystem.stages;
        foreach ( var stage in stages )
        {
            CreateStageDataCard(stage.Value);
        }
    }

        /// <summary>
    /// 製作一個EarnMoneyLabel，自動移動至錢錢顯示位置後加錢
    /// </summary>
    public void CreateStageDataCard(IStageData stageData)
    {
        var g = Instantiate(stageDataCardPrefab, transform);
        // g.transform.SetParent(transform);

        var l = g.GetComponent<IStageDataCard>();
        l.Initialize(stageData);
    }

}