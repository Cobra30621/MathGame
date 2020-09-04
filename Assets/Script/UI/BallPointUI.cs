using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPointUI : MonoBehaviour
{
    private static BallPointUI instance;

    [SerializeField] private GameObject PointLabelBlue;
    [SerializeField] private GameObject PointLabelRed;
    [SerializeField] private GameObject PointLabelBoss;

    void Awake()
    {
        instance = this;
    }

    
    public static void CreateGetPointLabel(Vector3 vector, int point){
        instance.CreatePointLabelBlue(vector, $"+{point}");
    }

    public static void CreateAddCombolLabel(Vector3 vector){
        instance.CreatePointLabelBlue(vector, $"Great");
    }

    public static void CreateMissLabel(Vector3 vector){
        instance.CreatePointLabelRed(vector, $"Miss");
    }

    public static void CreateLossPointLabel(Vector3 vector, int point){
        instance.CreatePointLabelRed(vector, $"-{point}");
    }

    public static void CreatePointLabelBoss(Vector3 vector, int point)
    {
        var g = Instantiate(instance.PointLabelBoss, instance.transform);

        var l = g.GetComponent<PointLabel>();
        l.Set(vector, $"+{point}");
    }

    // 產生藍、紅色標籤

    public void CreatePointLabelBlue(Vector3 vector, string text)
    {
        var g = Instantiate(instance.PointLabelBlue, instance.transform);

        var l = g.GetComponent<PointLabel>();
        l.Set(vector, text);
    }

    public void CreatePointLabelRed(Vector3 vector, string text)
    {
        var g = Instantiate(instance.PointLabelRed, instance.transform);

        var l = g.GetComponent<PointLabel>();
        l.Set(vector, text);
    }

    

}


