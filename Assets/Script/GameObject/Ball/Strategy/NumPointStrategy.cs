

public class NumPointStrategy : IPointStrategy{
        // 設定球的分數
    public override int GetBallPoint(BallColor color, Ball ball){
        int point ;
        point = ball.GetNumber(); //    分數為球的數字
    /*
    switch(BallColor){
        case BallColor.Blue :
        case BallColor.Gray :    
            point = ball.GetNumber();
            break;
    } */

        return point;
    }

}