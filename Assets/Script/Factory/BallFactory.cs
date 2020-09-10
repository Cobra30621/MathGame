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
    private float ballSpeed = 1f;

    // BallImage
    private Sprite img_judge, img_prime, img_boss, img_composite, img_dead;

    // 設定球的分數
    private Dictionary< BallColor, int > points = new Dictionary< BallColor, int >()
        { 
            {BallColor.Judge, 100},
            {BallColor.Prime, 50},
            {BallColor.Boss, 500},
            {BallColor.Composite, 100},
            {BallColor.Dead, 250}
        };

    public void SetBallSpeed(float speed){
        ballSpeed = speed;
    }

    // ---------------------------------
    // ------------產生球-------------
    // ---------------------------------

    // 產生直線行走球 Speed.x = 0
    public void CreateStraightBall(int num, BallColor ballColor){
        Ball ball = CreateBall(num, ballColor);
        ball.SetSpeed(GetBallVectoryXZero());
    }


    public void CreateTwoBall(Ball ball){
        List<int> nums = FactorizeToTwoCloseNum(ball.GetNumber());

        BallColor MotherBallColor = ball.GetBallColor();
        Ball ball1;
        Ball ball2;

        // 產生魔王球
        if(MotherBallColor == BallColor.Boss)
        {
            ball1 = CreateBall(nums[0], BallColor.Boss);
            ball2 = CreateBall(nums[1], BallColor.Boss);
        }
        else // 產生一般球
        {
            // 產生Ball1
            if(WhetherPrime(nums[0]))
                ball1 = CreateBall(nums[0], BallColor.Prime);
            else
                ball1 = CreateBall(nums[0], BallColor.Composite);

            // 產生Ball2
            if(WhetherPrime(nums[1]))
                ball2 = CreateBall(nums[1], BallColor.Prime);
            else
                ball2 = CreateBall(nums[1], BallColor.Composite);
        }

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

        // 刪除目前的球
        ball.Release();
    }

    public override Ball CreateBall (int num, BallColor ballColor){
        Initiaize(); // 取得Ball和BallCanvas
        // 建立物件模組
        GameObject ball = GameObject.Instantiate( BallPrefab , BallCanvas.transform);
        Ball Ball = ball.GetComponent<Ball>();

        // 以數字判斷球為質數或合數
        if(WhetherPrime(num))
            Ball.SetBallType(BallType.Prime);
        else
            Ball.SetBallType(BallType.Composite);
        
        // 設定球類型與圖片
        Ball.SetBallColor(ballColor);
        Ball.SetBallImage(GetBallImage(ballColor));

        // 設定數值
        Ball.SetNumber(num);
        Ball.SetSpeed(GetBallVectory(1));
        Ball.SetPosition(GetBallPosition());
        Ball.SetWhetherTouch(true);
        SetBallPoint(BallColor.Judge, Ball);

        Debug.Log("新增Ball"+ Ball.ballType);
        // 加入管理器
        GameMeditor.Instance.AddBall(Ball);

        return Ball;

    }

    public Sprite GetBallImage(BallColor ballColor){ 
        Initiaize(); //為了ShowBall的初始化    
        switch(ballColor){
            case BallColor.Judge:
                return img_judge;
            case BallColor.Prime:
                return img_prime;
            case BallColor.Composite:
                return img_composite;
            case BallColor.Dead:
                return img_dead;
            case BallColor.Boss:
                return img_boss;
            default:
                Debug.LogError("傳入錯誤球的造型");
                return null;
        }
    }

    private void Initiaize(){
        if (BallCanvas == null)
            SetBallCanvas();
        if (BallPrefab == null)
            SetBall();
        if (img_judge == null) // 社啥奇耙的判斷
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
        ResourceAssetFactory resourceAssetFactory = MainFactory.GetResourceAssetFactory();
        BallPrefab = resourceAssetFactory.LoadBallPrefab("Ball");
        /*
        BallPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        if (BallPrefab == null)
            Debug.LogError("找不到BallPrefabs");
        else 
            Debug.Log("找到BallPrefab" );
            */
    }

    private void SetImage(){
        // string path = "NCUBall/";
        // string path = "SnowBall/";
        // string path = "Tomato/";
        ResourceAssetFactory resourceAssetFactory = MainFactory.GetResourceAssetFactory();

        img_judge = resourceAssetFactory.LoadBallImage("judge") ;
        img_prime = resourceAssetFactory.LoadBallImage("prime") ;
        img_boss = resourceAssetFactory.LoadBallImage("boss") ;
        img_dead = resourceAssetFactory.LoadBallImage("dead") ;
        img_composite = resourceAssetFactory.LoadBallImage("composite") ;

        /*

        if (img_judge == null)
            Debug.LogError("找不到img_judge");
        else 
            Debug.Log("找到img_judge" );

        if (img_prime == null)
            Debug.LogError("找不到img_prime");
        else 
            Debug.Log("找到img_prime" );

        if (img_boss == null)
            Debug.LogError("找不到img_boss");
        else 
            Debug.Log("找到img_boss" );

        if (img_composite == null)
            Debug.LogError("找不到img_composite");
        else 
            Debug.Log("找到img_composite" );

        if (img_dead == null)
            Debug.LogError("找不到img_dead");
        else 
            Debug.Log("找到img_dead" );*/
    }

    // 取的死球（灰球圖片）
    public void SetDeadBallImage(Ball ball){
        ball.SetBallImage(img_dead);
    }

    // 造球方法
    public void SetBallPoint(BallColor color, Ball ball){
        int point = GameMeditor.Instance.GetBallPoint(color, ball);
        ball.SetPoint(point);
    }



    //-------------數學方法--------------

    // 取的一顆球最大Combol數（分裂次數 + 拆解的質數個數)
    public int GetNumMaxCombol(int num){
        int primeNum = GetPrimeCount(num);
        return primeNum * 2 -1;
    }

    // 取得質因數個數 
    // ex:90 = 2 x 3 x 3 x 5，GetPrimeCount(90) = 1+ 1+1 +1 =4
    // ex: 2 =2 ，GetPrimeCount(2) = 1 
    public int GetPrimeCount(int num){
        int PrimeNume =0;
        int i = 2;
        while (i <= num)
        {
            if (num % i == 0)
            {
                PrimeNume ++ ;
                num /= i;
            }
            else
            {
                i++;
            }
        }
        return PrimeNume;
    }

    public List<int> FactorizeToTwoCloseNum(int num){
        if(WhetherPrime(num))
        {
            Debug.LogError("質數被輸入");
            return new List<int>{2, 2};
        }

        List<int> numsList = new List<int>();
        bool WhetherFind = false;

        // 找出所有因數
        for(int i = 1; i <= num; i++){
            if (num % i == 0)
                numsList.Add(i);
        }

        // 數字1與數字2
        int num1 = 2;
        int num2 = 2;
        int Count = numsList.Count;

        if( Count % 2 == 0) // 總質數個數為偶數
        {
            num1 = numsList[(Count / 2) -1 ];
            num2 = numsList[(Count / 2)  ];
        }
        else // 總質數個數為基數
        {
            num1 = numsList[(Count -1) / 2 ];
            num2 = numsList[(Count -1) / 2 ];
        }
        
        
        Debug.Log($"num{num}>num1:{num1}+num2:{num2}");

        List<int> nums = new List<int>{num1, num2};

        return nums;
    }

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
        // float y = direction * Random.Range(-2.2f, -1.7f);
        float y = direction * Random.Range(-2f * ballSpeed, -1.5f * ballSpeed);
        // Debug.Log($"x:{x}, y:{y}");
        return new Vector3(x, y, 0);
    }

    public Vector3 GetBallVectoryXZero(){
        float x = 0;
        // float y = direction * Random.Range(-2.2f, -1.7f);
        float y = Random.Range(-2f * ballSpeed, -1.5f * ballSpeed);
        return new Vector3(x, y, 0);
    }

    public Vector3 GetBallPosition(){
        float x = Random.Range(-2f, 2f);
        return new Vector3(x, 4f, 0);
    }



    
}