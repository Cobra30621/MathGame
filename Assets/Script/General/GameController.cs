using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject swipeTrigger;


    public void SwipeBall(Vector2 _direction){
        swipeTrigger.SetActive(true);
        swipeTrigger.transform.position = _direction;
        
        StartCoroutine(WaitAndClose());
        // swipeTrigger = Instantiate( Resources.Load<GameObject>("Prefabs/swipeTrigger"));
    }

    IEnumerator WaitAndClose()
    {
        Debug.Log("CoroutineTest Start At" + Time.time);
        yield return new WaitForSeconds(0.1f);
        swipeTrigger.SetActive(false);
        Debug.Log("CoroutineTest End At" + Time.time);
    }

}
