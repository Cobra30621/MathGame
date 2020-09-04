using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBall : MonoBehaviour
{
    [SerializeField] private Text lab_num;

    public void SetNum(int num){
        lab_num.text = $"{num}";
    }

    public void SetImage(Sprite img){
        GetComponent<Image>().sprite = img;
    }
}