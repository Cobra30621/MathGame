using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneInputUI : MonoBehaviour
{
    public static PhoneInputUI instance;
    [SerializeField] private BallBar Bar;
    [SerializeField] private GameObject RightBut, LeftBut;

    void Awake()
    {
        instance = this;
    }

    // 是否可以開始點擊Bar
    public static void EnableMoveBar(bool bo){
        instance.SetBarActive(bo);
    }

    public void SetBarActive(bool bo){
        if(bo){
            RightBut.SetActive(true);
            LeftBut.SetActive(true);
        }
        else
        {
            RightBut.SetActive(false);
            LeftBut.SetActive(false);
        }
    }

    public void MoveRightDown(){
        Bar._moveType = MoveType.Right;
    }

    public void MoveRightUp(){
        Bar._moveType = MoveType.Stop;
    }

    public void MoveLeftDown(){
        Bar._moveType = MoveType.Left;
    }

    public void MoveLeftUp(){
        Bar._moveType = MoveType.Stop;
    }

}
