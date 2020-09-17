using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    private static StageSelectUI instance;
    // private Animator m_animator;

    [SerializeField] private Text lab_money;

    void Start()
    {
        instance = this;
        Initialize();
        Refresh();
    }

    public static void Initialize(){
        
    }
    public static void RefreshInfo(){
        instance.Refresh();
    }

    public void Refresh(){
        int money = GameMeditor.Instance.GetMoney();
        lab_money.text = $"{money}";
    }
}
