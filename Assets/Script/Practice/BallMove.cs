using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BallMove : MonoBehaviour
{
    public Vector3 value = new Vector3(0, 10, 0);
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputProcess();
    }

    public void InputProcess(){
      if(Input.GetKey(KeyCode.RightArrow))
        transform.DOMoveX(transform.position.x + 1, 1);
      if(Input.GetKey(KeyCode.LeftArrow))
        transform.DOMoveX(transform.position.x - 1, 1);
      if(Input.GetKey(KeyCode.UpArrow))
      {
        transform.DOMoveY(transform.position.y + 1, 1).SetEase(Ease.OutQuart);
        image.DOFade(1, 1).SetEase(Ease.OutQuart);
        //transform.DOJump(transform.position, 2, 1, 3);
      }
      if(Input.GetKey(KeyCode.DownArrow))
      {
        transform.DOMoveY(transform.position.y - 1, 1).SetEase(Ease.OutQuart);
        image.DOFade(0, 1).SetEase(Ease.OutQuart);
      }
    }
}
