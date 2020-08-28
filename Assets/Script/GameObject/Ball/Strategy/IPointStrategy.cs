using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPointStrategy 
{
    public virtual int GetBallPoint(BallColor color, Ball ball){
        return 0;
    }


}
