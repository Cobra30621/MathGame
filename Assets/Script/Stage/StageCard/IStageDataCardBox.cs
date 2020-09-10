using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IStageDataCardBox : MonoBehaviour
{
    [SerializeField] private Text lab_name, lab_Butt;
    [SerializeField] private Button buyButt;
    private GameObject stageDataCardPrefab;

    public IStageDataBox _stageBox;
    public List<IStageDataCard> _stateCards;
    // 製作CardList，RefreshButton


    // 購買關卡
    public void BuyStage(){
        if(GameMeditor.Instance.WhetherBuyStage(_stageBox._stagePrice)) // 如果錢夠
        {
            // PlayBuyAnime()
            GameMeditor.Instance.BuyThing(_stageBox._stagePrice);
            _stageBox._cardState = CardState.HasBuy;
            _stageBox.stageDatas[0].m_stageState = StageState.Open; // 開啟第一關
        }
        else
        {
            // 顯示錢不夠
        }
        RefreshInfo();
    }

    public void RefreshInfo(){
        RefershBuyButton();
        RefreshCard();
    }

    // 更新購買按鈕顏色
    public void RefershBuyButton(){
        _stageBox.UpdateCardState(); // 更新卡片狀態
        
        // 設定按鈕顏色
        ColorBlock cb= buyButt.colors;
        switch (_stageBox._cardState){
            case CardState.Lock:
                lab_Butt.text = "Unlock"; // 設定按鈕文字
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case CardState.NeedMoney:
                buyButt.onClick.AddListener(delegate{  });

                lab_Butt.text = $" ${_stageBox._stagePrice}"; // 設定按鈕文字
                cb.normalColor = Color.gray; // 按鈕設定為灰色
                cb.highlightedColor = Color.gray; //滑鼠觸碰顏色
                break;
            case CardState.EnoughMoney:
                buyButt.onClick.AddListener(delegate
                { 
                    BuyStage(); // 購買關卡
                });

                lab_Butt.text = $" ${_stageBox._stagePrice}";
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case CardState.HasBuy:
                buyButt.onClick.AddListener(delegate{  });

                lab_Butt.text = "已解鎖"; // 設定按鈕文字
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            case CardState.Complete:
                buyButt.onClick.AddListener(delegate{  });

                lab_Butt.text = "Complete"; // 設定按鈕文字
                cb.normalColor = Color.white; // 按鈕設定為灰色
                cb.highlightedColor = Color.white; //滑鼠觸碰顏色
                break;
            default:
                Debug.LogError("出現不該出現的CardState");
                break;
        }
         
        buyButt.colors = cb; // 設定按鈕顏色
    }

    // 更新所有的卡片
    public void RefreshCard(){
        foreach(IStageDataCard card in _stateCards){
            card.RefreshInfo();
        }
    }



    // ================= 初始化介面==================


    public void Initialize(IStageDataBox stageBox){
        // 設置StageBataBox
        _stageBox = stageBox;

        lab_name.text = _stageBox.stageName;
        RefershBuyButton();

        // 加入管理器
        GameMeditor.Instance.AddStageCardBox(this);

        // 讀取字卡Prefabs
        ResourceAssetFactory factory = MainFactory.GetResourceAssetFactory();
        stageDataCardPrefab = factory.LoadStageInfoCard();
        
        // 讀取關卡資料，產生字卡
        foreach ( var stage in _stageBox.stageDatas )
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
        _stateCards.Add(l);
    }


    // ===測試用====
    public void OverComeStage(){
        _stageBox.SetStagesState(StageState.Open); // 開啟所有關卡
        _stageBox.UnlockNextStageBox(); // 開啟下一關
        RefreshCard(); // 更新卡片
        GameMeditor.Instance.RefreshAllCard(); // 更新所有卡片
    }


}
