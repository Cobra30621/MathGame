using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPointUI : MonoBehaviour
{
    private static BallPointUI instance;

    [SerializeField] private GameObject PointLabelBlue;
    [SerializeField] private GameObject PointLabelRed;

    void Awake()
    {
        instance = this;
    }

    public static void CreateGetPointLabel(Vector3 vector, int point){
        CreatePointLabelBlue(vector, $"+{point}");
    }

    public static void CreateNoPointLabel(Vector3 vector){
        CreatePointLabelRed(vector, $"Miss");
    }

    public static void CreateLossPointLabel(Vector3 vector, int point){
        CreatePointLabelRed(vector, $"-{point}");
    }

    public static void CreatePointLabelBlue(Vector3 vector, string text)
    {
        var g = Instantiate(instance.PointLabelBlue, instance.transform);

        var l = g.GetComponent<PointLabel>();
        l.Set(vector, text);
    }

    public static void CreatePointLabelRed(Vector3 vector, string text)
    {
        var g = Instantiate(instance.PointLabelRed, instance.transform);

        var l = g.GetComponent<PointLabel>();
        l.Set(vector, text);
    }

}


