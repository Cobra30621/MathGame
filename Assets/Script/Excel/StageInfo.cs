/// <summary>
/// 選單實體類
/// </summary>

public class StageInfo 
{
    public string _stageName;
    public int[] _primes;
    public int[] _composites;
    public int[] _probability;
    public string _startText;  // 開始文字
    public int _ballCounts; // 通關所需要球的數量
    public float _fallingSpeed; // 球掉落速度
    public int _ballCountOnce; // 一次出球數
    public float _CoolDown; // 出球頻率
    public BallMoveMethon _ballMoveMethon; // 球移動方式
    public BGM _bgm; // BGM


    //probability	startText	ballCounts	fallingSpeed	ballCountCreate	CoolDown	BallStrategy
    
}