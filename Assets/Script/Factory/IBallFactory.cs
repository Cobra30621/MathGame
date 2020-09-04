using UnityEngine;
using System.Collections;

// 產生武器工廠界面
public abstract class IBallFactory
{
	// 建立武器
	public abstract Ball CreateBall( int num, BallColor ballColor);
}

