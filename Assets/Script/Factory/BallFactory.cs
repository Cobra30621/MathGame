using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallFactory : IBallFactory
{
    public GameObject BallCanvas ;
    public GameObject BallPrefab;
    private Vector3 StartSpeed  = new Vector3(2f,-2f,0);
    private Vector3 StartPosition  = new Vector3(0,4,0);

    // BallImage
    private Image img_blue, img_gray, img_purple, img_orange;

    // 設定球的分數
    private Dictionary< BallColor, int > points = new Dictionary< BallColor, int >()
        { 
            {BallColor.Blue, 100},
            {BallColor.Gray, 50},
            {BallColor.Purple, 500},
            {BallColor.Orange, 250}
        };

    public void CreateBossBall(int num){ // 建立魔王球
        Ball ball = CreateBall(num);
        ball.SetBallColor(BallColor.Purple);
        ball.SetBallImage(img_purple);
        ball.SetPoint(points[BallColor.Purple]);
    }

    public void CreateTwoBossBall(Ball ball){
        List<int> nums = FactorizeToTwoNum(ball.GetNumber());

        // 產生球
        Ball ball1 = CreateBall(nums[0]);
        Ball ball2 = CreateBall(nums[1]);

        //  設定球的球的位置
        Vector3 vec = ball.transform.position;
        ball1.SetPosition(vec);
        ball2.SetPosition(vec);

        // 設定球的速度
        ball1.SetSpeed(GetBallVectory(-1));
        ball2.SetSpeed(GetBallVectory(-1));

        // 球還無法回擊
        ball1.SetWhetherTouch(false);
        ball2.SetWhetherTouch(false);

        // 設定球類型與圖片
        ball1.SetBallColor(BallColor.Orange);
        ball1.SetBallImage(img_orange);
        ball1.SetPoint(points[BallColor.Orange]);

        ball2.SetBallColor(BallColor.Orange);
        ball2.SetBallImage(img_orange);
        ball2.SetPoint(points[BallColor.Orange]);

        // 刪除目前的球
        ball.Release();
    }

    public void CreateTwoBall(Ball ball){
        List<int> nums = FactorizeToTwoNum(ball.GetNumber());

        // 產生球
        Ball ball1 = CreateBall(nums[0]);
        Ball ball2 = CreateBall(nums[1]);

        //  設定球的球的位置
        Vector3 vec = ball.transform.position;
        ball1.SetPosition(vec);
        ball2.SetPosition(vec);

        // 設定球的速度
        ball1.SetSpeed(GetBallVectory(-1));
        ball2.SetSpeed(GetBallVectory(-1));

        // 球還無法回擊
        ball1.SetWhetherTouch(false);
        ball2.SetWhetherTouch(false);

        // 設定球類型與圖片
        ball1.SetBallColor(BallColor.Gray);
        ball1.SetBallImage(img_gray);
        ball1.SetPoint(points[BallColor.Gray]);

        ball2.SetBallColor(BallColor.Gray);
        ball2.SetBallImage(img_gray);
        ball2.SetPoint(points[BallColor.Gray]);

        // 刪除目前的球
        ball.Release();
    }

    public override Ball CreateBall(int num){
        Initiaize(); // 取得Ball和BallCanvas
        // 建立物件模組
        GameObject ball = GameObject.Instantiate( BallPrefab , BallCanvas.transform);
        Ball Ball = ball.GetComponent<Ball>();

        if(WhetherPrime(num))
            Ball.SetBallType(BallType.Prime);
        else
            Ball.SetBallType(BallType.Composite);
        
        // 設定球類型與圖片
        Ball.SetBallColor(BallColor.Blue);
        Ball.SetBallImage(img_blue);

        // 設定數值
        Ball.SetNumber(num);
        Ball.SetSpeed(GetBallVectory(1));
        Ball.SetPosition(GetBallPosition());
        Ball.SetWhetherTouch(true);

        Debug.Log("Ball"+ Ball);
        Debug.Log("Ball"+ Ball.ballType);
        // 加入管理器
        GameMeditor.Instance.AddBall(Ball);

        return Ball;
    }


    private void Initiaize(){
        if (BallCanvas == null)
            SetBallCanvas();
        if (BallPrefab == null)
            SetBall();
        if (img_blue == null)
            SetImage();
    }

    private void SetBallCanvas(){
        BallCanvas = GameObject.Find("BallCanvas");
        
        if (BallCanvas == null)
            Debug.LogError("找不到BallCanvas");
        else 
            Debug.Log("找到BallCanvas" );
    }

    private void SetBall(){
        BallPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        if (BallPrefab == null)
            Debug.LogError("找不到BallPrefabs");
        else 
            Debug.Log("找到BallPrefab" );
    }

    private void SetImage(){
        img_blue = Resources.Load<Image>("Image/BlueBall");
        img_gray = Resources.Load<Image>("Image/GrayBall");
        img_purple = Resources.Load<Image>("Image/PurpleBall");
        img_orange = Resources.Load<Image>("Image/OrangeBall");

        if (img_blue == null)
            Debug.LogError("找不到img_blue");
        else 
            Debug.Log("找到img_blue" );

        if (img_gray == null)
            Debug.LogError("找不到img_gray");
        else 
            Debug.Log("找到img_gray" );

        if (img_purple == null)
            Debug.LogError("找不到img_purple");
        else 
            Debug.Log("找到img_purple" );

        if (img_orange == null)
            Debug.LogError("找不到img_orange");
        else 
            Debug.Log("找到img_orange" );
    }

    //-------------數學方法--------------

    public List<int> FactorizeToTwoNum(int num){
        if(WhetherPrime(num))
        {
            Debug.LogError("質數被輸入");
            return new List<int>{2, 2};
        }

        int num1 = 2;
        int num2 = 1;
        bool WhetherFind = false;

        while (!WhetherFind)
        {
            if (num % num1 == 0)
            {
                WhetherFind = true;
            }
            else
            {
                num1++;
            }
        }
        num2 = num/ num1;
        Debug.Log($"num{num}>num1:{num1}+num2{num2}");

        List<int> nums = new List<int>{num1, num2};

        return nums;

    }

    public bool WhetherPrime(int num){
        int aa = 0;
        for (int i = 1; i <= num; i++)
        {
            if (num % i == 0)
            {
                aa = aa + 1;
            }

            // 若超過2就不會是質數了，所以不需要繼續計算
            if (aa > 2) break;
        }

        if (aa == 2)
            return true;
        else
            return false;
    }

    public Vector3 GetBallVectory(int direction){
        float x = Random.Range(-2f, 2f);
        float y = direction * Random.Range(-2f, -1.5f);
        return new Vector3(x, y, 0);
    }

    public Vector3 GetBallPosition(){
        float x = Random.Range(0f, 2f);
        return new Vector3(x, 4f, 0);
    }

    
}