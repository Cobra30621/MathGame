using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeInfoUI : MonoBehaviour
{
    private static GradeInfoUI instance;
    private Animator m_animator;

    [SerializeField] private Text lab_point, lab_combol, lab_Time, lab_FullCombol, lab_stageName;

    void Awake()
    {
        instance = this;
        m_animator = GetComponent<Animator>();
        Refresh();
        Initialize();
    }

    public static void Initialize(){
        instance.CloseFullCombol();
        RefreshInfo();
    }

    public static void RefreshInfo(){
        instance.Refresh();
    }

    public void Refresh(){
        int point = GameMeditor.Instance.GetPoint();
        if(point < 0)
            lab_point.text = "$ 0";
        else
            lab_point.text = $"$ {point}";

        int combol = GameMeditor.Instance.GetCombol();
        lab_combol.text = $"{combol}";

        /* combol加成
        int combolPlus = combol * 10;
        if (combol == 0)
            lab_combolPlus.text = "";
        else
            lab_combolPlus.text = $"+{combolPlus}%";
        */

        string stageName = GameMeditor.Instance.GetStageName();
        lab_stageName.text = stageName;
        
    }

    public static void UpdateTime(){
        instance.RefreshTime();
    }

    public void RefreshTime(){
        float Rawtime = GameMeditor.Instance.GetGameTime();
        int time;
        if (Rawtime <= 0)
            time = 0;
        else
            time = Mathf.FloorToInt(Rawtime) + 1;

        lab_Time.text = $"{time}";
    }

    public static void ShowWhetherFullCombol(){
        instance.ShowFullCombol();
    }

    public void ShowFullCombol(){
        int MissCombol = GameMeditor.Instance.GetMissCombol();
        lab_FullCombol.gameObject.SetActive(true);
        if (MissCombol == 0)
            lab_FullCombol.text = "Full Combol";
        else
            lab_FullCombol.text = $"Miss Combol: {MissCombol}";
        
        lab_Time.gameObject.SetActive(false); // 影藏時間
    }

    public void CloseFullCombol(){
        lab_FullCombol.gameObject.SetActive(false);
        lab_Time.gameObject.SetActive(true); // 顯示時間
    }

    public static void PlayBossComingAnime(){
		instance.m_animator.Play("BossComing", 0 , 0f);
	}

    public void BossComingAnimeEnd(){  // Boss球動畫播完
        GameMeditor.Instance.BossComingAnimeEnd();
    }

}
