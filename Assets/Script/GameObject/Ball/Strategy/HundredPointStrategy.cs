using System.Collections.Generic;

public class HundredPointStrategy : IPointStrategy{
        // 設定球的分數
    private Dictionary< BallColor, int > points = new Dictionary< BallColor, int >()
    { 
        {BallColor.Blue, 100},
        {BallColor.Gray, 50},
        {BallColor.Purple, 500},
        {BallColor.Orange, 100},
        {BallColor.Green, 250}
    };

    public override int GetBallPoint(BallColor color, Ball ball){
        return points[color];
    }

}