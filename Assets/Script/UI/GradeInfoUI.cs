using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeInfoUI : MonoBehaviour
{
    private static GradeInfoUI instance;

    [SerializeField] private Text lab_point, lab_combol, lab_combolPlus;

    void Start()
    {
        instance = this;
        Refresh();
    }

    public static void RefreshInfo(){
        instance.Refresh();
    }

    public void Refresh(){
        int point = GameMeditor.Instance.GetPoint();
        if(point < 0)
            lab_point.text = "Grade:0";
        else
            lab_point.text = $"Grade:{point}";

        int combol = GameMeditor.Instance.GetCombol();
        lab_combol.text = $"{combol}";

        int combolPlus = combol * 10;
        if (combol == 0)
            lab_combolPlus.text = "";
        else
            lab_combolPlus.text = $"+{combolPlus}%";
        
    
    }



}
