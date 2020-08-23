using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainFactory
{
    private static BallFactory m_BallFactory = null;

   	// 取得將Unity Asset實作化的工廠
	public static BallFactory GetBallFactory()
	{
		if( m_BallFactory == null)
		{
			m_BallFactory = new BallFactory();
		}
		return m_BallFactory;
	}
}